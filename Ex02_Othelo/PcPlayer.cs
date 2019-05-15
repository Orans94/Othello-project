using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class PcPlayer
    {
        private string m_PlayerName = "PC";
        GameUtilities.PlayerColor m_PlayerColor = GameUtilities.PlayerColor.BLACK_PLAYER;


        public string Play(LinkedList<Cell> i_PcPlayerOptions, out int io_CurrentMoveRowIndex, out int io_CurrentMoveColumnIndex)
        {

            // =======================
            io_CurrentMoveRowIndex = 0;
            io_CurrentMoveColumnIndex = 0;
            return null;
            // =======================
        }

        private string randomCell(LinkedList<Cell> i_PcPlayerOptions, out int io_CurrentMoveRowIndex, out int io_CurrentMoveColumnIndex)
        {
            //this method is choosing a random cell from the PcPlayerOptions
            // =======================
            io_CurrentMoveRowIndex = 0;
            io_CurrentMoveColumnIndex = 0;
            return null;
            // =======================
        }
    }
}
