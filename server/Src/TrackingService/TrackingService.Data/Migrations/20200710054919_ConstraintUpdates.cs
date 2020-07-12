using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackingService.Data.Migrations
{
    public partial class ConstraintUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Records",
                table: "Records");

            migrationBuilder.RenameTable(
                name: "Records",
                newName: "User");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "User",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "User",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Id",
                table: "User",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Id",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Records");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Records",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Records",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Records",
                table: "Records",
                column: "Id");
        }
    }
}
