﻿// <auto-generated />
using System;
using AppAvailabilityTracker.Services.AppStorage.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AppAvailabilityTracker.Services.AppStorage.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationStoreContext))]
    [Migration("20250223155515_SchemaChanges")]
    partial class SchemaChanges
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AppAvailabilityTracker.Services.AppStorage.Infrastructure.Models.Application", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("boolean")
                        .HasColumnName("is_available");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Store")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("store");

                    b.Property<string>("StoreLink")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("store_link");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("applications_pkey");

                    b.ToTable("applications", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
