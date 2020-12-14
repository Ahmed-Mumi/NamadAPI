using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NomadAPI.Entities;
using System;

namespace NomadAPI.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole,
        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    //public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        //public DbSet<AppUser> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<UserReaction> UserReactions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Travel> Travels { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<TravelCity> TravelCities { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Continent> Continents { get; set; }
        public DbSet<CountryUser> CountryUsers { get; set; }
        public DbSet<CountryUserStatus> CountryUserStatuses { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LanguageUserStatus> LanguageUserStatuses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
               .HasMany(ur => ur.UserRoles)
               .WithOne(u => u.Role)
               .HasForeignKey(ur => ur.RoleId)
               .IsRequired();

            builder.Entity<UserReaction>()
                .HasKey(k => new { k.ReactedByUserId, k.ReactedUserId });

            builder.Entity<TravelCity>()
            .HasKey(k => new { k.CityId, k.TravelId });

            builder.Entity<UserReaction>()
                .HasOne(s => s.ReactedUser)
                .WithMany(r => r.ReactedUsers)
                .HasForeignKey(s => s.ReactedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserReaction>()
                .HasOne(s => s.ReactedByUser)
                .WithMany(r => r.ReactedByUsers)
                .HasForeignKey(s => s.ReactedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Message>()
               .HasOne(s => s.Recipient)
               .WithMany(r => r.MessagesReceived)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(s => s.Sender)
                .WithMany(r => r.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Application>()
               .HasKey(k => new { k.UserAppliedAdId, k.TravelId });

            builder.Entity<Application>()
               .HasOne(s => s.UserAppliedAd)
               .WithMany(g => g.Applications)
               .HasForeignKey(s => s.UserAppliedAdId)
               .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Application>()
               .HasOne(s => s.Travel)
               .WithMany(g => g.Applications)
               .HasForeignKey(s => s.TravelId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Report>()
            .HasOne(s => s.UserReported)
            .WithMany(r => r.UsersReported)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Report>()
                .HasOne(s => s.UserReports)
                .WithMany(r => r.UsersReports)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ApplyUtcDateTimeConverter();
        }
    }

    public static class UtcDateAnnotation
    {
        private const String IsUtcAnnotation = "IsUtc";
        private static readonly ValueConverter<DateTime, DateTime> UtcConverter =
          new ValueConverter<DateTime, DateTime>(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        private static readonly ValueConverter<DateTime?, DateTime?> UtcNullableConverter =
          new ValueConverter<DateTime?, DateTime?>(v => v, v => v == null ? v : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));

        public static PropertyBuilder<TProperty> IsUtc<TProperty>(this PropertyBuilder<TProperty> builder, Boolean isUtc = true) =>
          builder.HasAnnotation(IsUtcAnnotation, isUtc);

        public static Boolean IsUtc(this IMutableProperty property) =>
          ((Boolean?)property.FindAnnotation(IsUtcAnnotation)?.Value) ?? true;

        /// <summary>
        /// Make sure this is called after configuring all your entities.
        /// </summary>
        public static void ApplyUtcDateTimeConverter(this ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (!property.IsUtc())
                    {
                        continue;
                    }

                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(UtcConverter);
                    }

                    if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(UtcNullableConverter);
                    }
                }
            }
        }
    }
}
