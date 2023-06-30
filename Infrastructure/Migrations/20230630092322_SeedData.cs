using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CollectionThemes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "0765dc52-0de7-4145-aba6-55a71c165808", "Jewelry" },
                    { "1f2ab922-854b-4482-af57-454347e312c0", "Dolls" },
                    { "220852f6-f3cd-4053-af8f-93494b278c3d", "Postcards" },
                    { "2a71bf2f-22fe-4484-9345-c47e31fda4a6", "Books" },
                    { "3dd5662e-f21b-430a-8065-6bb31c444db6", "Coins" },
                    { "3fdcce9a-eae7-4269-896f-eb0f5977a774", "Candles" },
                    { "6a0f5c11-49f8-4baf-a9ad-cfa95e6a42d9", "Lighters" },
                    { "72fc757b-1bee-4255-94f9-839a103ed73e", "Model Trains" },
                    { "9701c730-045a-43ea-81f6-06c46e3f9a70", "Toy Cars" },
                    { "a40f0d34-ce27-4a29-8923-a6615bb416e6", "Comics" },
                    { "a9d76061-b42c-440e-ac90-142f36a4da06", "Hats" },
                    { "ae1b4ac6-111d-4268-9295-6db607e996da", "Board Games" },
                    { "ce0c105b-c624-4112-ad42-36bc1416c950", "Trading Cards" },
                    { "fe93dd69-cd9a-4f1b-ada1-e20c50537528", "Autographs" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "0765dc52-0de7-4145-aba6-55a71c165808");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "1f2ab922-854b-4482-af57-454347e312c0");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "220852f6-f3cd-4053-af8f-93494b278c3d");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "2a71bf2f-22fe-4484-9345-c47e31fda4a6");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "3dd5662e-f21b-430a-8065-6bb31c444db6");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "3fdcce9a-eae7-4269-896f-eb0f5977a774");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "6a0f5c11-49f8-4baf-a9ad-cfa95e6a42d9");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "72fc757b-1bee-4255-94f9-839a103ed73e");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "9701c730-045a-43ea-81f6-06c46e3f9a70");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "a40f0d34-ce27-4a29-8923-a6615bb416e6");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "a9d76061-b42c-440e-ac90-142f36a4da06");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "ae1b4ac6-111d-4268-9295-6db607e996da");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "ce0c105b-c624-4112-ad42-36bc1416c950");

            migrationBuilder.DeleteData(
                table: "CollectionThemes",
                keyColumn: "Id",
                keyValue: "fe93dd69-cd9a-4f1b-ada1-e20c50537528");
        }
    }
}
