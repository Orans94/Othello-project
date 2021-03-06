﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othello
{
    public class Cell
    {
        public const char k_Empty = ' ';
        private char m_Sign = k_Empty;
        private int m_Row;
        private int m_Column;

        public Cell(int i_Row, int i_Column, char i_Sign = k_Empty)
        {
            // cell c'tor
            m_Row = i_Row;
            m_Column = i_Column;
            m_Sign = i_Sign;
        }

        public Cell()
        {
            // default cell c'tor
        }
        
        public int Row
        {
            // a propertie for m_Row
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
            // a propertie for m_Column
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
            // a propertie for m_Sign
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
            // this method checks if a cell is empty
            bool isCellEmpty;

            if(m_Sign == k_Empty)
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
