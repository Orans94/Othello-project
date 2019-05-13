using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class Cell
    {
        private char m_Sign = ' ';

        public char Sign
        {
            get
            {
                return m_Sign;
            }
            set
            {
                m_Sign = value;
            }
        }
    }
}
