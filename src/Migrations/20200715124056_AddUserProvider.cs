using Microsoft.EntityFrameworkCore.Migrations;

namespace BDPDatabase
{
    public partial class AddUserProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProviderUser",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProviderUser",
                table: "Users");
        }
    }
}
