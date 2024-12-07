﻿// <auto-generated />
using System;
using Library.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Library.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Library.Domain.Models.Book", b =>
                {
                    b.Property<Guid>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UploadedBy")
                        .HasColumnType("uuid");

                    b.HasKey("BookId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Library.Domain.Models.Department", b =>
                {
                    b.Property<Guid>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            DepartmentId = new Guid("731d0de2-0da9-4ae4-a1f3-64e4804a5483"),
                            Description = "Department of Computer Science",
                            Name = "Computer Science"
                        },
                        new
                        {
                            DepartmentId = new Guid("dff29960-cba9-4edc-bb7c-6315c1d194c8"),
                            Description = "Department of Mathematics",
                            Name = "Mathematics"
                        });
                });

            modelBuilder.Entity("Library.Domain.Models.Lecture", b =>
                {
                    b.Property<Guid>("LectureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UploadedBy")
                        .HasColumnType("uuid");

                    b.HasKey("LectureId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Lectures");
                });

            modelBuilder.Entity("Library.Domain.Models.NotificationModel", b =>
                {
                    b.Property<Guid>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RecipientUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("NotificationId");

                    b.HasIndex("RecipientUserId");

                    b.HasIndex("SenderId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Library.Domain.Models.Student", b =>
                {
                    b.Property<Guid>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("StudentId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Students");

                    b.HasData(
                        new
                        {
                            StudentId = new Guid("f44ed9a6-19ac-459a-91d6-ff061724243e"),
                            UserId = new Guid("5dad3994-0944-409c-8e67-f84b04c05d4b")
                        });
                });

            modelBuilder.Entity("Library.Domain.Models.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("Library.Domain.Models.Teacher", b =>
                {
                    b.Property<Guid>("TeacherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("TeacherId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Teachers");

                    b.HasData(
                        new
                        {
                            TeacherId = new Guid("354168b3-0dee-4c5d-9c70-71aa2b0c002a"),
                            UserId = new Guid("916c0887-96fc-4789-aeec-74f29530f098")
                        });
                });

            modelBuilder.Entity("Library.Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("fce4e50f-8a74-4747-9e43-f5f8e0684fd5"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "e1a2a0d0-3be5-4c9f-811c-0f402155b9ab",
                            Email = "admin@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@GMAIL.COM",
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAIAAYagAAAAEB06+sY86pJ8aS/cc9CPo9ut/NBhGXU6rZO/YXvY33qmZqz2L97P27e13UvDnGx+7Q==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "40bbc720-20d4-4623-bc2d-f5e9cec4446d",
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        },
                        new
                        {
                            Id = new Guid("916c0887-96fc-4789-aeec-74f29530f098"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "e9322210-cb9d-4a38-98bf-e2ce2c0bee4e",
                            Email = "teacher@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "TEACHER@GMAIL.COM",
                            NormalizedUserName = "TEACHER",
                            PasswordHash = "AQAAAAIAAYagAAAAEB06+sY86pJ8aS/cc9CPo9ut/NBhGXU6rZO/YXvY33qmZqz2L97P27e13UvDnGx+7Q==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "143c53bc-ae07-403c-b2ea-d53f2dad444f",
                            TwoFactorEnabled = false,
                            UserName = "teacher"
                        },
                        new
                        {
                            Id = new Guid("5dad3994-0944-409c-8e67-f84b04c05d4b"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "b3f68e0a-4457-4fbf-bd31-f582a35e4174",
                            Email = "student@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "STUDENT@GMAIL.COM",
                            NormalizedUserName = "STUDENT",
                            PasswordHash = "AQAAAAIAAYagAAAAEB06+sY86pJ8aS/cc9CPo9ut/NBhGXU6rZO/YXvY33qmZqz2L97P27e13UvDnGx+7Q==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "3aedc7b0-1ee1-4774-a612-cf5a7bf8dd6e",
                            TwoFactorEnabled = false,
                            UserName = "student"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("339a6c4b-6d1a-405e-98ce-6d54b41af0b0"),
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = new Guid("f2a4f1fc-70e8-4b9a-bd8c-ce330a97128c"),
                            Name = "teacher",
                            NormalizedName = "TEACHER"
                        },
                        new
                        {
                            Id = new Guid("b2c0218a-f04b-47cf-8564-528baeaaa465"),
                            Name = "student",
                            NormalizedName = "STUDENT"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = new Guid("fce4e50f-8a74-4747-9e43-f5f8e0684fd5"),
                            RoleId = new Guid("339a6c4b-6d1a-405e-98ce-6d54b41af0b0")
                        },
                        new
                        {
                            UserId = new Guid("916c0887-96fc-4789-aeec-74f29530f098"),
                            RoleId = new Guid("f2a4f1fc-70e8-4b9a-bd8c-ce330a97128c")
                        },
                        new
                        {
                            UserId = new Guid("5dad3994-0944-409c-8e67-f84b04c05d4b"),
                            RoleId = new Guid("b2c0218a-f04b-47cf-8564-528baeaaa465")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("StudentSubject", b =>
                {
                    b.Property<Guid>("StudentsStudentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubjectsId")
                        .HasColumnType("uuid");

                    b.HasKey("StudentsStudentId", "SubjectsId");

                    b.HasIndex("SubjectsId");

                    b.ToTable("StudentSubject");
                });

            modelBuilder.Entity("Library.Domain.Models.Book", b =>
                {
                    b.HasOne("Library.Domain.Models.Subject", "Subject")
                        .WithMany("Books")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Library.Domain.Models.Lecture", b =>
                {
                    b.HasOne("Library.Domain.Models.Subject", "Subject")
                        .WithMany("Lectures")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Library.Domain.Models.NotificationModel", b =>
                {
                    b.HasOne("Library.Domain.Models.User", "RecipientUser")
                        .WithMany("ReceivedNotifications")
                        .HasForeignKey("RecipientUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Library.Domain.Models.User", "SenderUser")
                        .WithMany("SentNotifications")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("RecipientUser");

                    b.Navigation("SenderUser");
                });

            modelBuilder.Entity("Library.Domain.Models.Student", b =>
                {
                    b.HasOne("Library.Domain.Models.User", "User")
                        .WithOne("Student")
                        .HasForeignKey("Library.Domain.Models.Student", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Library.Domain.Models.Subject", b =>
                {
                    b.HasOne("Library.Domain.Models.Department", "Department")
                        .WithMany("Subjects")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.Domain.Models.Teacher", "Teacher")
                        .WithMany("Subjects")
                        .HasForeignKey("TeacherId");

                    b.Navigation("Department");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Library.Domain.Models.Teacher", b =>
                {
                    b.HasOne("Library.Domain.Models.User", "User")
                        .WithOne("Teacher")
                        .HasForeignKey("Library.Domain.Models.Teacher", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Library.Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Library.Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Library.Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StudentSubject", b =>
                {
                    b.HasOne("Library.Domain.Models.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentsStudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.Domain.Models.Subject", null)
                        .WithMany()
                        .HasForeignKey("SubjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Library.Domain.Models.Department", b =>
                {
                    b.Navigation("Subjects");
                });

            modelBuilder.Entity("Library.Domain.Models.Subject", b =>
                {
                    b.Navigation("Books");

                    b.Navigation("Lectures");
                });

            modelBuilder.Entity("Library.Domain.Models.Teacher", b =>
                {
                    b.Navigation("Subjects");
                });

            modelBuilder.Entity("Library.Domain.Models.User", b =>
                {
                    b.Navigation("ReceivedNotifications");

                    b.Navigation("SentNotifications");

                    b.Navigation("Student")
                        .IsRequired();

                    b.Navigation("Teacher")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
