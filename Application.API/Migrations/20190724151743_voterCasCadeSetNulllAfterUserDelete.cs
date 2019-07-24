using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.API.Migrations
{
    public partial class voterCasCadeSetNulllAfterUserDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voters_AspNetUsers_ReferenceId",
                table: "Voters");

            migrationBuilder.AddForeignKey(
                name: "FK_Voters_AspNetUsers_ReferenceId",
                table: "Voters",
                column: "ReferenceId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voters_AspNetUsers_ReferenceId",
                table: "Voters");

            migrationBuilder.AddForeignKey(
                name: "FK_Voters_AspNetUsers_ReferenceId",
                table: "Voters",
                column: "ReferenceId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
