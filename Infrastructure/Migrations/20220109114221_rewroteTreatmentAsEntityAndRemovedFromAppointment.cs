using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class rewroteTreatmentAsEntityAndRemovedFromAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Explanation",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "TreatmentCode",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "TreatmentDescription",
                table: "Appointment");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Treatments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "Treatments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "Treatments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Treatments");

            migrationBuilder.AddColumn<string>(
                name: "Explanation",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TreatmentCode",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TreatmentDescription",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
