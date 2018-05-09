using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ReCommand.API.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectReCommands",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Company = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    EnumReCommandType = table.Column<int>(nullable: false),
                    FinStage = table.Column<string>(nullable: true),
                    FromUserAvator = table.Column<string>(nullable: true),
                    FromUserId = table.Column<int>(nullable: false),
                    FromUserName = table.Column<string>(nullable: true),
                    Introduction = table.Column<string>(nullable: true),
                    ProjectAvator = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    ReCommandTime = table.Column<DateTime>(nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectReCommands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectReferenceUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ProjectReCommandId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectReferenceUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectReferenceUsers_ProjectReCommands_ProjectReCommandId",
                        column: x => x.ProjectReCommandId,
                        principalTable: "ProjectReCommands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReferenceUsers_ProjectReCommandId",
                table: "ProjectReferenceUsers",
                column: "ProjectReCommandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectReferenceUsers");

            migrationBuilder.DropTable(
                name: "ProjectReCommands");
        }
    }
}
