using Microsoft.EntityFrameworkCore.Migrations;

namespace FysioApp.Migrations
{
    public partial class AddedPatientFileTreatmentPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmountOfSessionsPerWeek",
                table: "PatientFile",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SessionDuration",
                table: "PatientFile",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfSessionsPerWeek",
                table: "PatientFile");

            migrationBuilder.DropColumn(
                name: "SessionDuration",
                table: "PatientFile");
        }
    }
}
