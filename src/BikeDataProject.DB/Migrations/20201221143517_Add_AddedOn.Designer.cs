﻿// <auto-generated />
using System;
using BikeDataProject.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BikeDataProject.DB.Migrations
{
    [DbContext(typeof(BikeDataDbContext))]
    [Migration("20201221143517_Add_AddedOn")]
    partial class Add_AddedOn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("BikeDataProject.DB.Contribution", b =>
                {
                    b.Property<int>("ContributionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime?>("AddedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Distance")
                        .HasColumnType("integer");

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<byte[]>("PointsGeom")
                        .HasColumnType("bytea");

                    b.Property<DateTime[]>("PointsTime")
                        .HasColumnType("timestamp without time zone[]");

                    b.Property<DateTime>("TimeStampStart")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("TimeStampStop")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserAgent")
                        .HasColumnType("text");

                    b.HasKey("ContributionId");

                    b.ToTable("Contributions");
                });

            modelBuilder.Entity("BikeDataProject.DB.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("AccessToken")
                        .HasColumnType("text");

                    b.Property<int>("ExpiresAt")
                        .HasColumnType("integer");

                    b.Property<int>("ExpiresIn")
                        .HasColumnType("integer");

                    b.Property<bool>("IsHistoryFetched")
                        .HasColumnType("boolean");

                    b.Property<string>("Provider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderUser")
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTime>("TokenCreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("UserIdentifier")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BikeDataProject.DB.UserContribution", b =>
                {
                    b.Property<int>("UserContributionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("ContributionId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("UserContributionId");

                    b.HasIndex("UserId");

                    b.ToTable("UserContributions");
                });

            modelBuilder.Entity("BikeDataProject.DB.UserContribution", b =>
                {
                    b.HasOne("BikeDataProject.DB.User", null)
                        .WithMany("UserContributions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BikeDataProject.DB.User", b =>
                {
                    b.Navigation("UserContributions");
                });
#pragma warning restore 612, 618
        }
    }
}
