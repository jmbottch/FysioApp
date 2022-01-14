using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class CorrectedNotationInModelsForAvailability : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Availabilities_AvailabilityId",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_Availabilities_AvailabilityId",
                table: "Teacher");

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityId",
                table: "Teacher",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityId",
                table: "Student",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Availabilities_AvailabilityId",
                table: "Student",
                column: "AvailabilityId",
                principalTable: "Availabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_Availabilities_AvailabilityId",
                table: "Teacher",
                column: "AvailabilityId",
                principalTable: "Availabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Availabilities_AvailabilityId",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_Availabilities_AvailabilityId",
                table: "Teacher");

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityId",
                table: "Teacher",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityId",
                table: "Student",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Availabilities_AvailabilityId",
                table: "Student",
                column: "AvailabilityId",
                principalTable: "Availabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_Availabilities_AvailabilityId",
                table: "Teacher",
                column: "AvailabilityId",
                principalTable: "Availabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
