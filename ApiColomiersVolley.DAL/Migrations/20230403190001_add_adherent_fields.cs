using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class add_adherent_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "main_section_info",
                table: "adherent");

            migrationBuilder.AddColumn<bool>(
                name: "image_right",
                table: "adherent",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "rgpd",
                table: "adherent",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "saison",
                table: "adherent",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sections",
                table: "adherent",
                type: "varchar(400)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "signature",
                table: "adherent",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uid",
                table: "adherent",
                type: "varchar(36)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_right",
                table: "adherent");

            migrationBuilder.DropColumn(
                name: "rgpd",
                table: "adherent");

            migrationBuilder.DropColumn(
                name: "saison",
                table: "adherent");

            migrationBuilder.DropColumn(
                name: "sections",
                table: "adherent");

            migrationBuilder.DropColumn(
                name: "signature",
                table: "adherent");

            migrationBuilder.DropColumn(
                name: "uid",
                table: "adherent");

            migrationBuilder.AddColumn<string>(
                name: "main_section_info",
                table: "adherent",
                type: "varchar(500)",
                nullable: true);
        }
    }
}
