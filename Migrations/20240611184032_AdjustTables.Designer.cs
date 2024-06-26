﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SistemaHoteis.Data;

#nullable disable

namespace SistemaHoteis.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240611184032_AdjustTables")]
    partial class AdjustTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SistemaHoteis.Models.CheckIn", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataCheckin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataCheckout")
                        .HasColumnType("datetime2");

                    b.Property<int>("HospedeId")
                        .HasColumnType("int");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("HospedeId");

                    b.HasIndex("HotelId");

                    b.HasIndex("Id", "HotelId")
                        .IsUnique();

                    b.ToTable("Checkins");
                });

            modelBuilder.Entity("SistemaHoteis.Models.Hospede", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("Id");

                    b.ToTable("Hospedes");
                });

            modelBuilder.Entity("SistemaHoteis.Models.Hotel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Endereco")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("NumeroDeQuartos")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Hoteis");
                });

            modelBuilder.Entity("SistemaHoteis.Models.CheckIn", b =>
                {
                    b.HasOne("SistemaHoteis.Models.Hospede", "Hospede")
                        .WithMany("Checkins")
                        .HasForeignKey("HospedeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SistemaHoteis.Models.Hotel", "Hotel")
                        .WithMany("Checkins")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hospede");

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("SistemaHoteis.Models.Hospede", b =>
                {
                    b.Navigation("Checkins");
                });

            modelBuilder.Entity("SistemaHoteis.Models.Hotel", b =>
                {
                    b.Navigation("Checkins");
                });
#pragma warning restore 612, 618
        }
    }
}
