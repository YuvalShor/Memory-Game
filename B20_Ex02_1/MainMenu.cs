using System;
using System.Runtime.InteropServices;

namespace B20_Ex02
{
    internal class MainMenu
    {
        public void Run(out string o_PlayerName1, out string o_PlayerName2, out int o_Width, out int o_Height)
        {
            Console.WriteLine("Welcome to Memory Game!");
            Console.WriteLine("Please enter your name:");
            o_PlayerName1 = Console.ReadLine();

            Console.WriteLine("Hello {0}! Please choose a game mode:", o_PlayerName1);
            o_PlayerName2 = SelectGameMode();

            GetBoardSize(out o_Height, out o_Width);
        }

        public void GetBoardSize(out int o_Height, out int o_Width)
        {
            bool isBoardSizeEven = false;
            o_Height = 6;
            o_Width = 6;

            while (!isBoardSizeEven)
            {
                Console.WriteLine("Choose board height:");
                o_Height = GameUI.GetNumberInRange(4, 6);

                Console.WriteLine("Choose board width:");
                o_Width = GameUI.GetNumberInRange(4, 6);

                isBoardSizeEven = o_Height * o_Width % 2 == 0;
            }
        }

        private string SelectGameMode()
        {
            Console.WriteLine("1) Player vs. Player");
            Console.WriteLine("2) Player vs. Computer");

            string playerSelection = validateSelection();
            string playerName2;

            if (playerSelection == "1")
            {
                Console.WriteLine("Please enter player 2 name:");
                playerName2 = Console.ReadLine();

                MemoryGame.GameMode = eGameModes.PlayerVsPlayer;
            }
            else
            {
                playerName2 = "Computer";
                MemoryGame.GameMode = eGameModes.PlayerVsAi;
            }

            return playerName2;
        }

        private string validateSelection()
        {
            string playerSelection = Console.ReadLine();

            while(playerSelection != "1" && playerSelection != "2")
            {
                Console.WriteLine("Wrong option entered. Please enter a different number: ");
                playerSelection = Console.ReadLine();
            }

            return playerSelection;
        }
    }
}
