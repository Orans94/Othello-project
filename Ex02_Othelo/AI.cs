using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    public class AI
    {

        public static int Minimax(Board i_GameBoardState, int i_Depth, GameUtilities.ePlayerColor i_MaximizingPlayer, ref Cell io_Cell,ref List<KeyValuePair<int, List<Cell>>> i_ListOfKeyValue) 
        {
            // this method return a List of pairs < heuristic score , list of cells that lead to this score >
            // using Minimax algorithm, it return that list of pair I described by ref 

            GameManager gameMangaerAI = new GameManager(i_GameBoardState, i_MaximizingPlayer);
            List<Cell> playerOptionList = new List<Cell>();
            List<Cell> playerMovesList = new List<Cell>();
            KeyValuePair<int, List<Cell>> scoreAndCellsListPair;
            Board copiedBoard;
            int eval, minEval, maxEval;

            if (i_Depth == 0 || isGameOver(i_GameBoardState, i_MaximizingPlayer))
            {
                return heuristic(i_GameBoardState, i_MaximizingPlayer);
            }
            else
            {
                gameMangaerAI.updatePlayersOptions();
                if (i_MaximizingPlayer == GameUtilities.ePlayerColor.BlackPlayer) // this is PC's turn - Choose max value
                {
                    maxEval = int.MinValue;
                    foreach (Cell cellIteator in gameMangaerAI.BlackPlayerOptions)
                    {
                        copiedBoard = gameMangaerAI.GameBoard.Clone();
                        gameMangaerAI.isPlayerMoveBlockingEnemy(cellIteator.Row, cellIteator.Column, ref playerOptionList);
                        copiedBoard.UpdateBoard(playerOptionList, i_MaximizingPlayer);
                        eval = Minimax(copiedBoard, i_Depth - 1, GameUtilities.ePlayerColor.WhitePlayer,ref io_Cell, ref i_ListOfKeyValue);
                        if (eval > maxEval)
                        {
                            playerMovesList.Add(io_Cell);
                            scoreAndCellsListPair = new KeyValuePair<int, List<Cell>>(eval,playerMovesList);
                            i_ListOfKeyValue.Add(scoreAndCellsListPair);
                            io_Cell.Row = cellIteator.Row;
                            io_Cell.Column= cellIteator.Column;                           
                            maxEval = eval;
                        }
                    }
                    return maxEval;
                }
                else // this is Human player - Choose min value
                {
                    minEval = int.MaxValue;
                    foreach (Cell cellIteator in gameMangaerAI.WhitePlayerOptions)
                    {
                        copiedBoard = gameMangaerAI.GameBoard.Clone();
                        gameMangaerAI.isPlayerMoveBlockingEnemy(cellIteator.Row, cellIteator.Column, ref playerOptionList);
                        copiedBoard.UpdateBoard(playerOptionList, i_MaximizingPlayer);
                        eval = Minimax(copiedBoard, i_Depth - 1, GameUtilities.ePlayerColor.BlackPlayer, ref io_Cell, ref i_ListOfKeyValue);
                        if (eval < minEval)
                        {
                            playerMovesList.Add(io_Cell);
                            scoreAndCellsListPair = new KeyValuePair<int, List<Cell>>(eval, playerMovesList);
                            i_ListOfKeyValue.Add(scoreAndCellsListPair);
                            minEval = eval;
                        }
                    }
                    return minEval;
                }
            }
        }

        private static int differencePCScoreHumanScore(Board i_GameBoardState)
        {
            // this method calculate the difference between the PC player score and the human score and return it
            int whiteCharsInBoard, blackCharsInBoard, difference;

            whiteCharsInBoard = i_GameBoardState.CountSignAppearances('O');
            blackCharsInBoard = i_GameBoardState.CountSignAppearances('X');
            difference = blackCharsInBoard - whiteCharsInBoard;

            return difference;
        }

        private static bool isGameOver(Board i_GameBoardState, GameUtilities.ePlayerColor i_MaximizingPlayer)
        {
            //this method passing all cell in the list and check if their is an option for maximizingPlayer
            GameManager gm = new GameManager(i_GameBoardState, i_MaximizingPlayer);
            List<Cell> cellLists = new List<Cell>();
            bool addToCellsList = false;
            foreach (Cell cellIteator in i_GameBoardState.Matrix)
            {
                if (gm.isPlayerMoveBlockingEnemy(cellIteator.Row, cellIteator.Column, ref cellLists, addToCellsList))
                {
                    return false;
                }
            }
            return true;
        }

        internal static void PCPlay(Board i_GameBoard, out int io_CurrentMoveRowIndex, out int io_CurrentMoveColumnIndex)
        {
            // this method choosing appropriate move using Minimax algorithm. 
            int minmaxOutput;
            Cell chosenCell = new Cell();
            List<KeyValuePair<int, List<Cell>>> listOfScoreAndMoveList = new List<KeyValuePair<int, List<Cell>>>();

            minmaxOutput = Minimax(i_GameBoard, 2, GameUtilities.ePlayerColor.BlackPlayer, ref chosenCell, ref listOfScoreAndMoveList);

            // sorting the list of heuristic scores and moves that lead them,
            // sorting this list by their score.
            listOfScoreAndMoveList.Sort((x, y) => x.Key.CompareTo(y.Key));

            // choose the best score from the listOfScoreAndMoveList (the best located in the last index) and pick up the first Cell => will be PC chose
            // that lead to this best score.
            io_CurrentMoveRowIndex = listOfScoreAndMoveList[listOfScoreAndMoveList.Count-1].Value[0].Row;
            io_CurrentMoveColumnIndex = listOfScoreAndMoveList[listOfScoreAndMoveList.Count-1].Value[0].Column;

        }
        private static int getCornersHeuristic(Board i_Board, char i_Sign)
        {
            int result, edgesOfBoard;

            edgesOfBoard = (int)i_Board.Size - 1;
            result = 0;
            Cell[] boardCorners =
                {
                i_Board.Matrix[0, 0],
                i_Board.Matrix[0, edgesOfBoard],
                i_Board.Matrix[edgesOfBoard, 0],
                i_Board.Matrix[edgesOfBoard, edgesOfBoard],
            };

            foreach (Cell corner in boardCorners)
            {
                if (corner.Sign == i_Sign)
                {
                    result += 20;
                }
            }

            return result;
        }

        private static int heuristic( Board i_Board, GameUtilities.ePlayerColor i_playerTurn)
        {
            // heuristic method for Minimax algorithm
            int heuristicResult;
            int differencePCHuman;
            char playerTurnSign;

            heuristicResult = 0;
            differencePCHuman = differencePCScoreHumanScore(i_Board);
            playerTurnSign = i_playerTurn == GameUtilities.ePlayerColor.BlackPlayer ? 'X' : 'O';
            heuristicResult += getCornersHeuristic(i_Board, playerTurnSign);
            return heuristicResult + differencePCHuman;
        }
    }
}
