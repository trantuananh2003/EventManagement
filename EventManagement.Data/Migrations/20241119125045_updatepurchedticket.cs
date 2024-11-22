using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatepurchedticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedTickets_OrderDetails_OrderDetailIdOrderDetail",
                table: "PurchasedTickets");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedTickets_OrderDetailIdOrderDetail",
                table: "PurchasedTickets");

            migrationBuilder.DropColumn(
                name: "OrderDetailIdOrderDetail",
                table: "PurchasedTickets");

            migrationBuilder.AddColumn<string>(
                name: "OrderHeaderId",
                table: "PurchasedTickets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedTickets_OrderHeaderId",
                table: "PurchasedTickets",
                column: "OrderHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedTickets_OrderHeaders_OrderHeaderId",
                table: "PurchasedTickets",
                column: "OrderHeaderId",
                principalTable: "OrderHeaders",
                principalColumn: "IdOrderHeader",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedTickets_OrderHeaders_OrderHeaderId",
                table: "PurchasedTickets");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedTickets_OrderHeaderId",
                table: "PurchasedTickets");

            migrationBuilder.DropColumn(
                name: "OrderHeaderId",
                table: "PurchasedTickets");

            migrationBuilder.AddColumn<string>(
                name: "OrderDetailIdOrderDetail",
                table: "PurchasedTickets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedTickets_OrderDetailIdOrderDetail",
                table: "PurchasedTickets",
                column: "OrderDetailIdOrderDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedTickets_OrderDetails_OrderDetailIdOrderDetail",
                table: "PurchasedTickets",
                column: "OrderDetailIdOrderDetail",
                principalTable: "OrderDetails",
                principalColumn: "IdOrderDetail");
        }
    }
}
