using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webhookapi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebhookConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinationUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TryCount = table.Column<int>(type: "int", nullable: false),
                    RetryTimeSpan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebhookHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Headers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeaderValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Headers_WebhookConfigurations_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "WebhookConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebhookStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggerEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimesSuccessfullyFired = table.Column<int>(type: "int", nullable: false),
                    CurrentFailedAttempts = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebhookStatus_WebhookConfigurations_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "WebhookConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Headers_ConfigId",
                table: "Headers",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookStatus_ConfigId",
                table: "WebhookStatus",
                column: "ConfigId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Headers");

            migrationBuilder.DropTable(
                name: "WebhookHistory");

            migrationBuilder.DropTable(
                name: "WebhookStatus");

            migrationBuilder.DropTable(
                name: "WebhookConfigurations");
        }
    }
}
