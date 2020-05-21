using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace B20_Ex02
{
    internal class GameLogic
    {
        private const int k_DifficultyOdds = 80;
        private static eGameStates s_CurrentGameState = eGameStates.Menu;
        private static Random s_Random = new Random();
        private readonly eGameModes r_GameMode;
        private GameData m_GameData;
        private Dictionary<Cell, char> m_AiMemory;
        private Cell m_AiSelection;
        private BoardLetter m_FirstSelection;
        private BoardLetter m_SecondSelection;
        private bool m_FoundMatch;
        private bool m_IsFirstSelection;
        private bool m_SelectionNotMatching;
        private bool m_AiHasMatches;

        public static int GetRandomNumber(int i_RangeStart, int i_RangeEnd)
        { 
            return s_Random.Next(i_RangeStart, i_RangeEnd);
        }

        public GameLogic(Player i_Player1, Player i_Player2, int i_Width, int i_Height, eGameModes i_GameMode)
        {
            m_GameData = new GameData(i_Player1, i_Player2, i_Width, i_Height);
            m_GameData.InitializeBoardMatrix();
            r_GameMode = i_GameMode;
            m_SelectionNotMatching = false;
            m_IsFirstSelection = true;
            s_CurrentGameState = eGameStates.Running;
            m_FoundMatch = false;

            if(r_GameMode == eGameModes.PlayerVsComputer)
            {
                m_AiMemory = new Dictionary<Cell, char>();
            }
        }

        public static eGameStates CurrentGameState
        {
            get
            {
                return s_CurrentGameState;
            }

            set
            {
                s_CurrentGameState = value;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_GameData.CurrentPlayer;
            }

            set
            {
                m_GameData.CurrentPlayer = value;
            }
        }

        public BoardLetter[,] Letters
        {
            get
            {
                return m_GameData.Letters;
            }
        }

        public bool AiHasMatches
        {
            get
            {
                return m_AiHasMatches;
            }

            set
            {
                m_AiHasMatches = value;
            }
        }

        public bool SelectionNotMatching
        {
            get
            {
                return m_SelectionNotMatching;
            }

            set
            {
                m_SelectionNotMatching = value;
            }
        }

        public int Width
        {
            get
            {
                return m_GameData.Width;
            }
        }

        public int Height
        {
            get
            {
                return m_GameData.Height;
            }
        }

        public void UpdateData(Cell i_UserSelection)
        {
            if(!m_SelectionNotMatching)
            {
                updateNextTurn(i_UserSelection);
            }

            if ((m_GameData.PlayerOne.PlayerScore + m_GameData.PlayerTwo.PlayerScore) == (Width * Height) / 2)
            {
                s_CurrentGameState = eGameStates.GameOver;
            }
        }

        private void addToAiMemory(Cell i_CellToBeAdded)
        {
            if(!m_AiMemory.ContainsKey(i_CellToBeAdded))
            {
                m_AiMemory.Add(i_CellToBeAdded, Letters[i_CellToBeAdded.Row, i_CellToBeAdded.Column].Letter);
            }
        }

        private void updateNextTurn(Cell i_UserSelection)
        {
            m_SecondSelection = m_GameData.Letters[i_UserSelection.Row, i_UserSelection.Column];

            if(r_GameMode == eGameModes.PlayerVsComputer)
            {
                if(GameLogic.GetRandomNumber(0, 100) < k_DifficultyOdds)
                {
                    addToAiMemory(i_UserSelection);
                }
            }

            if (m_IsFirstSelection)
            {
                m_FirstSelection = m_SecondSelection;
                m_FirstSelection.IsHidden = false;
                m_IsFirstSelection = false;
            }
            else
            {
                m_SecondSelection.IsHidden = false;

                if (m_SecondSelection.Letter != m_FirstSelection.Letter)
                {
                    m_SelectionNotMatching = true;
                }
                else
                {
                    if (r_GameMode == eGameModes.PlayerVsComputer)
                    { 
                        removeLetterFromAiMemory(m_SecondSelection.Letter);
                    }

                    CurrentPlayer.PlayerScore++;
                }

                m_IsFirstSelection = true;
            }
        }

        private void removeLetterFromAiMemory(char i_SecondSelectionLetter)
        {
            foreach(var memorizedLetter in m_AiMemory)
            {
                if(memorizedLetter.Value == i_SecondSelectionLetter)
                {
                    m_AiMemory.Remove(memorizedLetter.Key);
                    break;
                }
            }
        }

        public string GetScoreboard()
        {
            return string.Format(
                "Score: {0} {1} - {2} {3}",
                m_GameData.PlayerOne.PlayerName,
                m_GameData.PlayerOne.PlayerScore,
                m_GameData.PlayerTwo.PlayerName,
                m_GameData.PlayerTwo.PlayerScore);
        }

        public void TogglePlayer()
        {
            if(m_GameData.CurrentPlayer == m_GameData.PlayerOne)
            {
                m_GameData.CurrentPlayer = m_GameData.PlayerTwo;
            }
            else
            {
                m_GameData.CurrentPlayer = m_GameData.PlayerOne;
            }

            m_SecondSelection.IsHidden = true;
            m_FirstSelection.IsHidden = true;
            m_SelectionNotMatching = false;
        }

        public string GetGameOverStatus()
        {
            Player playerOne = m_GameData.PlayerOne;
            Player playerTwo = m_GameData.PlayerTwo;
            string gameResult = null;

            if(playerOne.PlayerScore > playerTwo.PlayerScore)
            {
                gameResult = getGameResultText(playerOne.PlayerName);
            }
            else if(playerOne.PlayerScore < playerTwo.PlayerScore)
            {
                gameResult = getGameResultText(playerTwo.PlayerName);
            }
            else
            {
                gameResult = getGameResultText(null);
            }

            return gameResult;
        }

        private string getGameResultText(string i_PlayerName)
        {
            string gameResultText = null;

            if(i_PlayerName != null)
            {
                gameResultText = string.Format(
                    "{0} is the winner!{1}{2}",
                    i_PlayerName,
                    System.Environment.NewLine,
                    GetScoreboard(),
                    System.Environment.NewLine);
            }
            else
            {
                gameResultText = string.Format(
                    "It's a tie!{0}{1}",
                    System.Environment.NewLine,
                    GetScoreboard());
            }

            return gameResultText;
        }

        public void ResetRound(int i_Height, int i_Width)
        {
            CurrentPlayer = m_GameData.PlayerOne.PlayerScore < m_GameData.PlayerTwo.PlayerScore
                                ? m_GameData.PlayerOne
                                : m_GameData.PlayerTwo;
            m_GameData.PlayerOne.PlayerScore = 0;
            m_GameData.PlayerTwo.PlayerScore = 0;

            m_GameData.Height = i_Height;
            m_GameData.Width = i_Width;
            m_GameData.Letters = new BoardLetter[i_Height, i_Width];

            m_GameData.InitializeBoardMatrix();

            s_CurrentGameState = eGameStates.Running;
        }

        public string CalculateAiInput()
        {
            string aiSelection = null;

            if(m_AiMemory.Count == 0)
            {
                m_AiHasMatches = false;
                aiSelection = getRandomUnmemorizedSquare();
            }
            else
            {
                aiSelection = m_IsFirstSelection ?
                                  calculateFirstSelection() :
                                  calculateSecondSelection();
            }

            return aiSelection;
        }

        private string calculateFirstSelection()
        {
            string firstSelection = null;

            m_FoundMatch = findLetterMatch(ref firstSelection);

            if(m_FoundMatch)
            {
                m_AiHasMatches = true;
            }
            else
            {
                m_AiHasMatches = false;
                firstSelection = getRandomUnmemorizedSquare();
            }

            return firstSelection;
        }

        private string calculateSecondSelection()
        {
            string secondSelection = null;

            if(m_FoundMatch)
            {
                secondSelection = m_AiSelection.ToString();
            }
            else
            {
                secondSelection = findLetterInMemory(m_AiSelection);

                if(secondSelection != null)
                {
                    m_AiHasMatches = true;
                }
                else
                {
                    m_AiHasMatches = false;
                    secondSelection = getRandomUnmemorizedSquare();
                }
            }

            return secondSelection;
        }

        private string findLetterInMemory(Cell i_FirstSelectionCell)
        {
            string foundLetter = null;

            foreach(var memorizedLetter in m_AiMemory)
            {
                Cell currentKey = memorizedLetter.Key;
                char firstSelectionLetter = Letters[i_FirstSelectionCell.Row, i_FirstSelectionCell.Column].Letter;

                if(!currentKey.Equals(i_FirstSelectionCell) && memorizedLetter.Value
                   == firstSelectionLetter) 
                {
                    foundLetter = memorizedLetter.Key.ToString();
                }
            }

            return foundLetter;
        }

        private string getRandomUnmemorizedSquare()
        {
            int row = Letters.GetLength(0);
            int column = Letters.GetLength(1);
            Cell[] cellsNotInMemory = new Cell[(Height * Width) - m_AiMemory.Count];
            int indexOfCellNotInMemory = 0;

            for (int i = 0; i < row; i++)
            {
                for(int j = 0; j < column; j++)
                {
                    if(Letters[i, j].IsHidden)
                    {
                        if(!m_AiMemory.ContainsKey(new Cell(i, j)))
                        {
                            cellsNotInMemory[indexOfCellNotInMemory++] = new Cell(i, j);
                        }
                    }
                }
            }

            indexOfCellNotInMemory = GetRandomNumber(0, indexOfCellNotInMemory);
            m_AiSelection = cellsNotInMemory[indexOfCellNotInMemory];

            return m_AiSelection.ToString();
        }

        private bool findLetterMatch(ref string i_MemorizedMatchingLetter)
        {
            bool foundMatch = false;

            foreach (var firstMemorizedLetter in m_AiMemory)
            {
                foreach (var secondMemorizedLetter in m_AiMemory)
                {
                    if (!firstMemorizedLetter.Key.Equals(secondMemorizedLetter.Key))
                    {
                        if (firstMemorizedLetter.Value == secondMemorizedLetter.Value)
                        {
                            i_MemorizedMatchingLetter = firstMemorizedLetter.Key.ToString();
                            m_AiSelection = secondMemorizedLetter.Key;
                            foundMatch = true;
                        }
                    }
                }
            }

            return foundMatch;
        }
    }
}