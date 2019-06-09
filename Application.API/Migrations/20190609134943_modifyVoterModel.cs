using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.API.Migrations
{
    public partial class modifyVoterModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeEngenere",
                table: "Voters");

            migrationBuilder.RenameColumn(
                name: "CodePharmacist",
                table: "Voters",
                newName: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Voters",
                newName: "CodePharmacist");

            migrationBuilder.AddColumn<int>(
                name: "CodeEngenere",
                table: "Voters",
                nullable: false,
                defaultValue: 0);
        }
    }
}
