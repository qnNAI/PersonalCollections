using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddItemTagIntermidiate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemTag_Items_ItemsId",
                table: "ItemTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemTag_Tags_TagsId",
                table: "ItemTag");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "ItemTag",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "ItemsId",
                table: "ItemTag",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemTag_TagsId",
                table: "ItemTag",
                newName: "IX_ItemTag_TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemTag_Items_ItemId",
                table: "ItemTag",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemTag_Tags_TagId",
                table: "ItemTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemTag_Items_ItemId",
                table: "ItemTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemTag_Tags_TagId",
                table: "ItemTag");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "ItemTag",
                newName: "TagsId");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "ItemTag",
                newName: "ItemsId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemTag_TagId",
                table: "ItemTag",
                newName: "IX_ItemTag_TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemTag_Items_ItemsId",
                table: "ItemTag",
                column: "ItemsId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemTag_Tags_TagsId",
                table: "ItemTag",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
