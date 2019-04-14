using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.API.Migrations
{
    public partial class FixUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrgId",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
