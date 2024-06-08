using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROD_VS.Data.Migrations
{
    public partial class NormalizeUserNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE AspNetUsers SET NormalizedUserName = UPPER(UserName) WHERE NormalizedUserName IS NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
