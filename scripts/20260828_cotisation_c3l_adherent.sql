-- =============================================================================
-- Ajout de la part CLLL par adhérent (adherent.cotisation_c3l) + reprise des
-- inscriptions de la saison en cours.
--
-- À exécuter sur MySQL de production (phpMyAdmin IONOS), en deux temps :
--   * PARTIE 1 avant le déploiement de l'API
--   * PARTIE 2 après le déploiement de l'API et du front
--
-- Le script généré par `dotnet ef migrations script --idempotent` n'est pas
-- utilisable ici : sa syntaxe `IF NOT EXISTS ... BEGIN ... END` n'est pas valide
-- en MySQL hors procédure stockée. La PARTIE 1 fait le même travail à la main,
-- volontairement limitée à l'ajout de colonne (voir la note en fin de fichier).
--
-- Adapter @saison si la saison en cours n'est pas 2026.
-- =============================================================================

SET @saison = 2026;


-- =============================================================================
-- PARTIE 1 -- Schéma. À exécuter AVANT de déployer la nouvelle API.
--
-- Additif et nullable : l'API actuellement en production ignore cette colonne,
-- les inscriptions peuvent donc continuer pendant et après cette étape.
-- =============================================================================

ALTER TABLE `adherent` ADD COLUMN `cotisation_c3l` int NULL;

-- Volontairement, aucune ligne n'est ajoutée à `__EFMigrationsHistory`.
--
-- Cette table existe bien en production, mais son contenu s'arrête à
-- `20230403190001_add_adherent_fields` : les dix migrations suivantes du dépôt
-- (ajout_role, ajout_VerifC3L, ajout_order, ajout_adherent_histo,
-- ajout_parametres...) sont bel et bien présentes en base, mais y ont été
-- appliquées à la main sans jamais être enregistrées. L'historique EF est donc
-- figé depuis avril 2023 et ne reflète plus l'état réel du schéma.
--
-- Y insérer cette seule migration laisserait croire à un suivi partiel, alors
-- que `dotnet ef database update` resterait de toute façon inutilisable ici (il
-- tenterait de rejouer les dix migrations manquantes, sur des colonnes qui
-- existent déjà). Mieux vaut un historique franchement obsolète qu'un historique
-- trompeur. Voir la note en fin de fichier.


-- =============================================================================
-- PARTIE 2 -- Reprise des données. À exécuter APRÈS le déploiement API + front.
--
-- Les inscriptions déjà finalisées l'ont été avec l'ancien calcul, ce qui rend
-- la reprise exacte (et non approximative) :
--   * le payeur : commande.cotisation_c3l ne contenait que sa propre ligne
--     d'adhésion, c'est donc exactement sa part ;
--   * chaque membre du foyer : l'ancien code facturait systématiquement
--     parametres.tarifMember, sans condition d'âge ni de lien.
--
-- Les inscriptions saisies après le déploiement ont déjà leur part renseignée :
-- la clause `cotisation_c3l IS NULL` les laisse intactes et rend le script
-- rejouable sans risque.
--
-- Non couverts, laissés à NULL : les paiements manuels, qui n'ont pas de
-- commande et pour lesquels aucune ventilation CLLL/CLUB n'est affichée.
-- =============================================================================

-- Contrôle avant reprise : nombre de lignes qui vont être renseignées.
SELECT
  SUM(a.id_parent IS NULL)     AS payeurs_a_reprendre,
  SUM(a.id_parent IS NOT NULL) AS membres_a_reprendre
FROM adherent a
WHERE a.saison = @saison
  AND a.cotisation_c3l IS NULL
  AND EXISTS (
    SELECT 1 FROM commande c
    WHERE c.saison = @saison
      AND c.id_adherent = COALESCE(a.id_parent, a.id)
  );

-- Payeurs : leur part est le montant CLLL porté par leur commande la plus récente.
--
-- Surtout pas la somme de leurs commandes : un adhérent peut en avoir plusieurs sur
-- la même saison (cas constaté en production, deux paiements à deux minutes d'intervalle
-- pour le même panier). Sommer doublerait sa cotisation.
UPDATE adherent a
JOIN commande c ON c.id_adherent = a.id AND c.saison = @saison
LEFT JOIN commande plus_recente ON plus_recente.id_adherent = a.id
     AND plus_recente.saison = @saison
     AND (plus_recente.date > c.date OR (plus_recente.date = c.date AND plus_recente.id > c.id))
