using Microsoft.EntityFrameworkCore.Migrations;

namespace BarSi.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Hospital_hospitalId",
                table: "Patient");

            migrationBuilder.RenameColumn(
                name: "hospitalId",
                table: "Patient",
                newName: "HospitalId");

            migrationBuilder.RenameIndex(
                name: "IX_Patient_hospitalId",
                table: "Patient",
                newName: "IX_Patient_HospitalId");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Patient",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Patient",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Patient",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Doctor",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PatientStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientStatus_PatientStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "PatientStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patient_CityId",
                table: "Patient",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_DoctorId",
                table: "Patient",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_StatusId",
                table: "Patient",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_CityId",
                table: "Doctor",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientStatus_StatusId",
                table: "PatientStatus",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_City_CityId",
                table: "Doctor",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_City_CityId",
                table: "Patient",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_Doctor_DoctorId",
                table: "Patient",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_Hospital_HospitalId",
                table: "Patient",
                column: "HospitalId",
                principalTable: "Hospital",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_PatientStatus_StatusId",
                table: "Patient",
                column: "StatusId",
                principalTable: "PatientStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_City_CityId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_City_CityId",
                table: "Patient");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Doctor_DoctorId",
                table: "Patient");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Hospital_HospitalId",
                table: "Patient");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_PatientStatus_StatusId",
                table: "Patient");

            migrationBuilder.DropTable(
                name: "PatientStatus");

            migrationBuilder.DropIndex(
                name: "IX_Patient_CityId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Patient_DoctorId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Patient_StatusId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Doctor_CityId",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Doctor");

            migrationBuilder.RenameColumn(
                name: "HospitalId",
                table: "Patient",
                newName: "hospitalId");

            migrationBuilder.RenameIndex(
                name: "IX_Patient_HospitalId",
                table: "Patient",
                newName: "IX_Patient_hospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_Hospital_hospitalId",
                table: "Patient",
                column: "hospitalId",
                principalTable: "Hospital",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
