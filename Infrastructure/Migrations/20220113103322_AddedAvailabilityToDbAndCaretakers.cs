using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddedAvailabilityToDbAndCaretakers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "Treatments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityId",
                table: "Teacher",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityId",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Availabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MondayStart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MondayEnd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TuesdayStart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TuesdayEnd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WednesdayStart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WednesdayEnd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThursdayStart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThursdayEnd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FridayStart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FridayEnd = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availabilities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_PatientFileId",
                table: "Treatments",
                column: "PatientFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_StudentId",
                table: "Treatments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_AvailabilityId",
                table: "Teacher",
                column: "AvailabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_AvailabilityId",
                table: "Student",
                column: "AvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Availabilities_AvailabilityId",
                table: "Student",
                column: "AvailabilityId",
                principalTable: "Availabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_Availabilities_AvailabilityId",
                table: "Teacher",
                column: "AvailabilityId",
                principalTable: "Availabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_PatientFile_PatientFileId",
                table: "Treatments",
                column: "PatientFileId",
                principalTable: "PatientFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_Student_StudentId",
                table: "Treatments",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Availabilities_AvailabilityId",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_Availabilities_AvailabilityId",
                table: "Teacher");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_PatientFile_PatientFileId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Student_StudentId",
                table: "Treatments");

            migrationBuilder.DropTable(
                name: "Availabilities");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_PatientFileId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_StudentId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Teacher_AvailabilityId",
                table: "Teacher");

            migrationBuilder.DropIndex(
                name: "IX_Student_AvailabilityId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "AvailabilityId",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "AvailabilityId",
                table: "Student");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "Treatments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
