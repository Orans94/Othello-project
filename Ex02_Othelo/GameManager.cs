using System;
using System.Collections.Generic;
using System.Text;
namespace Ex02_Othelo
{
    class GameManager
    {
        public enum eGameMode { HUMAN_VS_HUMAN = 1, HUMAN_VS_PC = 2 };
        public enum eGameDecision { REMATCH = 1, EXIT = 2 };
        public enum eDirection { UP = -1, DOWN = 1, LEFT = -1, RIGHT = 1, NO_DIRECTION = 0};

        private Board m_GameBoard;
        private List<Cell> m_BlackPlayerOptions;
        private List<Cell> m_WhitePlayerOptions;
        private GameUtilities.ePlayerColor m_PlayerTurn;
        private eGameMode m_GameMode;

        public void Run()
        {
            //this method maintains the main loop of the game.
            HumanPlayer whiteHumanPlayer = new HumanPlayer(GameUtilities.ePlayerColor.WHITE_PLAYER);
            HumanPlayer blackHumanPlayer = new HumanPlayer(GameUtilities.ePlayerColor.BLACK_PLAYER);
            PcPlayer blackPCPlayer = new PcPlayer();
            int currentPlayerMoveRowIndex, currentPlayerMoveColumnIndex;
            bool isPlayerMoveLegal, isGameEnd = false;
            List<Cell> cellsToUpdate = new List<Cell>();
            m_BlackPlayerOptions = new List<Cell>();
            m_WhitePlayerOptions = new List<Cell>();
            eGameDecision rematchOrExit;

            configureGameSettings(whiteHumanPlayer, blackHumanPlayer);
            initialize();
            while (true)
            {
                UI.Draw(m_GameBoard);
                do
                {
                    tellCurrentPlayerToPlay(blackHumanPlayer, whiteHumanPlayer, blackPCPlayer, BlackPlayerOptions,
                        out currentPlayerMoveRowIndex, out currentPlayerMoveColumnIndex);
                    if (currentPlayerMoveColumnIndex == (int)HumanPlayer.eUserRequest.EXIT)
                    {
                        isGameEnd = true;
                        break;
                    }
                    
                    isPlayerMoveLegal = isLegalMove(currentPlayerMoveRowIndex, currentPlayerMoveColumnIndex, ref cellsToUpdate);
                }
                while (!isPlayerMoveLegal);

                if (isGameEnd)
                {
                    //the user chose to exit the game.
                    break;
                }

                GameBoard.UpdateBoard(cellsToUpdate, m_PlayerTurn);
                updatePlayersOptions();
                turnChangingManager();
                isGameEnd = isGameOver();
                Ex02.ConsoleUtils.Screen.Clear();
                if (isGameEnd)
                {
                    determineWinner();
                    rematchOrExit = UI.AskUserForRematchOrExit();
                    if (rematchOrExit == eGameDecision.REMATCH)
                    {
                        initialize();
                    }
                    else
                    {
                        // the user chose to exit game
                        break;
                    }
                }
            }
            UI.ShowExitMessage();
            System.Threading.Thread.Sleep(5000);
        }

        public void configureGameSettings(HumanPlayer whiteHumanPlayer, HumanPlayer blackHumanPlayer)
        {
            string whitePlayerName, blackPlayerName;
            GameManager.eGameMode userGameModeChoice;
            Board.eBoardSize userBoardSizeChoice;

            whitePlayerName = UI.AskUserForUserName();
            whiteHumanPlayer.Name = whitePlayerName;

            userGameModeChoice = UI.AskUserForGameMode();
            m_GameMode = userGameModeChoice;
            if (userGameModeChoice == eGameMode.HUMAN_VS_HUMAN)
            {
                blackPlayerName = UI.AskUserForUserName();
                blackHumanPlayer.Name = blackPlayerName;
            }

            userBoardSizeChoice = UI.AskUserForBoardSize();
            m_GameBoard = new Board(userBoardSizeChoice);
        }

