using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class ajout_parametres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "parametres",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    inscriptionOpened = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    reinscription = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    inscriptionFilter = table.Column<string>(type: "varchar(500)", nullable: true),
                    adoOpened = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    loisirOpened = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    competOpened = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    nbAdoMax = table.Column<int>(type: "int", nullable: false),
                    tarifLocal = table.Column<int>(type: "int", nullable: false),
                    tarifExterior = table.Column<int>(type: "int", nullable: false),
                    tarifMember = table.Column<int>(type: "int", nullable: false),
                    tarifLoisir = table.Column<int>(type: "int", nullable: false),
                    tarifLicense = table.Column<int>(type: "int", nullable: false),
                    tarifAdo = table.Column<int>(type: "int", nullable: false),
                    subHeader = table.Column<string>(type: "varchar(800)", nullable: true),
                    text1 = table.Column<string>(type: "varchar(1000)", nullable: true),
                    text2 = table.Column<string>(type: "varchar(1000)", nullable: true),
                    text3 = table.Column<string>(type: "varchar(1000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parametres", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "parametres");

        }
    }
}
