using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AYSTravel.Web.Data.Migrations
{
    public partial class stade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ✅ Supprimer ImageStade
            migrationBuilder.Sql(@"
                DECLARE @constraint NVARCHAR(200)
                SELECT @constraint = name FROM sys.default_constraints
                WHERE parent_object_id = OBJECT_ID('Matchs')
                AND parent_column_id = (
                    SELECT column_id FROM sys.columns
                    WHERE object_id = OBJECT_ID('Matchs') AND name = 'ImageStade'
                )
                IF @constraint IS NOT NULL
                    EXEC('ALTER TABLE Matchs DROP CONSTRAINT ' + @constraint)

                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Matchs' AND COLUMN_NAME = 'ImageStade')
                    ALTER TABLE Matchs DROP COLUMN ImageStade
            ");

            // ✅ Supprimer VilleId et ses dépendances
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys 
                    WHERE name = 'FK_Matchs_Villes_VilleId')
                    ALTER TABLE Matchs DROP CONSTRAINT FK_Matchs_Villes_VilleId

                IF EXISTS (SELECT 1 FROM sys.indexes 
                    WHERE name = 'IX_Matchs_VilleId' AND object_id = OBJECT_ID('Matchs'))
                    DROP INDEX IX_Matchs_VilleId ON Matchs

                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Matchs' AND COLUMN_NAME = 'VilleId')
                BEGIN
                    DECLARE @c2 NVARCHAR(200)
                    SELECT @c2 = name FROM sys.default_constraints
                    WHERE parent_object_id = OBJECT_ID('Matchs')
                    AND parent_column_id = (
                        SELECT column_id FROM sys.columns
                        WHERE object_id = OBJECT_ID('Matchs') AND name = 'VilleId'
                    )
                    IF @c2 IS NOT NULL
                        EXEC('ALTER TABLE Matchs DROP CONSTRAINT ' + @c2)
                    ALTER TABLE Matchs DROP COLUMN VilleId
                END
            ");

            // ✅ Créer Stades si elle n'existe pas
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = 'Stades')
                BEGIN
                    CREATE TABLE Stades (
                        Id INT IDENTITY(1,1) NOT NULL,
                        Name NVARCHAR(MAX) NOT NULL,
                        ImageStade NVARCHAR(MAX) NOT NULL,
                        VilleId INT NOT NULL,
                        CONSTRAINT PK_Stades PRIMARY KEY (Id),
                        CONSTRAINT FK_Stades_Villes_VilleId FOREIGN KEY (VilleId) 
                            REFERENCES Villes(Id) ON DELETE CASCADE
                    )
                    CREATE INDEX IX_Stades_VilleId ON Stades(VilleId)
                END
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
                    WHERE TABLE_NAME = 'Matchs' AND COLUMN_NAME = 'StadeId')
                BEGIN
                    DECLARE @c NVARCHAR(200)
                    SELECT @c = name FROM sys.default_constraints
                    WHERE parent_object_id = OBJECT_ID('Matchs')
                    AND parent_column_id = (
                        SELECT column_id FROM sys.columns
                        WHERE object_id = OBJECT_ID('Matchs') AND name = 'StadeId'
                    )
                    IF @c IS NOT NULL
                        EXEC('ALTER TABLE Matchs DROP CONSTRAINT ' + @c)
                    ALTER TABLE Matchs DROP COLUMN StadeId
                END

                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = 'Stades')
                    DROP TABLE Stades

                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Matchs' AND COLUMN_NAME = 'VilleId')
                    ALTER TABLE Matchs ADD VilleId INT NOT NULL DEFAULT 0

                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys 
                    WHERE name = 'FK_Matchs_Villes_VilleId')
                    ALTER TABLE Matchs ADD CONSTRAINT FK_Matchs_Villes_VilleId
                        FOREIGN KEY (VilleId) REFERENCES Villes(Id)

                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Matchs' AND COLUMN_NAME = 'ImageStade')
                    ALTER TABLE Matchs ADD ImageStade NVARCHAR(MAX) NOT NULL DEFAULT ''
            ");
        }
    }
}