using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeDataProject.DB.Migrations
{
    public partial class Add_AddedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedOn",
                table: "Contributions",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "Contributions");
        }
    }
}
