using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex02
{
    internal class BoardLetter
    {
        private char m_Letter;
        private bool m_IsHidden;

        public BoardLetter(char i_Letter, bool i_IsHidden)
        {
            m_Letter = i_Letter;
            m_IsHidden = i_IsHidden;
        }

        public char Letter
        {
            get => m_Letter;
            set => m_Letter = value;
        }
        public bool IsHidden
        {
            get => m_IsHidden;
            set => m_IsHidden = value;
        }
    }
}
