﻿// <auto-generated />
using System;
using Isaac_SensorSettingService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Isaac_SensorSettingService.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20201122193403_TabelGroupsAdded")]
    partial class TabelGroupsAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Isaac_SensorSettingService.Models.SensorGroupModel", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Floor")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("GroupId");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("Isaac_SensorSettingService.Models.SensorModel", b =>
                {
                    b.Property<string>("Floor")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Floor", "X", "Y");

                    b.HasIndex("GroupId");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("Isaac_SensorSettingService.Models.SettingsModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ExpirationTime")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("KeepData")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("RefreshRate")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("SensorSettings");
                });

            modelBuilder.Entity("Isaac_SensorSettingService.Models.SensorModel", b =>
                {
                    b.HasOne("Isaac_SensorSettingService.Models.SensorGroupModel", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");
                });
#pragma warning restore 612, 618
        }
    }
}
