using System;
using System.Collections.Generic;
using System.Text;
namespace Ex02_Othelo
{
    class GameManager
    {
        public enum GameMode { HUMAN_VS_HUMAN = 1, HUMAN_VS_PC = 2 };
        public enum GameDecision { REMATCH = 1, EXIT = 2 };
        //public enum PlayerColor { BLACK_PLAYER, WHITE_PLAYER };

        private Board m_GameBoard;
        private LinkedList<Cell> m_BlackPlayerOptions;
        private LinkedList<Cell> m_WhitePlayerOptions;
        private GameUtilities.PlayerColor m_PlayerTurn;
        private GameMode m_GameMode;

        public void Run()
        {
            string blackPlayerName, whitePlayerName;
            GameMode userGameModeChoice;
            Board.eBoardSize userBoardSizeChoice;
            HumanPlayer whiteHumanPlayer = new HumanPlayer();
            HumanPlayer blackHumanPlayer = new HumanPlayer();
            PcPlayer blackPCPlayer = new PcPlayer();
            int currentPlayerMoveRowIndex, currentPlayerMoveColumnIndex;
            bool isPlayerMoveLegal;
            LinkedList<Cell> cellsToUpdate = new LinkedList<Cell>();

            //this method maintains the main loop of the game.

            //TODO: pack all the initializing of the user input into one method

            // 1. Ask user name
            whitePlayerName = UI.AskUserForUserName();
            whiteHumanPlayer.Name = whitePlayerName;
            // 2. Ask for gamemode
            userGameModeChoice = UI.AskUserForGameMode();
            Mode = userGameModeChoice;
            //      2.1 if the user chose human vs human then enter second player name.
            if (userGameModeChoice == GameMode.HUMAN_VS_HUMAN)
            {
                blackPlayerName = UI.AskUserForUserName();
                blackHumanPlayer.Name = blackPlayerName;
            }
            // 3. Choose matrix size
            userBoardSizeChoice = UI.AskUserForBoardSize();
            //      3.1 make m_GameBoard according to user board size choice
            m_GameBoard = new Board(userBoardSizeChoice);
            // 4. Initialize board
            GameBoard.Initialize();
            // 5. Draw
            UI.Draw(GameBoard);
            // start the main loop of the game:

            // 6. ask player to play
            //maybe put those two in one function that will demand legal move in a while loop
            do
            {
                tellCurrentPlayerToPlay(blackHumanPlayer, whiteHumanPlayer, blackPCPlayer, BlackPlayerOptions,
                    out currentPlayerMoveRowIndex, out currentPlayerMoveColumnIndex);
                isPlayerMoveLegal = isLegalMove(currentPlayerMoveRowIndex, currentPlayerMoveColumnIndex, ref cellsToUpdate);
            }
            while (!isPlayerMoveLegal);
            // 7. Update board (the user input is legal at this stage)

            // 8. 


        }

        private void tellCurrentPlayerToPlay(HumanPlayer i_BlackHumanPlayer, HumanPlayer i_WhiteHumanPlayer, PcPlayer i_BlackPcPlayer, LinkedList<Cell> i_BlackPcPlayerOptions,
            out int io_CurrentMoveRowIndex, out int io_CurrentMoveColumnIndex)
        {
            bool isPlayerMoveLegal;
            //this method recieves the players, and the pcplayer options, checks who should play now and tell them to play.
            //the method will keep asking for legal input as long as it is not logicaly legal. 

            if (Mode == GameMode.HUMAN_VS_HUMAN)
            {
                if (Turn == GameUtilities.PlayerColor.BLACK_PLAYER)
                {
                    i_BlackHumanPlayer.Play(GameBoard.Size, out io_CurrentMoveRowIndex, out io_CurrentMoveColumnIndex);
                }
                else
                {
                    i_WhiteHumanPlayer.Play(GameBoard.Size, out io_CurrentMoveRowIndex, out io_CurrentMoveColumnIndex);
                }
            }
            else
            {
                if (Turn == GameUtilities.PlayerColor.BLACK_PLAYER)
                {
                    i_BlackPcPlayer.Play(i_BlackPcPlayerOptions, out io_CurrentMoveRowIndex, out io_CurrentMoveColumnIndex);
                }
                else
                {
                    i_WhiteHumanPlayer.Play(GameBoard.Size, out io_CurrentMoveRowIndex, out io_CurrentMoveColumnIndex);
                }
            }

        }

        public Board GameBoard
        {
            // a propertie for m_GameBoard.
            get
            {
                return m_GameBoard;
            }
            //TODO: ask guy about better way to initilzie the Board.
            //set
            //{
            //    m_GameBoard = new Board(value);
            //}
        }

        public LinkedList<Cell> BlackPlayerOptions
        {
            get
            {
                return m_BlackPlayerOptions;
            }
        }

        public LinkedList<Cell> WhitePlayerOptions
        {
            get
            {
                return m_WhitePlayerOptions;
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

        private bool isLegalMove(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref LinkedList<Cell> io_CellsToUpdate)
        {
            //this method recieves a player move and return true if the move is legal, false otherwise.
            bool isPlayerMoveLegal, isCellEmpty, isMoveBlockingEnemy;

            isCellEmpty = GameBoard.IsCellEmpty(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex);
            isMoveBlockingEnemy = isPlayerMoveBlockingEnemy(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate);
            isPlayerMoveLegal = isCellEmpty && isMoveBlockingEnemy;

            return isPlayerMoveLegal;
        }

        private bool isPlayerMoveBlockingEnemy(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref LinkedList<Cell> io_CellsToUpdate)
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

        private void determineWinner()
        {
            int whitePlayerScore, blackPlayerScore;
            GameUtilities.PlayerColor winner;

            whitePlayerScore = GameBoard.CountSignAppearances('O');
            blackPlayerScore = GameBoard.CountSignAppearances('X');
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
