using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    public class Board
    {
        public enum eBoardSize
        {
            smallBoard = 6,
            bigBoard = 8
        }

        private eBoardSize m_BoardSize;
        private Cell[,] m_Board;

        public Board(eBoardSize i_BoardSize)
        {
            // create board according to the size that the user chose.
            m_BoardSize = i_BoardSize;
            m_Board = new Cell[(int)i_BoardSize, (int)i_BoardSize];
            for (int rowIndex = 0; rowIndex < (int)i_BoardSize; rowIndex++)
            {
                for (int colIndex = 0; colIndex < (int)i_BoardSize; colIndex++)
                {
                    m_Board[rowIndex, colIndex] = new Cell(rowIndex, colIndex);
                }
            }
        }

        public Board Clone()
        {
            Board newBoard = new Board(this.m_BoardSize);
            Cell cellToCopy;

            for (int row = 0; row < (int)m_BoardSize; row++)
            {
                for (int column = 0; column < (int)m_BoardSize; column++)
                {
                    cellToCopy = new Cell(row, column, m_Board[row, column].Sign);
                    newBoard.m_Board[row, column] = cellToCopy;
                }
            }

            return newBoard;
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
            // updating the board after the player move
            m_Board[i_RowIndex, i_ColumnIndex].Sign = i_Sign;
        }

        public void Initialize()
        {
            // TODO: Add consts
            clear();
            if (Size == eBoardSize.bigBoard)
            {
                m_Board[3, 3].Sign = 'O';
                m_Board[4, 3].Sign = 'X';
                m_Board[4, 4].Sign = 'O';
                m_Board[3, 4].Sign = 'X';
            }
            else if (Size == eBoardSize.smallBoard)
            {
                m_Board[2, 2].Sign = 'O';
                m_Board[3, 2].Sign = 'X';
                m_Board[3, 3].Sign = 'O';
                m_Board[2, 3].Sign = 'X';
            }
        }

        private void clear()
        {
            // clearing the board and putting in all the cells space sign ' '.
            foreach (Cell cell in m_Board)
            {
                cell.Sign = Cell.k_Empty;
            }
        }

        public void UpdateBoard(List<Cell> i_CellsToUpdate, GameUtilities.ePlayerColor i_PlayingPlayer)
        {
            // this method recieves a list of cells and a player color and put the correct sign in those cells.
            foreach (Cell currentCell in i_CellsToUpdate)
            {
                m_Board[currentCell.Row, currentCell.Column].Sign = (char)i_PlayingPlayer;
            }

            i_CellsToUpdate.Clear();
        }

        public bool IsCellEmpty(int i_RowIndex, int i_ColumnIndex)
        {
            // checking if a cell is empty (has ' ' char in it).
            bool isCellEmpty;

            isCellEmpty = m_Board[i_RowIndex, i_ColumnIndex].IsEmpty();

            return isCellEmpty;
        }

        public bool IsCellInBoard(Cell i_CellIterator)
        {
            // checking if the cell given is in board limits.
            bool isCellInBoard;

            isCellInBoard = (i_CellIterator.Row < (int)m_BoardSize) && (i_CellIterator.Row >= 0) && (i_CellIterator.Column < (int)m_BoardSize) && (i_CellIterator.Column >= 0);

            return isCellInBoard;
        }

        public bool IsCellInBoard(int i_CellRowIndex, int i_CellColumnIndex)
        {
            // checking if the indices given is in board limits.
            bool isCellInBoard;

            isCellInBoard = (i_CellRowIndex < (int)m_BoardSize) && (i_CellRowIndex >= 0) && (i_CellColumnIndex < (int)m_BoardSize) && (i_CellColumnIndex >= 0);

            return isCellInBoard;
        }

        public int CountSignAppearances(char i_Sign)
        {
            // this method recieves a char and return the amount of appearances of that char in the board.
            int countOfSignAppearances = 0;

            foreach (Cell cell in m_Board)
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