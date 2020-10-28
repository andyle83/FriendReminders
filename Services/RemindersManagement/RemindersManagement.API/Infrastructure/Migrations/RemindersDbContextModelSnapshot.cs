﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RemindersManagement.API.Infrastructure.Data;

namespace RemindersManagement.API.Infrastructure.Migrations
{
    [DbContext(typeof(RemindersDbContext))]
    partial class RemindersDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("RemindersManagement.API.Domain.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("HexaColor")
                        .HasColumnType("TEXT");

                    b.Property<string>("Icon")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Categories","reminders");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e76910df-ff41-483c-b284-0873b55a936b"),
                            Name = "Default"
                        });
                });

            modelBuilder.Entity("RemindersManagement.API.Domain.Models.Reminder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("RemiderTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Reminders","reminders");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8b5d82df-e621-4b9d-96a4-1c67301e5fa5"),
                            CategoryId = new Guid("e76910df-ff41-483c-b284-0873b55a936b"),
                            Description = "Learning Microservices",
                            Priority = 1,
                            Status = 1
                        },
                        new
                        {
                            Id = new Guid("7170f7b0-60d5-4f7e-a4f5-2cbdc0dd5d65"),
                            CategoryId = new Guid("e76910df-ff41-483c-b284-0873b55a936b"),
                            Description = "Writing Blog",
                            Priority = 1,
                            Status = 0
                        },
                        new
                        {
                            Id = new Guid("ab78c3ec-c7e9-44dd-8e58-ce40444a6d5c"),
                            CategoryId = new Guid("e76910df-ff41-483c-b284-0873b55a936b"),
                            Description = "Presentation prepare",
                            Priority = 1,
                            Status = 0
                        });
                });

            modelBuilder.Entity("RemindersManagement.API.Domain.Models.Reminder", b =>
                {
                    b.HasOne("RemindersManagement.API.Domain.Models.Category", "Category")
                        .WithMany("Reminders")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
