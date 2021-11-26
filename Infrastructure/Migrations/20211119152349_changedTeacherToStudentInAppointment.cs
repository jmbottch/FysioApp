using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class changedTeacherToStudentInAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Teacher_TeacherId",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Appointment",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_TeacherId",
                table: "Appointment",
                newName: "IX_Appointment_StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Student_StudentId",
                table: "Appointment",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Student_StudentId",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Appointment",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_StudentId",
                table: "Appointment",
                newName: "IX_Appointment_TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Teacher_TeacherId",
                table: "Appointment",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
