﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.Persistence.Tables.AdjustmentTable", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<Guid>("InventoryId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("INVENTORY_ID");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ORDER_ID");

                    b.Property<Guid?>("OrderItemId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ORDER_ITEM_ID");

                    b.Property<int>("Quantity")
                        .HasMaxLength(10)
                        .HasColumnType("int")
                        .HasColumnName("QUANTITY");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("REASON");

                    b.HasKey("Id")
                        .HasName("PK_ADJUSTMENT");

                    b.HasIndex("Id")
                        .HasDatabaseName("IX_ADJUSTMENT_ID");

                    b.HasIndex("InventoryId")
                        .HasDatabaseName("IX_ADJUSTMENT_INVENTORY_ID");

                    b.HasIndex("OrderId")
                        .HasDatabaseName("IX_ADJUSTMENT_ORDER_ID");

                    b.ToTable("ADJUSTMENT", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Persistence.Tables.InventoryTable", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("PRODUCT_ID");

                    b.Property<int>("Quantity")
                        .HasMaxLength(10)
                        .HasColumnType("int")
                        .HasColumnName("QUANTITY");

                    b.HasKey("Id")
                        .HasName("PK_INVENTORY");

                    b.HasIndex("Id")
                        .HasDatabaseName("IX_INVENTORY_ID");

                    b.HasIndex("ProductId")
                        .IsUnique()
                        .HasDatabaseName("IX_INVENTORY_PRODUCT_ID")
                        .HasFilter("[PRODUCT_ID] IS NOT NULL");

                    b.ToTable("INVENTORY", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Persistence.Tables.OrderItemTable", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ORDER_ID");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("PRODUCT_ID");

                    b.Property<Guid>("ReservationId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RESERVATION_ID");

                    b.ComplexProperty<Dictionary<string, object>>("OrderItemPrice", "Infrastructure.Persistence.Tables.OrderItemTable.OrderItemPrice#OrderItemPrice", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Quantity")
                                .HasMaxLength(10)
                                .HasColumnType("int")
                                .HasColumnName("QUANTITY");

                            b1.Property<decimal>("UnitPrice")
                                .HasColumnType("decimal(14,4)")
                                .HasColumnName("UNIT_PRICE");
                        });

                    b.HasKey("Id")
                        .HasName("PK_ORDER_ITEM");

                    b.HasIndex("Id")
                        .HasDatabaseName("IX_ORDER_ITEM_ID");

                    b.HasIndex("OrderId")
                        .HasDatabaseName("IX_ORDER_ITEM_ORDER_ID");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("IX_ORDER_ITEM_PRODUCT_ID");

                    b.ToTable("ORDER_ITEM", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Persistence.Tables.OrderTable", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<DateTime?>("CanceledDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("CANCELED_DATE");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("CREATED_DATE");

                    b.Property<string>("CustomerEmail")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("CUSTOMER_EMAIL");

                    b.Property<DateTime?>("PaidDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("PAID_DATE");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("STATUS");

                    b.HasKey("Id")
                        .HasName("PK_ORDER");

                    b.HasIndex("Id")
                        .HasDatabaseName("IX_ORDER_ID");

                    b.ToTable("ORDER", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Persistence.Tables.ProductTable", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<Guid?>("InventoryId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("INVENTORY_ID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("NAME");

                    b.HasKey("Id")
                        .HasName("PK_PRODUCT");

                    b.HasIndex("Id")
                        .HasDatabaseName("IX_PRODUCT_ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("IX_PRODUCT_NAME");

                    b.ToTable("PRODUCT", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Persistence.Tables.ReservationTable", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<Guid>("InventoryId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("INVENTORY_ID");

                    b.Property<Guid?>("OrderItemId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ORDER_ITEM_ID");

                    b.Property<int>("Quantity")
                        .HasMaxLength(10)
                        .HasColumnType("int")
                        .HasColumnName("QUANTITY");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("STATUS");

                    b.HasKey("Id")
                        .HasName("PK_RESERVATION");

                    b.HasIndex("Id")
                        .HasDatabaseName("IX_RESERVATION_ID");

                    b.HasIndex("InventoryId")
                        .HasDatabaseName("IX_RESERVATION_INVENTORY_ID");

                    b.HasIndex("OrderItemId")
                        .IsUnique()
                        .HasDatabaseName("IX_RESERVATION_ORDER_ITEM_ID")
                        .HasFilter("[ORDER_ITEM_ID] IS NOT NULL");

                    b.ToTable("RESERVATION", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Persistence.Tables.AdjustmentTable", b =>
                {
                    b.HasOne("Infrastructure.Persistence.Tables.InventoryTable", "Inventory")
                        .WithMany("Adjustments")
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_ADJUSTMENT_INVENTORY_INVENTORY_ID");

                    b.HasOne("Infrastructure.Persistence.Tables.OrderTable", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .HasConstraintName("FK_ADJUSTMENT_ORDER_ORDER_ID");

                    b.Navigation("Inventory");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Tables.InventoryTable", b =>
                {
                    b.HasOne("Infrastructure.Persistence.Tables.ProductTable", "Product")
                        .WithOne("Inventory")
                        .HasForeignKey("Infrastructure.Persistence.Tables.InventoryTable", "ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("FK_INVENTORY_PRODUCT_PRODUCT_ID");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Tables.OrderItemTable", b =>
                {
                    b.HasOne("Infrastructure.Persistence.Tables.OrderTable", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_ORDER_ITEM_ORDER_ORDER_ID");

                    b.HasOne("Infrastructure.Persistence.Tables.ProductTable", "Product")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_ORDER_ITEM_PRODUCT_PRODUCT_ID");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Tables.ReservationTable", b =>
                {
                    b.HasOne("Infrastructure.Persistence.Tables.InventoryTable", "Inventory")
                        .WithMany("Reservations")
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_RESERVATION_INVENTORY_INVENTORY_ID");

                    b.HasOne("Infrastructure.Persistence.Tables.OrderItemTable", "OrderItem")
                        .WithOne("Reservation")
                        .HasForeignKey("Infrastructure.Persistence.Tables.ReservationTable", "OrderItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("FK_RESERVATION_ORDER_ITEM_ORDER_ITEM_ID");

                    b.Navigation("Inventory");

                    b.Navigation("OrderItem");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Tables.InventoryTable", b =>
                {
                    b.Navigation("Adjustments");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Tables.OrderItemTable", b =>
                {
                    b.Navigation("Reservation")
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Persistence.Tables.OrderTable", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Infrastructure.Persistence.Tables.ProductTable", b =>
                {
                    b.Navigation("Inventory");

                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
