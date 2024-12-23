using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class PlayerTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameInstancePlayer_AspNetUsers_UserId",
                table: "GameInstancePlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_GameInstancePlayer_GameInstances_GameInstanceId",
                table: "GameInstancePlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadyPlayer_AspNetUsers_UserId",
                table: "ReadyPlayer");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ReadyPlayer_Game_UserId",
                table: "ReadyPlayer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadyPlayer",
                table: "ReadyPlayer");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_GameInstancePlayer_GameInstanceId_UserId",
                table: "GameInstancePlayer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameInstancePlayer",
                table: "GameInstancePlayer");

            migrationBuilder.RenameTable(
                name: "ReadyPlayer",
                newName: "ReadyPlayers");

            migrationBuilder.RenameTable(
                name: "GameInstancePlayer",
                newName: "GameInstancePlayers");

            migrationBuilder.RenameIndex(
                name: "IX_ReadyPlayer_UserId",
                table: "ReadyPlayers",
                newName: "IX_ReadyPlayers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GameInstancePlayer_UserId",
                table: "GameInstancePlayers",
                newName: "IX_GameInstancePlayers_UserId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ReadyPlayers_Game_UserId",
                table: "ReadyPlayers",
                columns: new[] { "Game", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadyPlayers",
                table: "ReadyPlayers",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_GameInstancePlayers_GameInstanceId_UserId",
                table: "GameInstancePlayers",
                columns: new[] { "GameInstanceId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameInstancePlayers",
                table: "GameInstancePlayers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameInstancePlayers_AspNetUsers_UserId",
                table: "GameInstancePlayers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameInstancePlayers_GameInstances_GameInstanceId",
                table: "GameInstancePlayers",
                column: "GameInstanceId",
                principalTable: "GameInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadyPlayers_AspNetUsers_UserId",
                table: "ReadyPlayers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameInstancePlayers_AspNetUsers_UserId",
                table: "GameInstancePlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_GameInstancePlayers_GameInstances_GameInstanceId",
                table: "GameInstancePlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadyPlayers_AspNetUsers_UserId",
                table: "ReadyPlayers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ReadyPlayers_Game_UserId",
                table: "ReadyPlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadyPlayers",
                table: "ReadyPlayers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_GameInstancePlayers_GameInstanceId_UserId",
                table: "GameInstancePlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameInstancePlayers",
                table: "GameInstancePlayers");

            migrationBuilder.RenameTable(
                name: "ReadyPlayers",
                newName: "ReadyPlayer");

            migrationBuilder.RenameTable(
                name: "GameInstancePlayers",
                newName: "GameInstancePlayer");

            migrationBuilder.RenameIndex(
                name: "IX_ReadyPlayers_UserId",
                table: "ReadyPlayer",
                newName: "IX_ReadyPlayer_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GameInstancePlayers_UserId",
                table: "GameInstancePlayer",
                newName: "IX_GameInstancePlayer_UserId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ReadyPlayer_Game_UserId",
                table: "ReadyPlayer",
                columns: new[] { "Game", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadyPlayer",
                table: "ReadyPlayer",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_GameInstancePlayer_GameInstanceId_UserId",
                table: "GameInstancePlayer",
                columns: new[] { "GameInstanceId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameInstancePlayer",
                table: "GameInstancePlayer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameInstancePlayer_AspNetUsers_UserId",
                table: "GameInstancePlayer",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameInstancePlayer_GameInstances_GameInstanceId",
                table: "GameInstancePlayer",
                column: "GameInstanceId",
                principalTable: "GameInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadyPlayer_AspNetUsers_UserId",
                table: "ReadyPlayer",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
