using System;
using System.Linq;
using System.Threading;

namespace B20_Ex02
{
    internal class GameUI
    {
        private readonly Menu r_Menu;
        private GameLogic m_GameLogic;

        public static int GetNumberInRange(int i_RangeStart, int i_RangeEnd)
        {
            int userInput = i_RangeStart;
            bool isNumber = false;
            bool isWithinRange = false;

            while (!isNumber || !isWithinRange)
            {
                Console.WriteLine("Please enter a value (must be between {0} and {1}):", i_RangeStart, i_RangeEnd);
                isNumber = int.TryParse(Console.ReadLine(), out userInput);

                if (isNumber)
                {
                    isWithinRange = userInput >= i_RangeStart && userInput <= i_RangeEnd;
                }
                
                if(!isNumber || !isWithinRange)
                {
                    Console.WriteLine("Incorrect value");
                }
            }

            return userInput;
        }

        public GameUI()
        {
            r_Menu = new Menu();
        }

        public void StartGame()
        {
            if(GameLogic.CurrentGameState == eGameStates.Menu)
            {
                runMenu();
            }

            runGame();
            gameOver();
        }

        private void runGame()
        {
            while(GameLogic.CurrentGameState != eGameStates.GameOver)
            {
                DrawData();
                string playerInput = getPlayerInput();
                sendInputAndUpdateUI(playerInput);
            }
        }

        private string getPlayerInput()
        {
            string playerInput;

            if (m_GameLogic.CurrentPlayer.Type == ePlayerTypes.Human)
            {
                playerInput = GetHumanInput();
            }
            else
            {
                playerInput = m_GameLogic.CalculateAiInput().ToString();

                DrawText(
                    m_GameLogic.AiHasMatches
                        ? "Computer has got something in memory..."
                        : "Computer is guessing...");

                System.Threading.Thread.Sleep(3000);
            }

            return playerInput;
        }

        private void sendInputAndUpdateUI(string i_PlayerInput)
        {
            if(i_PlayerInput == "Q")
            {
                stopGame();
            }
            else
            {
                m_GameLogic.UpdateData(Cell.Parse(i_PlayerInput));

                if(m_GameLogic.SelectionNotMatching)
                {
                    DrawData();
                    m_GameLogic.TogglePlayer();
                }
            }
        }

        private void stopGame()
        {
            DrawText("Goodbye!");
            System.Threading.Thread.Sleep(2000);
            Environment.Exit(0);
        }

        private void gameOver()
        {
            DrawData();

            DrawText(m_GameLogic.GetGameOverStatus());

            bool restartNeeded = CheckRestart();

            if (restartNeeded)
            {
                ClearWindow();
                restartGame();
            }
            else
            {
                stopGame();
            }
        }

        private void restartGame()
        {
            r_Menu.GetBoardSize(out int height, out int width);
            m_GameLogic.ResetRound(height, width);
            StartGame();
        }

        private void runMenu()
        {
            string playerName1, playerName2;
            int width, height;
            eGameModes desiredGameMode = r_Menu.Run(out playerName1, out playerName2, out width, out height);
            Player playerOne = new Player(playerName1, ePlayerTypes.Human);
            ePlayerTypes type = desiredGameMode == eGameModes.PlayerVsPlayer ? ePlayerTypes.Human : ePlayerTypes.Computer;
            Player playerTwo = new Player(playerName2, type);
            
            m_GameLogic = new GameLogic(playerOne, playerTwo, width, height, desiredGameMode);
        }

        public void DrawData()
        {
            int amountOfEqualSigns = (m_GameLogic.Width * 4) + 1;
            string equalLine = new string('=', amountOfEqualSigns);

            ClearWindow();
            drawTurnStatus();
            drawTopLetterRow(m_GameLogic.Width);
            Console.WriteLine("  " + equalLine);

            for (int i = 0; i < m_GameLogic.Height; i++)
            {
                drawRowAtIndex(i, m_GameLogic.Letters);
                Console.WriteLine("  " + equalLine);
            }

            Console.WriteLine();

            if(m_GameLogic.SelectionNotMatching)
            {
                Console.WriteLine("Mismatch, remember this!");
                System.Threading.Thread.Sleep(2000);
            }
        }

