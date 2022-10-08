## Memory Game

A classic memory card console game built with C#.

This project is a part of me and [Laddeus's](https://github.com/Laddeus) C# & .NET Environment OOP course.

<p align="center">
  <img src="https://user-images.githubusercontent.com/10600102/146855140-d07417a1-f097-44ff-be1f-69b5f764555f.png" alt="screenshot" />
</p>

The game can be played single player (vs. the PC) or multiplayer (PvP).

The goal of the game is to get more matching pairs than your opponent.

For the PC we've implemented the following AI:

- The AI has a determined chance of "remembering" cards that have been revealed on the board (this determines the difficulty).

- If the AI has a matching pair in it's memory, it will attempt to flip those before going for the next random card.
