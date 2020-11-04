﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SocialNetwork.Models;

namespace SocialNetwork.Migrations
{
    [DbContext(typeof(UsersContext))]
    partial class UsersContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("SocialNetwork.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PostId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("SocialNetwork.Models.Dialog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("User1Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("User2Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Dialogs");
                });

            modelBuilder.Entity("SocialNetwork.Models.Friendship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RequestFromUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RequestToUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RequestFromUserId");

                    b.HasIndex("RequestToUserId");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("SocialNetwork.Models.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PostId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PostId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("SocialNetwork.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int?>("DialogId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserFromId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UserToId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DialogId");

                    b.HasIndex("UserFromId");

                    b.HasIndex("UserToId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("SocialNetwork.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PostId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("SocialNetwork.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("SocialNetwork.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("BirthDay")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("JobPlace")
                        .HasColumnType("TEXT");

                    b.Property<string>("JobPosition")
                        .HasColumnType("TEXT");

                    b.Property<string>("MobiePhone")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("School")
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .HasColumnType("TEXT");

                    b.Property<string>("University")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SocialNetwork.Models.Comment", b =>
                {
                    b.HasOne("SocialNetwork.Models.User", "Owner")
                        .WithMany("Comments")
                        .HasForeignKey("OwnerId");

                    b.HasOne("SocialNetwork.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("SocialNetwork.Models.Friendship", b =>
                {
                    b.HasOne("SocialNetwork.Models.User", "RequestFrom")
                        .WithMany("OutgoingFrienshipRequests")
                        .HasForeignKey("RequestFromUserId");

                    b.HasOne("SocialNetwork.Models.User", "RequestTo")
                        .WithMany("IncomingFrienshipRequests")
                        .HasForeignKey("RequestToUserId");
                });

            modelBuilder.Entity("SocialNetwork.Models.Like", b =>
                {
                    b.HasOne("SocialNetwork.Models.User", "Owner")
                        .WithMany("Likes")
                        .HasForeignKey("OwnerId");

                    b.HasOne("SocialNetwork.Models.Post", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("SocialNetwork.Models.Message", b =>
                {
                    b.HasOne("SocialNetwork.Models.Dialog", "Dialog")
                        .WithMany("Messages")
                        .HasForeignKey("DialogId");

                    b.HasOne("SocialNetwork.Models.User", "UserFrom")
                        .WithMany("MessageFrom")
                        .HasForeignKey("UserFromId");

                    b.HasOne("SocialNetwork.Models.User", "UserTo")
                        .WithMany("MessageTo")
                        .HasForeignKey("UserToId");
                });

            modelBuilder.Entity("SocialNetwork.Models.Photo", b =>
                {
                    b.HasOne("SocialNetwork.Models.Post", "Post")
                        .WithMany("Photos")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("SocialNetwork.Models.Post", b =>
                {
                    b.HasOne("SocialNetwork.Models.User", "Owner")
                        .WithMany("Posts")
                        .HasForeignKey("OwnerId");
                });
#pragma warning restore 612, 618
        }
    }
}
