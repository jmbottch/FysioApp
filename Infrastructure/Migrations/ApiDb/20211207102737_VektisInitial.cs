using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations.ApiDb
{
    public partial class VektisInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vektis",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BodyLocalization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pathology = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vektis", x => x.Code);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vektis");
        }
    }
}
