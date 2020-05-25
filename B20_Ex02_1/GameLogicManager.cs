using System;
using System.Collections.Generic;

namespace B20_Ex02
{
    internal class GameLogicManager
    {
        private const int k_DifficultyOdds = 80;

        private static readonly Random sr_Random = new Random();
        public const int k_MinBoardWidth = 4;
        public const int k_MaxBoardWidth = 6;
        public const int k_MinBoardHeight = 4;
        public const int k_MaxBoardHeight = 6;

        private static eGameStates s_CurrentGameState = eGameStates.Menu;

        private readonly eGameModes r_GameMode;
        private readonly GameData r_GameData;
        private readonly Dictionary<Cell, char> r_AiMemory;

        private Cell m_AiSelection;
        private Cell m_CurrentUserSelection;
        private Cell m_PreviousUserSelection;
        private bool m_FoundMatch;
        private bool m_IsFirstSelection;
        private bool m_SelectionNotMatching;
        private bool m_AiHasMatches;

        public static int GetRandomNumber(int i_RangeStart, int i_RangeEnd)
        { 
            return sr_Random.Next(i_RangeStart, i_RangeEnd);
        }

        public GameLogicManager(Player i_Player1, Player i_Player2, int i_Width, int i_Height, eGameModes i_GameMode)
        {
            r_GameData = new GameData(i_Player1, i_Player2, i_Width, i_Height);
            r_GameData.InitializeBoardMatrix();
            r_GameMode = i_GameMode;
            s_CurrentGameState = eGameStates.Running;

            initializeLogicData();

            if (r_GameMode == eGameModes.PlayerVsComputer)
            {
                r_AiMemory = new Dictionary<Cell, char>();
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
                return r_GameData.CurrentPlayer;
            }

            set
            {
                r_GameData.CurrentPlayer = value;
            }
        }

