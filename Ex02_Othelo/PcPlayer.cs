using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    public class PcPlayer
    {
        private string m_PlayerName = "PC";
        private int m_PlayerScore;
        private bool m_isPlayerPlaying = false;

        public void Play(Board i_GameBoard, GameManager.eGameMode gameMode, out int io_CurrentMoveRowIndex, out int io_CurrentMoveColumnIndex)
        {
            Random randomMove = new Random();
            Cell randomedCell = new Cell();

            UI.PCIsThinkingMessage();
            AI.PCPlay(i_GameBoard, out io_CurrentMoveRowIndex, out io_CurrentMoveColumnIndex);
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

        public string Name
        {
            get
            {

                return m_PlayerName;
            }
        }

        public bool Active
        {
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
