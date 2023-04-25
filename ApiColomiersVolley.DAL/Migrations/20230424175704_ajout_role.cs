using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class ajout_role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "user");

            migrationBuilder.AddColumn<int>(
                name: "id_role",
                table: "user",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "article",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "varchar(250)", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    link = table.Column<string>(type: "varchar(500)", nullable: true),
                    author = table.Column<string>(type: "varchar(250)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    libelle = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_id_role",
                table: "user",
                column: "id_role");

            

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_id_role",
                table: "user",
                column: "id_role",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql("insert into role (libelle) values ('admin')");
            migrationBuilder.Sql("insert into role (libelle) values ('manager')");
            migrationBuilder.Sql("insert into role (libelle) values ('contributor')");

            migrationBuilder.Sql("update user set id_role = 1 where id = 1");

            migrationBuilder.AlterColumn<string>(
                name: "id_role",
                table: "user",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_role_id_role",
                table: "user");

            migrationBuilder.DropTable(
                name: "article");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropIndex(
                name: "IX_user_id_role",
                table: "user");

            migrationBuilder.DropColumn(
                name: "id_role",
                table: "user");

            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "user",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");
        }
    }
}
