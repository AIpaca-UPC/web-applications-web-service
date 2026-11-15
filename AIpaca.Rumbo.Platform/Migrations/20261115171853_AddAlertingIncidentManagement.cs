using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace AIpaca.Rumbo.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddAlertingIncidentManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    category = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    message = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    read_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    route_id = table.Column<int>(type: "int", nullable: false),
                    trip_id = table.Column<int>(type: "int", nullable: false),
                    student_id = table.Column<int>(type: "int", nullable: false),
                    recipient_profile_id = table.Column<int>(type: "int", nullable: false),
                    journey_event_type = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_notifications", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notifications");
        }
    }
}
