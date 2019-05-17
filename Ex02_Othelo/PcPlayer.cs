﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class PcPlayer
    {
        private string m_PlayerName = "PC";
        GameUtilities.ePlayerColor m_PlayerColor = GameUtilities.ePlayerColor.BLACK_PLAYER;


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
    }
}
