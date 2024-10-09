using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDataBasePhase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    IdOrganization = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdUserOwner = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NameOrganization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.IdOrganization);
                    table.ForeignKey(
                        name: "FK_Organizations_AspNetUsers_IdUserOwner",
                        column: x => x.IdUserOwner,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    IdEvent = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.IdEvent);
                    table.ForeignKey(
                        name: "FK_Events_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "IdOrganization");
                });

            migrationBuilder.CreateTable(
                name: "Agendas",
                columns: table => new
                {
                    IdAgenda = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendas", x => x.IdAgenda);
                    table.ForeignKey(
                        name: "FK_Agendas_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "IdEvent");
                });

            migrationBuilder.CreateTable(
                name: "OverviewEvents",
                columns: table => new
                {
                    IdOverviewEvent = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TypeOverView = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverviewEvents", x => x.IdOverviewEvent);
                    table.ForeignKey(
                        name: "FK_OverviewEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "IdEvent");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agendas_EventId",
                table: "Agendas",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganizationId",
                table: "Events",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_IdUserOwner",
                table: "Organizations",
                column: "IdUserOwner",
                unique: true,
                filter: "[IdUserOwner] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OverviewEvents_EventId",
                table: "OverviewEvents",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agendas");

            migrationBuilder.DropTable(
                name: "OverviewEvents");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
