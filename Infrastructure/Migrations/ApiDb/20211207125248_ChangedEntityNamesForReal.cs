using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations.ApiDb
{
    public partial class ChangedEntityNamesForReal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Vektis",
                table: "Vektis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DCSPH",
                table: "DCSPH");

            migrationBuilder.RenameTable(
                name: "Vektis",
                newName: "Diagnoses");

            migrationBuilder.RenameTable(
                name: "DCSPH",
                newName: "Operations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Diagnoses",
                table: "Diagnoses",
                column: "Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Operations",
                table: "Operations",
                column: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Operations",
                table: "Operations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Diagnoses",
                table: "Diagnoses");

            migrationBuilder.RenameTable(
                name: "Operations",
                newName: "DCSPH");

            migrationBuilder.RenameTable(
                name: "Diagnoses",
                newName: "Vektis");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DCSPH",
                table: "DCSPH",
                column: "Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vektis",
                table: "Vektis",
                column: "Code");
        }
    }
}