        private void initialize()
        {
            GameBoard.Initialize();
            initializePlayersOptions();
            m_PlayerTurn = GameUtilities.ePlayerColor.WHITE_PLAYER;
        }

        private void turnChangingManager()
        {
            if (m_PlayerTurn == GameUtilities.ePlayerColor.BLACK_PLAYER && WhitePlayerOptions.Count > 0)
            {
                m_PlayerTurn = GameUtilities.ePlayerColor.WHITE_PLAYER;
            }
            else if (m_PlayerTurn == GameUtilities.ePlayerColor.WHITE_PLAYER && BlackPlayerOptions.Count > 0)
            {
                m_PlayerTurn = GameUtilities.ePlayerColor.BLACK_PLAYER;
            }
        }

        private void tellCurrentPlayerToPlay(HumanPlayer i_BlackHumanPlayer, HumanPlayer i_WhiteHumanPlayer, PcPlayer i_BlackPcPlayer, List<Cell> i_BlackPcPlayerOptions,
            out int io_CurrentMoveRowIndex, out int io_CurrentMoveColumnIndex)
        {
            //this method recieves the players, and the pcplayer options, checks who should play now and tell them to play.
            //the method will keep asking for legal input as long as it is not logicaly legal. 

            if (m_GameMode == eGameMode.HUMAN_VS_HUMAN)
            {
                if (m_PlayerTurn == GameUtilities.ePlayerColor.BLACK_PLAYER)
                {
                    i_BlackHumanPlayer.Play(m_GameBoard.Size, out io_CurrentMoveRowIndex, out io_CurrentMoveColumnIndex);
                }
                else
                {
                    i_WhiteHumanPlayer.Play(m_GameBoard.Size, out io_CurrentMoveRowIndex, out io_CurrentMoveColumnIndex);
                }
            }
            else
            {
                if (m_PlayerTurn == GameUtilities.ePlayerColor.BLACK_PLAYER)
                {
                    i_BlackPcPlayer.Play(i_BlackPcPlayerOptions, m_GameMode, out io_CurrentMoveRowIndex, out io_CurrentMoveColumnIndex);
                }
                else
                {
                    i_WhiteHumanPlayer.Play(m_GameBoard.Size, out io_CurrentMoveRowIndex, out io_CurrentMoveColumnIndex);
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

        public List<Cell> BlackPlayerOptions
        {
            get
            {
                return m_BlackPlayerOptions;
            }
        }

        public List<Cell> WhitePlayerOptions
        {
            get
            {
                return m_WhitePlayerOptions;
            }
        }

        private void updatePlayersOptions()
        {
            GameUtilities.ePlayerColor lastPlayerTurn;

            // clear players options lists
            m_WhitePlayerOptions.Clear();
            m_BlackPlayerOptions.Clear();
            // saving the last player's turn
            lastPlayerTurn = m_PlayerTurn;

            //this method is updating the players options.
            bool isCellAnOption;
            List<Cell> cellList = new List<Cell>();

            foreach (Cell cellIteator in m_GameBoard.Matrix)
            {
                if (cellIteator.Sign == ' ')
                {
                    m_PlayerTurn = GameUtilities.ePlayerColor.WHITE_PLAYER;
                    isCellAnOption = isPlayerMoveBlockingEnemy(cellIteator.Row, cellIteator.Column, ref cellList, false);
                    if (isCellAnOption)
                    {
                        m_WhitePlayerOptions.Add(cellIteator);
                    }
                    else
                    {
                        m_PlayerTurn = GameUtilities.ePlayerColor.BLACK_PLAYER;
                        isCellAnOption = isPlayerMoveBlockingEnemy(cellIteator.Row, cellIteator.Column, ref cellList, false);
                        if (isCellAnOption)
                        {
                            m_BlackPlayerOptions.Add(cellIteator);
                        }
                    }
                }
            }

            // restore the turn of the last player
            m_PlayerTurn = lastPlayerTurn;
        }

        public GameUtilities.ePlayerColor Turn
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

        private bool isLegalMove(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref List<Cell> io_CellsToUpdate)
        {
            //this method recieves a player move and return true if the move is legal, false otherwise.
            bool isPlayerMoveLegal, isCellEmpty, isMoveBlockingEnemy, isPlayerMoveAnOption;

            isPlayerMoveAnOption = isMoveInOptionsList(m_GameBoard.Matrix[i_PlayerMoveRowIndex,i_PlayerMoveColumnIndex]);

            if (isPlayerMoveAnOption)
            {
                isCellEmpty = m_GameBoard.IsCellEmpty(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex);
                isMoveBlockingEnemy = isPlayerMoveBlockingEnemy(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate);
                isPlayerMoveLegal = isCellEmpty && isMoveBlockingEnemy;
            }
            else
            {
                isPlayerMoveLegal = false;
            }
            return isPlayerMoveLegal;
        }

        private bool isMoveInOptionsList(Cell cellToSearchForInOptionsList)
        {
            bool isCellFoundInOptionsList, areCellsEqual;

            isCellFoundInOptionsList = false;
            if (m_PlayerTurn == GameUtilities.ePlayerColor.BLACK_PLAYER)
            {
                foreach (Cell cellIteator in BlackPlayerOptions)
                {
                    areCellsEqual = areTwoCellsEquals(cellIteator, cellToSearchForInOptionsList);
                    if (areCellsEqual)
                    {
                        isCellFoundInOptionsList = true;
                        break;
                    }
                }
            }
            else
            {
                foreach (Cell cellIteator in WhitePlayerOptions)
                {
                    areCellsEqual = areTwoCellsEquals(cellIteator, cellToSearchForInOptionsList);
                    if (areCellsEqual)
                    {
                        isCellFoundInOptionsList = true;
                        break;
                    }
                }
            }

            return isCellFoundInOptionsList;
        }

        private bool areTwoCellsEquals(Cell i_FirstCell, Cell i_SecondCell)
        {
            bool areCellsEqual;

            areCellsEqual = i_FirstCell.Row == i_SecondCell.Row && i_FirstCell.Column == i_SecondCell.Column;

            return areCellsEqual;
        }

        private bool isPlayerMoveBlockingEnemy(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref List<Cell> io_CellsToUpdate, bool i_AddCellsToList = true)
        {
            //this method recieves a player move and return true if the move is blocking the enemy.
            //its also updates the list of cells to update.
            bool isVerticalBlocking, isHorizontalBlocking, isDiagonalOneBlocking, isDiagonalTwoBlocking, isMoveBlockingEnemy;

            isVerticalBlocking = isVerticallyBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.UP, (int)eDirection.NO_DIRECTION, i_AddCellsToList);
            isVerticalBlocking = isVerticallyBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.DOWN, (int)eDirection.NO_DIRECTION, i_AddCellsToList) || isVerticalBlocking;

            isHorizontalBlocking = isHorizontallyBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.NO_DIRECTION, (int)eDirection.LEFT, i_AddCellsToList);
            isHorizontalBlocking = isHorizontallyBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.NO_DIRECTION, (int)eDirection.RIGHT, i_AddCellsToList) || isHorizontalBlocking;

