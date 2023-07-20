using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ItemFieldOnDeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemFields_Items_ItemId",
                table: "ItemFields");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemFields_Items_ItemId",
                table: "ItemFields",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemFields_Items_ItemId",
                table: "ItemFields");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemFields_Items_ItemId",
                table: "ItemFields",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
