using Abstractions;

namespace BlazorApp.Components.Games.Mini2PGame;

public class State : GameState<Player, Piece>
{
	public override uint Width => 20;

	public override uint Height => 30;

	protected override Location[] GetValidMovesInner(Player player, Piece piece)
	{
		throw new NotImplementedException();
	}

	protected override (string currentPlayer, string? logTemplate, object?[] logParams) PlayInner(Player player, Piece piece, Location location)
	{
		throw new NotImplementedException();
	}

	protected override (bool result, string? reason) ValidateInner(Player player, Piece piece, Location location)
	{
		throw new NotImplementedException();
	}
}
