﻿// <auto-generated />
using System;
using AttendanceSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AttendanceSystem.Data.Migrations
{
    [DbContext(typeof(CoursesContext))]
    [Migration("20250304220013_RegisterToCollection")]
    partial class RegisterToCollection
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AttendanceSystem.Domain.Model.AttendanceRecord", b =>
                {
                    b.Property<Guid>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Record")
                        .HasColumnType("int");

                    b.HasKey("SessionId", "StudentId");

                    b.ToTable("AttendanceRecord");
                });

            modelBuilder.Entity("AttendanceSystem.Domain.Model.Course", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.PrimitiveCollection<string>("Students")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.PrimitiveCollection<string>("Teachers")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("AttendanceSystem.Domain.Model.Department", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("AttendanceSystem.Domain.Model.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("AttendanceSystem.Domain.Model.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Role").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("AttendanceSystem.Domain.Model.Administrator", b =>
                {
                    b.HasBaseType("AttendanceSystem.Domain.Model.User");

                    b.HasDiscriminator().HasValue("Administrator");
                });

            modelBuilder.Entity("AttendanceSystem.Domain.Model.Student", b =>
                {
                    b.HasBaseType("AttendanceSystem.Domain.Model.User");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("AttendanceSystem.Domain.Model.Teacher", b =>
                {
                    b.HasBaseType("AttendanceSystem.Domain.Model.User");

                    b.HasDiscriminator().HasValue("Teacher");
                });

            modelBuilder.Entity("AttendanceSystem.Domain.Model.AttendanceRecord", b =>
                {
                    b.HasOne("AttendanceSystem.Domain.Model.Session", null)
                        .WithMany("Register")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AttendanceSystem.Domain.Model.Course", b =>
                {
                    b.HasOne("AttendanceSystem.Domain.Model.User", null)
                        .WithMany("Courses")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("AttendanceSystem.Domain.Model.Session", b =>
                {
                    b.HasOne("AttendanceSystem.Domain.Model.Course", "Course")
                        .WithMany("Sessions")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("AttendanceSystem.Domain.Model.Course", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("AttendanceSystem.Domain.Model.Session", b =>
                {
                    b.Navigation("Register");
                });

            modelBuilder.Entity("AttendanceSystem.Domain.Model.User", b =>
                {
                    b.Navigation("Courses");
                });
#pragma warning restore 612, 618
        }
    }
}
