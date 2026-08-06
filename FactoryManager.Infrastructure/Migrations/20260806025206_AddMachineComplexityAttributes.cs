using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMachineComplexityAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Machines");

            migrationBuilder.AddColumn<decimal>(
                name: "EnergyConsumptionPerMinute",
                table: "Machines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMaintenanceAt",
                table: "Machines",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaintenanceIntervalInDays",
                table: "Machines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxProductionPerMinute",
                table: "Machines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Machines",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergyConsumptionPerMinute",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "LastMaintenanceAt",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "MaintenanceIntervalInDays",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "MaxProductionPerMinute",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Machines");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Machines",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
