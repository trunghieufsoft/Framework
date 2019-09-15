﻿// <auto-generated />
using System;
using DataAccess.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(DataDbContext))]
    [Migration("20190812031014_Initial-Database")]
    partial class InitialDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataAccess.Entity.SystemConfiguration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<string>("CreatedBy")
                        .HasColumnName("CREATED_USER")
                        .HasMaxLength(2048);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnName("CREATED_TM");

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("DELETED");

                    b.Property<string>("KeyStr")
                        .HasColumnName("KEY")
                        .HasMaxLength(2048);

                    b.Property<string>("LastModifiedBy")
                        .HasColumnName("LAST_MDF_USER")
                        .HasMaxLength(2048);

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnName("LAST_MDF_TM");

                    b.Property<string>("Value")
                        .HasColumnName("VALUE")
                        .HasMaxLength(2048);

                    b.Property<string>("ValueUnit")
                        .HasColumnName("VALUE_UNIT")
                        .HasMaxLength(2048);

                    b.HasKey("Id");

                    b.ToTable("SYS_CONFIG");
                });

            modelBuilder.Entity("DataAccess.Entity.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnName("ADDRESS")
                        .HasMaxLength(2048);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnName("CODE")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnName("CREATED_USER")
                        .HasMaxLength(2048);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnName("CREATED_TM");

                    b.Property<string>("Email")
                        .HasColumnName("EMAIL")
                        .HasMaxLength(2048);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnName("FULL_NAME")
                        .HasMaxLength(2048);

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("DELETED");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnName("LAST_MDF_USER")
                        .HasMaxLength(2048);

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnName("LAST_MDF_TM");

                    b.Property<int?>("LoginFailedNumber")
                        .HasColumnName("LOGIN_FAILED_NR");

                    b.Property<DateTime?>("LoginTime")
                        .HasColumnName("LOGIN_TM");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("PASSWORD")
                        .HasMaxLength(1024);

                    b.Property<DateTime?>("PasswordLastUdt")
                        .HasColumnName("PASSWORD_LAST_UDT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnName("PHONE")
                        .HasMaxLength(128);

                    b.Property<string>("StatusStr")
                        .IsRequired()
                        .HasColumnName("STATUS")
                        .HasMaxLength(2048);

                    b.Property<string>("SubcriseToken")
                        .HasColumnName("SUBCRISE_TOKEN")
                        .HasMaxLength(2048);

                    b.Property<string>("Token")
                        .HasColumnName("TOKEN")
                        .HasMaxLength(2048);

                    b.Property<DateTime?>("TokenExpiredDate")
                        .HasColumnName("TOKEN_EXPIRED_DT");

                    b.Property<string>("UserTypeStr")
                        .IsRequired()
                        .HasColumnName("USER_TYP")
                        .HasMaxLength(2048);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnName("USERNAME")
                        .HasMaxLength(2048);

                    b.HasKey("Id");

                    b.ToTable("USER");
                });
#pragma warning restore 612, 618
        }
    }
}
