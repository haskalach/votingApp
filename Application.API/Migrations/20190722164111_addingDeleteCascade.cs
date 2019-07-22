using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.API.Migrations
{
    public partial class addingDeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VotingYears_Organizations_OrganizationId",
                table: "VotingYears");

            migrationBuilder.DropForeignKey(
                name: "FK_VotingYears_Voters_VoterId",
                table: "VotingYears");

            migrationBuilder.AddForeignKey(
                name: "FK_VotingYears_Organizations_OrganizationId",
                table: "VotingYears",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VotingYears_Voters_VoterId",
                table: "VotingYears",
                column: "VoterId",
                principalTable: "Voters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VotingYears_Organizations_OrganizationId",
                table: "VotingYears");

            migrationBuilder.DropForeignKey(
                name: "FK_VotingYears_Voters_VoterId",
                table: "VotingYears");

            migrationBuilder.AddForeignKey(
                name: "FK_VotingYears_Organizations_OrganizationId",
                table: "VotingYears",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VotingYears_Voters_VoterId",
                table: "VotingYears",
                column: "VoterId",
                principalTable: "Voters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
