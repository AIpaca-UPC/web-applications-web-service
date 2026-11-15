using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace AIpaca.Rumbo.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionBilling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "billing_plans",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    reference_price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    billing_period_months = table.Column<int>(type: "int", nullable: true),
                    vehicle_limit = table.Column<int>(type: "int", nullable: true),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_billing_plans", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "billing_subscriptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    plan_id = table.Column<int>(type: "int", nullable: false),
                    driver_profile_id = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    started_at = table.Column<DateOnly>(type: "date", nullable: false),
                    current_period_start = table.Column<DateOnly>(type: "date", nullable: false),
                    current_period_end = table.Column<DateOnly>(type: "date", nullable: false),
                    vehicle_limit = table.Column<int>(type: "int", nullable: true),
                    auto_renew = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    paused_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    cancelled_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_billing_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "f_k_billing_subscriptions_billing_plans_plan_id",
                        column: x => x.plan_id,
                        principalTable: "billing_plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_billing_plans_name",
                table: "billing_plans",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_billing_subscriptions_driver_profile_id",
                table: "billing_subscriptions",
                column: "driver_profile_id");

            migrationBuilder.CreateIndex(
                name: "i_x_billing_subscriptions_plan_id",
                table: "billing_subscriptions",
                column: "plan_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "billing_subscriptions");

            migrationBuilder.DropTable(
                name: "billing_plans");
        }
    }
}
