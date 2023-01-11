using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AM_DanceStudio.Data.Migrations
{
    public partial class Validare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Valid",
                table: "Classes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valid",
                table: "Classes");
        }
    }
}
