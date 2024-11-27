using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSChatRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OrganizationId",
                table: "SupportChatRooms",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SupportChatRooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportChatRooms_UserId",
                table: "SupportChatRooms",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportChatRooms_AspNetUsers_UserId",
                table: "SupportChatRooms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportChatRooms_AspNetUsers_UserId",
                table: "SupportChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_SupportChatRooms_UserId",
                table: "SupportChatRooms");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SupportChatRooms");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationId",
                table: "SupportChatRooms",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
