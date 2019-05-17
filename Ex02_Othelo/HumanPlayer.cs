using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class HumanPlayer
    {
        public enum eUserRequest { EXIT = -1 };
        private string m_PlayerName;
        GameUtilities.ePlayerColor m_PlayerColor;
        private int m_PlayerScore = 0;

        // This method ask from the player to play till his input is valid, then it return his move by ref
        // Example for valid move - "E3", then io_CurrentPlayerColumnMove will hold 4 and io_CurrentPlayerRowMove will hold 2
        public void Play(Board.eBoardSize i_BoardSize, out int io_CurrentPlayerRowMove, out int io_CurrentPlayerColumnMove)
        {
            string playerMoveString;
            bool isUserRequsetToExit;

            playerMoveString = UI.RequestPlayerToPlay(m_PlayerName, m_PlayerColor, i_BoardSize);
            isUserRequsetToExit = playerMoveString.Length == 1;
            if (isUserRequsetToExit)
            {
                io_CurrentPlayerColumnMove = (int)eUserRequest.EXIT;
                io_CurrentPlayerRowMove = (int)eUserRequest.EXIT;
            }
            else
            {
                io_CurrentPlayerColumnMove = playerMoveString[0] - 'A';
                io_CurrentPlayerRowMove = playerMoveString[1] - '1';
            }

        }

        public HumanPlayer(GameUtilities.ePlayerColor i_PlayerColor)
        {
            m_PlayerColor = i_PlayerColor;
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

        public int Score
        {
            get
            {
                return m_PlayerScore;
            }
            set
            {
                m_PlayerScore = value;
            }
        }
        public GameUtilities.ePlayerColor Color
        {
            //a propertie for m_PlayerName.
            get
            {
                return m_PlayerColor;
            }
            set
            {
                m_PlayerColor = value;
            }
        }
    }
}
