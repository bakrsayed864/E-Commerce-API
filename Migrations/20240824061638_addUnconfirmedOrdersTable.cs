using Microsoft.EntityFrameworkCore.Migrations;

namespace Own_Service.Migrations
{
    public partial class addUnconfirmedOrdersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnConfirmedOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PoductId = table.Column<int>(type: "int", nullable: false),
                    customerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnConfirmedOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnConfirmedOrders_Customers_customerId",
                        column: x => x.customerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnConfirmedOrders_Products_PoductId",
                        column: x => x.PoductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnConfirmedOrders_customerId",
                table: "UnConfirmedOrders",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_UnConfirmedOrders_PoductId",
                table: "UnConfirmedOrders",
                column: "PoductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnConfirmedOrders");
        }
    }
}
