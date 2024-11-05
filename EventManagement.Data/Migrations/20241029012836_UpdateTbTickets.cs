using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTbTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_EventDates_EventDateId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Events_EventId",
                table: "Ticket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket");

            migrationBuilder.RenameTable(
                name: "Ticket",
                newName: "Tickets");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_EventId",
                table: "Tickets",
                newName: "IX_Tickets_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_EventDateId",
                table: "Tickets",
                newName: "IX_Tickets_EventDateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "IdTicket");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_EventDates_EventDateId",
                table: "Tickets",
                column: "EventDateId",
                principalTable: "EventDates",
                principalColumn: "IdEventDate",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Events_EventId",
                table: "Tickets",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "IdEvent",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_EventDates_EventDateId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Events_EventId",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            migrationBuilder.RenameTable(
                name: "Tickets",
                newName: "Ticket");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_EventId",
                table: "Ticket",
                newName: "IX_Ticket_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_EventDateId",
                table: "Ticket",
                newName: "IX_Ticket_EventDateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket",
                column: "IdTicket");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_EventDates_EventDateId",
                table: "Ticket",
                column: "EventDateId",
                principalTable: "EventDates",
                principalColumn: "IdEventDate",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Events_EventId",
                table: "Ticket",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "IdEvent",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
