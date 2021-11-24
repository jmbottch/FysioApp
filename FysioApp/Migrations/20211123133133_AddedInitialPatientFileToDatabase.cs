using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FysioApp.Migrations
{
    public partial class AddedInitialPatientFileToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    ComplaintsDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntakeDoneById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IntakeSupervisedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HeadPractitionerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfArrival = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfDeparture = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientFile_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientFile_Student_HeadPractitionerId",
                        column: x => x.HeadPractitionerId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientFile_Student_IntakeDoneById",
                        column: x => x.IntakeDoneById,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFile_Teacher_IntakeSupervisedById",
                        column: x => x.IntakeSupervisedById,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientFile_HeadPractitionerId",
                table: "PatientFile",
                column: "HeadPractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFile_IntakeDoneById",
                table: "PatientFile",
                column: "IntakeDoneById");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFile_IntakeSupervisedById",
                table: "PatientFile",
                column: "IntakeSupervisedById");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFile_PatientId",
                table: "PatientFile",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientFile");
        }
    }
}
