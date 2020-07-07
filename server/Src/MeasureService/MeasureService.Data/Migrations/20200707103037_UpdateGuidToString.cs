using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeasureService.Data.Migrations
{
    public partial class UpdateGuidToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserFileId",
                table: "Measure",
                type: "string",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserFileId",
                table: "Measure",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "string");
        }
    }
}
