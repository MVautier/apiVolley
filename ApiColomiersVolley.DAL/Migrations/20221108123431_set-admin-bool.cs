using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiColomiersVolley.DAL.Migrations
{
    public partial class setadminbool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "admin",
                table: "user",
                type: "bit",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "admin",
                table: "user",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
