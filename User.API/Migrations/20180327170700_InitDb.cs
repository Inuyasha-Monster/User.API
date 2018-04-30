using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace User.API.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Address = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    Company = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Gender = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NameCard = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    ProvinceId = table.Column<int>(nullable: false),
                    Tel = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserTags",
                columns: table => new
                {
                    AppUserId = table.Column<int>(nullable: false),
                    Tag = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTags", x => new { x.AppUserId, x.Tag });
                });

            migrationBuilder.CreateTable(
                name: "BpFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    AppUserId = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FormatFilePath = table.Column<string>(nullable: true),
                    OriginFilePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BpFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserProperties",
                columns: table => new
                {
                    AppUserId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 100, nullable: false),
                    Value = table.Column<string>(maxLength: 100, nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserProperties", x => new { x.AppUserId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_AppUserProperties_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserProperties");

            migrationBuilder.DropTable(
                name: "AppUserTags");

            migrationBuilder.DropTable(
                name: "BpFiles");

            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}
