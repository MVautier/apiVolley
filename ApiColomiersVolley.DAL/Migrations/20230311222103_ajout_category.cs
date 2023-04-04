using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class ajout_category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorie",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(1)", nullable: false),
                    libelle = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorie", x => x.id);
                });

            migrationBuilder.Sql("insert into categorie (code, libelle) values ('C', 'Adultes avec licence FSGT')");
            migrationBuilder.Sql("insert into categorie (code, libelle) values ('L', 'Adultes en loisirs détente')");
            migrationBuilder.Sql("insert into categorie (code, libelle) values ('E', 'Ados 13/17 ans avec licence FSGT')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categorie");
        }
    }
}
