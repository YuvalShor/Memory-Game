using System;

namespace B20_Ex02
{
    public struct Cell
    {
        private int m_Row;
        private int m_Column;

        public static Cell Parse(string i_ToParse)
        {
            int column = i_ToParse[0] - 'A';
            int row = i_ToParse[1] - '1';

            return new Cell(row, column);
        }

        public Cell(int i_Row, int i_Column)
        {
            m_Row = i_Row;
            m_Column = i_Column;
        }

        public int Row
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public int Column
        {
            get
            {
                return m_Column;
            }

            set
            {
                m_Column = value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", (char)(m_Column + 'A'), (char)(m_Row + '1'));
        }
    }
}