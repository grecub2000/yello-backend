using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yello.Infrastructure.Migrations
{
    public partial class Cards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardSprint");

            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SprintId",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_SprintId",
                table: "Cards",
                column: "SprintId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Sprints_SprintId",
                table: "Cards",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Sprints_SprintId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_SprintId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "SprintId",
                table: "Cards");

            migrationBuilder.CreateTable(
                name: "CardSprint",
                columns: table => new
                {
                    CardsId = table.Column<int>(type: "int", nullable: false),
                    SprintsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardSprint", x => new { x.CardsId, x.SprintsId });
                    table.ForeignKey(
                        name: "FK_CardSprint_Cards_CardsId",
                        column: x => x.CardsId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardSprint_Sprints_SprintsId",
                        column: x => x.SprintsId,
                        principalTable: "Sprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CardSprint_SprintsId",
                table: "CardSprint",
                column: "SprintsId");
        }
    }
}
