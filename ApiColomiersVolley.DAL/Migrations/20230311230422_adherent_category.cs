using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class adherent_category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_categorie",
                table: "adherent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_adherent_id_categorie",
                table: "adherent",
                column: "id_categorie");

            migrationBuilder.AddForeignKey(
                name: "FK_adherent_categorie_id_categorie",
                table: "adherent",
                column: "id_categorie",
                principalTable: "categorie",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("update adherent set id_categorie = 1 where categorie='C'");
            migrationBuilder.Sql("update adherent set id_categorie = 2 where categorie='L'");
            migrationBuilder.Sql("update adherent set id_categorie = 3 where categorie='E'");

            migrationBuilder.DropColumn(
                name: "categorie",
                table: "adherent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adherent_categorie_id_categorie",
                table: "adherent");

            migrationBuilder.DropIndex(
                name: "IX_adherent_id_categorie",
                table: "adherent");

            migrationBuilder.DropColumn(
                name: "id_categorie",
                table: "adherent");

            migrationBuilder.AddColumn<string>(
                name: "categorie",
                table: "adherent",
                type: "varchar(1)",
                nullable: false,
                defaultValue: "");
        }
    }
}
