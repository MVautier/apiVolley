using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class ajout_fields_order_adherent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "date_naissance",
                table: "commande",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "commande",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nom",
                table: "commande",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "prennom",
                table: "commande",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_parent",
                table: "adherent",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_naissance",
                table: "commande");

            migrationBuilder.DropColumn(
                name: "email",
                table: "commande");

            migrationBuilder.DropColumn(
                name: "nom",
                table: "commande");

            migrationBuilder.DropColumn(
                name: "prennom",
                table: "commande");

            migrationBuilder.DropColumn(
                name: "id_parent",
                table: "adherent");
        }
    }
}
