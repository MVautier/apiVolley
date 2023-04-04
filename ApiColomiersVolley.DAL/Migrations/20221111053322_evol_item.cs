using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class evol_item : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "creationDate",
                table: "item");

            migrationBuilder.DropColumn(
                name: "updateDate",
                table: "item");

            migrationBuilder.DropColumn(
                name: "idUser",
                table: "item");

            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "item",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "modified",
                table: "item",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "author",
                table: "item",
                type: "int",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date",
                table: "item");

            migrationBuilder.DropColumn(
                name: "modified",
                table: "item");

            migrationBuilder.DropColumn(
                name: "author",
                table: "item");

            migrationBuilder.AddColumn<DateTime>(
                name: "creationDate",
                table: "item",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updateDate",
                table: "item",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "idUser",
                table: "item",
                type: "int",
                nullable: false);
        }
    }
}
