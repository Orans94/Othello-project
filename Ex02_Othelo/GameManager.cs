using System;
using System.Collections.Generic;
using System.Text;
namespace Ex02_Othelo
{
    class GameManager
    {
        public enum GameMode { HUMAN_VS_HUMAN = 1, HUMAN_VS_PC = 2 };
        public enum GameDecision { REMATCH = 1, EXIT = 2 };
        public enum Direction { UP = -1, DOWN = 1, LEFT = -1, RIGHT = 1, NO_DIRECTION = 0};
        

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
            HumanPlayer whiteHumanPlayer = new HumanPlayer(GameUtilities.PlayerColor.WHITE_PLAYER);
            HumanPlayer blackHumanPlayer = new HumanPlayer(GameUtilities.PlayerColor.BLACK_PLAYER);
            PcPlayer blackPCPlayer = new PcPlayer();
            int currentPlayerMoveRowIndex, currentPlayerMoveColumnIndex;
            bool isPlayerMoveLegal, isGameEnd = false;
            LinkedList<Cell> cellsToUpdate = new LinkedList<Cell>();
            m_BlackPlayerOptions = new LinkedList<Cell>();
            m_WhitePlayerOptions = new LinkedList<Cell>();
            GameDecision rematchOrExit;

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
            initialize();

            // 5. Draw
            while (true)
            {
                UI.Draw(GameBoard);
                // start the main loop of the game:

                // 6. ask player to play
                //maybe put those two in one function that will demand legal move in a while loop
                do
                {
                    tellCurrentPlayerToPlay(blackHumanPlayer, whiteHumanPlayer, blackPCPlayer, BlackPlayerOptions,
                        out currentPlayerMoveRowIndex, out currentPlayerMoveColumnIndex);
                    if (currentPlayerMoveColumnIndex == (int)HumanPlayer.UserRequest.EXIT)
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
                // 7. Update board (the user input is legal at this stage)
                GameBoard.UpdateBoard(cellsToUpdate, Turn);
                cellsToUpdate.Clear();


                // 8. Update both players options linked lists.
                updatePlayersOptions();

                // 9. Manage turns changing
                turnChangingManager();

                // 10. Check if game is over
                isGameEnd = isGameOver();

                Ex02.ConsoleUtils.Screen.Clear();

                if (isGameEnd)
                {
                    determineWinner();
                    rematchOrExit = UI.AskUserForRematchOrExit();
                    if (rematchOrExit == GameDecision.REMATCH)
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

        private void initialize()
        {
            GameBoard.Initialize();
            initializePlayersOptions();
            Turn = GameUtilities.PlayerColor.WHITE_PLAYER;
        }

        private void turnChangingManager()
        {
            if (Turn == GameUtilities.PlayerColor.BLACK_PLAYER && WhitePlayerOptions.Count > 0)
            {
                Turn = GameUtilities.PlayerColor.WHITE_PLAYER;
            }
            else if (Turn == GameUtilities.PlayerColor.WHITE_PLAYER && BlackPlayerOptions.Count > 0)
            {
                Turn = GameUtilities.PlayerColor.BLACK_PLAYER;
            }
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

        private void updatePlayersOptions()
        {
            GameUtilities.PlayerColor lastPlayerTurn;

            // clear players options lists
            m_WhitePlayerOptions.Clear();
            m_BlackPlayerOptions.Clear();


            // saving the last player's turn
            lastPlayerTurn = Turn;

            //this method is updating the players options.
            bool isCellAnOption;
            LinkedList<Cell> cellList = new LinkedList<Cell>();

            foreach (Cell cellIteator in GameBoard.Matrix)
            {
                if (cellIteator.Sign == ' ')
                {
                    Turn = GameUtilities.PlayerColor.WHITE_PLAYER;
                    isCellAnOption = isPlayerMoveBlockingEnemy(cellIteator.Row, cellIteator.Column, ref cellList, false);
                    if (isCellAnOption)
                    {
                        WhitePlayerOptions.AddLast(cellIteator);
                    }
                    else
                    {
                        Turn = GameUtilities.PlayerColor.BLACK_PLAYER;
                        isCellAnOption = isPlayerMoveBlockingEnemy(cellIteator.Row, cellIteator.Column, ref cellList, false);
                        if (isCellAnOption)
                        {
                            BlackPlayerOptions.AddLast(cellIteator);
                        }
                    }
                }
            }

            // restore the turn of the last player
            Turn = lastPlayerTurn;
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
            bool isPlayerMoveLegal, isCellEmpty, isMoveBlockingEnemy, isPlayerMoveAnOption;

            isPlayerMoveAnOption = isMoveInOptionsList(GameBoard.Matrix[i_PlayerMoveRowIndex,i_PlayerMoveColumnIndex]);

            if (isPlayerMoveAnOption)
            {
                isCellEmpty = GameBoard.IsCellEmpty(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex);
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
            if (Turn == GameUtilities.PlayerColor.BLACK_PLAYER)
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

        private bool isPlayerMoveBlockingEnemy(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref LinkedList<Cell> io_CellsToUpdate, bool i_AddCellsToList = true)
        {
            //this method recieves a player move and return true if the move is blocking the enemy.
            //its also updates the list of cells to update.
            bool isVerticalBlocking, isHorizontalBlocking, isDiagonalOneBlocking, isDiagonalTwoBlocking, isMoveBlockingEnemy;

            isVerticalBlocking = isVerticallyBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)Direction.UP, (int)Direction.NO_DIRECTION, i_AddCellsToList);
            isVerticalBlocking = isVerticallyBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)Direction.DOWN, (int)Direction.NO_DIRECTION, i_AddCellsToList) || isVerticalBlocking;

            isHorizontalBlocking = isHorizontallyBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)Direction.NO_DIRECTION, (int)Direction.LEFT, i_AddCellsToList);
            isHorizontalBlocking = isHorizontallyBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)Direction.NO_DIRECTION, (int)Direction.RIGHT, i_AddCellsToList) || isHorizontalBlocking;

