using Microsoft.EntityFrameworkCore.Migrations;

namespace MeasureService.Data.Migrations
{
    public partial class EnumFluentApi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Measure",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(24)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Measure",
                type: "nvarchar(24)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
