using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeasureService.Data.Migrations
{
    public partial class Enum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Measure",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    UserFileId = table.Column<Guid>(nullable: false),
                    Weight = table.Column<float>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Status = table.Column<string>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measure", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measure");
        }
    }
}
