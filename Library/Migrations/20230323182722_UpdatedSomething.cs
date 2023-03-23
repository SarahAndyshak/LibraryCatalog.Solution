using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    public partial class UpdatedSomething : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCatalog_Books_BookId",
                table: "BookCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCatalog_Catalogs_CatalogId",
                table: "BookCatalog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookCatalog",
                table: "BookCatalog");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "BookCatalog",
                newName: "BookCatalogs");

            migrationBuilder.RenameIndex(
                name: "IX_BookCatalog_CatalogId",
                table: "BookCatalogs",
                newName: "IX_BookCatalogs_CatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_BookCatalog_BookId",
                table: "BookCatalogs",
                newName: "IX_BookCatalogs_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookCatalogs",
                table: "BookCatalogs",
                column: "BookCatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCatalogs_Books_BookId",
                table: "BookCatalogs",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCatalogs_Catalogs_CatalogId",
                table: "BookCatalogs",
                column: "CatalogId",
                principalTable: "Catalogs",
                principalColumn: "CatalogId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCatalogs_Books_BookId",
                table: "BookCatalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCatalogs_Catalogs_CatalogId",
                table: "BookCatalogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookCatalogs",
                table: "BookCatalogs");

            migrationBuilder.RenameTable(
                name: "BookCatalogs",
                newName: "BookCatalog");

            migrationBuilder.RenameIndex(
                name: "IX_BookCatalogs_CatalogId",
                table: "BookCatalog",
                newName: "IX_BookCatalog_CatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_BookCatalogs_BookId",
                table: "BookCatalog",
                newName: "IX_BookCatalog_BookId");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Books",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookCatalog",
                table: "BookCatalog",
                column: "BookCatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCatalog_Books_BookId",
                table: "BookCatalog",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCatalog_Catalogs_CatalogId",
                table: "BookCatalog",
                column: "CatalogId",
                principalTable: "Catalogs",
                principalColumn: "CatalogId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
