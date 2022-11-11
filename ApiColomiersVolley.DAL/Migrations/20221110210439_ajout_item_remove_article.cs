using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class ajout_item_remove_article : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "article");

            migrationBuilder.AddColumn<DateTime>(
                name: "updateDate",
                table: "user",
                type: "datetime",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "item",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "varchar(250)", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    creationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    updateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    type = table.Column<string>(type: "varchar(50)", nullable: false),
                    slug = table.Column<string>(type: "varchar(800)", nullable: true),
                    description = table.Column<string>(type: "varchar(800)", nullable: true),
                    resume = table.Column<string>(type: "varchar(800)", nullable: true),
                    ip = table.Column<string>(type: "varchar(800)", nullable: true),
                    order = table.Column<int>(type: "int", nullable: true),
                    @public = table.Column<bool>(name: "public", type: "tinyint(1)", nullable: true),
                    idUser = table.Column<int>(type: "int", nullable: true),
                    idParent = table.Column<int>(type: "int", nullable: true),
                    idCategory = table.Column<int>(type: "int", nullable: true),
                    idPost = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropColumn(
                name: "updateDate",
                table: "user");

            migrationBuilder.CreateTable(
                name: "article",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    author = table.Column<string>(type: "varchar(250)", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateTime>(type: "datetime", nullable: false),
                    link = table.Column<string>(type: "varchar(500)", nullable: true),
                    title = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article", x => x.id);
                });
        }
    }
}
