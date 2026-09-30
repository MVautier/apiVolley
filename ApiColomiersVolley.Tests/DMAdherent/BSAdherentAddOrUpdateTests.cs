using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Business;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using Moq;

namespace ApiColomiersVolley.Tests.DMAdherent
{
    /// <summary>
    /// Une personne = une ligne adherent, mise a jour a chaque saison. Un enregistrement
    /// arrivant avec IdAdherent = 0 et l'uid d'un adherent existant (parcours "Nouvelle
    /// inscription" d'un ancien adherent) doit mettre a jour sa ligne, pas en creer une.
    /// </summary>
    public class BSAdherentAddOrUpdateTests
    {
        private const string ExistingUid = "4affabef-bd5a-4242-9bcb-b73bc1932ea9";

        private readonly Mock<IDMAdherentRepo> _adherentRepo = new();
        private readonly Mock<IDMOrderRepo> _orderRepo = new();
        private readonly List<DtoAdherent> _saved = new();
        private readonly BSAdherent _bs;

        public BSAdherentAddOrUpdateTests()
        {
            _adherentRepo
                .Setup(r => r.AddOrUpdate(It.IsAny<DtoAdherent>()))
                .ReturnsAsync((DtoAdherent a) =>
                {
                    _saved.Add(a);
                    if (a.IdAdherent == 0)
                    {
                        a.IdAdherent = 999;
                    }
                    return a;
                });
            _orderRepo.Setup(r => r.GetByAdherent(It.IsAny<int>())).ReturnsAsync(new List<DtoOrder>());

            _bs = new BSAdherent(_adherentRepo.Object, Mock.Of<IFileManager>(), _orderRepo.Object, Mock.Of<IServiceSendMail>());
        }

        private static DtoAdherent Existing() => new()
        {
            IdAdherent = 421,
            Uid = ExistingUid,
            LastName = "BELANGER",
            FirstName = "Nixon",
            Saison = 2024,
            Histo = new List<DtoHisto>
            {
                new() { saison = 2024, date = "2024-09-22", Category = 1 },
                new() { saison = 2023, date = "2023-10-21", Category = 1 }
            }
        };

        private static DtoAdherent Incoming(int idAdherent = 0, string? uid = ExistingUid) => new()
        {
            IdAdherent = idAdherent,
            Uid = uid,
            LastName = "BELANGER",
            FirstName = "Nixon",
            Category = "C",
            Saison = 2026,
            InscriptionDate = new DateOnly(2026, 9, 24),
            Membres = new List<DtoAdherent>(),
            Orders = new List<DtoOrder>(),
            Histo = new List<DtoHisto>()
        };

        [Fact]
        public async Task IdAdherent0_uid_connu_met_a_jour_la_ligne_existante()
        {
            _adherentRepo.Setup(r => r.GetByUid(ExistingUid)).ReturnsAsync(Existing());

            var result = await _bs.AddOrUpdate(Incoming());

            Assert.Equal(421, result.IdAdherent);
            Assert.Single(_saved);
            Assert.Equal(421, _saved[0].IdAdherent);
        }

        [Fact]
        public async Task IdAdherent0_uid_connu_conserve_l_historique_des_saisons_precedentes()
        {
            _adherentRepo.Setup(r => r.GetByUid(ExistingUid)).ReturnsAsync(Existing());

            var result = await _bs.AddOrUpdate(Incoming());

            Assert.Equal(new int?[] { 2026, 2024, 2023 }, result.Histo.Select(h => h.saison));
        }

        [Fact]
        public async Task IdAdherent0_uid_connu_ne_duplique_pas_une_saison_deja_presente()
        {
            _adherentRepo.Setup(r => r.GetByUid(ExistingUid)).ReturnsAsync(Existing());
            var incoming = Incoming();
            incoming.Histo.Add(new DtoHisto { saison = 2024, date = "2024-09-22", Category = 1 });

            var result = await _bs.AddOrUpdate(incoming);

            Assert.Single(result.Histo, h => h.saison == 2024);
        }

        [Fact]
        public async Task IdAdherent0_uid_inconnu_cree_une_nouvelle_ligne()
        {
            _adherentRepo.Setup(r => r.GetByUid(It.IsAny<string>())).ReturnsAsync((DtoAdherent)null!);

            var result = await _bs.AddOrUpdate(Incoming(uid: "uid-inconnu"));

            // 999 = id attribue par le repo simule a une insertion (IdAdherent = 0 a l'appel)
            Assert.Equal(999, result.IdAdherent);
            _adherentRepo.Verify(r => r.GetByUid("uid-inconnu"), Times.Once);
        }

        [Fact]
        public async Task IdAdherent0_sans_uid_ne_cherche_pas_d_adherent_existant()
        {
            await _bs.AddOrUpdate(Incoming(uid: null));

            _adherentRepo.Verify(r => r.GetByUid(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task IdAdherent_renseigne_ne_cherche_pas_par_uid()
        {
            var result = await _bs.AddOrUpdate(Incoming(idAdherent: 421));

            Assert.Equal(421, result.IdAdherent);
            _adherentRepo.Verify(r => r.GetByUid(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Membre_IdAdherent0_uid_connu_met_a_jour_sa_ligne()
        {
            const string membreUid = "uid-membre";
            _adherentRepo.Setup(r => r.GetByUid(ExistingUid)).ReturnsAsync((DtoAdherent)null!);
            _adherentRepo.Setup(r => r.GetByUid(membreUid)).ReturnsAsync(new DtoAdherent { IdAdherent = 500, Uid = membreUid });
            var incoming = Incoming();
            incoming.Membres.Add(new DtoAdherent { IdAdherent = 0, Uid = membreUid, LastName = "BELANGER", FirstName = "Membre" });

            await _bs.AddOrUpdate(incoming);

            var membre = _saved.Single(a => a.Uid == membreUid);
            Assert.Equal(500, membre.IdAdherent);
            Assert.Equal(999, membre.IdParent);
        }

        [Fact]
        public async Task Batch_IdAdherent0_uid_connu_met_a_jour_la_ligne_existante()
        {
            _adherentRepo.Setup(r => r.GetByUid(ExistingUid)).ReturnsAsync(Existing());

            var results = await _bs.AddOrUpdateBatch(new List<DtoAdherent> { Incoming() });

            Assert.Equal(421, results.Single().Adherent.IdAdherent);
        }
    }
}
