using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameReadyPlayerGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_ReadyPlayers_Game_UserId",
                table: "ReadyPlayers");

            migrationBuilder.RenameColumn(
                name: "Game",
                table: "ReadyPlayers",
                newName: "GameType");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ReadyPlayers_GameType_UserId",
                table: "ReadyPlayers",
                columns: new[] { "GameType", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_ReadyPlayers_GameType_UserId",
                table: "ReadyPlayers");

            migrationBuilder.RenameColumn(
                name: "GameType",
                table: "ReadyPlayers",
                newName: "Game");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ReadyPlayers_Game_UserId",
                table: "ReadyPlayers",
                columns: new[] { "Game", "UserId" });
        }
    }
}
