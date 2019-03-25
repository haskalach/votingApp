using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.API.Migrations
{
    public partial class AddingVotersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Voters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    Speciality = table.Column<string>(nullable: true),
                    SubChapter = table.Column<string>(nullable: true),
                    Religion = table.Column<string>(nullable: true),
                    Politic = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    VotedYear = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    BirthCountry = table.Column<string>(nullable: true),
                    BirthPlace = table.Column<string>(nullable: true),
                    CivilIdMother = table.Column<string>(nullable: true),
                    CivilIdKad = table.Column<string>(nullable: true),
                    CivilIdRegion = table.Column<string>(nullable: true),
                    RegisteryNumber = table.Column<string>(nullable: true),
                    CivilIdPlace = table.Column<string>(nullable: true),
                    Registration = table.Column<DateTime>(nullable: false),
                    Graduation = table.Column<DateTime>(nullable: false),
                    School = table.Column<string>(nullable: true),
                    GraduationLocation = table.Column<string>(nullable: true),
                    AddressWork = table.Column<string>(nullable: true),
                    MobileWork = table.Column<string>(nullable: true),
                    PhoneWork = table.Column<string>(nullable: true),
                    AddressHome = table.Column<string>(nullable: true),
                    MobileHome = table.Column<string>(nullable: true),
                    PhoneHome = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Vote = table.Column<int>(nullable: false),
                    Attend = table.Column<int>(nullable: false),
                    Transport = table.Column<int>(nullable: false),
                    Voted = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voters", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voters");
        }
    }
}
