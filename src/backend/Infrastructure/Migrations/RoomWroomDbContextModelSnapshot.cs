﻿// <auto-generated />
using System;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(RoomWroomDbContext))]
    partial class RoomWroomDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.ReceiptAggregate.Receipt", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Qr")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Receipts", (string)null);
                });

            modelBuilder.Entity("Domain.ReceiptAggregate.ValueObjects.ShopItemAssociation", b =>
                {
                    b.Property<string>("OriginalName")
                        .HasColumnType("text");

                    b.Property<Guid>("ShopItemId")
                        .HasColumnType("uuid");

                    b.HasKey("OriginalName");

                    b.ToTable("ShopItemAssociation", (string)null);
                });

            modelBuilder.Entity("Domain.RoomAggregate.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<decimal>("BudgetLowerBound")
                        .HasColumnType("numeric");

                    b.Property<bool>("MoneyRoundingRequired")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Rooms", (string)null);
                });

            modelBuilder.Entity("Domain.ReceiptAggregate.Receipt", b =>
                {
                    b.OwnsMany("Domain.ReceiptAggregate.Receipt.Items#Domain.ReceiptAggregate.ValueObjects.ReceiptItem", "Items", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("ReceiptId")
                                .HasColumnType("uuid");

                            b1.Property<Guid?>("AssociatedShopItemId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<decimal>("Quantity")
                                .HasColumnType("numeric");

                            b1.HasKey("Id", "ReceiptId");

                            b1.HasIndex("ReceiptId");

                            b1.ToTable("ReceiptItems", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ReceiptId");

                            b1.OwnsOne("Domain.ReceiptAggregate.Receipt.Items#Domain.ReceiptAggregate.ValueObjects.ReceiptItem.Price#Domain.Common.ValueObjects.Money", "Price", b2 =>
                                {
                                    b2.Property<int>("ReceiptItemId")
                                        .HasColumnType("integer");

                                    b2.Property<Guid>("ReceiptItemReceiptId")
                                        .HasColumnType("uuid");

                                    b2.HasKey("ReceiptItemId", "ReceiptItemReceiptId");

                                    b2.ToTable("ReceiptItems", (string)null);

                                    b2.WithOwner()
                                        .HasForeignKey("ReceiptItemId", "ReceiptItemReceiptId");
                                });

                            b1.OwnsOne("Domain.ReceiptAggregate.Receipt.Items#Domain.ReceiptAggregate.ValueObjects.ReceiptItem.Sum#Domain.Common.ValueObjects.Money", "Sum", b2 =>
                                {
                                    b2.Property<int>("ReceiptItemId")
                                        .HasColumnType("integer");

                                    b2.Property<Guid>("ReceiptItemReceiptId")
                                        .HasColumnType("uuid");

                                    b2.HasKey("ReceiptItemId", "ReceiptItemReceiptId");

                                    b2.ToTable("ReceiptItems", (string)null);

                                    b2.WithOwner()
                                        .HasForeignKey("ReceiptItemId", "ReceiptItemReceiptId");
                                });

                            b1.Navigation("Price")
                                .IsRequired();

                            b1.Navigation("Sum")
                                .IsRequired();
                        });

                    b.Navigation("Items");
                });

            modelBuilder.Entity("Domain.RoomAggregate.Room", b =>
                {
                    b.OwnsOne("Domain.RoomAggregate.Room.Budget#Domain.Common.ValueObjects.Money", "Budget", b1 =>
                        {
                            b1.Property<Guid>("RoomId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric")
                                .HasColumnName("BudgetAmount");

                            b1.Property<int>("Currency")
                                .HasColumnType("integer")
                                .HasColumnName("BudgetCurrency");

                            b1.HasKey("RoomId");

                            b1.ToTable("Rooms", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RoomId");
                        });

                    b.OwnsMany("Domain.RoomAggregate.Room.OwnedShopItems#Domain.RoomAggregate.ValueObjects.OwnedShopItem", "OwnedShopItems", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("RoomId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("ShopItemId")
                                .HasColumnType("uuid")
                                .HasColumnName("ShopItemId");

                            b1.HasKey("Id");

                            b1.HasIndex("RoomId");

                            b1.ToTable("RoomsOwnedShopItems", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RoomId");

                            b1.OwnsOne("Domain.RoomAggregate.Room.OwnedShopItems#Domain.RoomAggregate.ValueObjects.OwnedShopItem.Price#Domain.Common.ValueObjects.Money", "Price", b2 =>
                                {
                                    b2.Property<int>("OwnedShopItemId")
                                        .HasColumnType("integer");

                                    b2.HasKey("OwnedShopItemId");

                                    b2.ToTable("RoomsOwnedShopItems", (string)null);

                                    b2.WithOwner()
                                        .HasForeignKey("OwnedShopItemId");
                                });

                            b1.Navigation("Price")
                                .IsRequired();
                        });

                    b.OwnsMany("Domain.RoomAggregate.Room.UserIds#Domain.UserAggregate.ValueObjects.UserId", "UserIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("RoomId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("UserId");

                            b1.HasKey("Id", "RoomId");

                            b1.HasIndex("RoomId");

                            b1.ToTable("RoomUserIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RoomId");
                        });

                    b.Navigation("Budget")
                        .IsRequired();

                    b.Navigation("OwnedShopItems");

                    b.Navigation("UserIds");
                });
#pragma warning restore 612, 618
        }
    }
}
