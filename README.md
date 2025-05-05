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

Another "game" is [Mini2PGame](https://github.com/adamfoneil/BoardGameSDK/tree/master/BlazorApp/Components/Games/Mini2PGame). This is very similar to the 1P game, but here two players alternate moving 3 pieces 5 spaces per turn.

![mini2pgame](https://github.com/user-attachments/assets/790f5309-02bf-403c-bb3c-a4c8a98bb344)

The core Blazor components:
- [GameGrid](https://github.com/adamfoneil/BoardGameSDK/blob/master/BlazorApp/Components/GameGrid.razor) displays the game and handles click events

# Realtime updates across browsers
A key part of this is making sure events in one browser reflect in other players' browsers. Here's my approach to this:
- abstract class [EventRelay](https://github.com/adamfoneil/BoardGameSDK/blob/master/Abstractions/EventRelay.cs) is used as a singleton, acting as the core event broker for any number of games in progress. There are two parts to this:
  - when it's your turn and you make a move, your action calls this [StateChangedAsync](https://github.com/adamfoneil/BoardGameSDK/blob/master/Abstractions/EventRelay.cs#L27) method
  - other players must be subscribed to the [StateChanged](https://github.com/adamfoneil/BoardGameSDK/blob/master/Abstractions/EventRelay.cs#L22) event. The data related to the player action is pushed through here.
- individual games override the `EventRelay` class -- for example [ApplicationEventRelay](https://github.com/adamfoneil/BoardGameSDK/blob/master/BlazorApp/ApplicationEventRelay.cs), where they provide a way to query the active players of a game, so the `EventRelay` will know whom to notify that a play has been made
- a specific [game page](https://github.com/adamfoneil/BoardGameSDK/blob/master/BlazorApp/Components/Games/Mini2PGame/Page.razor#L6) injects `ApplicationEventRelay` and [subscribes](https://github.com/adamfoneil/BoardGameSDK/blob/master/BlazorApp/Components/Games/Mini2PGame/Page.razor#L50) to the state change event.
- moreover, when a player makes a move, [SaveState](https://github.com/adamfoneil/BoardGameSDK/blob/master/BlazorApp/Components/Games/Mini2PGame/Page.razor#L75) is called, which persists data about the move, then the event is broadcast or relayed to other active players.
