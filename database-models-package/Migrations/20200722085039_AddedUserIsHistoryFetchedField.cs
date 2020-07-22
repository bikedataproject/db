using Microsoft.EntityFrameworkCore.Migrations;

namespace bikedataproject_database_models_package.Migrations
{
    public partial class AddedUserIsHistoryFetchedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHistoryFetched",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHistoryFetched",
                table: "Users");
        }
    }
}
