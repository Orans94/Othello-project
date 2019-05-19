using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    public class HumanPlayer
    {
        public enum eUserRequest
        {
            Exit = -1
        }

        private string m_PlayerName;
        private int m_PlayerScore;
        private bool m_isPlayerPlaying = false;
        private GameUtilities.ePlayerColor m_PlayerColor;

        public void Play(Board.eBoardSize i_BoardSize, out int io_CurrentPlayerRowMove, out int io_CurrentPlayerColumnMove)
        {
        // This method ask from the player to play till his input is valid, then it return his move by ref
        // Example for valid move - "E3", then io_CurrentPlayerColumnMove will hold 4 and io_CurrentPlayerRowMove will hold 2
            string playerMoveString;
            bool isUserRequsetToExit;

            playerMoveString = UI.RequestPlayerToPlay(m_PlayerName, m_PlayerColor, i_BoardSize);
            isUserRequsetToExit = playerMoveString.Length == 1;
            if (isUserRequsetToExit)
            {
                io_CurrentPlayerColumnMove = (int)eUserRequest.Exit;
                io_CurrentPlayerRowMove = (int)eUserRequest.Exit;
            }
            else
            {
                io_CurrentPlayerColumnMove = playerMoveString[0] - 'A';
                io_CurrentPlayerRowMove = playerMoveString[1] - '1';
            }
        }

        public HumanPlayer(GameUtilities.ePlayerColor i_PlayerColor)
        {
            // Human player c'tor
            m_PlayerColor = i_PlayerColor;
        }

        public string Name
        {
            // a propertie for m_PlayerName.
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
            // a propertie for m_PlayerScore
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
            // a propertie for m_PlayerName.
            get
            {

                return m_PlayerColor;
            }

            set
            {
                m_PlayerColor = value;
            }
        }

        public bool Active
        {
            // a propertie for m_isPlayerPlaying
            get
            {

                return m_isPlayerPlaying;
            }

            set
            {
                m_isPlayerPlaying = value;
            }
        }
    }
}
