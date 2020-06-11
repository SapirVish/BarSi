using Microsoft.EntityFrameworkCore.Migrations;

namespace BarSi.Migrations
{
    public partial class Init22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalEquipmentSupply_MedicalEquipment_HospitalId",
                table: "MedicalEquipmentSupply");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalEquipmentSupply_Hospital_MedicalEquipmentId",
                table: "MedicalEquipmentSupply");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalEquipmentSupply_Hospital_HospitalId",
                table: "MedicalEquipmentSupply",
                column: "HospitalId",
                principalTable: "Hospital",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalEquipmentSupply_MedicalEquipment_MedicalEquipmentId",
                table: "MedicalEquipmentSupply",
                column: "MedicalEquipmentId",
                principalTable: "MedicalEquipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalEquipmentSupply_Hospital_HospitalId",
                table: "MedicalEquipmentSupply");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalEquipmentSupply_MedicalEquipment_MedicalEquipmentId",
                table: "MedicalEquipmentSupply");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalEquipmentSupply_MedicalEquipment_HospitalId",
                table: "MedicalEquipmentSupply",
                column: "HospitalId",
                principalTable: "MedicalEquipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalEquipmentSupply_Hospital_MedicalEquipmentId",
                table: "MedicalEquipmentSupply",
                column: "MedicalEquipmentId",
                principalTable: "Hospital",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
