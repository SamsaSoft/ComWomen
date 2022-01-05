using Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Core.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        internal DbSet<Media> Medias { get; set; }
        internal DbSet<MediaTranslation> MediaTranslations { get; set; }
        internal DbSet<News> News { get; set; }
        internal DbSet<NewsTranslation> NewsTranslations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Media>()
                .Property(e => e.MediaType)
                .HasConversion<int>();

            modelBuilder
                .Entity<Media>()
                .HasMany(e => e.Translations)
                .WithOne(o => o.Media)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<MediaTranslation>()
                .HasQueryFilter(x => Settings.ActiveLanguages.Contains(x.Language));

            modelBuilder
                .Entity<News>()
                .HasMany(e => e.Translations)
                .WithOne(o => o.News)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<NewsTranslation>()
                .HasQueryFilter(x => Settings.ActiveLanguages.Contains(x.Language));
                       
            base.OnModelCreating(modelBuilder);
        }
    }
}
