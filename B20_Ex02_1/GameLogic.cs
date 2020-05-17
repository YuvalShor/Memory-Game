using System;
using System.Runtime.InteropServices;
using System.Text;

namespace B20_Ex02
{
    internal class GameLogic
    {
        private GameData m_GameData;
        private bool m_SelectionNotMatching;
        private static Random s_Random = new Random();
        private BoardLetter m_PreviousSelection;
        private BoardLetter m_CurrentSelection;
        private bool m_IsFirstSelection;

        public GameLogic(Player i_Player1, Player i_Player2, int i_Width, int i_Height)
        {
            m_GameData = new GameData(i_Player1, i_Player2, i_Width, i_Height);
            m_GameData.InitializeMatrix();
            m_SelectionNotMatching = false;
            m_IsFirstSelection = true;
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
            if(MemoryGame.GameMode == eGameModes.PlayerVsPlayer)
            {
                UpdatePlayerVsPlayer(i_UserInput);
            }
            else
            {
                UpdatePlayerVsComputer(i_UserInput);
            }

            if ((m_GameData.Player1.PlayerScore + m_GameData.Player2.PlayerScore) == (Width * Height) / 2)
            {
                MemoryGame.LastRoundMode = MemoryGame.GameMode;
                MemoryGame.GameMode = eGameModes.GameOver;
            }
        }

        private void UpdatePlayerVsComputer(string i_UserInput)
        {
            // implement please
        }

        private void UpdatePlayerVsPlayer(string i_UserInput)
        {
            int column = i_UserInput[0] - 'A';
            int row = i_UserInput[1] - '1';
            m_CurrentSelection = m_GameData.Letters[row, column];

            if (m_IsFirstSelection)
            {
                m_PreviousSelection = m_CurrentSelection;
                m_PreviousSelection.IsHidden = false;
                m_IsFirstSelection = false;
            }
            else
            {
                m_CurrentSelection.IsHidden = false;

                if (m_CurrentSelection.Letter != m_PreviousSelection.Letter)
                {
                    m_SelectionNotMatching = true;
                }
                else
                {
                    CurrentPlayer.PlayerScore += 1;
                }

                m_IsFirstSelection = true;
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

        public static int getRandomNumber(int i_RangeStart, int i_RangeEnd)
        { 
            return s_Random.Next(i_RangeStart, i_RangeEnd);
        }

        public string GetScoreStatus()
        {
            return String.Format(
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

            m_CurrentSelection.IsHidden = true;
            m_PreviousSelection.IsHidden = true;
            m_SelectionNotMatching= false;
        }

        public string GetGameOverStatus()
        {
            Player playerOne = m_GameData.Player1;
            Player playerTwo = m_GameData.Player2;
            string gameResult = null;

            if(playerOne.PlayerScore > playerTwo.PlayerScore)
            {
                gameResult = String.Format(
                    "{0} is the winner!{1}{2}",
                    playerOne.PlayerName,
                    System.Environment.NewLine,
                    GetScoreStatus());
            }
            else if(playerOne.PlayerScore < playerTwo.PlayerScore)
            {
                gameResult = String.Format(
                    "{0} is the winner!{1}{2}",
                    playerTwo.PlayerName,
                    System.Environment.NewLine,
                    GetScoreStatus());
            }
            else
            {
                gameResult = String.Format(
                    "It's a tie!{0}{1}",
                    System.Environment.NewLine,
                    GetScoreStatus());
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

            MemoryGame.GameMode = MemoryGame.LastRoundMode;
        }
    }
}
