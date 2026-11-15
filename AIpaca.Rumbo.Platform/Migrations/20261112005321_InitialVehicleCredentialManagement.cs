using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace AIpaca.Rumbo.Platform.Migrations
{
    /// <inheritdoc />
    public partial class InitialVehicleCredentialManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    license_plate = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    brand = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    model = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    capacity = table.Column<int>(type: "int", nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    driver_name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vehicles", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "credentials",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    type = table.Column<int>(type: "int", nullable: false),
                    number = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    expires_at = table.Column<DateOnly>(type: "date", nullable: true),
                    verification_status = table.Column<int>(type: "int", nullable: false),
                    verification_source = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    verified_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    vehicle_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_credentials", x => x.id);
                    table.ForeignKey(
                        name: "f_k_credentials_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_credentials_vehicle_id",
                table: "credentials",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "i_x_vehicles_license_plate",
                table: "vehicles",
                column: "license_plate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "credentials");

            migrationBuilder.DropTable(
                name: "vehicles");
        }
    }
}