SET a.cotisation_c3l = c.cotisation_c3l
WHERE plus_recente.id IS NULL
  AND a.saison = @saison
  AND a.id_parent IS NULL
  AND a.cotisation_c3l IS NULL;

-- Membres du foyer : l'ancien code leur appliquait toujours le tarif membre.
UPDATE adherent m
JOIN adherent p ON p.id = m.id_parent
JOIN commande c ON c.id_adherent = p.id AND c.saison = @saison
CROSS JOIN (SELECT tarifMember FROM parametres LIMIT 1) t
SET m.cotisation_c3l = t.tarifMember
WHERE m.saison = @saison
  AND m.cotisation_c3l IS NULL;

-- Contrôles après reprise. Les trois doivent retourner 0 / aucune ligne.
--
-- Ne pas chercher à vérifier que la somme des parts d'un foyer égale le montant CLLL de sa
-- commande : sur les données historiques l'écart est normal et vaut précisément la part des
-- membres, que l'ancien calcul omettait. C'est le bug d'origine, pas une erreur de reprise.

-- 1. Plus aucune part manquante pour une personne rattachée à une commande.
SELECT COUNT(*) AS parts_manquantes
FROM adherent a
WHERE a.saison = @saison
  AND a.cotisation_c3l IS NULL
  AND EXISTS (
    SELECT 1 FROM commande c
    WHERE c.saison = @saison AND c.id_adherent = COALESCE(a.id_parent, a.id)
  );

-- 2. La part de chaque payeur est bien celle de sa commande la plus récente.
SELECT a.id, a.nom, a.cotisation_c3l AS part, c.cotisation_c3l AS c3l_commande
FROM adherent a
JOIN commande c ON c.id_adherent = a.id AND c.saison = @saison
LEFT JOIN commande plus_recente ON plus_recente.id_adherent = a.id
     AND plus_recente.saison = @saison
     AND (plus_recente.date > c.date OR (plus_recente.date = c.date AND plus_recente.id > c.id))
WHERE plus_recente.id IS NULL
  AND a.saison = @saison
  AND a.id_parent IS NULL
  AND a.cotisation_c3l <> c.cotisation_c3l;

-- 3. Chaque membre porte bien le tarif membre historique.
SELECT m.id, m.nom, m.cotisation_c3l
FROM adherent m
CROSS JOIN (SELECT tarifMember FROM parametres LIMIT 1) t
WHERE m.saison = @saison
  AND m.id_parent IS NOT NULL
  AND m.cotisation_c3l IS NOT NULL
  AND m.cotisation_c3l <> t.tarifMember;


-- =============================================================================
-- NOTE -- pourquoi ne pas appliquer la migration EF telle quelle.
--
-- La migration générée par `dotnet ef migrations add` embarque, en plus de la
-- colonne, un alignement de `adherent.id_section` / `adherent.id_categorie` en
-- nullable et une recréation des clés étrangères en RESTRICT.
--
-- Ces opérations échoueraient : d'après le dump de production, ces deux colonnes
-- sont déjà nullables et la table `adherent` ne porte aucune clé étrangère. Les
-- `DROP CONSTRAINT` de la migration ne trouveraient donc rien à supprimer.
--
-- Plus largement, les migrations EF ne décrivent plus la production depuis avril
-- 2023 (cf. PARTIE 1) : le schéma y évolue à la main, et le mécanisme EF n'est
-- plus qu'un artefact du dépôt. Tant que cet écart n'est pas résorbé, toute
-- évolution de schéma doit passer par un script SQL comme celui-ci, et la
-- migration générée par EF ne sert que de trace de l'intention.
--
-- Remise à plat possible, hors période d'inscriptions : constater l'état réel de
-- la base, régénérer un snapshot qui lui corresponde, et réaligner
-- `__EFMigrationsHistory` en une fois.
-- =============================================================================
