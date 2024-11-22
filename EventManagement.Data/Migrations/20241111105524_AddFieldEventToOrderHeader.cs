using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldEventToOrderHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventId",
                table: "OrderHeaders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_EventId",
                table: "OrderHeaders",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_Events_EventId",
                table: "OrderHeaders",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "IdEvent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_Events_EventId",
                table: "OrderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_EventId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "OrderHeaders");
        }
    }
}
