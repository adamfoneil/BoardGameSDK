namespace BlazorApp;

public class PlayArgs
{
	public string? LogMessage { get; init; }
	public object?[] LogParams { get; init; } = [];
}
