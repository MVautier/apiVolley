using ApiColomiersVolley.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace ApiColomiersVolley.DAL
{
    public class ColomiersVolleyContext : DbContext
    {
        internal IConfiguration _config { get; }

        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Connexion> Connexions { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<ArticlePage> ArticlePages { get; set; }
        public DbSet<Adherent> Adherents { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Parametres> Parametres { get; set; }

        public ColomiersVolleyContext()
        {
        }

        public ColomiersVolleyContext(DbContextOptions<ColomiersVolleyContext> options)
                : base(options)
        {
        }

        public ColomiersVolleyContext(DbContextOptions<ColomiersVolleyContext> options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string constring = "server=127.0.0.1;database=colomiers_volley;port=3306;uid=root;password=macmarny31";
                optionsBuilder.UseMySQL(constring);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Dates calendaires pures (pas de notion d'heure/fuseau) : DateOnly cote CLR,
            // colonne MySQL "datetime" inchangee (pas de migration de schema necessaire).
            var dateOnlyConverter = new ValueConverter<DateOnly?, DateTime?>(
                d => d.HasValue ? d.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                d => d.HasValue ? DateOnly.FromDateTime(d.Value) : (DateOnly?)null);

            modelBuilder.Entity<Adherent>(e =>
            {
                e.Property(a => a.BirthdayDate).HasConversion(dateOnlyConverter);
                e.Property(a => a.InscriptionDate).HasConversion(dateOnlyConverter);
                e.Property(a => a.HealthStatementDate).HasConversion(dateOnlyConverter);
                e.Property(a => a.CertificateDate).HasConversion(dateOnlyConverter);
            });

            modelBuilder.Entity<Order>(e =>
            {
                e.Property(o => o.DateNaissance).HasConversion(dateOnlyConverter);

                // Order.Date est le seul vrai timestamp (instant du paiement) : on force le Kind=Utc
                // a la lecture, car le driver MySQL renvoie systematiquement Kind=Unspecified, ce qui
                // empechait System.Text.Json d'emettre le suffixe "Z" attendu par le front.
                e.Property(o => o.Date).HasConversion(
                    d => d,
                    d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : (DateTime?)null);
            });
        }
    }
}