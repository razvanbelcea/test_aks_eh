using Microsoft.EntityFrameworkCore.Migrations;

namespace eathappy.order.data.Migrations
{
    public partial class AddFlagForStoreServedOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ServedByStore",
                schema: "catalog",
                table: "orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServedByStore",
                schema: "catalog",
                table: "orders");
        }
    }
}
