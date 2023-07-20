using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "001e472a-8735-410a-9b8d-96fe85a89011");

            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "16f60c19-4586-4029-85ee-3603981174f7");

            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "1f25f424-cfa9-4d3d-b147-4902c6242579");

            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "3f3179d0-25bd-452b-9631-36043eb1e93b");

            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "adc1f2b9-8283-43c4-99c3-cb39816db869");

            migrationBuilder.InsertData(
                table: "CollectionFieldTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "824127c4-4800-4bae-a0cb-315c89230dc1", "STRING" },
                    { "a56ef62e-1e7a-442e-ac8b-7fd104176a3c", "DATE" },
                    { "b0220bcb-9478-4589-80cf-27fefa24dfb2", "TEXT" },
                    { "bafd6d28-009e-4d16-aae5-5a17ea93bd3f", "INTEGER" },
                    { "c5d215b6-a4c6-4250-aadb-b4049bd092fb", "BOOLEAN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "824127c4-4800-4bae-a0cb-315c89230dc1");

            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "a56ef62e-1e7a-442e-ac8b-7fd104176a3c");

            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "b0220bcb-9478-4589-80cf-27fefa24dfb2");

            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "bafd6d28-009e-4d16-aae5-5a17ea93bd3f");

            migrationBuilder.DeleteData(
                table: "CollectionFieldTypes",
                keyColumn: "Id",
                keyValue: "c5d215b6-a4c6-4250-aadb-b4049bd092fb");

            migrationBuilder.InsertData(
                table: "CollectionFieldTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "001e472a-8735-410a-9b8d-96fe85a89011", "TEXT" },
                    { "16f60c19-4586-4029-85ee-3603981174f7", "INT" },
                    { "1f25f424-cfa9-4d3d-b147-4902c6242579", "STR" },
                    { "3f3179d0-25bd-452b-9631-36043eb1e93b", "DATE" },
                    { "adc1f2b9-8283-43c4-99c3-cb39816db869", "BOOL" }
                });
        }
    }
}
