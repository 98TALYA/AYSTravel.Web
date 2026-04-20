using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AYSTravel.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class addimageurl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Restaurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageStade",
                table: "Matchs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Restaurations");

            migrationBuilder.DropColumn(
                name: "ImageStade",
                table: "Matchs");
        }
    }
}
