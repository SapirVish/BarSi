using Microsoft.EntityFrameworkCore.Migrations;

namespace BarSi.Migrations
{
    public partial class Init8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientStatus_PatientStatus_StatusId",
                table: "PatientStatus");

            migrationBuilder.DropIndex(
                name: "IX_PatientStatus_StatusId",
                table: "PatientStatus");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "PatientStatus");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PatientStatus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "PatientStatus");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "PatientStatus",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientStatus_StatusId",
                table: "PatientStatus",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientStatus_PatientStatus_StatusId",
                table: "PatientStatus",
                column: "StatusId",
                principalTable: "PatientStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
