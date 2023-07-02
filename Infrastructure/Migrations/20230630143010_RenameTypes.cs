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

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "019d024d-3708-4abc-906c-50e554d15d5e");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "075ad3a4-210c-4cf7-a6d3-db745cca5e8f");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "247f903c-fd99-42f4-be36-a88f341f82cb");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "282e3588-7922-4ef7-a730-0d52af58091d");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "59f83eae-26bd-4dc3-b248-0e8a59400a6a");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "6d354273-e4bd-4bfe-83ea-d86132dd902c");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "88e2a82d-3ca6-43a2-b537-d17501560881");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "8fba599a-b2f7-4bd5-87bc-866c2b370d85");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "9135462a-cefb-4355-9281-51aac5066b91");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "99835869-6abc-435c-bde7-bd4b5ba1bc6c");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "b05f8822-31a2-4f18-880c-2553c2122d32");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "d33080aa-beaa-4087-8964-41d84ef58314");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "e92abaa6-f972-43d9-825e-7538404aeb7c");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "fc12c387-ea78-4687-ab08-427c7e378c0a");

            migrationBuilder.InsertData(
                table: "CollectionFieldTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "824127c4-4800-4bae-a0cb-315c89230dc1", "STRING" },
                    { "a56ef62e-1e7a-442e-ac8b-7fd104176a3c", "DATETIME" },
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
