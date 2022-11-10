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

        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }

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
            modelBuilder.Entity<Article>()
                .Property(p => p.Content)
                .HasColumnType("text");

            base.OnModelCreating(modelBuilder);
        }
    }
}