﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace bikedataproject_database_models_package.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contributions",
                columns: table => new
                {
                    ContributionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserAgent = table.Column<string>(nullable: true),
                    Distance = table.Column<int>(nullable: false),
                    TimeStampStart = table.Column<DateTimeOffset>(nullable: false),
                    TimeStampStop = table.Column<DateTimeOffset>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    PointsGeom = table.Column<byte[]>(nullable: true),
                    PointsTime = table.Column<DateTimeOffset[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributions", x => x.ContributionId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserIdentifier = table.Column<Guid>(nullable: false),
                    Provider = table.Column<string>(nullable: true),
                    ProviderUser = table.Column<string>(nullable: true),
                    AccessToken = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    TokenCreationDate = table.Column<DateTime>(nullable: false),
                    ExpiresAt = table.Column<int>(nullable: false),
                    ExpiresIn = table.Column<int>(nullable: false),
                    IsHistoryFetched = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserContributions",
                columns: table => new
                {
                    UserContributionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(nullable: false),
                    ContributionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContributions", x => x.UserContributionId);
                    table.ForeignKey(
                        name: "FK_UserContributions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserContributions_UserId",
                table: "UserContributions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contributions");

            migrationBuilder.DropTable(
                name: "UserContributions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}