            isDiagonalOneBlocking = isDiagonallyOneBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.UP, (int)eDirection.RIGHT, i_AddCellsToList);
            isDiagonalOneBlocking = isDiagonallyOneBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.DOWN, (int)eDirection.LEFT, i_AddCellsToList) || isDiagonalOneBlocking;

            isDiagonalTwoBlocking = isDiagonallyTwoBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.UP, (int)eDirection.LEFT, i_AddCellsToList);
            isDiagonalTwoBlocking = isDiagonallyTwoBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.DOWN, (int)eDirection.RIGHT, i_AddCellsToList) || isDiagonalTwoBlocking;

            isMoveBlockingEnemy = isVerticalBlocking || isHorizontalBlocking || isDiagonalOneBlocking || isDiagonalTwoBlocking;

            return isMoveBlockingEnemy;

        }

        private bool isDiagonallyTwoBlocking(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref List<Cell> io_CellsToUpdate, int i_VerticalDirection, int i_HorizontalDirection, bool i_AddCellsToList)
        {
            int currentRow, currentColumn, j;
            Cell cellIterator = null;
            bool isBlockingLine, isInBoardLimits;

            currentRow = i_PlayerMoveRowIndex + i_VerticalDirection;
            currentColumn = i_PlayerMoveColumnIndex + i_HorizontalDirection;

            isInBoardLimits = GameBoard.IsCellInBoard(currentRow, currentColumn);
            isBlockingLine = false;
            if (isInBoardLimits)
            {
                cellIterator = new Cell(currentRow, currentColumn, m_GameBoard.Matrix[currentRow, currentColumn].Sign);
                isBlockingLine = isSeriesFound(ref cellIterator, i_VerticalDirection, i_HorizontalDirection);
            }

            if (isBlockingLine && i_AddCellsToList == true)
            {
                if (i_HorizontalDirection == (int)eDirection.LEFT && i_VerticalDirection == (int)eDirection.UP)
                {
                    j = i_PlayerMoveRowIndex;
                    for (int i = i_PlayerMoveColumnIndex; i > cellIterator.Column; i--)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[j, i]);
                        j += (int)eDirection.UP;
                    }
                }
                else
                {// elsewise we are going RIGHT and DOWN
                    j = i_PlayerMoveRowIndex;
                    for (int i = i_PlayerMoveColumnIndex; i < cellIterator.Column; i++)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[j, i]);
                        j += (int)eDirection.DOWN;
                    }
                }
            }
            return isBlockingLine;
        }

        private bool isDiagonallyOneBlocking(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref List<Cell> io_CellsToUpdate, int i_VerticalDirection, int i_HorizontalDirection, bool i_AddCellsToList)
        {
            int currentRow, currentColumn, j;
            Cell cellIterator = null;
            bool isBlockingLine, isInBoardLimits;

            currentRow = i_PlayerMoveRowIndex + i_VerticalDirection;
            currentColumn = i_PlayerMoveColumnIndex + i_HorizontalDirection;

            isInBoardLimits = m_GameBoard.IsCellInBoard(currentRow, currentColumn);
            isBlockingLine = false;
            if (isInBoardLimits)
            {
                cellIterator = new Cell(currentRow, currentColumn, m_GameBoard.Matrix[currentRow, currentColumn].Sign);
                isBlockingLine = isSeriesFound(ref cellIterator, i_VerticalDirection, i_HorizontalDirection);

            }


            if (isBlockingLine && i_AddCellsToList == true)
            {
                if (i_HorizontalDirection == (int)eDirection.LEFT && i_VerticalDirection == (int)eDirection.DOWN)
                {
                    j = i_PlayerMoveRowIndex;
                    for (int i = i_PlayerMoveColumnIndex; i > cellIterator.Column; i--)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[j,i]);
                        j += (int)eDirection.DOWN;
                    }
                }
                else
                {// elsewise we are going UP and RIGHT
                    j = i_PlayerMoveRowIndex;
                    for (int i = i_PlayerMoveColumnIndex; i < cellIterator.Column; i++)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[j,i]);
                        j += (int)eDirection.UP;
                    }
                }
            }
            return isBlockingLine;
        }

        private bool isHorizontallyBlocking(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref List<Cell> io_CellsToUpdate, int i_VerticalDirection, int i_HorizontalDirection, bool i_AddCellsToList)
        {
            int currentRow, currentColumn;
            Cell cellIterator = null;
            bool isBlockingLine, isInBoardLimits;

            currentRow = i_PlayerMoveRowIndex + i_VerticalDirection;
            currentColumn = i_PlayerMoveColumnIndex + i_HorizontalDirection;

            isInBoardLimits = m_GameBoard.IsCellInBoard(currentRow, currentColumn);
            isBlockingLine = false;
            if (isInBoardLimits)
            {
                cellIterator = new Cell(currentRow, currentColumn, m_GameBoard.Matrix[currentRow, currentColumn].Sign);
                isBlockingLine = isSeriesFound(ref cellIterator, i_VerticalDirection, i_HorizontalDirection);
            }


            if (isBlockingLine && i_AddCellsToList == true)
            {
                if (i_HorizontalDirection == (int)eDirection.LEFT)
                {
                    for (int i = i_PlayerMoveColumnIndex; i > cellIterator.Column; i--)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[i_PlayerMoveRowIndex, i]);
                    }
                }
                else
                {
                    for (int i = i_PlayerMoveColumnIndex; i < cellIterator.Column; i++)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[i_PlayerMoveRowIndex, i]);
                    }
                }
            }
            return isBlockingLine;
        }


        private bool isSeriesFound(ref Cell i_CellIterator, int i_VerticalDirection, int i_HorizontalDirection)
        {
            // this method get directions, cell by ref
            // this method return true if series of blocks has been found

            bool isCellEnemy, isInBoardLimits;
            bool isBlockingLine, isCharSimilarToMeFound;

            isCellEnemy = false;
            isBlockingLine = false;
            isInBoardLimits = m_GameBoard.IsCellInBoard(i_CellIterator);
            if (isInBoardLimits)
            {
                isCellEnemy = isCellAnEnemy(i_CellIterator, Turn);
            }

            if (isInBoardLimits && isCellEnemy) // this condition check if the first cell is an enemy and in board, if it is countiue
            {
                do
                {
                    i_CellIterator.Row += i_VerticalDirection;
                    i_CellIterator.Column += i_HorizontalDirection;
                    //i_CellIterator.Sign = GameBoard.Matrix[i_CellIterator.Row, i_CellIterator.Column].Sign;

                    isInBoardLimits = m_GameBoard.IsCellInBoard(i_CellIterator);
                    if (isInBoardLimits)
                    {
                        i_CellIterator.Sign = m_GameBoard.Matrix[i_CellIterator.Row, i_CellIterator.Column].Sign;
                        isCellEnemy = isCellAnEnemy(i_CellIterator, Turn);
                    }

                }
                while (isInBoardLimits && isCellEnemy);
                // check why the while has been stopped
                if (isInBoardLimits)
                {
                    isCharSimilarToMeFound = i_CellIterator.Sign == (char)Turn;
                    if (isCharSimilarToMeFound)
                    {
                        isBlockingLine = true;
                    }
                }
            }
            return isBlockingLine;
        }

        private bool isVerticallyBlocking(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref List<Cell> io_CellsToUpdate, int i_VerticalDirection, int i_HorizontalDirection, bool i_AddCellsToList)
        {
            int currentRow, currentColumn;
            bool isBlockingLine, isInBoardLimits;
            Cell cellIterator = null;

            currentRow = i_PlayerMoveRowIndex + i_VerticalDirection;
            currentColumn = i_PlayerMoveColumnIndex + i_HorizontalDirection;

            isInBoardLimits = m_GameBoard.IsCellInBoard(currentRow, currentColumn);
            isBlockingLine = false;
            if (isInBoardLimits)
            {
                cellIterator = new Cell(currentRow, currentColumn, m_GameBoard.Matrix[currentRow, currentColumn].Sign);
                isBlockingLine = isSeriesFound(ref cellIterator, i_VerticalDirection, i_HorizontalDirection);

            }
         

            if (isBlockingLine && i_AddCellsToList == true)
            {
                if (i_VerticalDirection == (int)eDirection.UP)
                {
                    for (int i = i_PlayerMoveRowIndex; i > cellIterator.Row; i--)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[i, i_PlayerMoveColumnIndex]);
                    }
                }
                else
                {
                    for (int i = i_PlayerMoveRowIndex; i < cellIterator.Row; i++)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[i, i_PlayerMoveColumnIndex]);
                    }
                }
            }
            return isBlockingLine;

        }

        private bool isCellAnEnemy(Cell i_CellIterator, GameUtilities.ePlayerColor i_CurrentPlayerTurn)
        {
            bool isCellEnemy;

            isCellEnemy = i_CellIterator.Sign != (char)i_CurrentPlayerTurn && i_CellIterator.Sign != ' ';

            return isCellEnemy;
        }

        private bool isPlayerOptionEmpty(GameUtilities.ePlayerColor i_PlayerColor)
        {
            //this method recieve a PlayerColor and check if his options list is empty.
            bool isOptionListEmpty;

            if (i_PlayerColor == GameUtilities.ePlayerColor.BLACK_PLAYER)
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

            doesBothPlayersHasNoOptions = isPlayerOptionEmpty(GameUtilities.ePlayerColor.BLACK_PLAYER) && isPlayerOptionEmpty(GameUtilities.ePlayerColor.WHITE_PLAYER);

            return doesBothPlayersHasNoOptions;
        }

        public eGameMode Mode
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
            GameUtilities.ePlayerColor winner;

            whitePlayerScore = m_GameBoard.CountSignAppearances('O');
            blackPlayerScore = m_GameBoard.CountSignAppearances('X');
            if (whitePlayerScore > blackPlayerScore)
            {
                winner = GameUtilities.ePlayerColor.WHITE_PLAYER;
            }
            else
            {
                winner = GameUtilities.ePlayerColor.BLACK_PLAYER;
            }
            UI.DeclareWinner(whitePlayerScore, blackPlayerScore, winner);
        }

        private void initializePlayersOptions()
        {
            if (m_BlackPlayerOptions.Count != 0)
            {
                m_BlackPlayerOptions.Clear();
            }
            if (m_WhitePlayerOptions.Count != 0)
            {
                m_WhitePlayerOptions.Clear();
            }
            initializeBlackPlayerOptions();
            initializeWhitePlayerOptions();
        }
        private void initializeBlackPlayerOptions()
        {
            Cell cellToBeAddedToOptions1;
            Cell cellToBeAddedToOptions2;
            Cell cellToBeAddedToOptions3;
            Cell cellToBeAddedToOptions4;

            if (m_GameBoard.Size == Board.eBoardSize.bigBoard)
            {
                cellToBeAddedToOptions1 = new Cell(2, 3);
                cellToBeAddedToOptions2 = new Cell(3, 2);
                cellToBeAddedToOptions3 = new Cell(5, 4);
                cellToBeAddedToOptions4 = new Cell(4, 5);
            }
            else
            {
                cellToBeAddedToOptions1 = new Cell(1, 2);
                cellToBeAddedToOptions2 = new Cell(2, 1);
                cellToBeAddedToOptions3 = new Cell(4, 3);
                cellToBeAddedToOptions4 = new Cell(3, 4);
            }

            m_BlackPlayerOptions.Add(cellToBeAddedToOptions1);
            m_BlackPlayerOptions.Add(cellToBeAddedToOptions2);
            m_BlackPlayerOptions.Add(cellToBeAddedToOptions3);
            m_BlackPlayerOptions.Add(cellToBeAddedToOptions4);
        }

        private void initializeWhitePlayerOptions()
        {
            Cell cellToBeAddedToOptions1;
            Cell cellToBeAddedToOptions2;
            Cell cellToBeAddedToOptions3;
            Cell cellToBeAddedToOptions4;

            if (m_GameBoard.Size == Board.eBoardSize.bigBoard)
            {
                cellToBeAddedToOptions1 = new Cell(2, 4);
                cellToBeAddedToOptions2 = new Cell(3, 5);
                cellToBeAddedToOptions3 = new Cell(4, 2);
                cellToBeAddedToOptions4 = new Cell(5, 3);
            }
            else
            {
                cellToBeAddedToOptions1 = new Cell(1, 3);
                cellToBeAddedToOptions2 = new Cell(2, 4);
                cellToBeAddedToOptions3 = new Cell(3, 1);
                cellToBeAddedToOptions4 = new Cell(4, 2);
            }

            m_WhitePlayerOptions.Add(cellToBeAddedToOptions1);
            m_WhitePlayerOptions.Add(cellToBeAddedToOptions2);
            m_WhitePlayerOptions.Add(cellToBeAddedToOptions3);
            m_WhitePlayerOptions.Add(cellToBeAddedToOptions4);
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
