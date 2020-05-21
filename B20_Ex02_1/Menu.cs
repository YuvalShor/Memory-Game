using System;
using System.Runtime.InteropServices;

namespace B20_Ex02
{
    internal class Menu
    {
        public eGameModes Run(out string o_PlayerName1, out string o_PlayerName2, out int o_Width, out int o_Height)
        {
            Console.WriteLine("Welcome to Memory Game!");
            Console.WriteLine("Please enter your name:");
            o_PlayerName1 = Console.ReadLine();

            Console.WriteLine("Hello {0}! Please choose a game mode:", o_PlayerName1);
            eGameModes desiredGameMode = selectGameMode(out o_PlayerName2);

            GetBoardSize(out o_Height, out o_Width);

            return desiredGameMode;
        }

        public void GetBoardSize(out int io_Height, out int io_Width)
        {
            bool isBoardSizeEven = false;
            io_Height = 6;
            io_Width = 6;

            while (!isBoardSizeEven)
            {
                Console.WriteLine("Choose board height:");
                io_Height = GameUI.GetNumberInRange(4, 6);

                Console.WriteLine("Choose board width:");
                io_Width = GameUI.GetNumberInRange(4, 6);

                isBoardSizeEven = (io_Height * io_Width) % 2 == 0;

                if(!isBoardSizeEven)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input, board size must be even.");
                    Console.WriteLine();
                }
            }
        }

        private eGameModes selectGameMode(out string o_PlayerTwoName)
        {
            eGameModes selectedGameMode = eGameModes.PlayerVsComputer;

            Console.WriteLine("1) Player vs. Player");
            Console.WriteLine("2) Player vs. Computer");

            string playerSelection = validateGameModeInput();

            o_PlayerTwoName = "Computer";

            if (playerSelection == "1")
            {
                Console.WriteLine("Please enter player 2 name:");
                o_PlayerTwoName = Console.ReadLine();
                selectedGameMode = eGameModes.PlayerVsPlayer;
            }

            return selectedGameMode;
        }

        private string validateGameModeInput()
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
