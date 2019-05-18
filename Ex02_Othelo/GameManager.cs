using System;
using System.Collections.Generic;
using System.Text;
namespace Ex02_Othelo
{
    public class GameManager
    {
        public enum eGameMode 
        { 
            HumanVsHuman = 1,
            HumanVsPC = 2
        }

        public enum eGameDecision
        {
            Rematch = 1,
            Exit = 2 
        }

        public enum eDirection
        { 
            Up = -1,
            Down = 1,
            Left = -1,
            Right = 1,
            NoDirection = 0
        }

        private Board m_GameBoard;
        private List<Cell> m_BlackPlayerOptions = new List<Cell>();
        private List<Cell> m_WhitePlayerOptions = new List<Cell>();
        private GameUtilities.ePlayerColor m_PlayerTurn;
        private eGameMode m_GameMode;

        public void Run()
        {
            //this method maintains the main loop of the game.
            HumanPlayer whiteHumanPlayer = new HumanPlayer(GameUtilities.ePlayerColor.WhitePlayer);
            HumanPlayer blackHumanPlayer = new HumanPlayer(GameUtilities.ePlayerColor.BlackPlayer);
            PcPlayer blackPCPlayer = new PcPlayer();
            int currentPlayerMoveRowIndex, currentPlayerMoveColumnIndex;
            bool isPlayerMoveLegal, isGameEnd = false;
            List<Cell> cellsToUpdate = new List<Cell>();
            //m_BlackPlayerOptions = new List<Cell>();
            //m_WhitePlayerOptions = new List<Cell>();
            eGameDecision rematchOrExit;

            configureGameSettings(whiteHumanPlayer, blackHumanPlayer, blackPCPlayer);
            initialize(whiteHumanPlayer, blackHumanPlayer, blackPCPlayer);
            while (true)
            {
                UI.Draw(m_GameBoard, whiteHumanPlayer, blackHumanPlayer, blackPCPlayer);
                do
                {
                    //UI.Draw(m_GameBoard, whiteHumanPlayer, blackHumanPlayer, blackPCPlayer); // DELETE

                    tellCurrentPlayerToPlay(blackHumanPlayer, whiteHumanPlayer, blackPCPlayer, m_BlackPlayerOptions,
                        out currentPlayerMoveRowIndex, out currentPlayerMoveColumnIndex);
                    if (currentPlayerMoveColumnIndex == (int)HumanPlayer.eUserRequest.Exit)
                    {
                        isGameEnd = true;
                        break;
                    }
                    //UI.Draw(m_GameBoard, whiteHumanPlayer, blackHumanPlayer, blackPCPlayer); // DELETE

                    isPlayerMoveLegal = isLegalMove(currentPlayerMoveRowIndex, currentPlayerMoveColumnIndex, ref cellsToUpdate);
                }
                while (!isPlayerMoveLegal);

                if (isGameEnd)
                {
                    //the user chose to exit the game.
                    break;
                }

                m_GameBoard.UpdateBoard(cellsToUpdate, m_PlayerTurn);
                updatePlayersOptions();
                updatePlayersScore(whiteHumanPlayer, blackHumanPlayer, blackPCPlayer);
                turnChangingManager();
                isGameEnd = isGameOver();
                //Ex02.ConsoleUtils.Screen.Clear(); // DELETE - RETURN COMMAND
                if (isGameEnd)
                {
                    determineWinner();
                    rematchOrExit = UI.AskUserForRematchOrExit();
                    if (rematchOrExit == eGameDecision.Rematch)
                    {
                        restartGame(whiteHumanPlayer, blackHumanPlayer, blackPCPlayer);
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
        public GameManager (Board i_GameBoard, GameUtilities.ePlayerColor i_PlayerTurn)
        {
            Board copiedBoard = i_GameBoard;
            m_GameBoard = copiedBoard;
            m_PlayerTurn = i_PlayerTurn;
        }
        public GameManager()
        {
        }


        private void updatePlayersScore(HumanPlayer i_WhiteHumanPlayer, HumanPlayer i_BlackHumanPlayer, PcPlayer i_BlackPCPlayer)
        {
            int updatedWhitePlayerScore, updatedBlackPlayerScore;

            updatedWhitePlayerScore = m_GameBoard.CountSignAppearances('O');
            updatedBlackPlayerScore = m_GameBoard.CountSignAppearances('X');
            i_WhiteHumanPlayer.Score = updatedWhitePlayerScore;
            if (i_BlackHumanPlayer.Active)
            {
                i_BlackHumanPlayer.Score = updatedBlackPlayerScore;
            }
            else
            {
                i_BlackPCPlayer.Score = updatedBlackPlayerScore;
            }
        }

        public void configureGameSettings(HumanPlayer i_WhiteHumanPlayer, HumanPlayer i_BlackHumanPlayer, PcPlayer i_BlackPCPlayer)
        {
            string whitePlayerName, blackPlayerName;
            GameManager.eGameMode userGameModeChoice;
            Board.eBoardSize userBoardSizeChoice;

            i_WhiteHumanPlayer.Active = true;
            whitePlayerName = UI.AskUserForUserName();
            i_WhiteHumanPlayer.Name = whitePlayerName;
            userGameModeChoice = UI.AskUserForGameMode();
            m_GameMode = userGameModeChoice;
            if (userGameModeChoice == eGameMode.HumanVsHuman)
            {
                i_BlackHumanPlayer.Active = true;
                blackPlayerName = UI.AskUserForUserName();
                i_BlackHumanPlayer.Name = blackPlayerName;
            }
            else
            {
                i_BlackPCPlayer.Active = true;
            }

            userBoardSizeChoice = UI.AskUserForBoardSize();
            m_GameBoard = new Board(userBoardSizeChoice);
        }

        private void initialize(HumanPlayer i_WhiteHumanPlayer, HumanPlayer i_BlackHumanPlayer, PcPlayer i_BlackPCPlayer)
        {
            m_GameBoard.Initialize();
            initializePlayersOptions();
            initializePlayersScores(i_WhiteHumanPlayer, i_BlackHumanPlayer, i_BlackPCPlayer);
            m_PlayerTurn = GameUtilities.ePlayerColor.WhitePlayer;
        }

        private void initializePlayersScores(HumanPlayer i_WhiteHumanPlayer, HumanPlayer i_BlackHumanPlayer, PcPlayer i_BlackPCPlayer)
        {
            i_WhiteHumanPlayer.Score = 2;
            i_BlackHumanPlayer.Score = 2;
            i_BlackPCPlayer.Score = 2;
        }

        private void turnChangingManager()
        {
            if (m_PlayerTurn == GameUtilities.ePlayerColor.BlackPlayer && WhitePlayerOptions.Count > 0)
            {
                m_PlayerTurn = GameUtilities.ePlayerColor.WhitePlayer;
            }
            else if (m_PlayerTurn == GameUtilities.ePlayerColor.WhitePlayer && BlackPlayerOptions.Count > 0)
            {
                m_PlayerTurn = GameUtilities.ePlayerColor.BlackPlayer;
            }
        }

        private void tellCurrentPlayerToPlay(HumanPlayer i_BlackHumanPlayer, HumanPlayer i_WhiteHumanPlayer, PcPlayer i_BlackPcPlayer, List<Cell> i_BlackPcPlayerOptions,
            out int io_CurrentMoveRowIndex, out int io_CurrentMoveColumnIndex)
        {
            //this method recieves the players, and the pcplayer options, checks who should play now and tell them to play.
            //the method will keep asking for legal input as long as it is not logicaly legal. 
            if (m_GameMode == eGameMode.HumanVsHuman)
            {
                if (m_PlayerTurn == GameUtilities.ePlayerColor.BlackPlayer)
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
                if (m_PlayerTurn == GameUtilities.ePlayerColor.BlackPlayer)
                {
                    i_BlackPcPlayer.Play(m_GameBoard, m_GameMode, out io_CurrentMoveRowIndex, out io_CurrentMoveColumnIndex);
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

        public void updatePlayersOptions()
        {
            List<Cell> cellList = new List<Cell>();
            GameUtilities.ePlayerColor lastPlayerTurn;
            bool isCellAnOption, shouldMethodAddCellsToUpdateList;

            // clear players options lists
            m_WhitePlayerOptions.Clear();
            m_BlackPlayerOptions.Clear();
            // saving the last player's turn
            lastPlayerTurn = m_PlayerTurn;
            //this method is updating the players options.
            shouldMethodAddCellsToUpdateList = false;
            foreach (Cell cellIteator in m_GameBoard.Matrix)
            {
                if (cellIteator.Sign == Cell.k_Empty)
                {
                    m_PlayerTurn = GameUtilities.ePlayerColor.WhitePlayer;
                    isCellAnOption = isPlayerMoveBlockingEnemy(cellIteator.Row, cellIteator.Column, ref cellList, shouldMethodAddCellsToUpdateList);
                    if (isCellAnOption)
                    {
                        m_WhitePlayerOptions.Add(cellIteator);
                    }
                    else
                    {
                        m_PlayerTurn = GameUtilities.ePlayerColor.BlackPlayer;
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

        private bool isMoveInOptionsList(Cell i_CellToSearchForInOptionsList)
        {
            bool isCellFoundInOptionsList, areCellsEqual;

            isCellFoundInOptionsList = false;
            if (m_PlayerTurn == GameUtilities.ePlayerColor.BlackPlayer)
            {
                foreach (Cell cellIteator in BlackPlayerOptions)
                {
                    areCellsEqual = areTwoCellsEquals(cellIteator, i_CellToSearchForInOptionsList);
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
                    areCellsEqual = areTwoCellsEquals(cellIteator, i_CellToSearchForInOptionsList);
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

        public bool isPlayerMoveBlockingEnemy(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref List<Cell> io_CellsToUpdate, bool i_AddCellsToList = true)
        {
            //this method recieves a player move and return true if the move is blocking the enemy.
            //its also updates the list of cells to update.
            bool isVerticalBlocking, isHorizontalBlocking, isDiagonalOneBlocking, isDiagonalTwoBlocking, isMoveBlockingEnemy;

            isVerticalBlocking = isVerticallyBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.Up, (int)eDirection.NoDirection, i_AddCellsToList);
            isVerticalBlocking = isVerticallyBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.Down, (int)eDirection.NoDirection, i_AddCellsToList) || isVerticalBlocking;
            isHorizontalBlocking = isHorizontallyBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.NoDirection, (int)eDirection.Left, i_AddCellsToList);
            isHorizontalBlocking = isHorizontallyBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.NoDirection, (int)eDirection.Right, i_AddCellsToList) || isHorizontalBlocking;
            isDiagonalOneBlocking = isDiagonallyOneBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.Up, (int)eDirection.Right, i_AddCellsToList);
            isDiagonalOneBlocking = isDiagonallyOneBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.Down, (int)eDirection.Left, i_AddCellsToList) || isDiagonalOneBlocking;
            isDiagonalTwoBlocking = isDiagonallyTwoBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.Up, (int)eDirection.Left, i_AddCellsToList);
            isDiagonalTwoBlocking = isDiagonallyTwoBlocking(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, ref io_CellsToUpdate, (int)eDirection.Down, (int)eDirection.Right, i_AddCellsToList) || isDiagonalTwoBlocking;
            isMoveBlockingEnemy = isVerticalBlocking || isHorizontalBlocking || isDiagonalOneBlocking || isDiagonalTwoBlocking;

            return isMoveBlockingEnemy;
        }

        private bool isBlockingLine(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, int i_VerticalDirection, int i_HorizontalDirection, out Cell o_CellIterator)
        {
            int currentRow, currentColumn;
            Cell cellIterator;
            bool isBlockFound, isInBoardLimits;

            currentRow = i_PlayerMoveRowIndex + i_VerticalDirection;
            currentColumn = i_PlayerMoveColumnIndex + i_HorizontalDirection;
            isInBoardLimits = GameBoard.IsCellInBoard(currentRow, currentColumn);
            isBlockFound = false;
            if (isInBoardLimits)
            {
                cellIterator = new Cell(currentRow, currentColumn, m_GameBoard.Matrix[currentRow, currentColumn].Sign);
                isBlockFound = isSeriesFound(ref cellIterator, i_VerticalDirection, i_HorizontalDirection);
            }
            else
            {
                cellIterator = null;
            }

            o_CellIterator = cellIterator;

            return isBlockFound;
        }

        private bool isDiagonallyTwoBlocking(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref List<Cell> io_CellsToUpdate, int i_VerticalDirection, int i_HorizontalDirection, bool i_AddCellsToList)
        {
            Cell cellIterator = null;
            bool isBlockFound;
            int j;

            isBlockFound = isBlockingLine(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, i_VerticalDirection, i_HorizontalDirection, out cellIterator);
            if (isBlockFound && i_AddCellsToList == true)
            {
                if (i_HorizontalDirection == (int)eDirection.Left && i_VerticalDirection == (int)eDirection.Up)
                {
                    j = i_PlayerMoveRowIndex;
                    for (int i = i_PlayerMoveColumnIndex; i > cellIterator.Column; i--)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[j, i]);
                        j += (int)eDirection.Up;
                    }
                }
                else
                {// elsewise we are going RIGHT and DOWN
                    j = i_PlayerMoveRowIndex;
                    for (int i = i_PlayerMoveColumnIndex; i < cellIterator.Column; i++)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[j, i]);
                        j += (int)eDirection.Down;
                    }
                }
            }

            return isBlockFound;
        }

        private bool isDiagonallyOneBlocking(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref List<Cell> io_CellsToUpdate, int i_VerticalDirection, int i_HorizontalDirection, bool i_AddCellsToList)
        {
            Cell cellIterator = null;
            bool isBlockFound;
            int j;

            isBlockFound = isBlockingLine(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, i_VerticalDirection, i_HorizontalDirection, out cellIterator);
            if (isBlockFound && i_AddCellsToList == true)
            {
                if (i_HorizontalDirection == (int)eDirection.Left && i_VerticalDirection == (int)eDirection.Down)
                {
                    j = i_PlayerMoveRowIndex;
                    for (int i = i_PlayerMoveColumnIndex; i > cellIterator.Column; i--)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[j,i]);
                        j += (int)eDirection.Down;
                    }
                }
                else
                {// elsewise we are going UP and RIGHT
                    j = i_PlayerMoveRowIndex;
                    for (int i = i_PlayerMoveColumnIndex; i < cellIterator.Column; i++)
                    {
                        io_CellsToUpdate.Add(m_GameBoard.Matrix[j,i]);
                        j += (int)eDirection.Up;
                    }
                }
            }

            return isBlockFound;
        }

        private bool isHorizontallyBlocking(int i_PlayerMoveRowIndex, int i_PlayerMoveColumnIndex, ref List<Cell> io_CellsToUpdate, int i_VerticalDirection, int i_HorizontalDirection, bool i_AddCellsToList)
        {
            Cell cellIterator = null;
            bool isBlockFound;

            isBlockFound = isBlockingLine(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, i_VerticalDirection, i_HorizontalDirection, out cellIterator);
            if (isBlockFound && i_AddCellsToList == true)
            {
                if (i_HorizontalDirection == (int)eDirection.Left)
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

            return isBlockFound;
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
            Cell cellIterator = null;
            bool isBlockFound;

            isBlockFound = isBlockingLine(i_PlayerMoveRowIndex, i_PlayerMoveColumnIndex, i_VerticalDirection, i_HorizontalDirection, out cellIterator);
            if (isBlockFound && i_AddCellsToList == true)
            {
                if (i_VerticalDirection == (int)eDirection.Up)
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

            return isBlockFound;
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

            if (i_PlayerColor == GameUtilities.ePlayerColor.BlackPlayer)
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

            doesBothPlayersHasNoOptions = isPlayerOptionEmpty(GameUtilities.ePlayerColor.BlackPlayer) && isPlayerOptionEmpty(GameUtilities.ePlayerColor.WhitePlayer);

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
                winner = GameUtilities.ePlayerColor.WhitePlayer;
                UI.DeclareWinner(whitePlayerScore, blackPlayerScore, winner);
            }
            else if (whitePlayerScore < blackPlayerScore)
            {
                winner = GameUtilities.ePlayerColor.BlackPlayer;
                UI.DeclareWinner(whitePlayerScore, blackPlayerScore, winner);
            }
            else
            {
                UI.DeclareDraw(whitePlayerScore, blackPlayerScore);
            }
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

        private void restartGame(HumanPlayer i_WhiteHumanPlayer, HumanPlayer i_BlackHumanPlayer, PcPlayer i_BlackPCPlayer)
        {
            //this method restarts a game.
            initialize(i_WhiteHumanPlayer, i_BlackHumanPlayer, i_BlackPCPlayer);
        }

        private void exitGame()
        {
            //this method exiting the game and calls the ui to show the exit message.
            UI.ShowExitMessage();
        }
    }
}