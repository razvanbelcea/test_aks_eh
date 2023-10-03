using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eathappy.order.data.Migrations
{
    public partial class AddStoreDetailsForOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CostCenterId",
                schema: "catalog",
                table: "orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FlinkCodeId",
                schema: "catalog",
                table: "orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PosId",
                schema: "catalog",
                table: "orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostCenterId",
                schema: "catalog",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "FlinkCodeId",
                schema: "catalog",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "PosId",
                schema: "catalog",
                table: "orders");
        }
    }
}
