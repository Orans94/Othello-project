﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class PcPlayer
    {
        private string m_PlayerName = "PC";
        private int m_PlayerScore = 2;
        private bool m_isPlayerPlaying = false;

        public void Play(List<Cell> i_PcPlayerOptions, GameManager.eGameMode gameMode, out int io_CurrentMoveRowIndex, out int io_CurrentMoveColumnIndex)
        {
            Random randomMove = new Random();
            int randomNum;
            Cell randomedCell = new Cell();

            UI.PCIsThinkingMessage();
            randomNum = randomMove.Next(i_PcPlayerOptions.Count);
            randomedCell = i_PcPlayerOptions[randomNum];

            io_CurrentMoveColumnIndex = randomedCell.Column;
            io_CurrentMoveRowIndex = randomedCell.Row;
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
