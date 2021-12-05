using Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        internal DbSet<MediaType> MediaTypes { get; set; }
        internal DbSet<Media> Medias { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
              .Entity<Media>()
              .Property(e => e.MediaTypeId)
              .HasConversion<int>();

            modelBuilder
                .Entity<MediaType>()
                .Property(e => e.Id)
                .HasConversion<int>();

            modelBuilder
                .Entity<MediaType>().HasData(
                    Enum.GetValues(typeof(MediaTypeEnum))
                        .Cast<MediaTypeEnum>()
                        .Select(e => new MediaType()
                        {
                            Id = e,
                            Name = e.ToString(),
                            IsEnabled = true
                        })
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
