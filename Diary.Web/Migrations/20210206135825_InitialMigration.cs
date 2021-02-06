using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Diary.Web.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UndoActionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangedId = table.Column<int>(type: "int", nullable: true),
                    GetUndoActionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UndoActionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UndoActionItems_EntryItems_ChangedId",
                        column: x => x.ChangedId,
                        principalTable: "EntryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UndoActionItems_ChangedId",
                table: "UndoActionItems",
                column: "ChangedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UndoActionItems");

            migrationBuilder.DropTable(
                name: "EntryItems");
        }
    }
}
