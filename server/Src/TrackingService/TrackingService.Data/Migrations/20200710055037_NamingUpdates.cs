using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackingService.Data.Migrations
{
    public partial class NamingUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Record");

            migrationBuilder.RenameIndex(
                name: "IX_User_Id",
                table: "Record",
                newName: "IX_Record_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Record",
                table: "Record",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Record",
                table: "Record");

            migrationBuilder.RenameTable(
                name: "Record",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_Record_Id",
                table: "User",
                newName: "IX_User_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");
        }
    }
}
