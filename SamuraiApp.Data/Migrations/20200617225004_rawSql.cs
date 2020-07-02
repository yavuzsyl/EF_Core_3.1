using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class rawSql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HorseId",
                table: "Samurais");

            migrationBuilder.Sql(
                @"CREATE FUNCTION [dbo].[EarliestBattleFoughtBySamurai](@samuraiId int)
                  RETURNS char(30) AS
                  BEGIN
                    DECLARE @ret char(30)
                    SELECT TOP 1 @ret = Name
                    FROM Battles
                    WHERE Battles.Id IN(SELECT BattleId
                                       FROM dbo.SamuraiBattles
                                      WHERE SamuraiId = @samuraiId)
                    ORDER BY StartDate
                    RETURN @ret
                    END"
            );
            migrationBuilder.Sql(
                @"CREATE OR ALTER VIEW dbo.SamuraiBattleStats
                  AS
                  SELECT dbo.Samurais.Name,
                  COUNT(dbo.SamuraiBattles.BattleId) AS NumberOfBattles,
                          dbo.EarliestBattleFoughtBySamurai(MIN(dbo.Samurais.Id)) AS EarliestBattle
                  FROM dbo.SamuraiBattles INNER JOIN
                       dbo.Samurais ON dbo.SamuraiBattles.SamuraiId = dbo.Samurais.Id
                  GROUP BY dbo.Samurais.Name, dbo.SamuraiBattles.SamuraiId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HorseId",
                table: "Samurais",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"DROP FUNCTION [dbo].[EarliestBattleFoughtBySamurai]");
            migrationBuilder.Sql(@"DROP VIEW dbo.SamuraiBattleStats");
        }
    }
}
