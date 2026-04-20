using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AYSTravel.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToVille : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activites_Ville_VilleId",
                table: "Activites");

            

            migrationBuilder.DropForeignKey(
                name: "FK_Monuments_Ville_VilleId",
                table: "Monuments");

            migrationBuilder.DropForeignKey(
                name: "FK_MoyensTransports_Ville_VilleId",
                table: "MoyensTransports");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurations_Ville_VilleId",
                table: "Restaurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ville",
                table: "Ville");

            migrationBuilder.RenameTable(
                name: "Ville",
                newName: "Villes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Villes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Villes",
                table: "Villes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activites_Villes_VilleId",
                table: "Activites",
                column: "VilleId",
                principalTable: "Villes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Villes_VilleId",
                table: "Matchs",
                column: "VilleId",
                principalTable: "Villes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Monuments_Villes_VilleId",
                table: "Monuments",
                column: "VilleId",
                principalTable: "Villes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoyensTransports_Villes_VilleId",
                table: "MoyensTransports",
                column: "VilleId",
                principalTable: "Villes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurations_Villes_VilleId",
                table: "Restaurations",
                column: "VilleId",
                principalTable: "Villes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activites_Villes_VilleId",
                table: "Activites");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Villes_VilleId",
                table: "Matchs");

            migrationBuilder.DropForeignKey(
                name: "FK_Monuments_Villes_VilleId",
                table: "Monuments");

            migrationBuilder.DropForeignKey(
                name: "FK_MoyensTransports_Villes_VilleId",
                table: "MoyensTransports");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurations_Villes_VilleId",
                table: "Restaurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Villes",
                table: "Villes");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Villes");

            migrationBuilder.RenameTable(
                name: "Villes",
                newName: "Ville");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ville",
                table: "Ville",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activites_Ville_VilleId",
                table: "Activites",
                column: "VilleId",
                principalTable: "Ville",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Ville_VilleId",
                table: "Matchs",
                column: "VilleId",
                principalTable: "Ville",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Monuments_Ville_VilleId",
                table: "Monuments",
                column: "VilleId",
                principalTable: "Ville",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoyensTransports_Ville_VilleId",
                table: "MoyensTransports",
                column: "VilleId",
                principalTable: "Ville",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurations_Ville_VilleId",
                table: "Restaurations",
                column: "VilleId",
                principalTable: "Ville",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
