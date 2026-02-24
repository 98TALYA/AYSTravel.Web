using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AYSTravel.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVilleSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Horaire",
                table: "Matchs",
                newName: "Equipe2");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MoyensTransports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Equipe1",
                table: "Matchs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Activites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "MoyensTransports");

            migrationBuilder.DropColumn(
                name: "Equipe1",
                table: "Matchs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Activites");

            migrationBuilder.RenameColumn(
                name: "Equipe2",
                table: "Matchs",
                newName: "Horaire");
        }
    }
}
