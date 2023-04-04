using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class ajout_adherent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "adherent",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    categorie = table.Column<string>(type: "varchar(1)", nullable: false),
                    autorisation_sortie = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    nom = table.Column<string>(type: "varchar(100)", nullable: false),
                    prenom = table.Column<string>(type: "varchar(100)", nullable: false),
                    genre = table.Column<string>(type: "varchar(1)", nullable: false),
                    date_naissance = table.Column<DateTime>(type: "datetime", nullable: true),
                    date_questionnaire = table.Column<DateTime>(type: "datetime", nullable: true),
                    date_certificat = table.Column<DateTime>(type: "datetime", nullable: true),
                    tel = table.Column<string>(type: "varchar(14)", nullable: false),
                    tel_parent = table.Column<string>(type: "varchar(100)", nullable: true),
                    adresse = table.Column<string>(type: "varchar(120)", nullable: false),
                    cp = table.Column<string>(type: "varchar(5)", nullable: false),
                    ville = table.Column<string>(type: "varchar(100)", nullable: false),
                    email = table.Column<string>(type: "varchar(150)", nullable: false),
                    paiement = table.Column<string>(type: "varchar(25)", nullable: true),
                    photo = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    equipe1 = table.Column<string>(type: "varchar(20)", nullable: true),
                    equipe2 = table.Column<string>(type: "varchar(20)", nullable: true),
                    licence = table.Column<string>(type: "varchar(20)", nullable: true),
                    verif_paiement = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adherent", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "adherent");
        }
    }
}
