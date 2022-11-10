using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class addpassworduser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "user",
                type: "varchar(250)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "user");
        }
    }
}
