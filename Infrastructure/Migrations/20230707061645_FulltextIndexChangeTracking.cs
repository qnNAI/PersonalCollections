using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FulltextIndexChangeTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                sql: "ALTER FULLTEXT INDEX ON Items SET CHANGE_TRACKING AUTO",
                suppressTransaction: true);

            migrationBuilder.Sql(
                 sql: "ALTER FULLTEXT INDEX ON Items SET STOPLIST OFF",
                 suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
