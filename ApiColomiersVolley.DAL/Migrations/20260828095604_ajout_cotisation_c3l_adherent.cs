using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class ajout_cotisation_c3l_adherent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adherent_categorie_id_categorie",
                table: "adherent");

            migrationBuilder.DropForeignKey(
                name: "FK_adherent_section_id_section",
                table: "adherent");

            migrationBuilder.AlterColumn<int>(
                name: "id_section",
                table: "adherent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "id_categorie",
                table: "adherent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "cotisation_c3l",
                table: "adherent",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_adherent_categorie_id_categorie",
                table: "adherent",
                column: "id_categorie",
                principalTable: "categorie",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_adherent_section_id_section",
                table: "adherent",
                column: "id_section",
                principalTable: "section",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adherent_categorie_id_categorie",
                table: "adherent");

            migrationBuilder.DropForeignKey(
                name: "FK_adherent_section_id_section",
                table: "adherent");

            migrationBuilder.DropColumn(
                name: "cotisation_c3l",
                table: "adherent");

            migrationBuilder.AlterColumn<int>(
                name: "id_section",
                table: "adherent",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id_categorie",
                table: "adherent",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_adherent_categorie_id_categorie",
                table: "adherent",
                column: "id_categorie",
                principalTable: "categorie",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_adherent_section_id_section",
                table: "adherent",
                column: "id_section",
                principalTable: "section",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
