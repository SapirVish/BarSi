using Microsoft.EntityFrameworkCore.Migrations;

namespace BarSi.Migrations
{
    public partial class addlocationmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Hospital",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lat = table.Column<int>(nullable: false),
                    Lng = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hospital_LocationId",
                table: "Hospital",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hospital_Location_LocationId",
                table: "Hospital",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hospital_Location_LocationId",
                table: "Hospital");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Hospital_LocationId",
                table: "Hospital");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Hospital");
        }
    }
}
