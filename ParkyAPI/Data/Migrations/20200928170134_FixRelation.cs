using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkyAPI.Data.Migrations
{
    public partial class FixRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trails_NationalParkId",
                table: "Trails");

            migrationBuilder.CreateIndex(
                name: "IX_Trails_NationalParkId",
                table: "Trails",
                column: "NationalParkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trails_NationalParkId",
                table: "Trails");

            migrationBuilder.CreateIndex(
                name: "IX_Trails_NationalParkId",
                table: "Trails",
                column: "NationalParkId",
                unique: true);
        }
    }
}
