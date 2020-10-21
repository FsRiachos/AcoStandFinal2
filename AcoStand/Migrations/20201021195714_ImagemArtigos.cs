using Microsoft.EntityFrameworkCore.Migrations;

namespace AcoStand.Migrations
{
    public partial class ImagemArtigos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Artigos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Artigos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Artigos");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Artigos");
        }
    }
}
