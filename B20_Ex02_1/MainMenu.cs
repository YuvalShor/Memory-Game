using System;

namespace B20_Ex02
{
    internal class MainMenu
    {
        public static void Run(Player io_PlayerOne, Player io_PlayerTwo)
        {
            Ex02.ConsoleUtils.Screen.Clear();

            Console.WriteLine("Welcome to Memory Game!");
            Console.WriteLine("Please enter your name:");
            io_PlayerOne.Name = Console.ReadLine();
            Console.WriteLine("Hello {0}! Please choose game mode:", io_PlayerOne.Name);
            Console.WriteLine("1) Player vs. Player");
            Console.WriteLine("2) Player vs. Computer");

            char playerSelection = validateSelection();

            if(playerSelection == '1')
            {
                io_PlayerTwo.Type = ePlayerTypes.Human;

                Console.WriteLine("Please enter player 2 name:");
                io_PlayerTwo.Name = Console.ReadLine();

                MemoryGame.GameMode = eGameModes.PlayerVsPlayer;
            }
            else
            {
                io_PlayerTwo.Type = ePlayerTypes.CPU;
                MemoryGame.GameMode = eGameModes.PlayerVsAi;
            }

            Console.WriteLine("Please enter board height (must be between 4 and 6):");

            bool isNumber = int.TryParse(Console.ReadLine(), out MemoryGame.BoardHeight);
            bool isWithinRange = false;

            while(!isNumber || !isWithinRange)
            {
                Console.WriteLine("Incorrect height input.");
                Console.WriteLine("Please enter board height (must be between 4 and 6):");
                isNumber = int.TryParse(Console.ReadLine(), out MemoryGame.BoardHeight);

                if(isNumber)
                {
                    isWithinRange = MemoryGame.BoardHeight > 4 && MemoryGame.BoardHeight < 6;
                }
            }

            isNumber = int.TryParse(Console.ReadLine(), out MemoryGame.BoardWidth);
            isWithinRange = false;

            while(!isNumber || !isWithinRange)
            {
                Console.WriteLine("Incorrect width input.");
                Console.WriteLine("Please enter board width (must be between 4 and 6):");
                isNumber = int.TryParse(Console.ReadLine(), out MemoryGame.BoardWidth);

                if(isNumber)
                {
                    isWithinRange = MemoryGame.BoardWidth > 4 && MemoryGame.BoardWidth < 6;
                }
            }
        }

        private static char validateSelection()
        {
            char playerSelection = Console.ReadKey().KeyChar;

            while(playerSelection != '1' || playerSelection != '2')
            {
                Console.WriteLine("Wrong option entered. Please enter a different number: ");
                playerSelection = Console.ReadKey().KeyChar;
            }

            return playerSelection;
        }
    }
}