            isDiagonalOneBlocking = isDiagonallyOneBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)Direction.UP, (int)Direction.RIGHT, i_AddCellsToList);
            isDiagonalOneBlocking = isDiagonallyOneBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)Direction.DOWN, (int)Direction.LEFT, i_AddCellsToList) || isDiagonalOneBlocking;

            isDiagonalTwoBlocking = isDiagonallyTwoBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)Direction.UP, (int)Direction.LEFT, i_AddCellsToList);
            isDiagonalTwoBlocking = isDiagonallyTwoBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)Direction.DOWN, (int)Direction.RIGHT, i_AddCellsToList) || isDiagonalTwoBlocking;

            isMoveBlockingEnemy = isVerticalBlocking || isHorizontalBlocking || isDiagonalOneBlocking || isDiagonalTwoBlocking;

            return isMoveBlockingEnemy;

        }

        private bool isDiagonallyTwoBlocking(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref LinkedList<Cell> io_CellsToUpdate, int i_VerticalDirection, int i_HorizontalDirection, bool i_AddCellsToList)
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
                cellIterator = new Cell(currentRow, currentColumn, GameBoard.Matrix[currentRow, currentColumn].Sign);
                isBlockingLine = isSeriesFound(ref cellIterator, i_VerticalDirection, i_HorizontalDirection);
            }


            if (isBlockingLine && i_AddCellsToList == true)
            {
                if (i_HorizontalDirection == (int)Direction.LEFT && i_VerticalDirection == (int)Direction.UP)
                {
                    j = i_PlayerMoveRowIndex;
                    for (int i = i_PlayerMoveColumnIndex; i > cellIterator.Column; i--)
                    {
                        io_CellsToUpdate.AddLast(GameBoard.Matrix[j, i]);
                        j += (int)Direction.UP;
                    }
                }
                else
                {// elsewise we are going RIGHT and DOWN
                    j = i_PlayerMoveRowIndex;
                    for (int i = i_PlayerMoveColumnIndex; i < cellIterator.Column; i++)
                    {
                        io_CellsToUpdate.AddLast(GameBoard.Matrix[j, i]);
                        j += (int)Direction.DOWN;
                    }
                }
            }
            return isBlockingLine;
        }

        private bool isDiagonallyOneBlocking(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref LinkedList<Cell> io_CellsToUpdate, int i_VerticalDirection, int i_HorizontalDirection, bool i_AddCellsToList)
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
                cellIterator = new Cell(currentRow, currentColumn, GameBoard.Matrix[currentRow, currentColumn].Sign);
                isBlockingLine = isSeriesFound(ref cellIterator, i_VerticalDirection, i_HorizontalDirection);

            }


            if (isBlockingLine && i_AddCellsToList == true)
            {
                if (i_HorizontalDirection == (int)Direction.LEFT && i_VerticalDirection == (int)Direction.DOWN)
                {
                    j = i_PlayerMoveRowIndex;
                    for (int i = i_PlayerMoveColumnIndex; i > cellIterator.Column; i--)
                    {
                        io_CellsToUpdate.AddLast(GameBoard.Matrix[j,i]);
                        j += (int)Direction.DOWN;
                    }
                }
                else
                {// elsewise we are going UP and RIGHT
                    j = i_PlayerMoveRowIndex;
                    for (int i = i_PlayerMoveColumnIndex; i < cellIterator.Column; i++)
                    {
                        io_CellsToUpdate.AddLast(GameBoard.Matrix[j,i]);
                        j += (int)Direction.UP;
                    }
                }
            }
            return isBlockingLine;
        }

        private bool isHorizontallyBlocking(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref LinkedList<Cell> io_CellsToUpdate, int i_VerticalDirection, int i_HorizontalDirection, bool i_AddCellsToList)
        {
            int currentRow, currentColumn;
            Cell cellIterator = null;
            bool isBlockingLine, isInBoardLimits;

            currentRow = i_PlayerMoveRowIndex + i_VerticalDirection;
            currentColumn = i_PlayerMoveColumnIndex + i_HorizontalDirection;

            isInBoardLimits = GameBoard.IsCellInBoard(currentRow, currentColumn);
            isBlockingLine = false;
            if (isInBoardLimits)
            {
                cellIterator = new Cell(currentRow, currentColumn, GameBoard.Matrix[currentRow, currentColumn].Sign);
                isBlockingLine = isSeriesFound(ref cellIterator, i_VerticalDirection, i_HorizontalDirection);
            }


            if (isBlockingLine && i_AddCellsToList == true)
            {
                if (i_HorizontalDirection == (int)Direction.LEFT)
                {
                    for (int i = i_PlayerMoveColumnIndex; i > cellIterator.Column; i--)
                    {
                        io_CellsToUpdate.AddLast(GameBoard.Matrix[i_PlayerMoveRowIndex, i]);
                    }
                }
                else
                {
                    for (int i = i_PlayerMoveColumnIndex; i < cellIterator.Column; i++)
                    {
                        io_CellsToUpdate.AddLast(GameBoard.Matrix[i_PlayerMoveRowIndex, i]);
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
            isInBoardLimits = GameBoard.IsCellInBoard(i_CellIterator);
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

                    isInBoardLimits = GameBoard.IsCellInBoard(i_CellIterator);
                    if (isInBoardLimits)
                    {
                        i_CellIterator.Sign = GameBoard.Matrix[i_CellIterator.Row, i_CellIterator.Column].Sign;
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

        private bool isVerticallyBlocking(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref LinkedList<Cell> io_CellsToUpdate, int i_VerticalDirection, int i_HorizontalDirection, bool i_AddCellsToList)
        {
            int currentRow, currentColumn;
            bool isBlockingLine, isInBoardLimits;
            Cell cellIterator = null;

            currentRow = i_PlayerMoveRowIndex + i_VerticalDirection;
            currentColumn = i_PlayerMoveColumnIndex + i_HorizontalDirection;

            isInBoardLimits = GameBoard.IsCellInBoard(currentRow, currentColumn);
            isBlockingLine = false;
            if (isInBoardLimits)
            {
                cellIterator = new Cell(currentRow, currentColumn, GameBoard.Matrix[currentRow, currentColumn].Sign);
                isBlockingLine = isSeriesFound(ref cellIterator, i_VerticalDirection, i_HorizontalDirection);

            }
         

            if (isBlockingLine && i_AddCellsToList == true)
            {
                if (i_VerticalDirection == (int)Direction.UP)
                {
                    for (int i = i_PlayerMoveRowIndex; i > cellIterator.Row; i--)
                    {
                        io_CellsToUpdate.AddLast(GameBoard.Matrix[i, i_PlayerMoveColumnIndex]);
                    }
                }
                else
                {
                    for (int i = i_PlayerMoveRowIndex; i < cellIterator.Row; i++)
                    {
                        io_CellsToUpdate.AddLast(GameBoard.Matrix[i, i_PlayerMoveColumnIndex]);
                    }
                }
            }
            return isBlockingLine;

        }

        private bool isCellAnEnemy(Cell i_CellIterator, GameUtilities.PlayerColor i_CurrentPlayerTurn)
        {
            bool isCellEnemy;

            isCellEnemy = i_CellIterator.Sign != (char)i_CurrentPlayerTurn && i_CellIterator.Sign != ' ';

            return isCellEnemy;
        }

        private bool isPlayerOptionEmpty(GameUtilities.PlayerColor i_PlayerColor)
        {
            //this method recieve a PlayerColor and check if his options list is empty.
            bool isOptionListEmpty;

            if (i_PlayerColor == GameUtilities.PlayerColor.BLACK_PLAYER)
            {
                isOptionListEmpty = BlackPlayerOptions.Count == 0;
            }
            else
            {
                //if not black player - than its a white player.
                isOptionListEmpty = WhitePlayerOptions.Count == 0;
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
            if (BlackPlayerOptions.Count != 0)
            {
                BlackPlayerOptions.Clear();
            }
            if (WhitePlayerOptions.Count != 0)
            {
                WhitePlayerOptions.Clear();
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

            if (GameBoard.Size == Board.eBoardSize.bigBoard)
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

            BlackPlayerOptions.AddLast(cellToBeAddedToOptions1);
            BlackPlayerOptions.AddLast(cellToBeAddedToOptions2);
            BlackPlayerOptions.AddLast(cellToBeAddedToOptions3);
            BlackPlayerOptions.AddLast(cellToBeAddedToOptions4);

        }

        private void initializeWhitePlayerOptions()
        {
            Cell cellToBeAddedToOptions1;
            Cell cellToBeAddedToOptions2;
            Cell cellToBeAddedToOptions3;
            Cell cellToBeAddedToOptions4;

            if (GameBoard.Size == Board.eBoardSize.bigBoard)
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

            WhitePlayerOptions.AddLast(cellToBeAddedToOptions1);
            WhitePlayerOptions.AddLast(cellToBeAddedToOptions2);
            WhitePlayerOptions.AddLast(cellToBeAddedToOptions3);
            WhitePlayerOptions.AddLast(cellToBeAddedToOptions4);
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
