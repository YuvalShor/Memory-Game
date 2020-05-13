using System;

namespace B20_Ex02
{
    internal class Player
    {
        private string m_playerName;
        private int m_playerScore;
        private ePlayerTypes m_playerType;

        public string Name
        {
            get
            {
                return m_playerName;
            }
            set
            {
                m_playerName = value;
            }
        }

        public ePlayerTypes Type
        {
            get
            {
                return m_playerType;
            }
            set
            {
                m_playerType = value;
            }
        }
    }
}