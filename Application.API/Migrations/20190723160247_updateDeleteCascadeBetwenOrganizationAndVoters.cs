using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.API.Migrations
{
    public partial class updateDeleteCascadeBetwenOrganizationAndVoters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VotingYears_Organizations_OrganizationId",
                table: "VotingYears");

            migrationBuilder.AddForeignKey(
                name: "FK_VotingYears_Organizations_OrganizationId",
                table: "VotingYears",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VotingYears_Organizations_OrganizationId",
                table: "VotingYears");

            migrationBuilder.AddForeignKey(
                name: "FK_VotingYears_Organizations_OrganizationId",
                table: "VotingYears",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
