using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatenew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_EventDates_EventDateId",
                table: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "EventDateId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_EventDates_EventDateId",
                table: "Tickets",
                column: "EventDateId",
                principalTable: "EventDates",
                principalColumn: "IdEventDate",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_EventDates_EventDateId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "EventDateId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_EventDates_EventDateId",
                table: "Tickets",
                column: "EventDateId",
                principalTable: "EventDates",
                principalColumn: "IdEventDate",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
