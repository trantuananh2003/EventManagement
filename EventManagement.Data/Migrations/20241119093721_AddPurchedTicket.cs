using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchedTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchasedTickets",
                columns: table => new
                {
                    IdPurchasedTicket = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderDetailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDetailIdOrderDetail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedTickets", x => x.IdPurchasedTicket);
                    table.ForeignKey(
                        name: "FK_PurchasedTickets_OrderDetails_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetails",
                        principalColumn: "IdOrderDetail",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchasedTickets_OrderDetails_OrderDetailIdOrderDetail",
                        column: x => x.OrderDetailIdOrderDetail,
                        principalTable: "OrderDetails",
                        principalColumn: "IdOrderDetail");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedTickets_OrderDetailId",
                table: "PurchasedTickets",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedTickets_OrderDetailIdOrderDetail",
                table: "PurchasedTickets",
                column: "OrderDetailIdOrderDetail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasedTickets");
        }
    }
}
