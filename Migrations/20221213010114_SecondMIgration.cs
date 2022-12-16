using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingPlanner.Migrations
{
    public partial class SecondMIgration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_Users_CreatorUserId",
                table: "Weddings");

            migrationBuilder.RenameColumn(
                name: "CreatorUserId",
                table: "Weddings",
                newName: "PlannerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Weddings_CreatorUserId",
                table: "Weddings",
                newName: "IX_Weddings_PlannerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_Users_PlannerUserId",
                table: "Weddings",
                column: "PlannerUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_Users_PlannerUserId",
                table: "Weddings");

            migrationBuilder.RenameColumn(
                name: "PlannerUserId",
                table: "Weddings",
                newName: "CreatorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Weddings_PlannerUserId",
                table: "Weddings",
                newName: "IX_Weddings_CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_Users_CreatorUserId",
                table: "Weddings",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
