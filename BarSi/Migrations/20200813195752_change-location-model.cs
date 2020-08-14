using Microsoft.EntityFrameworkCore.Migrations;

namespace BarSi.Migrations
{
    public partial class changelocationmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Lng",
                table: "Location",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Lat",
                table: "Location",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Lng",
                table: "Location",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Lat",
                table: "Location",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
