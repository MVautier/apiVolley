using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class evol_alert_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "alert_nom",
                table: "adherent");

            migrationBuilder.DropColumn(
                name: "alert_phone",
                table: "adherent");

            migrationBuilder.DropColumn(
                name: "alert_prenom",
                table: "adherent");

            migrationBuilder.AddColumn<string>(
                name: "alert1",
                table: "adherent",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "alert2",
                table: "adherent",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "alert3",
                table: "adherent",
                type: "varchar(500)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "alert1",
                table: "adherent");

            migrationBuilder.DropColumn(
                name: "alert2",
                table: "adherent");

            migrationBuilder.DropColumn(
                name: "alert3",
                table: "adherent");

            migrationBuilder.AddColumn<string>(
                name: "alert_nom",
                table: "adherent",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "alert_phone",
                table: "adherent",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "alert_prenom",
                table: "adherent",
                type: "varchar(100)",
                nullable: true);
        }
    }
}
