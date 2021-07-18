﻿// <auto-generated />
using System;
using CustomResourceServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CustomResourceServer.Migrations
{
    [DbContext(typeof(DataEventRecordContext))]
    [Migration("20210718104131_dbdataeventrecord")]
    partial class dbdataeventrecord
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.16");

            modelBuilder.Entity("CustomResourceServer.Models.DataEventRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DataEventRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
