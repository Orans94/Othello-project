using System;
using System.Collections.Generic;
using System.Text;
namespace Ex02_Othelo
{
    class GameManager
    {
        public enum GameMode {HUMAN_VS_HUMAN = 1, HUMAN_VS_PC = 2};
        public enum GameDecision{REMATCH = 1, EXIT = 2};
        //public enum PlayerColor { BLACK_PLAYER, WHITE_PLAYER };

        private Board m_GameBoard;
        private LinkedList<Cell> m_BlackPlayerOptions;
        private LinkedList<Cell> m_WhitePlayerOptions;
        private GameUtilities.PlayerColor m_PlayerTurn;
        private GameMode m_GameMode;

        public void Run()
        {
            string blackPlayerName = "PC", whitePlayerName;
            GameMode userGameModeChoice;
            Board.eBoardSize userBoardSizeChoice;
            //this method maintains the main loop of the game.

            // 1. Ask user name
            whitePlayerName = UI.AskUserForUserName();

            // 2. Ask for gamemode
            userGameModeChoice = UI.AskUserForGameMode();
            //      2.1 if the user chose human vs human than enter second player name.
            if(userGameModeChoice == GameMode.HUMAN_VS_HUMAN)
            {
                blackPlayerName = UI.AskUserForUserName();
            }
            // 3. Choose matrix size
            userBoardSizeChoice = UI.AskUserForBoardSize();
            //      3.1 make m_GameBoard according to user board size choice
            m_GameBoard = new Board(userBoardSizeChoice);
            // 4. Initialize board
            m_GameBoard.Initialize();
            // 5. Draw
            UI.Draw(m_GameBoard);
            // start the main loop of the game:
            //6. ask player to play
            //7.check if the play is legal(blocking enemy)
            //  7.1 if not legal, ask for a new move
            //  7.2 if legal, update options and update board

           
        }

        public Board GameBoard
        {
            // a propertie for m_GameBoard.
            get
            {
                return m_GameBoard;
            }
        }
        
        private void updatePlayersOptions(Board i_GameBoard, LinkedList<Cell> i_BlackPlayerOption, LinkedList<Cell> i_WhitePlayerOptions)
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
            return true;
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

            doesBothPlayersHasNoOptions = isPlayerOptionEmpty(GameUtilities.PlayerColor.BLACK_PLAYER) && isPlayerOptionEmpty(GameUtilities.PlayerColor.WHITE_PLAYER);

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

        private void initializePlayersOptions()
        {

        }

        private void restartGame()
        {
            //this method restarts a game.
        }

        private void exitGame()
        {
            //this method exiting the game and calls the ui to show the exit message.
            UI.ShowExitMessage();
        }
    }
}
