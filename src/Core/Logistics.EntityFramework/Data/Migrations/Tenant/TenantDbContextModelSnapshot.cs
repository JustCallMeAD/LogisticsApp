﻿// <auto-generated />
using System;
using Logistics.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Logistics.EntityFramework.Data.Migrations.Tenant
{
    [DbContext(typeof(TenantDbContext))]
    partial class TenantDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Logistics.Domain.Entities.Cargo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("AssignedDispatcherId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("AssignedTruckId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Destination")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("PickUpDate")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("PricePerMile")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Source")
                        .HasColumnType("longtext");

                    b.Property<double>("TotalTripMiles")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("AssignedDispatcherId");

                    b.HasIndex("AssignedTruckId");

                    b.ToTable("cargoes", (string)null);
                });

            modelBuilder.Entity("Logistics.Domain.Entities.Employee", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ExternalId")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("JoinedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("employees", (string)null);
                });

            modelBuilder.Entity("Logistics.Domain.Entities.Truck", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DriverId")
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("TruckNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DriverId")
                        .IsUnique();

                    b.ToTable("trucks", (string)null);
                });

            modelBuilder.Entity("Logistics.Domain.Entities.Cargo", b =>
                {
                    b.HasOne("Logistics.Domain.Entities.Employee", "AssignedDispatcher")
                        .WithMany("DispatcherCargoes")
                        .HasForeignKey("AssignedDispatcherId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Logistics.Domain.Entities.Truck", "AssignedTruck")
                        .WithMany("Cargoes")
                        .HasForeignKey("AssignedTruckId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.OwnsOne("Logistics.Domain.ValueObjects.CargoStatus", "Status", b1 =>
                        {
                            b1.Property<string>("CargoId")
                                .HasColumnType("varchar(255)");

                            b1.Property<int>("Id")
                                .HasColumnType("int");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.HasKey("CargoId");

                            b1.ToTable("cargoes");

                            b1.WithOwner()
                                .HasForeignKey("CargoId");
                        });

                    b.Navigation("AssignedDispatcher");

                    b.Navigation("AssignedTruck");

                    b.Navigation("Status")
                        .IsRequired();
                });

            modelBuilder.Entity("Logistics.Domain.Entities.Employee", b =>
                {
                    b.OwnsOne("Logistics.Domain.ValueObjects.EmployeeRole", "Role", b1 =>
                        {
                            b1.Property<string>("EmployeeId")
                                .HasColumnType("varchar(255)");

                            b1.Property<int>("Id")
                                .HasColumnType("int");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.HasKey("EmployeeId");

                            b1.ToTable("employees");

                            b1.WithOwner()
                                .HasForeignKey("EmployeeId");
                        });

                    b.Navigation("Role")
                        .IsRequired();
                });

            modelBuilder.Entity("Logistics.Domain.Entities.Truck", b =>
                {
                    b.HasOne("Logistics.Domain.Entities.Employee", "Driver")
                        .WithOne()
                        .HasForeignKey("Logistics.Domain.Entities.Truck", "DriverId");

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("Logistics.Domain.Entities.Employee", b =>
                {
                    b.Navigation("DispatcherCargoes");
                });

            modelBuilder.Entity("Logistics.Domain.Entities.Truck", b =>
                {
                    b.Navigation("Cargoes");
                });
#pragma warning restore 612, 618
        }
    }
}
