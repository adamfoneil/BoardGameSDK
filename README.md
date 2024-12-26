This is sort of a companion/iteration of [CardGameSDK](https://github.com/adamfoneil/CardGameSDK) for making 2D online board games.
- the [Abstractions](https://github.com/adamfoneil/BoardGameSDK/tree/master/Abstractions) project has low-level stuff:
  - [GameState](https://github.com/adamfoneil/BoardGameSDK/blob/master/Abstractions/GameState.cs) is where you implement the rules of a game
  - [GameStateManager](https://github.com/adamfoneil/BoardGameSDK/blob/master/Abstractions/GameStateManager.cs) handles persistence and initializing new games
  - [Player](https://github.com/adamfoneil/BoardGameSDK/blob/master/Abstractions/Player.cs), [Piece](https://github.com/adamfoneil/BoardGameSDK/blob/master/Abstractions/Piece.cs), and [Location](https://github.com/adamfoneil/BoardGameSDK/blob/master/Abstractions/Location.cs) round out the basics
