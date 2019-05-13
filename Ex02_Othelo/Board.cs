﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class Board
    {
        private int m_NumOfRows;
        private int m_NumOfColumns;
        private Cell[,] m_Board;

        public Board(int i_NumOfRow, int i_NumOfCol)
        {
            //create board according to the size that the user chose.
            m_NumOfColumns = i_NumOfCol;
            m_NumOfRows = i_NumOfRow;

            m_Board = new Cell[i_NumOfRow, i_NumOfCol];
            for (int rowIndex = 0; rowIndex < m_NumOfRows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < m_NumOfColumns; colIndex++)
                {
                    m_Board[rowIndex, colIndex] = new Cell();
                }
            }
        }

        public void UpdateCell(int i_RowIndex, int i_ColumnIndex, char i_Sign)
        {
            //updating the board after the player move
            m_Board[i_RowIndex, i_ColumnIndex].Sign = i_Sign;
        }

        public void Initialize()
        {
            // TODO: Add consts
            if (NumberOfRows == 8)
            {
                m_Board[3, 3].Sign = 'O';
                m_Board[4, 3].Sign = 'X';
                m_Board[4, 4].Sign = 'O';
                m_Board[3, 4].Sign = 'X';
            }
            else if (NumberOfRows == 6)
            {
                m_Board[2, 2].Sign = 'O';
                m_Board[3, 2].Sign = 'X';
                m_Board[3, 3].Sign = 'O';
                m_Board[2, 3].Sign = 'X';
            }
        }
        public int NumberOfRows
        {
            get { return m_NumOfRows; }
            set { m_NumOfRows = value; }
        }

        public int NumberOfColumns
        {
            get { return m_NumOfColumns; }
            set { m_NumOfColumns = value; }
        }

        private void clear()
        {
            //clearing the board and putting in all the cells space sign ' '.
            foreach(Cell cell in m_Board)
            {
                cell.Sign = ' ';
            }
        }


        public bool IsCellEmpty(int i_RowIndex, int i_ColumnIndex)
        {
            //checking if a cell is empty (has ' ' char in it).
            //TODO:MAKE A CONST FOR ' ' (EMPTY).
            bool isCellEmpty;

            isCellEmpty = m_Board[i_RowIndex, i_ColumnIndex].Sign == ' ';

            return isCellEmpty;
        }

        public bool IsCellInBoard(int i_RowIndex,int i_ColumnIndex)
        {
            //checking if the cell given is in board limits.
            bool isCellInBoard;

            isCellInBoard = (i_RowIndex <= m_NumOfRows) && (i_RowIndex >= 0) && (i_ColumnIndex <= m_NumOfColumns) && (i_ColumnIndex >= 0);

            return isCellInBoard;
        }

        public int CountSignAppearances(char i_Sign)
        {
            //this method recieves a char and return the amount of appearances of that char in the board.
            int countOfSignAppearances = 0;

            foreach(Cell cell in m_Board)
            {
                if (cell.Sign == i_Sign)
                {
                    countOfSignAppearances++;
                }
            }

            return countOfSignAppearances;
        }
    }
}
