using System.Collections.Generic;

namespace B20_Ex02
{
    internal class GameData
    {
        private Player m_PlayerOne;
        private Player m_PlayerTwo;
        private Player m_CurrentPlayer;
        private BoardLetter[,] m_Letters;
        private int m_Height;
        private int m_Width;

        public GameData(Player i_PlayerOne, Player i_PlayerTwo, int i_Width, int i_Height)
        {
            m_PlayerOne = i_PlayerOne;
            m_PlayerTwo = i_PlayerTwo;
            m_Width = i_Width;
            m_Height = i_Height;

            m_CurrentPlayer = m_PlayerOne;
            m_Letters = new BoardLetter[Height, Width];
        }

        public BoardLetter[,] Letters
        {
            get
            {
                return m_Letters;
            }

            set
            {
                m_Letters = value;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }

            set
            {
                m_CurrentPlayer = value;
            }
        }

        public int Height
        {
            get
            {
                return m_Height;
            }

            set
            {
                m_Height = value;
            }
        }

        public int Width
        {
            get
            {
                return m_Width;
            }

            set
            {
                m_Width = value;
            }
        }

        public Player PlayerOne
        {
            get
            {
                return m_PlayerOne;
            }
        }

        public Player PlayerTwo
        {
            get
            {
                return m_PlayerTwo;
            }
        }

        public void InitializeBoardMatrix()
        {
            char[] boardLetters = initializeBoardLetters();
            List<Cell> randomCells = getRandomCellsList();

            foreach(char letter in boardLetters)
            {
                int randomSelection = GameLogic.GetRandomNumber(0, randomCells.Count);
                Cell firstCell = randomCells[randomSelection];

                randomCells.Remove(firstCell);
                randomSelection = GameLogic.GetRandomNumber(0, randomCells.Count);

                Cell secondCell = randomCells[randomSelection];

                randomCells.Remove(secondCell);

                Letters[firstCell.Row, firstCell.Column] = new BoardLetter(letter, true);
                Letters[secondCell.Row, secondCell.Column] = new BoardLetter(letter, true);
            }
        }

        private List<Cell> getRandomCellsList()
        {
            List<Cell> randomCells = new List<Cell>(m_Height * m_Width);

            for(int i = 0; i < m_Height; i++)
            {
                for(int j = 0; j < m_Width; j++)
                {
                    randomCells.Add(new Cell(i, j));
                }
            }

            return randomCells;
        }

        private char[] initializeBoardLetters()
        {
            char[] boardLetters = new char[m_Height * m_Width / 2];

            for(int i = 0; i < boardLetters.Length; i++)
            {
                boardLetters[i] = (char)('A' + i);
            }

            return boardLetters;
        }
    }
}