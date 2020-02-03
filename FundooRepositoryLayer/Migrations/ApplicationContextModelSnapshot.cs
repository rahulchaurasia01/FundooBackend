﻿// <auto-generated />
using System;
using FundooRepositoryLayer.ModelContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FundooRepositoryLayer.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FundooCommonLayer.ModelDB.LabelDetails", b =>
                {
                    b.Property<int>("LabelId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("LabelId");

                    b.ToTable("LabelDetails");
                });

            modelBuilder.Entity("FundooCommonLayer.ModelDB.NotesDetails", b =>
                {
                    b.Property<int>("NotesId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Color");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<bool>("IsArchived");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPin");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<DateTime?>("Reminder");

                    b.Property<string>("Title");

                    b.Property<int>("UserId");

                    b.HasKey("NotesId");

                    b.ToTable("NotesDetails");
                });

            modelBuilder.Entity("FundooCommonLayer.ModelDB.NotesLabel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("LabelId");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<int>("NotesId");

                    b.HasKey("Id");

                    b.HasIndex("LabelId");

                    b.HasIndex("NotesId");

                    b.ToTable("NotesLabel");
                });

            modelBuilder.Entity("FundooCommonLayer.ModelDB.NotificationDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("Token");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("NotificationDetails");
                });

            modelBuilder.Entity("FundooCommonLayer.ModelDB.UserDetails", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("EmailId")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Type");

                    b.Property<string>("UserRole")
                        .IsRequired();

                    b.HasKey("UserId");

                    b.HasIndex("EmailId")
                        .IsUnique();

                    b.ToTable("UserDetails");
                });

            modelBuilder.Entity("FundooCommonLayer.ModelDB.UsersNotes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<int>("NoteId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UsersNotes");
                });

            modelBuilder.Entity("FundooCommonLayer.ModelDB.NotesLabel", b =>
                {
                    b.HasOne("FundooCommonLayer.ModelDB.LabelDetails")
                        .WithMany("Notes")
                        .HasForeignKey("LabelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FundooCommonLayer.ModelDB.NotesDetails")
                        .WithMany("Labels")
                        .HasForeignKey("NotesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
