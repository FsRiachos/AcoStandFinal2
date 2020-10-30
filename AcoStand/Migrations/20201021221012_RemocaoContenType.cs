using Microsoft.EntityFrameworkCore.Migrations;

namespace AcoStand.Migrations
{
    public partial class RemocaoContenType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Artigos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Artigos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
