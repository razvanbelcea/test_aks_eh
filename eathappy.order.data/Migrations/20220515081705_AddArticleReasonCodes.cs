using Microsoft.EntityFrameworkCore.Migrations;

namespace eathappy.order.data.Migrations
{
    public partial class AddArticleReasonCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReasonCode",
                schema: "catalog",
                table: "articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "None");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReasonCode",
                schema: "catalog",
                table: "articles");
        }
    }
}
