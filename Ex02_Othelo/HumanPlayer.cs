using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class HumanPlayer
    {
        private string m_PlayerName;
        GameUtilities.PlayerColor m_PlayerColor;

        // This method ask from the player to play till his input is valid, then it return his move by ref
        // Example for valid move - "E3", then io_CurrentPlayerColumnMove will hold 4 and io_CurrentPlayerRowMove will hold 2
        public void Play(Board.eBoardSize i_BoardSize, out int io_CurrentPlayerRowMove, out int io_CurrentPlayerColumnMove)
        {
            string playerMoveString;

            playerMoveString = UI.RequestPlayerToPlay(m_PlayerColor, i_BoardSize);
            io_CurrentPlayerColumnMove = playerMoveString[0] - 'A';
            io_CurrentPlayerRowMove = playerMoveString[1] - '1';
        }

        public string Name
        {
            //a propertie for m_PlayerName.
            get
            {
                return m_PlayerName;
            }
            set
            {
                 m_PlayerName = value;
            }
        }
    }
}
