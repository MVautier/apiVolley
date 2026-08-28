-- =============================================================================
-- Ajout du lien de parenté des membres du foyer (adherent.lien_parente).
--
-- À exécuter sur MySQL de production (phpMyAdmin IONOS) AVANT le déploiement de
-- l'API. Additif et nullable : l'API en place ignore la colonne, les inscriptions
-- peuvent continuer pendant l'opération.
--
-- Comme pour la migration précédente, ne rien insérer dans `__EFMigrationsHistory` :
-- cet historique est figé depuis avril 2023 et le schéma évolue à la main. Voir
-- 20260828_cotisation_c3l_adherent.sql.
-- =============================================================================

ALTER TABLE `adherent` ADD COLUMN `lien_parente` varchar(1) NULL;


-- =============================================================================
-- Pas de reprise possible.
--
-- Le lien de parenté était saisi à l'inscription mais n'a jamais été transmis à
-- l'API : il vivait uniquement dans le formulaire, le temps de la session. Aucune
-- donnée ne permet donc de le reconstituer pour les membres déjà enregistrés, qui
-- resteront à NULL.
--
-- Conséquence à connaître : pour ces membres-là, il reste impossible de vérifier
-- qu'un tarif réduit était justifié. Seules les inscriptions postérieures à ce
-- déploiement seront contrôlables.
--
-- Membres concernés (lien inconnu, part CLLL déjà enregistrée) :
--
--   SELECT m.id, m.nom, m.prenom, m.saison, m.cotisation_c3l,
--          p.nom AS nom_payeur, p.prenom AS prenom_payeur
--   FROM adherent m
--   JOIN adherent p ON p.id = m.id_parent
--   WHERE m.id_parent IS NOT NULL AND m.lien_parente IS NULL
--   ORDER BY m.saison DESC, m.id;
-- =============================================================================
