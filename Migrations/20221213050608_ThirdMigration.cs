using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingPlanner.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_Users_PlannerUserId",
                table: "Weddings");

            migrationBuilder.DropIndex(
                name: "IX_Weddings_PlannerUserId",
                table: "Weddings");

            migrationBuilder.DropColumn(
                name: "PlannerUserId",
                table: "Weddings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlannerUserId",
                table: "Weddings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weddings_PlannerUserId",
                table: "Weddings",
                column: "PlannerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_Users_PlannerUserId",
                table: "Weddings",
                column: "PlannerUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
