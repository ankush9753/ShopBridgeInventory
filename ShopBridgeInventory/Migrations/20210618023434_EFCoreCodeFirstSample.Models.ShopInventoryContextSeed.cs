using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopInventory.Migrations
{
    public partial class EFCoreCodeFirstSampleModelsShopInventoryContextSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "AvailableQuantity", "Category", "Color", "Description", "Name", "Price" },
                values: new object[] { 1L, 11, "Electronics", "Black", "Full HD LED Smart Android TV", "Android TV", 12000m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "AvailableQuantity", "Category", "Color", "Description", "Name", "Price" },
                values: new object[] { 2L, 5, "Electronics", "White", "HD Pin Hole Display, 16 MP Quad Rear Camera", "Mobile", 10000m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2L);
        }
    }
}
