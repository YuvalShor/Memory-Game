using System.Collections.Generic;

namespace B20_Ex02
{
    internal class GameData
    {
        private readonly Player r_PlayerOne;
        private readonly Player r_PlayerTwo;
        private Player m_CurrentPlayer;
        private BoardLetter[,] m_Letters;
        private int m_BoardHeight;
        private int m_BoardWidth;

        public GameData(Player i_PlayerOne, Player i_PlayerTwo, int i_BoardWidth, int i_BoardHeight)
        {
            r_PlayerOne = i_PlayerOne;
            r_PlayerTwo = i_PlayerTwo;
            m_BoardWidth = i_BoardWidth;
            m_BoardHeight = i_BoardHeight;

            m_CurrentPlayer = r_PlayerOne;
            m_Letters = new BoardLetter[BoardHeight, BoardWidth];
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

        public int BoardHeight
        {
            get
            {
                return m_BoardHeight;
            }

            set
            {
                m_BoardHeight = value;
            }
        }

        public int BoardWidth
        {
            get
            {
                return m_BoardWidth;
            }

            set
            {
                m_BoardWidth = value;
            }
        }

        public Player PlayerOne
        {
            get
            {
                return r_PlayerOne;
            }
        }

        public Player PlayerTwo
        {
            get
            {
                return r_PlayerTwo;
            }
        }

        public void InitializeBoardMatrix()
        {
            char[] boardLetters = initializeBoardLetters();
            List<Cell> randomCells = getRandomCellsList();

            foreach(char letter in boardLetters)
            {
                int randomSelection = GameLogicManager.GetRandomNumber(0, randomCells.Count);
                Cell firstCell = randomCells[randomSelection];

                randomCells.Remove(firstCell);
                randomSelection = GameLogicManager.GetRandomNumber(0, randomCells.Count);

                Cell secondCell = randomCells[randomSelection];

                randomCells.Remove(secondCell);

                Letters[firstCell.Row, firstCell.Column] = new BoardLetter(letter, true);
                Letters[secondCell.Row, secondCell.Column] = new BoardLetter(letter, true);
            }
        }

        private List<Cell> getRandomCellsList()
        {
            List<Cell> randomCells = new List<Cell>(m_BoardHeight * m_BoardWidth);

            for(int i = 0; i < m_BoardHeight; i++)
            {
                for(int j = 0; j < m_BoardWidth; j++)
                {
                    randomCells.Add(new Cell(i, j));
                }
            }

            return randomCells;
        }

        private char[] initializeBoardLetters()
        {
            char[] boardLetters = new char[m_BoardHeight * m_BoardWidth / 2];

            for(int i = 0; i < boardLetters.Length; i++)
            {
                boardLetters[i] = (char)('A' + i);
            }

            return boardLetters;
        }
    }
}