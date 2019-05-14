using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class HumanPlayer
    {
        private string m_PlayerName;
        GameUtilities.PlayerColor m_PlayerColor;

        public string Play()
        {
            string playerMove;

            playerMove = UI.RequestPlayerToPlay(m_PlayerColor);
            return null;
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
