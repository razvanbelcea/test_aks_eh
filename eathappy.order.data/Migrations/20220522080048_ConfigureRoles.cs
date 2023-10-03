using Microsoft.EntityFrameworkCore.Migrations;

namespace eathappy.order.data.Migrations
{
    public partial class ConfigureRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c5cbc3b-6de1-40c0-9cbc-f02d421615eb", "3d1e2dc9-9dba-4534-9044-981e81db774c", "Viewer", "VIEWER" },
                    { "be59ee8f-b522-4a4b-8924-e6e2ae55e69d", "30f12cf1-0087-4c8b-89ae-87c0e0c428fb", "StoreApprover", "STOREAPPROVER" },
                    { "411eb901-ba0f-47a1-ab84-1e84a704de66", "efd7d609-f1c6-49f4-8e43-eab8c99b0d07", "HubApprover", "HUBAPPROVER" },
                    { "2ddbcca8-2623-438f-9577-ab625e3e8c6e", "e55470c6-7696-450e-b7c5-dd218115b18f", "Administrator", "ADMINISTRATOR" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5cbc3b-6de1-40c0-9cbc-f02d421615eb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ddbcca8-2623-438f-9577-ab625e3e8c6e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "411eb901-ba0f-47a1-ab84-1e84a704de66");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be59ee8f-b522-4a4b-8924-e6e2ae55e69d");
        }
    }
}
