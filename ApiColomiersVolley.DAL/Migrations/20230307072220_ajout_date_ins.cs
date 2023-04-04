using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class ajout_date_ins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "date_inscription",
                table: "adherent",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_section",
                table: "adherent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_adherent_id_section",
                table: "adherent",
                column: "id_section");

            migrationBuilder.AddForeignKey(
                name: "FK_adherent_section_id_section",
                table: "adherent",
                column: "id_section",
                principalTable: "section",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("update adherent set id_section = 1 where date_naissance >= DATE_SUB(CURDATE(), INTERVAL 16 YEAR)");
            migrationBuilder.Sql("update adherent set id_section = 2 where date_naissance <= DATE_SUB(CURDATE(), INTERVAL 16 YEAR) and date_naissance >= DATE_SUB(CURDATE(), INTERVAL 18 YEAR)");
            migrationBuilder.Sql("update adherent set id_section = 3 where date_naissance < DATE_SUB(CURDATE(), INTERVAL 18 YEAR)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adherent_section_id_section",
                table: "adherent");

            migrationBuilder.DropIndex(
                name: "IX_adherent_id_section",
                table: "adherent");

            migrationBuilder.DropColumn(
                name: "date_inscription",
                table: "adherent");

            migrationBuilder.DropColumn(
                name: "id_section",
                table: "adherent");
        }
    }
}
