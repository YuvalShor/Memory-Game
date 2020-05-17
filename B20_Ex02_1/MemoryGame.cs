using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace B20_Ex02
{
    internal class MemoryGame
    {
        private MainMenu m_MainMenu;
        private GameLogic m_GameLogic;
        private GameUI m_GameUI;
        private static eGameModes s_GameMode;
        private static eGameModes s_LastRoundMode;

        public MemoryGame()
        {
            s_GameMode = eGameModes.Menu;
            m_MainMenu = new MainMenu();
            m_GameUI = new GameUI();
            GameMode = eGameModes.Menu;
        }

        public void StartGame()
        {
            while(s_GameMode != eGameModes.GameOver)
            {
                switch(s_GameMode)
                {
                    case eGameModes.Menu:
                        runMainMenu();
                        break;
                    case eGameModes.PlayerVsPlayer:
                    case eGameModes.PlayerVsAi:
                        runGame();
                        break;
                }
            }

            runGameOver(m_GameLogic.GetGameOverStatus());
        }

        private void runGame()
        {
            string scoreBoard = m_GameLogic.GetScoreStatus();

            m_GameUI.DrawData(m_GameLogic.Letters, m_GameLogic.CurrentPlayer, scoreBoard, m_GameLogic.SelectionNotMatching);

            if(!m_GameLogic.SelectionNotMatching)
            {
                string playerInput = m_GameUI.GetPlayerInput(m_GameLogic.Letters);

                if(playerInput == "Q")
                {
                    StopGame();
                }
                else
                {
                    m_GameLogic.UpdateData(playerInput);
                }
            }
            else
            {
                m_GameUI.DrawData(m_GameLogic.Letters, m_GameLogic.CurrentPlayer, scoreBoard, m_GameLogic.SelectionNotMatching);
                m_GameLogic.TogglePlayer();
            }
        }

        private void StopGame()
        {
            // implement
        }

        private void runGameOver(string i_GameOverStatus)
        {
            m_GameUI.DrawText(i_GameOverStatus);


            bool restartNeeded = m_GameUI.CheckRestart();

            if(restartNeeded)
            {
                m_GameUI.ClearWindow();
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

            m_MainMenu.GetBoardSize(out height, out width);
            m_GameLogic.ResetRound(height, width);
            StartGame();
        }

        private void runMainMenu()
        {
            string playerName1, playerName2;
            int width, height;

            m_MainMenu.Run(out playerName1, out playerName2, out width, out height);

            // initialize data 
            Player playerOne= new Player(playerName1, ePlayerTypes.Human);

            ePlayerTypes type = (GameMode == eGameModes.PlayerVsPlayer) ? 
                                    ePlayerTypes.Human 
                                    : ePlayerTypes.CPU;
            Player playerTwo = new Player(playerName2, type);

            m_GameLogic = new GameLogic(playerOne, playerTwo, width, height);
        }

        public static eGameModes GameMode
        {
            get => s_GameMode;
            set => s_GameMode = value;
        }

        public static eGameModes LastRoundMode
        {
            get => s_LastRoundMode;
            set => s_LastRoundMode = value;
        }
    }
}