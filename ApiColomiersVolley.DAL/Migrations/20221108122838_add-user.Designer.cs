﻿// <auto-generated />
using System;
using ApiColomiersVolley.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApiColomiersVolley.DAL.Migrations
{
    [DbContext(typeof(ColomiersVolleyContext))]
    [Migration("20221108122838_add-user")]
    partial class adduser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("ApiColomiersVolley.DAL.Entities.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Author")
                        .HasColumnType("varchar(250)")
                        .HasColumnName("author");

                    b.Property<string>("Content")
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime")
                        .HasColumnName("date");

                    b.Property<string>("Link")
                        .HasColumnType("varchar(500)")
                        .HasColumnName("link");

                    b.Property<string>("Title")
                        .HasColumnType("varchar(250)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("article");
                });

            modelBuilder.Entity("ApiColomiersVolley.DAL.Entities.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Admin")
                        .HasColumnType("varchar(250)")
                        .HasColumnName("admin");

                    b.Property<string>("ExpireDate")
                        .HasColumnType("varchar(250)")
                        .HasColumnName("expireDate");

                    b.Property<string>("Mail")
                        .HasColumnType("varchar(250)")
                        .HasColumnName("mail");

                    b.Property<string>("Nom")
                        .HasColumnType("varchar(250)")
                        .HasColumnName("nom");

                    b.Property<string>("Prenom")
                        .HasColumnType("varchar(250)")
                        .HasColumnName("prenom");

                    b.HasKey("IdUser");

                    b.ToTable("user");
                });
#pragma warning restore 612, 618
        }
    }
}
