using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations.ApiDb
{
    public partial class AddedDescriptionRequiredToDCSPH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DescriptionRequired",
                table: "DCSPH",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionRequired",
                table: "DCSPH");
        }
    }
}
