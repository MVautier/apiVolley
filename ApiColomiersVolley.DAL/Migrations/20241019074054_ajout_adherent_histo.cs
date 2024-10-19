using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class ajout_adherent_histo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "histo",
                table: "adherent",
                type: "varchar(4000)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "histo",
                table: "adherent");
        }
    }
}
