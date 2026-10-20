using ApiColomiersVolley.BLL.Core.Tools;
using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Business;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using Cartegie.BLL.CreationDeFichiers.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ApiColomiersVolley.Tests.DMAdherent
{
    /// <summary>
    /// Un ancien adhérent qui se réinscrit sans nouvelle photo doit récupérer celle d'une saison
    /// précédente, copiée dans le dossier de la saison (uid/saison/). Tests sur un vrai dossier
    /// temporaire, via le FileManager réel.
    /// </summary>
    public class BSAdherentPhotoTests : IDisposable
    {
        private const string Uid = "4affabef-bd5a-4242-9bcb-b73bc1932ea9";

        private readonly string _root = Path.Combine(Path.GetTempPath(), "apivolley-tests-" + Guid.NewGuid());
        private readonly Mock<IDMAdherentRepo> _adherentRepo = new();
        private readonly Mock<IDMOrderRepo> _orderRepo = new();
        private readonly BSAdherent _bs;

        public BSAdherentPhotoTests()
        {
            var env = new Mock<IHostingEnvironment>();
            env.Setup(e => e.WebRootPath).Returns(_root);
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Paths:Adherent"] = "adherents",
                    ["Paths:BaseUrl"] = "https://test/"
                })
                .Build();
            var fileManager = new FileManager(env.Object, config, Mock.Of<IFileExport>());

            _adherentRepo.Setup(r => r.AddOrUpdate(It.IsAny<DtoAdherent>())).ReturnsAsync((DtoAdherent a) => a);
            _orderRepo.Setup(r => r.GetByAdherent(It.IsAny<int>())).ReturnsAsync(new List<DtoOrder>());
            _bs = new BSAdherent(_adherentRepo.Object, fileManager, _orderRepo.Object, Mock.Of<IServiceSendMail>());
        }

        public void Dispose()
        {
            if (Directory.Exists(_root))
            {
                Directory.Delete(_root, true);
            }
        }

        private string AdherentDir(string? saison = null) =>
            saison == null ? Path.Combine(_root, "adherents", Uid) : Path.Combine(_root, "adherents", Uid, saison);

        private void CreateFile(string saison, string name, string content, DateTime? lastWrite = null)
        {
            var dir = AdherentDir(saison);
            Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, name);
            File.WriteAllText(path, content);
            if (lastWrite.HasValue)
            {
                File.SetLastWriteTime(path, lastWrite.Value);
            }
        }

        private static DtoAdherent Reinscription(string? photo = null) => new()
        {
            IdAdherent = 421,
            Uid = Uid,
            LastName = "BELANGER",
            FirstName = "Nixon",
            Saison = 2026,
            Photo = photo,
            Membres = new List<DtoAdherent>(),
            Orders = new List<DtoOrder>(),
            Histo = new List<DtoHisto>()
        };

        [Fact]
        public async Task Sans_photo_reprend_celle_de_la_saison_precedente()
        {
            CreateFile("2025", "photo.jpg", "photo-2025");

            var result = await _bs.AddOrUpdate(Reinscription());

            Assert.Equal(Uid + "/photo.jpg", result.Photo);
            Assert.Equal("photo-2025", File.ReadAllText(Path.Combine(AdherentDir("2026"), "photo.jpg")));
            Assert.True(File.Exists(Path.Combine(AdherentDir("2025"), "photo.jpg")), "l'original reste en place");
        }

        [Fact]
        public async Task Prend_la_photo_de_la_saison_la_plus_recente()
        {
            CreateFile("2024", "photo.png", "photo-2024");
            CreateFile("2025", "photo.jpg", "photo-2025");

            var result = await _bs.AddOrUpdate(Reinscription());

            Assert.Equal(Uid + "/photo.jpg", result.Photo);
            Assert.False(File.Exists(Path.Combine(AdherentDir("2026"), "photo.png")));
        }

        [Fact]
        public async Task Reprend_une_ancienne_photo_rangee_a_la_racine_du_dossier()
        {
            Directory.CreateDirectory(AdherentDir());
            File.WriteAllText(Path.Combine(AdherentDir(), "photo.jpg"), "photo-racine");

            var result = await _bs.AddOrUpdate(Reinscription());

            Assert.Equal(Uid + "/photo.jpg", result.Photo);
            Assert.True(File.Exists(Path.Combine(AdherentDir("2026"), "photo.jpg")));
        }

        [Fact]
        public async Task Photo_deja_dans_le_dossier_de_la_saison_est_simplement_referencee()
        {
            CreateFile("2026", "photo.png", "photo-2026");
            CreateFile("2025", "photo.jpg", "photo-2025");

            var result = await _bs.AddOrUpdate(Reinscription());

            Assert.Equal(Uid + "/photo.png", result.Photo);
            Assert.False(File.Exists(Path.Combine(AdherentDir("2026"), "photo.jpg")));
        }

        [Fact]
        public async Task Nouvelle_photo_fournie_ne_copie_rien()
        {
            CreateFile("2025", "photo.jpg", "photo-2025");

            var result = await _bs.AddOrUpdate(Reinscription(photo: Uid + "/photo.png"));

            Assert.Equal(Uid + "/photo.png", result.Photo);
            Assert.False(Directory.Exists(AdherentDir("2026")));
        }

        [Fact]
        public async Task Aucune_photo_anterieure_laisse_la_photo_vide()
        {
            CreateFile("2025", "attestation.pdf", "attestation");

            var result = await _bs.AddOrUpdate(Reinscription());

            Assert.Null(result.Photo);
        }

        [Fact]
        public async Task RepairPhotos_corrige_uniquement_les_adherents_de_la_saison_sans_photo()
        {
            CreateFile("2025", "photo.jpg", "photo-2025");
            var sansPhoto = Reinscription();
            var autreSaison = new DtoAdherent { IdAdherent = 1, Uid = Uid, Saison = 2025, LastName = "A", FirstName = "B" };
            var sansAnciennePhoto = new DtoAdherent { IdAdherent = 2, Uid = "uid-sans-photo", Saison = 2026, LastName = "C", FirstName = "D" };
            _adherentRepo.Setup(r => r.GetAdherents()).ReturnsAsync(new List<DtoAdherent> { sansPhoto, autreSaison, sansAnciennePhoto });

            var repaired = await _bs.RepairPhotos(2026);

            Assert.Equal(new[] { $"421 BELANGER Nixon -> {Uid}/photo.jpg" }, repaired);
            _adherentRepo.Verify(r => r.AddOrUpdate(It.Is<DtoAdherent>(a => a.IdAdherent == 421 && a.Photo == Uid + "/photo.jpg")), Times.Once);
            _adherentRepo.Verify(r => r.AddOrUpdate(It.Is<DtoAdherent>(a => a.IdAdherent != 421)), Times.Never);
            Assert.True(File.Exists(Path.Combine(AdherentDir("2026"), "photo.jpg")));
        }
    }
}