        public BoardLetter[,] Letters
        {
            get
            {
                return r_GameData.Letters;
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

        public int BoardWidth
        {
            get
            {
                return r_GameData.BoardWidth;
            }
        }

        public int BoardHeight
        {
            get
            {
                return r_GameData.BoardHeight;
            }
        }

        private void initializeLogicData()
        {
            m_SelectionNotMatching = false;
            m_IsFirstSelection = true;
            m_FoundMatch = false;
            m_AiHasMatches = false;
        }

        public void UpdateData(Cell i_UserSelection)
        {
            if(!m_SelectionNotMatching)
            {
                updateNextTurn(i_UserSelection);
            }

            if ((r_GameData.PlayerOne.PlayerScore + r_GameData.PlayerTwo.PlayerScore) == (BoardWidth * BoardHeight) / 2)
            {
                s_CurrentGameState = eGameStates.GameOver;
            }
        }

        private void addToAiMemory(Cell i_CellToBeAdded)
        {
            if(!r_AiMemory.ContainsKey(i_CellToBeAdded))
            {
                r_AiMemory.Add(i_CellToBeAdded, Letters[i_CellToBeAdded.Row, i_CellToBeAdded.Column].Letter);
            }
        }

        private void updateNextTurn(Cell i_UserSelection)
        {
            m_CurrentUserSelection = i_UserSelection;

            if (r_GameMode == eGameModes.PlayerVsComputer)
            {
                if (GameLogicManager.GetRandomNumber(0, 100) < k_DifficultyOdds)
                {
                    addToAiMemory(m_CurrentUserSelection);
                }
            }

            if (m_IsFirstSelection)
            {
                m_PreviousUserSelection = m_CurrentUserSelection;
                getBoardLetterAt(m_CurrentUserSelection).IsHidden = false;
                m_IsFirstSelection = false;
            }
            else
            {
                BoardLetter firstSelectionLetter = getBoardLetterAt(m_PreviousUserSelection);
                BoardLetter secondSelectionLetter = getBoardLetterAt(m_CurrentUserSelection);

                secondSelectionLetter.IsHidden = false;

                m_SelectionNotMatching = firstSelectionLetter.Letter != secondSelectionLetter.Letter;

                if (!m_SelectionNotMatching)
                {
                    if (r_GameMode == eGameModes.PlayerVsComputer)
                    {
                        r_AiMemory.Remove(m_CurrentUserSelection);
                        r_AiMemory.Remove(m_PreviousUserSelection);
                    }

                    CurrentPlayer.PlayerScore++;
                }

                m_IsFirstSelection = true;
            }
        }

        public string GetScoreboard()
        {
            return string.Format(
                "Score: {0} {1} - {2} {3}",
                r_GameData.PlayerOne.PlayerName,
                r_GameData.PlayerOne.PlayerScore,
                r_GameData.PlayerTwo.PlayerName,
                r_GameData.PlayerTwo.PlayerScore);
        }

        public void TogglePlayer()
        {
            CurrentPlayer = CurrentPlayer == r_GameData.PlayerOne ?
                                r_GameData.PlayerTwo : 
                                r_GameData.PlayerOne;

            getBoardLetterAt(m_CurrentUserSelection).IsHidden = true;
            getBoardLetterAt(m_PreviousUserSelection).IsHidden = true;
            m_SelectionNotMatching = false;
        }

        private BoardLetter getBoardLetterAt(Cell i_CellLocation)
        {
            return Letters[i_CellLocation.Row, i_CellLocation.Column];
        }

        public string GetGameOverStatus()
        {
            Player playerOne = r_GameData.PlayerOne;
            Player playerTwo = r_GameData.PlayerTwo;
            string gameResult;

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
            string gameResultText;

            if(i_PlayerName != null)
            {
                gameResultText = string.Format(
                    "{0} is the winner!{1}{2}",
                    i_PlayerName,
                    Environment.NewLine,
                    GetScoreboard());
            }
            else
            {
                gameResultText = string.Format(
                    "It's a tie!{0}{1}",
                    Environment.NewLine,
                    GetScoreboard());
            }

            return gameResultText;
        }

        public void ResetRound(int i_Height, int i_Width)
        {
            CurrentPlayer = r_GameData.PlayerOne.PlayerScore < r_GameData.PlayerTwo.PlayerScore
                                ? r_GameData.PlayerOne
                                : r_GameData.PlayerTwo;

            r_GameData.PlayerOne.PlayerScore = 0;
            r_GameData.PlayerTwo.PlayerScore = 0;

            r_GameData.BoardHeight = i_Height;
            r_GameData.BoardWidth = i_Width;

            r_GameData.Letters = new BoardLetter[i_Height, i_Width];
            r_GameData.InitializeBoardMatrix();

            initializeLogicData();

            s_CurrentGameState = eGameStates.Running;
        }

        public string CalculateAiInput()
        {
            string aiSelection;

            if(r_AiMemory.Count == 0)
            {
                m_AiHasMatches = false;
                aiSelection = getRandomUnmemorizedCell();
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
                firstSelection = getRandomUnmemorizedCell();
            }

            return firstSelection;
        }

        private string calculateSecondSelection()
        {
            string secondSelection;

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
                    secondSelection = getRandomUnmemorizedCell();
                }
            }

            return secondSelection;
        }

        private string findLetterInMemory(Cell i_FirstSelectionCell)
        {
            string foundLetter = null;

            foreach(var memorizedLetter in r_AiMemory)
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

        private string getRandomUnmemorizedCell()
        {
            int row = Letters.GetLength(0);
            int column = Letters.GetLength(1);
            Cell[] cellsNotInMemory = new Cell[(BoardHeight * BoardWidth) - r_AiMemory.Count];
            int indexOfCellNotInMemory = 0;

            for (int i = 0; i < row; i++)
            {
                for(int j = 0; j < column; j++)
                {
                    if(Letters[i, j].IsHidden)
                    {
                        if(!r_AiMemory.ContainsKey(new Cell(i, j)))
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

            foreach (var firstMemorizedLetter in r_AiMemory)
            {
                foreach (var secondMemorizedLetter in r_AiMemory)
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