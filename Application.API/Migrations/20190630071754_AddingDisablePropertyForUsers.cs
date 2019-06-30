using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.API.Migrations
{
    public partial class AddingDisablePropertyForUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disable",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disable",
                table: "AspNetUsers");
        }
    }
}
