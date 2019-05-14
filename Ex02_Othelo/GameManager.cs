using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class GameManager
    {
        public enum GameMode { HUMAN_VS_HUMAN, HUMAN_VS_PC };
        //public enum PlayerColor { BLACK_PLAYER, WHITE_PLAYER };

        private Board m_GameBoard;
        private List<Cell> m_BlackPlayerOptions;
        private List<Cell> m_WhitePlayerOptions;
        private PlayerColor m_PlayerTurn;
        private GameMode m_GameMode;

        public static void Run()
        {
            //this method maintains the main loop of the game.

        }

        public Board GameBoard
        {
            // a propertie for m_GameBoard.
            get
            {
                return m_GameBoard;
            }
        }
        
        private void updatePlayersOptions(Board i_GameBoard, List<Cell> i_BlackPlayerOption, List<Cell> i_WhitePlayerOptions)
        {
            //this method is updating the players options.
        }

        public GameUtilities.PlayerColor Turn
        {
            //a propertie for m_PlayerTurn
            get
            {
                return m_PlayerTurn;
            }
            set
            {
                m_PlayerTurn = value;
            }
        }

        private bool isLegalMove(string i_PlayerMove)
        {
            //this method recieves a player move and return true if the move is legal, false otherwise.
            int playerMoveRowIndex, playerMoveColumnIndex;
            bool isPlayerMoveLegal, isCellEmpty, isCellInBoard, isMoveBlockingEnemy;

            convertPlayerMoveFromStringToIndices(out playerMoveRowIndex, out playerMoveColumnIndex, i_PlayerMove);
            isCellEmpty = m_GameBoard.IsCellEmpty(playerMoveRowIndex, playerMoveColumnIndex);
            isCellInBoard = m_GameBoard.IsCellInBoard(playerMoveRowIndex, playerMoveColumnIndex);
            isMoveBlockingEnemy = isPlayerMoveBlockingEnemy(i_PlayerMove);
            isPlayerMoveLegal = isCellEmpty && isCellInBoard && isMoveBlockingEnemy;

            return isPlayerMoveLegal;
        }

        private bool isPlayerMoveBlockingEnemy(string i_PlayerMove)
        {
            //this method recieves a player move and return true if the move is blocking the enemy.
        }

        private bool isPlayerOptionEmpty(GameUtilities.PlayerColor i_PlayerColor)
        {
            //this method recieve a PlayerColor and check if his options list is empty.
            bool isOptionListEmpty;

            if (i_PlayerColor == GameUtilities.PlayerColor.BLACK_PLAYER)
            {
                isOptionListEmpty = m_BlackPlayerOptions.Count == 0;
            }
            else
            {
                //if not black player - than its a white player.
                isOptionListEmpty = m_WhitePlayerOptions.Count == 0;
            }

            return isOptionListEmpty;
        }

        private bool isGameOver()
        {
            //this method checks if the game is over(if both of the players has no options to play).
            bool doesBothPlayersHasNoOptions;

            doesBothPlayersHasNoOptions = isPlayerOptionEmpty(GameUtilities.PlayerColor.BLACK_PLAYER) && isPlayerOptionEmpty(PlayerColor.WHITE_PLAYER);

            return doesBothPlayersHasNoOptions;
        }

        public GameMode Mode
        {
            //a propertie for m_GameMode
            get
            {
                return m_GameMode;
            }
            set
            {
                m_GameMode = value;
            }
        }
        private void convertPlayerMoveFromStringToIndices(out int o_PlayerMoveRowIndex, out int o_PlayerMoveColumnIndex, string i_PlayerMoveString)
        {
            int rowIndex, columnIndex;

            rowIndex = i_PlayerMoveString[1] - '1';
            columnIndex = i_PlayerMoveString[0] - 'A';


            o_PlayerMoveColumnIndex = int.Parse(columnIndex.ToString());
            o_PlayerMoveRowIndex = int.Parse(rowIndex.ToString());
        }

        private void determineWinner()
        {
            int whitePlayerScore, blackPlayerScore;
            GameUtilities.PlayerColor winner;

            whitePlayerScore = m_GameBoard.CountSignAppearances('O');
            blackPlayerScore = m_GameBoard.CountSignAppearances('X');
            if (whitePlayerScore > blackPlayerScore)
            {
                winner = GameUtilities.PlayerColor.WHITE_PLAYER;
            }
            else
            {
                winner = GameUtilities.PlayerColor.BLACK_PLAYER;
            }
            UI.DeclareWinner(whitePlayerScore, blackPlayerScore, winner);
        }

        private void restartGame()
        {
            //this method restarts a game.
        }

        private void exitGame()
        {
            //this method exiting the game and calls the ui to show the exit message.
        }
    }
}
