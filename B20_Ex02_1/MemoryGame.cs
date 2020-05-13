using System;

namespace B20_Ex02
{
    internal class MemoryGame
    {
        private MainMenu m_mainMenu;
        private GameLogic m_gameLogic;
        private GameGraphics m_gameGraphics;
        private Player m_playerOne;
        private Player m_playerTwo;
        private static eGameModes s_GameMode;

        public MemoryGame()
        {
            s_GameMode = eGameModes.Menu;
            m_mainMenu = new MainMenu();
            m_gameLogic = new GameLogic();
            m_gameGraphics = new GameGraphics();
            m_playerOne = new Player();
            m_playerTwo = new Player();
        }

        public void StartGame()
        {
            while(true)
            {
                switch(s_GameMode)
                {
                    case eGameModes.Menu:
                        MainMenu.Run(m_playerOne, m_playerTwo);
                        break;
                    case eGameModes.PlayerVsPlayer:
                    case eGameModes.PlayerVsAi:
                        GameLogic.Validate(m_playerOne, m_playerTwo);
                        GameGraphics.Draw();
                        break;
                    case eGameModes.GameOver:
                        GameOver();
                        break;
                }
            }
        }

        public static eGameModes GameMode
        {
            get
            {
                return s_GameMode;
            }
            set
            {
                s_GameMode = value;
            }
        }
    }
}