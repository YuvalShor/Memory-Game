using System;

namespace B20_Ex02
{
    internal class Player
    {
        private string m_PlayerName;
        private int m_PlayerScore;
        private ePlayerTypes m_Type;

        public Player(string i_PlayerName, ePlayerTypes i_Type)
        {
            m_PlayerName = i_PlayerName;
            m_Type = i_Type;
            m_PlayerScore = 0;
        }

        public string PlayerName
        {
            get
            {
                return m_PlayerName;
            }

            set
            {
                m_PlayerName = value;
            }
        }

        public int PlayerScore
        {
            get
            {
                return m_PlayerScore;
            }

            set
            {
                m_PlayerScore = value;
            }
        }

        public ePlayerTypes Type
        {
            get
            {
                return m_Type;
            }

            set
            {
                m_Type = value;
            }
        }
    }
}