using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class update_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "admin",
                table: "user");

            migrationBuilder.DropColumn(
                name: "expireDate",
                table: "user");

            migrationBuilder.AddColumn<DateTime>(
                name: "creationDate",
                table: "user",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "user",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "creationDate",
                table: "user");

            migrationBuilder.DropColumn(
                name: "role",
                table: "user");

            migrationBuilder.AddColumn<bool>(
                name: "admin",
                table: "user",
                type: "boolean(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "expireDate",
                table: "user",
                type: "varchar(250)",
                nullable: true);
        }
    }
}
