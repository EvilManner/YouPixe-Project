﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VideoProcessor.Application.Infrastructure;

#nullable disable

namespace VideoProcessor.Application.Migrations.TempDirectoryDb
{
    [DbContext(typeof(TempDirectoryDbContext))]
    partial class TempDirectoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.11");

            modelBuilder.Entity("VideoProcessor.Application.Infrastructure.TempDirectory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TempDirectories");
                });
#pragma warning restore 612, 618
        }
    }
}
