using Microsoft.EntityFrameworkCore.Migrations;

namespace BarSi.Migrations
{
    public partial class Init21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalEquipmentSupply",
                columns: table => new
                {
                    HospitalId = table.Column<int>(nullable: false),
                    MedicalEquipmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalEquipmentSupply", x => new { x.HospitalId, x.MedicalEquipmentId });
                    table.ForeignKey(
                        name: "FK_MedicalEquipmentSupply_MedicalEquipment_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "MedicalEquipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalEquipmentSupply_Hospital_MedicalEquipmentId",
                        column: x => x.MedicalEquipmentId,
                        principalTable: "Hospital",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalEquipmentSupply_MedicalEquipmentId",
                table: "MedicalEquipmentSupply",
                column: "MedicalEquipmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalEquipmentSupply");
        }
    }
}
