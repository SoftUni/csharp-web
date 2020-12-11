using Microsoft.EntityFrameworkCore.Migrations;

namespace MoiteRecepti.Data.Migrations
{
    public partial class add_search_text : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SearchText",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SearchText",
                table: "Recipes");
        }
    }
}