        private void drawTurnStatus()
        {
            Console.WriteLine(
                "{0}'{1} turn",
                m_GameLogic.CurrentPlayer.PlayerName,
                m_GameLogic.CurrentPlayer.PlayerName.Last() == 's' ? string.Empty : "s");
            Console.WriteLine();
            Console.WriteLine(m_GameLogic.GetScoreboard());
            Console.WriteLine();
        }

        private bool validateUserSquareSelection(string i_UserInput)
        {
            bool isValidInput = true;
            
            if(i_UserInput != null)
            {
                i_UserInput = i_UserInput.ToUpper();

                if(i_UserInput != "Q")
                {
                    isValidInput = validateCellInputSelection(i_UserInput);

                    if(isValidInput)
                    {
                        isValidInput = checkSquareNotAlreadyRevealed(i_UserInput);
                    }
                }
            }
            else
            {
                Console.WriteLine("Input must not be empty");
                isValidInput = false;
            }

            return isValidInput;
        }

        private bool validateCellInputSelection(string i_UserCellInput)
        {
            bool isValidCell;

            if(i_UserCellInput.Length != 2)
            {
                Console.WriteLine("Input must have exactly 2 characters");
                isValidCell = false;
            }
            else
            {
                char letter = i_UserCellInput[0];
                char digit = i_UserCellInput[1];

                isValidCell = checkIfLetterInRange(letter) || checkIfDigitInRange(digit);
            }

            return isValidCell;
        }

        private bool checkIfLetterInRange(char i_Letter)
        {
            char maxAllowedLetter = (char)('A' + m_GameLogic.Width - 1);
            bool isValidLetter = true;

            if(i_Letter < 'A' || i_Letter > maxAllowedLetter)
            {
                Console.WriteLine(
                    "First character of input must be a character between A-{0}",
                    maxAllowedLetter);
                isValidLetter = false;
            }

            return isValidLetter;
        }

        private bool checkIfDigitInRange(char i_Digit)
        {
            char maxAllowedDigit = (char)('0' + m_GameLogic.Height);
            bool isValidDigit = true;

            if(i_Digit < '1' || i_Digit > maxAllowedDigit)
            {
                Console.WriteLine(
                    "Second character of input must be a digit between 1-{0}",
                    maxAllowedDigit);
                isValidDigit = false;
            }

            return isValidDigit;
        }

        private bool checkSquareNotAlreadyRevealed(string i_UserInput)
        {
            int column = i_UserInput[0] - 'A';
            int row = i_UserInput[1] - '1';

            bool isValidInput = m_GameLogic.Letters[row, column].IsHidden;

            if(!isValidInput)
            {
                Console.WriteLine("Square already revealed");
            }

            return isValidInput;
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

        private void drawRowAtIndex(int i_Index, BoardLetter[,] i_BoardLetterRow)
        {
            int width = i_BoardLetterRow.GetLength(1);

            Console.Write((i_Index + 1) + " |");
            for (int j = 0; j < width; j++)
            {
                BoardLetter currentBoardLetter = i_BoardLetterRow[i_Index, j];

                Console.Write(" ");
                Console.Write(currentBoardLetter.IsHidden ? ' ' : currentBoardLetter.Letter);
                Console.Write(" |");
            }

            Console.WriteLine();
        }

        public string GetHumanInput()
        {
            string userInput = string.Empty;
            bool isValidInput = false;

            while(!isValidInput)
            {
                userInput = getSquareToReveal();
                isValidInput = validateUserSquareSelection(userInput);

                if(!isValidInput)
                {
                    Console.WriteLine("Invalid input");
                }
            }

            return userInput;
        }

        private string getSquareToReveal()
        {
            Console.WriteLine("{0}, please choose a square to reveal:", m_GameLogic.CurrentPlayer.PlayerName);
            return Console.ReadLine();
        }

        public void DrawText(string i_TextToDraw)
        {
            Console.WriteLine(i_TextToDraw);
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
                userInput = char.ToUpper(userInput);
                isValid = userInput == 'Y' || userInput == 'N';
            }

            return char.ToUpper(userInput) == 'Y';
        }

        public void ClearWindow()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }
    }
}
