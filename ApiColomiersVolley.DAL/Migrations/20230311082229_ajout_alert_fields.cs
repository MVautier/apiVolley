using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class ajout_alert_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "tel",
                table: "adherent",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(14)");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "tel",
                table: "adherent",
                type: "varchar(14)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)");
        }
    }
}
