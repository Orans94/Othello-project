using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class Cell
    {
        public const char EMPTY = ' ';
        private char m_Sign = EMPTY;
        private int m_Row;
        private int m_Column;

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
        public bool IsEmpty()
        {
            bool isCellEmpty;
            if(Sign == EMPTY)
            {
                isCellEmpty = true;
            }
            else
            {
                isCellEmpty = false;
            }

            return isCellEmpty;
        }
    }
}
