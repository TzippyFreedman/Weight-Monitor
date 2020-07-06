using Microsoft.EntityFrameworkCore.Migrations;

namespace Subscriber.Data.Migrations
{
    public partial class AddedEmailVertification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivationCode",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivationCode",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "User");
        }
    }
}
