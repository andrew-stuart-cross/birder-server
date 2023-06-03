﻿// <auto-generated />
using System;
using Birder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Birder.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230603195008_InitialCreate")]
    [ExcludeFromCodeCoverageAttribute]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Birder.Data.Model.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("DefaultLocationLatitude")
                        .HasColumnType("float");

                    b.Property<double>("DefaultLocationLongitude")
                        .HasColumnType("float");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Birder.Data.Model.Bird", b =>
                {
                    b.Property<int>("BirdId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BirdId"));

                    b.Property<int>("BirderStatus")
                        .HasColumnType("int");

                    b.Property<string>("BtoStatusInBritain")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Class")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConservationStatusId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EnglishName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Family")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InternationalName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Order")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PopulationSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ThumbnailUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BirdId");

                    b.HasIndex("ConservationStatusId");

                    b.ToTable("Bird", (string)null);
                });

            modelBuilder.Entity("Birder.Data.Model.ConservationStatus", b =>
                {
                    b.Property<int>("ConservationStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConservationStatusId"));

                    b.Property<string>("ConservationList")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConservationListColourCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ConservationStatusId");

                    b.ToTable("ConservationStatus", (string)null);

                    b.HasData(
                        new
                        {
                            ConservationStatusId = 1,
                            ConservationList = "Red",
                            ConservationListColourCode = "Red",
                            CreationDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143),
                            Description = "Red is the highest conservation priority, with these species needing urgent action.  Species are placed on the Red-list if they meet one or more of the following criteria: are globally important, have declined historically, show recent severe decline, and have failed to recover from historic decline.",
                            LastUpdateDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143)
                        },
                        new
                        {
                            ConservationStatusId = 2,
                            ConservationList = "Amber",
                            ConservationListColourCode = "Yellow",
                            CreationDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143),
                            Description = "Amber is the second most critical group.  Species are placed on the Amber-list if they meet one or more of these criteria: are important in Europe, show recent moderate decline, show some recovery from historical decline, or occur in internationally important numbers, have a highly localised distribution or are important to the wider UK.",
                            LastUpdateDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143)
                        },
                        new
                        {
                            ConservationStatusId = 3,
                            ConservationList = "Green",
                            ConservationListColourCode = "Green",
                            CreationDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143),
                            Description = "Species on the green list are the least critical group.  These are species that occur regularly in the UK but do not qualify under the Red or Amber criteria.",
                            LastUpdateDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143)
                        },
                        new
                        {
                            ConservationStatusId = 4,
                            ConservationList = "Former breeder",
                            ConservationListColourCode = "Black",
                            CreationDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143),
                            Description = "This is species is a former breeder and was not was not assessed in the UK Birds of Conservation Concern 4.",
                            LastUpdateDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143)
                        },
                        new
                        {
                            ConservationStatusId = 5,
                            ConservationList = "Not assessed",
                            ConservationListColourCode = "Black",
                            CreationDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143),
                            Description = "This species was not assessed in the UK Birds of Conservation Concern 4.",
                            LastUpdateDate = new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143)
                        });
                });

            modelBuilder.Entity("Birder.Data.Model.Network", b =>
                {
                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FollowerId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ApplicationUserId", "FollowerId");

                    b.HasIndex("FollowerId");

                    b.ToTable("Network");
                });

            modelBuilder.Entity("Birder.Data.Model.Observation", b =>
                {
                    b.Property<int>("ObservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ObservationId"));

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("BirdId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HasPhotos")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastUpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ObservationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("SelectedPrivacyLevel")
                        .HasColumnType("int");

                    b.HasKey("ObservationId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("BirdId");

                    b.HasIndex("ObservationDateTime");

                    b.ToTable("Observation", (string)null);
                });

            modelBuilder.Entity("Birder.Data.Model.ObservationNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NoteType")
                        .HasColumnType("int");

                    b.Property<int?>("ObservationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ObservationId");

                    b.ToTable("ObservationNote", (string)null);
                });

            modelBuilder.Entity("Birder.Data.Model.ObservationPosition", b =>
                {
                    b.Property<int>("ObservationPositionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ObservationPositionId"));

                    b.Property<string>("FormattedAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<int>("ObservationId")
                        .HasColumnType("int");

                    b.Property<string>("ShortAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ObservationPositionId");

                    b.HasIndex("ObservationId")
                        .IsUnique();

                    b.ToTable("ObservationPosition");
                });

            modelBuilder.Entity("Birder.Data.Model.ObservationTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ObervationId")
                        .HasColumnType("int");

                    b.Property<int?>("ObservationId")
                        .HasColumnType("int");

                    b.Property<int?>("TagId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ObservationId");

                    b.HasIndex("TagId");

                    b.ToTable("ObservationTag", (string)null);
                });

            modelBuilder.Entity("Birder.Data.Model.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("TagId");

                    b.ToTable("Tag", (string)null);
                });

            modelBuilder.Entity("Birder.Data.Model.TweetDay", b =>
                {
                    b.Property<int>("TweetDayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TweetDayId"));

                    b.Property<int>("BirdId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DisplayDay")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastUpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SongUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TweetDayId");

                    b.HasIndex("BirdId");

                    b.ToTable("TweetDay", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Birder.Data.Model.Bird", b =>
                {
                    b.HasOne("Birder.Data.Model.ConservationStatus", "BirdConservationStatus")
                        .WithMany("Birds")
                        .HasForeignKey("ConservationStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BirdConservationStatus");
                });

            modelBuilder.Entity("Birder.Data.Model.Network", b =>
                {
                    b.HasOne("Birder.Data.Model.ApplicationUser", "ApplicationUser")
                        .WithMany("Followers")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Birder.Data.Model.ApplicationUser", "Follower")
                        .WithMany("Following")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Follower");
                });

            modelBuilder.Entity("Birder.Data.Model.Observation", b =>
                {
                    b.HasOne("Birder.Data.Model.ApplicationUser", "ApplicationUser")
                        .WithMany("Observations")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("Birder.Data.Model.Bird", "Bird")
                        .WithMany("Observations")
                        .HasForeignKey("BirdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Bird");
                });

            modelBuilder.Entity("Birder.Data.Model.ObservationNote", b =>
                {
                    b.HasOne("Birder.Data.Model.Observation", "Observation")
                        .WithMany("Notes")
                        .HasForeignKey("ObservationId");

                    b.Navigation("Observation");
                });

            modelBuilder.Entity("Birder.Data.Model.ObservationPosition", b =>
                {
                    b.HasOne("Birder.Data.Model.Observation", "Observation")
                        .WithOne("Position")
                        .HasForeignKey("Birder.Data.Model.ObservationPosition", "ObservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Observation");
                });

            modelBuilder.Entity("Birder.Data.Model.ObservationTag", b =>
                {
                    b.HasOne("Birder.Data.Model.Observation", "Observation")
                        .WithMany("ObservationTags")
                        .HasForeignKey("ObservationId");

                    b.HasOne("Birder.Data.Model.Tag", "Tag")
                        .WithMany("ObservationTags")
                        .HasForeignKey("TagId");

                    b.Navigation("Observation");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Birder.Data.Model.TweetDay", b =>
                {
                    b.HasOne("Birder.Data.Model.Bird", "Bird")
                        .WithMany("TweetDay")
                        .HasForeignKey("BirdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bird");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Birder.Data.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Birder.Data.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Birder.Data.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Birder.Data.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Birder.Data.Model.ApplicationUser", b =>
                {
                    b.Navigation("Followers");

                    b.Navigation("Following");

                    b.Navigation("Observations");
                });

            modelBuilder.Entity("Birder.Data.Model.Bird", b =>
                {
                    b.Navigation("Observations");

                    b.Navigation("TweetDay");
                });

            modelBuilder.Entity("Birder.Data.Model.ConservationStatus", b =>
                {
                    b.Navigation("Birds");
                });

            modelBuilder.Entity("Birder.Data.Model.Observation", b =>
                {
                    b.Navigation("Notes");

                    b.Navigation("ObservationTags");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("Birder.Data.Model.Tag", b =>
                {
                    b.Navigation("ObservationTags");
                });
#pragma warning restore 612, 618
        }
    }
}
