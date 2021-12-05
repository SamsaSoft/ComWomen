﻿using Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

            modelBuilder
                .Entity<Language>()
                .Property(e => e.Id)
                .HasConversion<int>();

            modelBuilder
                .Entity<Language>().HasData(
                    Enum.GetValues(typeof(LanguageEnum))
                        .Cast<LanguageEnum>()
                        .Select(e => new Language()
                        {
                            Id = e,
                            Name = new CultureInfo(e.ToString()).NativeName,
                            LanguageCode = new CultureInfo(e.ToString()).Name,
                            IsEnabled = true
                        })
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
