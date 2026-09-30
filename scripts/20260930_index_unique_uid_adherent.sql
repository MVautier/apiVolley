-- Une personne = une ligne adherent, mise a jour a chaque saison. Des doublons d'uid
-- (meme personne, une ligne par saison) faisaient finaliser la mauvaise ligne par le
-- webhook Helloasso. Doublons fusionnes le 30/09/2026 (479->585, 365->577, 362->578,
-- 417->579, 406->582) avant de poser l'index.

-- Verification prealable : doit renvoyer 0 ligne
SELECT uid, COUNT(*) FROM adherent WHERE uid IS NOT NULL GROUP BY uid HAVING COUNT(*) > 1;

ALTER TABLE adherent ADD UNIQUE INDEX ux_adherent_uid (uid);
