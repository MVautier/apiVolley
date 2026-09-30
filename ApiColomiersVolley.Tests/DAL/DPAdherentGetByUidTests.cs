using ApiColomiersVolley.DAL;
using ApiColomiersVolley.DAL.DataProviders;
using ApiColomiersVolley.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiColomiersVolley.Tests.DAL
{
    /// <summary>
    /// Le webhook Helloasso retrouve l'adherent par uid. D'anciens doublons (meme personne,
    /// une ligne par saison) faisaient renvoyer la plus ancienne ligne : GetByUid doit
    /// renvoyer celle de la saison la plus recente.
    /// </summary>
    public class DPAdherentGetByUidTests
    {
        private const string Uid = "4affabef-bd5a-4242-9bcb-b73bc1932ea9";

        private static ColomiersVolleyContext NewContext()
        {
            var options = new DbContextOptionsBuilder<ColomiersVolleyContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ColomiersVolleyContext(options);
        }

        private static Adherent Row(int id, int? saison, string uid = Uid) => new()
        {
            IdAdherent = id,
            Uid = uid,
            Saison = saison,
            LastName = "BELANGER",
            FirstName = "Nixon",
            Genre = "M",
            Phone = "",
            Address = "",
            PostalCode = "",
            City = "",
            Email = ""
        };

        [Fact]
        public async Task Renvoie_la_ligne_de_la_saison_la_plus_recente()
        {
            using var db = NewContext();
            db.Adherents.AddRange(Row(421, 2024), Row(595, 2026), Row(300, 2023));
            await db.SaveChangesAsync();

            var result = await new DPAdherent(db).GetByUid(Uid);

            Assert.Equal(595, result.IdAdherent);
        }

        [Fact]
        public async Task A_saison_egale_renvoie_la_ligne_la_plus_recente()
        {
            using var db = NewContext();
            db.Adherents.AddRange(Row(421, 2026), Row(595, 2026));
            await db.SaveChangesAsync();

            var result = await new DPAdherent(db).GetByUid(Uid);

            Assert.Equal(595, result.IdAdherent);
        }

        [Fact]
        public async Task Uid_inconnu_renvoie_null()
        {
            using var db = NewContext();
            db.Adherents.Add(Row(421, 2024));
            await db.SaveChangesAsync();

            var result = await new DPAdherent(db).GetByUid("uid-inconnu");

            Assert.Null(result);
        }
    }
}
