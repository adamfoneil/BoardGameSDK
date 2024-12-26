This is sort of a companion/iteration of [CardGameSDK](https://github.com/adamfoneil/CardGameSDK) for making 2D online board games.
- the [Abstractions](https://github.com/adamfoneil/BoardGameSDK/tree/master/Abstractions) project has low-level stuff:
  - [GameState](https://github.com/adamfoneil/BoardGameSDK/blob/master/Abstractions/GameState.cs) is where you implement the rules of a game
  - [GameStateManager](https://github.com/adamfoneil/BoardGameSDK/blob/master/Abstractions/GameStateManager.cs) handles persistence and initializing new games
  - [Player](https://github.com/adamfoneil/BoardGameSDK/blob/master/Abstractions/Player.cs), [Piece](https://github.com/adamfoneil/BoardGameSDK/blob/master/Abstractions/Piece.cs), and [Location](https://github.com/adamfoneil/BoardGameSDK/blob/master/Abstractions/Location.cs) round out the basics
- the [Database](https://github.com/adamfoneil/BoardGameSDK/tree/master/Database) project has all the persistence infrastructure, using EF Core

An actual "game" is [Mini1PGame](https://github.com/adamfoneil/BoardGameSDK/tree/master/BlazorApp/Components/Games/Mini1PGame) but all it does now is allow simple movement of pieces around a board:
- [MiniGameState](https://github.com/adamfoneil/BoardGameSDK/blob/master/BlazorApp/Components/Games/Mini1PGame/MiniGameState.cs) has the rules
- [StateManager](https://github.com/adamfoneil/BoardGameSDK/blob/master/BlazorApp/Components/Games/Mini1PGame/StateManager.cs) implements game creation and persistence
- The [Page](https://github.com/adamfoneil/BoardGameSDK/blob/master/BlazorApp/Components/Games/Mini1PGame/Page.razor) displays the game, using the `GameGrid` component below

The core Blazor components:
- [GameGrid](https://github.com/adamfoneil/BoardGameSDK/blob/master/BlazorApp/Components/GameGrid.razor) displays the game and handles click events
