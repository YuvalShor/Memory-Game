using System;

namespace B20_Ex02
{
    internal class GameUI
    {
        public void DrawData(BoardLetter [,] i_Letters, Player i_CurrentPlayer, string i_ScoreBoard, bool i_SelectionNotMatching)
        {
            Ex02.ConsoleUtils.Screen.Clear();

            int height = i_Letters.GetLength(0);
            int width = i_Letters.GetLength(1);

            int amountOfEquals = width * 4 + 1;
            string equalLine = new string('=', amountOfEquals);

            Console.WriteLine(@"{0}'s turn", i_CurrentPlayer.PlayerName);
            Console.WriteLine(i_ScoreBoard);

            drawTopLetterRow(width);
            Console.WriteLine("  " + equalLine);

            for (int i = 0; i < height; i++)
            {
                drawRowAtIndex(i, i_Letters);
                Console.WriteLine("  " + equalLine);
            }

            if(i_SelectionNotMatching)
            {
                Console.WriteLine("Mismatch, remember this!");
                System.Threading.Thread.Sleep(2000);
            }
        }

        private bool validateInput(BoardLetter[,] i_Letters, string i_UserInput)
        {
            bool returnValue = true;
            int width = i_Letters.GetLength(1);
            int height = i_Letters.GetLength(0);
            char maxAllowedLetter = (char)('A' + width - 1);
            char maxAllowedDigit = (char)('0' + height);

            if(i_UserInput != "Q")
            {
                if(i_UserInput == null)
                {
                    Console.WriteLine("Input must not be empty");
                    returnValue = false;
                }
                else if(i_UserInput.Length != 2)
                {
                    Console.WriteLine("Input must have exactly 2 characters");
                    returnValue = false;
                }
                else
                {
                    char letter = i_UserInput[0];
                    char digit = i_UserInput[1];

                    if(letter < 'A' || letter > maxAllowedLetter)
                    {
                        Console.WriteLine(
                            "First character of input must be a character between A-{0}",
                            maxAllowedLetter);
                        returnValue = false;
                    }

                    if(digit < '1' || digit > maxAllowedDigit)
                    {
                        Console.WriteLine("Second character of input must be a digit between 1-{0}", maxAllowedDigit);
                        returnValue = false;
                    }
                }

                if(returnValue)
                {
                    int column = i_UserInput[0] - 'A';
                    int row = i_UserInput[1] - '1';

                    returnValue = i_Letters[row, column].IsHidden;

                    if(!returnValue)
                    {
                        Console.WriteLine("Square already revealed");
                    }
                }
            }

            return returnValue;
        }

        private static void drawTopLetterRow(int i_LengthOfRow)
        {
            Console.Write(" ");
            for (int i = 0; i < i_LengthOfRow; i++)
            {
                Console.Write("   " + (char)(i + 'A'));

            }

            Console.WriteLine();
        }

        private static void drawRowAtIndex(int i_Index, BoardLetter[,] i_HidenLetterRow)
        {
            int width = i_HidenLetterRow.GetLength(1);

            Console.Write((i_Index + 1) + " |");
            for (int j = 0; j < width; j++)
            {
                BoardLetter currentBoardLetter = i_HidenLetterRow[i_Index, j];

                Console.Write(" ");
                Console.Write(currentBoardLetter.IsHidden ? ' ' : currentBoardLetter.Letter);
                Console.Write(" |");
            }
            Console.WriteLine();
        }

        public string GetPlayerInput(BoardLetter[,] i_Letters)
        {
            Console.WriteLine("Please choose the next square: ");
            string userInput = Console.ReadLine();

            while(!validateInput(i_Letters, userInput))
            {
                Console.WriteLine("Please choose the next square: ");
                userInput = Console.ReadLine();
            }

            return userInput;
        }

        public void DrawText(string i_GameOverStatus)
        {
            Console.WriteLine(i_GameOverStatus);
        }

        public bool CheckRestart()
        {
            Console.WriteLine();
            Console.WriteLine("Another round? (Y/N)");

            char userInput; 
            bool isValid = char.TryParse(Console.ReadLine(), out userInput);

            while(!isValid)
            {
                Console.WriteLine("Invalid input, please enter Y/N");
                Console.WriteLine("Another round? (Y/N)");
                isValid = char.TryParse(Console.ReadLine(), out userInput);

                isValid = userInput == 'Y' || userInput == 'N';
            }

            return userInput == 'Y';
        }

        public static int GetNumberInRange(int i_RangeStart, int i_RangeEnd)
        {
            Console.WriteLine("Please enter a value (must be between {0} and {1}):", i_RangeStart, i_RangeEnd);

            int userInput;
            bool isNumber = int.TryParse(Console.ReadLine(), out userInput);
            bool isWithinRange = false;

            if (isNumber)
            {
                isWithinRange = userInput >= 4 && userInput <= 6;
            }

            while (!isNumber || !isWithinRange)
            {
                Console.WriteLine("Incorrect value");
                Console.WriteLine("Please enter a value (must be between 4 and 6):");
                isNumber = int.TryParse(Console.ReadLine(), out userInput);
            }

            return userInput;
        }

        public void ClearWindow()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }
    }
}
