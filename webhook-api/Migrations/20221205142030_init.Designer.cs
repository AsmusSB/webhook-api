﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using webhook_api.Context;

#nullable disable

namespace webhookapi.Migrations
{
    [DbContext(typeof(WebhookDBContext))]
    [Migration("20221205142030_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("webhook_api.Models.Header", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ConfigId")
                        .HasColumnType("int");

                    b.Property<string>("HeaderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HeaderValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ConfigId");

                    b.ToTable("Headers");
                });

            modelBuilder.Entity("webhook_api.Models.WebhookConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DestinationUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RetryTimeSpan")
                        .HasColumnType("int");

                    b.Property<string>("TenantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TryCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("WebhookConfigurations");
                });

            modelBuilder.Entity("webhook_api.Models.WebhookHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusCode")
                        .HasColumnType("int");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("WebhookHistory");
                });

            modelBuilder.Entity("webhook_api.Models.WebhookStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConfigId")
                        .HasColumnType("int");

                    b.Property<int>("CurrentFailedAttempts")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimesSuccessfullyFired")
                        .HasColumnType("int");

                    b.Property<string>("TriggerEvent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ConfigId");

                    b.ToTable("WebhookStatus");
                });

            modelBuilder.Entity("webhook_api.Models.Header", b =>
                {
                    b.HasOne("webhook_api.Models.WebhookConfiguration", "Config")
                        .WithMany("Headers")
                        .HasForeignKey("ConfigId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Config");
                });

            modelBuilder.Entity("webhook_api.Models.WebhookStatus", b =>
                {
                    b.HasOne("webhook_api.Models.WebhookConfiguration", "Config")
                        .WithMany("Webhooks")
                        .HasForeignKey("ConfigId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Config");
                });

            modelBuilder.Entity("webhook_api.Models.WebhookConfiguration", b =>
                {
                    b.Navigation("Headers");

                    b.Navigation("Webhooks");
                });
#pragma warning restore 612, 618
        }
    }
}
