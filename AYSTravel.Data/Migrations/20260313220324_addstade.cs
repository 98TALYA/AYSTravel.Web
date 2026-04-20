using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AYSTravel.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class addstade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {



          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
     name: "FK_Matchs_Villes_VilleId",
     table: "Matchs");

            migrationBuilder.DropIndex(
                name: "IX_Matchs_VilleId",
                table: "Matchs");

            migrationBuilder.DropColumn(
                name: "VilleId",
                table: "Matchs");
        }
    }
}
