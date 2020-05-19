using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace B20_Ex02
{
    internal class GameLogic
    {
        private static Random s_Random = new Random();
        private readonly bool r_IsPlayerVsPlayer; // !m_IsPlayerVsPlayer = PlayerVsComputer
        private GameData m_GameData;
        private bool m_SelectionNotMatching;
        private Dictionary<Cell, char> m_AiMemory;
        private Cell m_AiSelection;
        private bool m_FoundMatch;
        private BoardLetter m_FirstSelection;
        private BoardLetter m_SecondSelection;
        private bool m_IsFirstSelection;
        private bool m_IsGameRunning; 

        public static int getRandomNumber(int i_RangeStart, int i_RangeEnd)
        { 
            return s_Random.Next(i_RangeStart, i_RangeEnd);
        }

        public GameLogic(Player i_Player1, Player i_Player2, int i_Width, int i_Height, bool i_IsPlayerVsPlayer)
        {
            m_GameData = new GameData(i_Player1, i_Player2, i_Width, i_Height);
            m_GameData.InitializeMatrix();
            r_IsPlayerVsPlayer = i_IsPlayerVsPlayer;
            m_SelectionNotMatching = false;
            m_IsFirstSelection = true;
            m_IsGameRunning = true;
            m_FoundMatch = false;

            if(!r_IsPlayerVsPlayer)
            {
                m_AiMemory = new Dictionary<Cell, char>();
            }
        }

        public Player CurrentPlayer
        {
            get => m_GameData.CurrentPlayer;
            set => m_GameData.CurrentPlayer = value;
        }

        public BoardLetter[,] Letters
        {
            get => m_GameData.Letters;
        }

        public void UpdateData(string i_UserInput)
        {
            int column = i_UserInput[0] - 'A';
            int row = i_UserInput[1] - '1';

            if(!m_SelectionNotMatching)
            {
                UpdateNextTurn(row, column);
            }

            if ((m_GameData.Player1.PlayerScore + m_GameData.Player2.PlayerScore) == (Width * Height) / 2)
            {
                m_IsGameRunning = false;
            }
        }

        private void AddToAiMemory(Cell i_CellToBeAdded)
        {
            if(!m_AiMemory.ContainsKey(i_CellToBeAdded))
            {
                m_AiMemory.Add(i_CellToBeAdded, Letters[i_CellToBeAdded.Row, i_CellToBeAdded.Column].Letter);
            }
        }

        private void UpdatePlayerVsComputer(string i_UserInput)
        {
            // implement please
        }

        private void UpdateNextTurn(int i_Row, int i_Column)
        {
            m_SecondSelection = m_GameData.Letters[i_Row, i_Column];

            if(!r_IsPlayerVsPlayer)
            {
                AddToAiMemory(new Cell(i_Row, i_Column));
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
                    if (!r_IsPlayerVsPlayer)
                    { 
                        RemoveLetterFromAiMemory(m_SecondSelection.Letter);
                    }

                    CurrentPlayer.PlayerScore++;
                }

                m_IsFirstSelection = true;
            }
        }

        private void RemoveLetterFromAiMemory(char i_SecondSelectionLetter)
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

        public bool SelectionNotMatching
        {
            get => m_SelectionNotMatching;
            set => m_SelectionNotMatching = value;
        }

        public int Width
        {
            get => m_GameData.Width;
        }

        public int Height
        {
            get => m_GameData.Height;
        }

        public string GetScoreboard()
        {
            return string.Format(
                "{0}: {1} - {2}: {3}",
                m_GameData.Player1.PlayerName,
                m_GameData.Player1.PlayerScore,
                m_GameData.Player2.PlayerName,
                m_GameData.Player2.PlayerScore);
        }

        public void TogglePlayer()
        {
            if(m_GameData.CurrentPlayer == m_GameData.Player1)
            {
                m_GameData.CurrentPlayer = m_GameData.Player2;
            }
            else
            {
                m_GameData.CurrentPlayer = m_GameData.Player1;
            }

            m_SecondSelection.IsHidden = true;
            m_FirstSelection.IsHidden = true;
            m_SelectionNotMatching = false;
        }

        public string GetGameOverStatus()
        {
            Player playerOne = m_GameData.Player1;
            Player playerTwo = m_GameData.Player2;
            string gameResult = null;

            if(playerOne.PlayerScore > playerTwo.PlayerScore)
            {
                gameResult = string.Format(
                    "{0} is the winner!{1}{2}",
                    playerOne.PlayerName,
                    System.Environment.NewLine,
                    GetScoreboard());
            }
            else if(playerOne.PlayerScore < playerTwo.PlayerScore)
            {
                gameResult = string.Format(
                    "{0} is the winner!{1}{2}",
                    playerTwo.PlayerName,
                    System.Environment.NewLine,
                    GetScoreboard());
            }
            else
            {
                gameResult = string.Format(
                    "It's a tie!{0}{1}",
                    System.Environment.NewLine,
                    GetScoreboard());
            }

            return gameResult;
        }

        public void ResetRound(int i_Height, int i_Width)
        {
            m_GameData.Player1.PlayerScore = 0;
            m_GameData.Player2.PlayerScore = 0;

            m_GameData.Height = i_Height;
            m_GameData.Width = i_Width;
            m_GameData.Letters = new BoardLetter[i_Height, i_Width];

            m_GameData.InitializeMatrix();

            m_IsGameRunning = true;
        }

        public bool IsGameRunning
        {
            get => m_IsGameRunning;
            set => m_IsGameRunning = value;
        }

        public string CalculateAiInput()
        {
            string returnValue = null;

            if(m_AiMemory.Count == 0)
            {
                returnValue = findFirstHiddenCell();
            }
            else
            {
                if(m_IsFirstSelection)
                {
                    m_FoundMatch = findLetterMatch(ref returnValue);

                    if(!m_FoundMatch)
                    {
                        returnValue = findFirstUnmemorizedCell();
                    }
                }
                else
                {
                    if(m_FoundMatch)
                    {
                        returnValue = m_AiSelection.ToString();
                    }
                    else
                    {
                        returnValue = findLetterInMemory(m_AiSelection);

                        if(returnValue == null)
                        {
                            returnValue = findFirstUnmemorizedCell();
                        }
                    }
                }
            }

            return returnValue;
        }

        private string findLetterInMemory(Cell i_FirstSelectionCell)
        {
            string returnValue = null;

            foreach(var memorizedLetter in m_AiMemory)
            {
                if(!memorizedLetter.Key.Equals(i_FirstSelectionCell) && memorizedLetter.Value
                   == Letters[i_FirstSelectionCell.Row, i_FirstSelectionCell.Column].Letter) 
                {
                    returnValue = memorizedLetter.Key.ToString();
                }
            }

            return returnValue;
        }

        private string findFirstHiddenCell()
        {
            string returnValue = null;
            int row = Letters.GetLength(0);
            int column = Letters.GetLength(1);
            bool foundAvailableCell = false;

            for (int i = 0; i < row && !foundAvailableCell; i++)
            {
                for (int j = 0; j < column && !foundAvailableCell; j++)
                {
                    if (Letters[i, j].IsHidden)
                    {
                        returnValue = string.Format("{0}{1}", (char)(j + 'A'), (char)(i + '1'));
                        foundAvailableCell = true;
                    }
                }
            }

            return returnValue;
        }

        private string findFirstUnmemorizedCell()
        {
            string returnValue = null;
            int row = Letters.GetLength(0);
            int column = Letters.GetLength(1);
            bool foundAvailableCell = false;

            for (int i = 0; i < row && !foundAvailableCell; i++)
            {
                for (int j = 0; j < column && !foundAvailableCell; j++)
                {
                    if (Letters[i, j].IsHidden)
                    {
                        if (Letters[i, j].IsHidden && !m_AiMemory.ContainsKey(new Cell(i, j)))
                        {
                            returnValue = string.Format("{0}{1}", (char)(j + 'A'), (char)(i + '1'));
                            foundAvailableCell = true;
                            m_AiSelection = new Cell(i, j);
                        }
                    }
                }
            }

            return returnValue;
        }

        private bool findLetterMatch(ref string i_ReturnValue)
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
                            i_ReturnValue = firstMemorizedLetter.Key.ToString();
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
