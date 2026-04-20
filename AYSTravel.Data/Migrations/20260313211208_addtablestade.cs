using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AYSTravel.Web.Data.Migrations
{
    public partial class addtablestade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Renommer Name en Nom si nécessaire
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Stades' AND COLUMN_NAME = 'Name')
                    EXEC sp_rename 'dbo.Stades.Name', 'Nom', 'COLUMN'
            ");

            // Renommer ImageStade en ImageUrl si nécessaire
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Stades' AND COLUMN_NAME = 'ImageStade')
                    EXEC sp_rename 'dbo.Stades.ImageStade', 'ImageUrl', 'COLUMN'
            ");

            // Créer index si nécessaire
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes 
                    WHERE name = 'IX_Matchs_StadeId' AND object_id = OBJECT_ID('Matchs'))
                    CREATE INDEX IX_Matchs_StadeId ON Matchs(StadeId)
            ");

            // Créer FK si nécessaire
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys 
                    WHERE name = 'FK_Matchs_Stades_StadeId')
                    ALTER TABLE Matchs ADD CONSTRAINT FK_Matchs_Stades_StadeId 
                        FOREIGN KEY (StadeId) REFERENCES Stades(Id) ON DELETE CASCADE
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys 
                    WHERE name = 'FK_Matchs_Stades_StadeId')
                    ALTER TABLE Matchs DROP CONSTRAINT FK_Matchs_Stades_StadeId

                IF EXISTS (SELECT 1 FROM sys.indexes 
                    WHERE name = 'IX_Matchs_StadeId' AND object_id = OBJECT_ID('Matchs'))
                    DROP INDEX IX_Matchs_StadeId ON Matchs

                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Stades' AND COLUMN_NAME = 'Nom')
                    EXEC sp_rename 'dbo.Stades.Nom', 'Name', 'COLUMN'

                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Stades' AND COLUMN_NAME = 'ImageUrl')
                    EXEC sp_rename 'dbo.Stades.ImageUrl', 'ImageStade', 'COLUMN'
            ");
        }
    }
}