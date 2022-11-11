using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class fix_user_dates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "creationDate",
                table: "user",
                type: "datetime",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "updateDate",
                table: "user",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "creationDate",
                table: "user");

            migrationBuilder.DropColumn(
                name: "updateDate",
                table: "user");
        }
    }
}
