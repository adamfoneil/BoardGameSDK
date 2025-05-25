using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class PriorGameStates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriorGameState_GameInstances_GameInstanceId",
                table: "PriorGameState");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriorGameState",
                table: "PriorGameState");

            migrationBuilder.RenameTable(
                name: "PriorGameState",
                newName: "PriorGameStates");

            migrationBuilder.RenameIndex(
                name: "IX_PriorGameState_GameInstanceId_MoveNumber",
                table: "PriorGameStates",
                newName: "IX_PriorGameStates_GameInstanceId_MoveNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriorGameStates",
                table: "PriorGameStates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PriorGameStates_GameInstances_GameInstanceId",
                table: "PriorGameStates",
                column: "GameInstanceId",
                principalTable: "GameInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriorGameStates_GameInstances_GameInstanceId",
                table: "PriorGameStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriorGameStates",
                table: "PriorGameStates");

            migrationBuilder.RenameTable(
                name: "PriorGameStates",
                newName: "PriorGameState");

            migrationBuilder.RenameIndex(
                name: "IX_PriorGameStates_GameInstanceId_MoveNumber",
                table: "PriorGameState",
                newName: "IX_PriorGameState_GameInstanceId_MoveNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriorGameState",
                table: "PriorGameState",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PriorGameState_GameInstances_GameInstanceId",
                table: "PriorGameState",
                column: "GameInstanceId",
                principalTable: "GameInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
