using Abstractions;

namespace BlazorApp.Components.Games.MiniGame;

public class MiniGamePlayer : Player
{
}

public class MiniGamePiece : Piece
{
}

/// <summary>
/// you have 3 pieces, and you can move up to 5 spaces on each turn across your pieces, but not overlapping.
/// single player with no objective besides taking 7 turns
/// </summary>
public class MiniGameState : GameState<MiniGamePlayer, MiniGamePiece>
{
	public const uint DefinedWidth = 20;
	public const uint DefinedHeight = 20;

	public const int SpacesPerTurn = 5;
	public const int TurnsPerGame = 7;

	public override uint Width { get; } = DefinedWidth;
	public override uint Height { get; } = DefinedHeight;

	protected override (string? logTemplate, object?[] logParams) PlayInner(MiniGamePlayer player, MiniGamePiece piece, (int x, int y) location)
	{
		throw new NotImplementedException();
	}

	protected override (bool result, string? reason) ValidateInner(MiniGamePlayer player, MiniGamePiece piece, (int x, int y) location)
	{
		throw new NotImplementedException();
	}
}
