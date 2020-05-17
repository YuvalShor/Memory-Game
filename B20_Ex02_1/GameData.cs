using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex02
{
    internal class GameData
    {
        private Player m_Player1;
        private Player m_Player2;
        private Player m_CurrentPlayer;
        private BoardLetter[,] m_Letters;
        private int m_Height;
        private int m_Width;

        public GameData(Player i_Player1, Player i_Player2, int i_Width, int i_Height)
        {
            m_Player1 = i_Player1;
            m_Player2 = i_Player2;
            m_Width = i_Width;
            m_Height = i_Height;

            m_CurrentPlayer = m_Player1;
            m_Letters = new BoardLetter[Height, Width];
        }

        public BoardLetter[,] Letters
        {
            get => m_Letters;
            set => m_Letters = value;
        }

        public Player CurrentPlayer
        {
            get => m_CurrentPlayer;
            set => m_CurrentPlayer = value;
        }

        public int Height
        {
            get => m_Height;
            set => m_Height = value;
        }
        public int Width
        {
            get => m_Width;
            set => m_Width = value;
        }

        public void InitializeMatrix()
        {
            char[] letters = new char[m_Height * m_Width / 2];

            for(int i = 0; i < letters.Length; i++)
            {
                letters[i] = (char)('A' + i);
            }

            List<Cell> randomPoints = new List<Cell>(m_Height * m_Width);

            for(int i = 0; i < m_Height; i++)
            {
                for(int j = 0; j < m_Width; j++)
                {
                    randomPoints.Add(new Cell(i, j));
                }
            }

            foreach(char letter in letters)
            {
                int randomSelection = GameLogic.getRandomNumber(0, randomPoints.Count);
                Cell firstCell = randomPoints[randomSelection];

                randomPoints.Remove(firstCell);
                randomSelection = GameLogic.getRandomNumber(0, randomPoints.Count);

                Cell secondCell = randomPoints[randomSelection];

                randomPoints.Remove(secondCell);

                Letters[firstCell.Row, firstCell.Column] = new BoardLetter(letter, true);
                Letters[secondCell.Row, secondCell.Column] = new BoardLetter(letter, true);
            }
        }

        public Player Player1
        {
            get => m_Player1;
        }

        public Player Player2
        {
            get => m_Player2;
        }
    }
}
