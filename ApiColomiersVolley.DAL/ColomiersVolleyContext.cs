using ApiColomiersVolley.DAL.Entities;
using Microsoft.EntityFrameworkCore;
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
        }
    }
}