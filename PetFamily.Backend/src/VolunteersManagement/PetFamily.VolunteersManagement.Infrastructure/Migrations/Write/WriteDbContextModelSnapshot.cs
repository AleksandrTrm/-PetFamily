﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetFamily.VolunteersManagement.Infrastructure.DbContexts;

#nullable disable

namespace PetFamily.VolunteersManagement.Infrastructure.Migrations.Write
{
    [DbContext(typeof(WriteDbContext))]
    partial class WriteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PetFamily.VolunteersManagement.Domain.AggregateRoot.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("Experience")
                        .HasColumnType("integer")
                        .HasColumnName("experience");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Requisites")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("requisites");

                    b.Property<string>("SocialMedias")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("social_medias");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "PetFamily.VolunteersManagement.Domain.AggregateRoot.Volunteer.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(3000)
                                .HasColumnType("character varying(3000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("FullName", "PetFamily.VolunteersManagement.Domain.AggregateRoot.Volunteer.FullName#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("name");

                            b1.Property<string>("Patronymic")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("character varying(150)")
                                .HasColumnName("patronymic");

                            b1.Property<string>("Surname")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("character varying(150)")
                                .HasColumnName("surname");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "PetFamily.VolunteersManagement.Domain.AggregateRoot.Volunteer.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(11)
                                .HasColumnType("character varying(11)")
                                .HasColumnName("phone_number");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("volunteers", (string)null);
                });

            modelBuilder.Entity("PetFamily.VolunteersManagement.Domain.Entities.Pets.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_of_birth");

                    b.Property<double>("Height")
                        .HasColumnType("double precision")
                        .HasColumnName("height");

                    b.Property<bool>("IsCastrated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_castrated");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vaccinated");

                    b.Property<string>("PetPhotos")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("pet_photos");

                    b.Property<string>("Requisites")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("requisites");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<Guid>("VolunteerId")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision")
                        .HasColumnName("weight");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "PetFamily.VolunteersManagement.Domain.Entities.Pets.Pet.Address#Address", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("District")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("character varying(150)")
                                .HasColumnName("address_district");

                            b1.Property<string>("House")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("character varying(150)")
                                .HasColumnName("address_house");

                            b1.Property<string>("Settlement")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("character varying(150)")
                                .HasColumnName("address_settlement");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("character varying(150)")
                                .HasColumnName("address_street");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Color", "PetFamily.VolunteersManagement.Domain.Entities.Pets.Pet.Color#Color", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("color_value");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Description", "PetFamily.VolunteersManagement.Domain.Entities.Pets.Pet.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(3000)
                                .HasColumnType("character varying(3000)")
                                .HasColumnName("description_value");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("HealthInfo", "PetFamily.VolunteersManagement.Domain.Entities.Pets.Pet.HealthInfo#HealthInfo", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("character varying(200)")
                                .HasColumnName("health_info_value");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Nickname", "PetFamily.VolunteersManagement.Domain.Entities.Pets.Pet.Nickname#Nickname", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("character varying(150)")
                                .HasColumnName("nickname_value");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("OwnerPhone", "PetFamily.VolunteersManagement.Domain.Entities.Pets.Pet.OwnerPhone#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(11)
                                .HasColumnType("character varying(11)")
                                .HasColumnName("owner_phone_value");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("SerialNumber", "PetFamily.VolunteersManagement.Domain.Entities.Pets.Pet.SerialNumber#SerialNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("serial_number_value");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("SpeciesBreed", "PetFamily.VolunteersManagement.Domain.Entities.Pets.Pet.SpeciesBreed#SpeciesBreed", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("BreedId")
                                .HasColumnType("uuid")
                                .HasColumnName("species_breed_breed_id");

                            b1.Property<Guid>("SpeciesId")
                                .HasColumnType("uuid")
                                .HasColumnName("species_breed_species_id");
                        });

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.HasIndex("VolunteerId")
                        .HasDatabaseName("ix_pets_volunteer_id");

                    b.ToTable("pets", (string)null);
                });

            modelBuilder.Entity("PetFamily.VolunteersManagement.Domain.Entities.Pets.Pet", b =>
                {
                    b.HasOne("PetFamily.VolunteersManagement.Domain.AggregateRoot.Volunteer", null)
                        .WithMany("Pets")
                        .HasForeignKey("VolunteerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_pets_volunteers_volunteer_id");
                });

            modelBuilder.Entity("PetFamily.VolunteersManagement.Domain.AggregateRoot.Volunteer", b =>
                {
                    b.Navigation("Pets");
                });
#pragma warning restore 612, 618
        }
    }
}
