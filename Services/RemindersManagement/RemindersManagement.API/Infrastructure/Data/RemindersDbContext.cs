using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RemindersManagement.API.Domain.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RemindersManagement.API.Infrastructure.Data
{
    [ExcludeFromCodeCoverage]
    public class RemindersDbContext : DbContext
    {
        private const string DEFAULT_SCHEMA = "reminders";

        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Category> Categories { get; set; }

        public RemindersDbContext(DbContextOptions<RemindersDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API
            modelBuilder.Entity<Reminder>(ConfigureReminders);
            modelBuilder.Entity<Category>(ConfigureCategories);

            // Relation Define
            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.Category)
                .WithMany(l => l.Reminders)
                .HasForeignKey(r => r.CategoryId);

            // Data Seed
            Category defaultList = new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Default"
            };

            modelBuilder.Entity<Category>().HasData(
                defaultList
            );

            modelBuilder.Entity<Reminder>().HasData(
                new Reminder
                {
                    Id = Guid.NewGuid(),
                    Description = "Learning Microservices",
                    Status = ReminderStatus.Finished,
                    CategoryId = defaultList.Id
                },
                new Reminder
                {
                    Id = Guid.NewGuid(),
                    Description = "Writing Blog",
                    Status = ReminderStatus.Doing,
                    CategoryId = defaultList.Id
                },
                new Reminder
                {
                    Id = Guid.NewGuid(),
                    Description = "Presentation prepare",
                    Status = ReminderStatus.Doing,
                    CategoryId = defaultList.Id
                },
                new Reminder
                {
                    Id = Guid.NewGuid(),
                    Description = "5km Running",
                    Status = ReminderStatus.Doing,
                    CategoryId = defaultList.Id
                },
                new Reminder
                {
                    Id = Guid.NewGuid(),
                    Description = "Eating less this week",
                    Status = ReminderStatus.Doing,
                    CategoryId = defaultList.Id
                },
                new Reminder
                {
                    Id = Guid.NewGuid(),
                    Description = "Doing Gym 10kg",
                    Status = ReminderStatus.Doing,
                    CategoryId = defaultList.Id
                },
                new Reminder
                {
                    Id = Guid.NewGuid(),
                    Description = "Weekend Movie Watching",
                    Status = ReminderStatus.Doing,
                    CategoryId = defaultList.Id
                },
                new Reminder
                {
                    Id = Guid.NewGuid(),
                    Description = "Weekend Pinic",
                    Status = ReminderStatus.Doing,
                    CategoryId = defaultList.Id
                },
                new Reminder
                {
                    Id = Guid.NewGuid(),
                    Description = "Karata Class",
                    Status = ReminderStatus.Doing,
                    CategoryId = defaultList.Id
                },
                new Reminder
                {
                    Id = Guid.NewGuid(),
                    Description = "Weight reducing",
                    Status = ReminderStatus.Doing,
                    CategoryId = defaultList.Id
                },
                new Reminder
                {
                    Id = Guid.NewGuid(),
                    Description = "Dalat visit",
                    Status = ReminderStatus.Doing,
                    CategoryId = defaultList.Id
                }
            );
        }

        private void ConfigureReminders(EntityTypeBuilder<Reminder> reminderConfiguration)
        {
            reminderConfiguration.ToTable("Reminders", DEFAULT_SCHEMA);
            reminderConfiguration.HasKey(r => r.Id);
            reminderConfiguration.Property(r => r.Description).IsRequired();
            reminderConfiguration.Property(r => r.Status).HasColumnType("varchar(50)").IsRequired();
        }

        private void ConfigureCategories(EntityTypeBuilder<Category> categoryConfiguration)
        {
            categoryConfiguration.ToTable("Categories", DEFAULT_SCHEMA);
            categoryConfiguration.HasKey(l => l.Id);
            categoryConfiguration.Property(l => l.Name).HasColumnType("varchar(50)").IsRequired();
        }
    }
}