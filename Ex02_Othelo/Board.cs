using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class Board
    {
        public enum eBoardSize { smallBoard = 6, bigBoard = 8 };
        private eBoardSize m_BoardSize;
        private Cell[,] m_Board;

        public Board(eBoardSize i_BoardSize)
        {
            //create board according to the size that the user chose.
            Size = i_BoardSize;

            m_Board = new Cell[(int)i_BoardSize, (int)i_BoardSize];
            for (int rowIndex = 0; rowIndex < (int)i_BoardSize; rowIndex++)
            {
                for (int colIndex = 0; colIndex < (int)i_BoardSize; colIndex++)
                {
                    Matrix[rowIndex, colIndex] = new Cell(rowIndex, colIndex);
                }
            }
        }

        public eBoardSize Size
        {
            get
            {
                return m_BoardSize;
            }
            set
            {
                m_BoardSize = value;
            }
        }
        public Cell[,] Matrix
        {
            get
            {
                return m_Board;
            }
        }

        public void UpdateCell(int i_RowIndex, int i_ColumnIndex, char i_Sign)
        {
            //updating the board after the player move
            Matrix[i_RowIndex, i_ColumnIndex].Sign = i_Sign;
        }

        public void Initialize()
        {
            clear();
            // TODO: Add consts
            if (Size == eBoardSize.bigBoard)
            {
                Matrix[3, 3].Sign = 'O';
                Matrix[4, 3].Sign = 'X';
                Matrix[4, 4].Sign = 'O';
                Matrix[3, 4].Sign = 'X';
            }
            else if (Size == eBoardSize.smallBoard)
            {
                Matrix[2, 2].Sign = 'O';
                Matrix[3, 2].Sign = 'X';
                Matrix[3, 3].Sign = 'O';
                Matrix[2, 3].Sign = 'X';
            }
        }

        private void clear()
        {
            //clearing the board and putting in all the cells space sign ' '.
            foreach(Cell cell in m_Board)
            {
                cell.Sign = ' ';
            }
        }

        public void UpdateBoard(LinkedList<Cell> i_CellsToUpdate, GameUtilities.PlayerColor i_PlayingPlayer)
        {
            //this method recieves a list of cells and a player color and put the correct sign in those cells.
            foreach (Cell currentCell in i_CellsToUpdate)
            {
                Matrix[currentCell.Row, currentCell.Column].Sign = (char)i_PlayingPlayer;
            }
        }

        public bool IsCellEmpty(int i_RowIndex, int i_ColumnIndex)
        {
            //checking if a cell is empty (has ' ' char in it).
            //TODO:MAKE A CONST FOR ' ' (EMPTY).
            bool isCellEmpty;

            isCellEmpty = Matrix[i_RowIndex, i_ColumnIndex].IsEmpty();

            return isCellEmpty;
        }

        public bool IsCellInBoard(Cell i_CellIterator)
        {
            //checking if the cell given is in board limits.
            bool isCellInBoard;

            isCellInBoard = (i_CellIterator.Row < (int)Size) && (i_CellIterator.Row >= 0) && (i_CellIterator.Column < (int)Size) && (i_CellIterator.Column >= 0);

            return isCellInBoard;
        }
        public bool IsCellInBoard(int i_CellRowIndex, int i_CellColumnIndex)
        {
            //checking if the indices given is in board limits.
            bool isCellInBoard;

            isCellInBoard = (i_CellRowIndex < (int)Size) && (i_CellRowIndex >= 0) && (i_CellColumnIndex < (int)Size) && (i_CellColumnIndex >= 0);

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
