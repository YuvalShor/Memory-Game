using System;
using System.Threading;

namespace B20_Ex02
{
    internal class GameUI
    {
        private Menu m_Menu;
        private GameLogic m_GameLogic;

        public static int GetNumberInRange(int i_RangeStart, int i_RangeEnd)
        {
            Console.WriteLine("Please enter a value (must be between {0} and {1}):", i_RangeStart, i_RangeEnd);

            bool isNumber = int.TryParse(Console.ReadLine(), out int userInput);
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

                if (isNumber)
                {
                    isWithinRange = userInput >= 4 && userInput <= 6;
                }
            }

            return userInput;
        }

        public GameUI()
        {
            m_Menu = new Menu();
        }

        public void StartGame()
        {
            runMenu();
            runGame();
            gameOver();
        }

        private void runGame()
        {
            while(m_GameLogic.IsGameRunning)
            {
                string playerInput;

                DrawData();

                if (m_GameLogic.CurrentPlayer.Type == ePlayerTypes.Human)
                {
                    playerInput = GetPlayerInput(m_GameLogic.Letters);
                }
                else
                {
                    System.Threading.Thread.Sleep(2000);
                    playerInput = m_GameLogic.CalculateAiInput();
                }

                if(playerInput == "Q")
                {
                    StopGame();
                }
                else
                {
                    m_GameLogic.UpdateData(playerInput);

                    if(m_GameLogic.SelectionNotMatching)
                    {
                        DrawData();
                        m_GameLogic.TogglePlayer();
                    }
                }
            }

            DrawData();
        }

        private void StopGame()
        {
            DrawText("Goodbye!");
            System.Threading.Thread.Sleep(2000);
            Environment.Exit(0);
        }

        private void gameOver()
        {
            DrawText(m_GameLogic.GetGameOverStatus());

            bool restartNeeded = CheckRestart();

            if (restartNeeded)
            {
                ClearWindow();
                RestartGame();
            }
            else
            {
                StopGame();
            }
        }

        private void RestartGame()
        {
            int height;
            int width;

            m_Menu.GetBoardSize(out height, out width);
            m_GameLogic.ResetRound(height, width);
            StartGame();
        }

        private void runMenu()
        {
            string playerName1, playerName2;
            int width, height;
            bool isPlayerVsPlayer = m_Menu.Run(out playerName1, out playerName2, out width, out height);
            Player playerOne = new Player(playerName1, ePlayerTypes.Human);
            ePlayerTypes type = isPlayerVsPlayer ? ePlayerTypes.Human : ePlayerTypes.CPU;
            Player playerTwo = new Player(playerName2, type);
            m_GameLogic = new GameLogic(playerOne, playerTwo, width, height, isPlayerVsPlayer);
        }

        public void DrawData()
        {
            Ex02.ConsoleUtils.Screen.Clear();

            int height = m_GameLogic.Letters.GetLength(0);
            int width = m_GameLogic.Letters.GetLength(1);
            int amountOfEquals = (width * 4) + 1;
            string equalLine = new string('=', amountOfEquals);

            Console.WriteLine(@"{0}'s turn", m_GameLogic.CurrentPlayer.PlayerName);
            Console.WriteLine(m_GameLogic.GetScoreboard());

            drawTopLetterRow(width);

            Console.WriteLine("  " + equalLine);

            for (int i = 0; i < height; i++)
            {
                drawRowAtIndex(i, m_GameLogic.Letters);
                Console.WriteLine("  " + equalLine);
            }

            if(m_GameLogic.SelectionNotMatching)
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

        private void drawTopLetterRow(int i_LengthOfRow)
        {
            Console.Write(" ");
            for (int i = 0; i < i_LengthOfRow; i++)
            {
                Console.Write("   " + (char)(i + 'A'));
            }

            Console.WriteLine();
        }

        private void drawRowAtIndex(int i_Index, BoardLetter[,] i_HidenLetterRow)
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

        public void ClearWindow()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }
    }
}
