﻿// <auto-generated />
using System;
using BooksCommand.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BooksCommand.Migrations
{
    [DbContext(typeof(BooksDbContext))]
    partial class BooksDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BooksCommand.Persistence.Datamodels.BookOutBoxDataModel", b =>
                {
                    b.Property<int>("StreamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StreamId"));

                    b.Property<Guid>("BookId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EventType")
                        .HasColumnType("integer");

                    b.Property<bool>("IsReserved")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("StreamId");

                    b.ToTable("OutBoxModels");
                });

            modelBuilder.Entity("BooksCommand.Persistence.Datamodels.BookWriteDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsReserved")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
