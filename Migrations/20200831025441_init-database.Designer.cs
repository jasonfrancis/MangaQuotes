﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using mangaQuotes.Data;

namespace mangaQuotes.Migrations
{
    [DbContext(typeof(QuoteDbContext))]
    [Migration("20200831025441_init-database")]
    partial class initdatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7");

            modelBuilder.Entity("mangaQuotes.Data.Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("mangaQuotes.Data.Quote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Chapter")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CharacterId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Page")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ParentQuoteId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("ParentQuoteId");

                    b.ToTable("Quotes");
                });

            modelBuilder.Entity("mangaQuotes.Data.Quote", b =>
                {
                    b.HasOne("mangaQuotes.Data.Character", "Character")
                        .WithMany()
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mangaQuotes.Data.Quote", "ParentQuote")
                        .WithMany()
                        .HasForeignKey("ParentQuoteId");
                });
#pragma warning restore 612, 618
        }
    }
}
