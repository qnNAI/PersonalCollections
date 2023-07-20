using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CollectionFieldOnDeleteRestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemFields_CollectionFields_CollectionFieldId",
                table: "ItemFields");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemFields_CollectionFields_CollectionFieldId",
                table: "ItemFields",
                column: "CollectionFieldId",
                principalTable: "CollectionFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemFields_CollectionFields_CollectionFieldId",
                table: "ItemFields");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemFields_CollectionFields_CollectionFieldId",
                table: "ItemFields",
                column: "CollectionFieldId",
                principalTable: "CollectionFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
