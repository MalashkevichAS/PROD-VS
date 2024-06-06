using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROD_VS.Data.Migrations
{
    public partial class AddDeveloperFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessLevel",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeveloper",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessLevel",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeveloper",
                table: "AspNetUsers");
        }
    }
}
