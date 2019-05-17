using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class AI
    {
        private Board m_GameBoard;

        public  AI(Board i_GameBoard)
        {
            m_GameBoard = i_GameBoard;
        }
        public static void Minmax()
        {
            
        }

        internal static void PCPlay(out int io_CurrentMoveRowIndex, out int io_CurrentMoveColumnIndex)
        {
            throw new NotImplementedException();
        }
    }
}
