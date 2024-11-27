using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldTbEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Privacy",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Privacy",
                table: "Events");
        }
    }
}
