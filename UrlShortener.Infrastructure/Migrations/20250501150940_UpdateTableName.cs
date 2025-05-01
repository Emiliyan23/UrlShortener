using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UrlEntities",
                table: "UrlEntities");

            migrationBuilder.RenameTable(
                name: "UrlEntities",
                newName: "Urls");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Urls",
                table: "Urls",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Urls",
                table: "Urls");

            migrationBuilder.RenameTable(
                name: "Urls",
                newName: "UrlEntities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UrlEntities",
                table: "UrlEntities",
                column: "Id");
        }
    }
}
