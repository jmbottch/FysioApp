using Microsoft.EntityFrameworkCore.Migrations;

namespace FysioApp.Migrations
{
    public partial class AddedPatientFileToComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientFileId",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PatientFileId",
                table: "Comment",
                column: "PatientFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_PatientFile_PatientFileId",
                table: "Comment",
                column: "PatientFileId",
                principalTable: "PatientFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_PatientFile_PatientFileId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_PatientFileId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "PatientFileId",
                table: "Comment");
        }
    }
}
