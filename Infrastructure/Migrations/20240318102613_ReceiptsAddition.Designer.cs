﻿// <auto-generated />
using System;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(RoomWroomDbContext))]
    [Migration("20240318102613_ReceiptsAddition")]
    partial class ReceiptsAddition
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("Domain.ReceiptAggregate.Receipt", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Qr")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Receipts", (string)null);
                });

            modelBuilder.Entity("Domain.RoomAggregate.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("BudgetLowerBound")
                        .HasColumnType("TEXT");

                    b.Property<bool>("MoneyRoundingRequired")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Rooms", (string)null);
                });

            modelBuilder.Entity("Domain.ReceiptAggregate.Receipt", b =>
                {
                    b.OwnsMany("Domain.ReceiptAggregate.ValueObjects.ReceiptItem", "Items", b1 =>
                        {
                            b1.Property<int>("Id")
                                .HasColumnType("INTEGER");

                            b1.Property<Guid?>("AssociatedShopItemId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("TEXT");

                            b1.Property<decimal>("Quantity")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("ReceiptId")
                                .HasColumnType("TEXT");

                            b1.HasKey("Id");

                            b1.HasIndex("ReceiptId");

                            b1.ToTable("ReceiptItems", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ReceiptId");

                            b1.OwnsOne("Domain.Common.ValueObjects.Money", "Price", b2 =>
                                {
                                    b2.Property<int>("ReceiptItemId")
                                        .HasColumnType("INTEGER");

                                    b2.HasKey("ReceiptItemId");

                                    b2.ToTable("ReceiptItems");

                                    b2.WithOwner()
                                        .HasForeignKey("ReceiptItemId");
                                });

                            b1.OwnsOne("Domain.Common.ValueObjects.Money", "Sum", b2 =>
                                {
                                    b2.Property<int>("ReceiptItemId")
                                        .HasColumnType("INTEGER");

                                    b2.HasKey("ReceiptItemId");

                                    b2.ToTable("ReceiptItems");

                                    b2.WithOwner()
                                        .HasForeignKey("ReceiptItemId");
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
                    b.OwnsOne("Domain.Common.ValueObjects.Money", "Budget", b1 =>
                        {
                            b1.Property<Guid>("RoomId")
                                .HasColumnType("TEXT");

                            b1.HasKey("RoomId");

                            b1.ToTable("Rooms");

                            b1.WithOwner()
                                .HasForeignKey("RoomId");
                        });

                    b.OwnsMany("Domain.RoomAggregate.ValueObjects.OwnedShopItem", "OwnedShopItems", b1 =>
                        {
                            b1.Property<int>("Id")
                                .HasColumnType("INTEGER");

                            b1.Property<Guid>("RoomId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("ShopItemId")
                                .HasColumnType("TEXT")
                                .HasColumnName("ShopItemId");

                            b1.HasKey("Id");

                            b1.HasIndex("RoomId");

                            b1.ToTable("RoomsOwnedShopItems", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RoomId");

                            b1.OwnsOne("Domain.Common.ValueObjects.Money", "Price", b2 =>
                                {
                                    b2.Property<int>("OwnedShopItemId")
                                        .HasColumnType("INTEGER");

                                    b2.HasKey("OwnedShopItemId");

                                    b2.ToTable("RoomsOwnedShopItems");

                                    b2.WithOwner()
                                        .HasForeignKey("OwnedShopItemId");
                                });

                            b1.Navigation("Price")
                                .IsRequired();
                        });

                    b.OwnsMany("Domain.UserAggregate.ValueObjects.UserId", "UserIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .HasColumnType("INTEGER");

                            b1.Property<Guid>("RoomId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("Value")
                                .HasColumnType("TEXT")
                                .HasColumnName("UserId");

                            b1.HasKey("Id");

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
