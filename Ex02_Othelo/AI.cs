using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    public class AI
    {
        
        public static int Minmax(Board i_GameBoardState, int i_Depth, GameUtilities.ePlayerColor i_MaximizingPlayer, ref Cell io_Cell)
        {
            GameManager gameMangaerAI = new GameManager(i_GameBoardState, i_MaximizingPlayer);
            Board copiedBoard;
            List<Cell> playerOptionList = new List<Cell>();
            UI.Draw(i_GameBoardState); // DELETE


            int eval, minEval, maxEval;

            if (i_Depth == 0 || isGameOver(i_GameBoardState, i_MaximizingPlayer))
            {
                return heuristic(i_GameBoardState);
            }
            else
            { 
                gameMangaerAI.updatePlayersOptions();
                if (i_MaximizingPlayer == GameUtilities.ePlayerColor.BlackPlayer) // this is PC's turn - Choose max value
                {
                    //playerOptionList = gameMangaerAI.BlackPlayerOptions;
                   //playerOptionList = gameMangaerAI.BlackPlayerOptions.ConvertAll(cell => new Cell(cell.Row, cell.Column));
                    maxEval = int.MinValue;
                    foreach (Cell cellIteator in gameMangaerAI.BlackPlayerOptions)
                    {
                        copiedBoard = gameMangaerAI.GameBoard.Clone();
                        //UI.Draw(copiedBoard); // DELETE

                        gameMangaerAI.isPlayerMoveBlockingEnemy(cellIteator.Row, cellIteator.Column, ref playerOptionList);
                        copiedBoard.UpdateBoard(playerOptionList, i_MaximizingPlayer);
                        //UI.Draw(copiedBoard); // DELETE
                        eval = Minmax(copiedBoard, i_Depth - 1, GameUtilities.ePlayerColor.WhitePlayer,ref io_Cell);
                        if (eval > maxEval)
                        {
                            io_Cell.Row = cellIteator.Row;
                            io_Cell.Column= cellIteator.Column;
                            maxEval = eval;
                        }
                    }
                    return maxEval;
                }
                else // this is Human player - Choose min value
                {
                    //playerOptionList = gameMangaerAI.WhitePlayerOptions;
                    //playerOptionList = gameMangaerAI.WhitePlayerOptions.ConvertAll(cell => new Cell(cell.Row, cell.Column));

                    minEval = int.MaxValue;
                    foreach (Cell cellIteator in gameMangaerAI.WhitePlayerOptions)
                    {
                        copiedBoard = gameMangaerAI.GameBoard.Clone();
                        //UI.Draw(copiedBoard); // DELETE

                        gameMangaerAI.isPlayerMoveBlockingEnemy(cellIteator.Row, cellIteator.Column, ref playerOptionList);
                        copiedBoard.UpdateBoard(playerOptionList, i_MaximizingPlayer);
                        //UI.Draw(copiedBoard); // DELETE

                        eval = Minmax(copiedBoard, i_Depth - 1, GameUtilities.ePlayerColor.BlackPlayer, ref io_Cell);
                        if (eval < minEval)
                        {
                           // io_Cell.Row = cellIteator.Row;
                            //io_Cell.Column = cellIteator.Column;
                            minEval = eval;
                        }
                    }
                    return minEval;
                }
            }
        }

        private static int heuristic(Board i_GameBoardState)
        {
            int whiteCharsInBoard, blackCharsInBoard, difference;

            whiteCharsInBoard = i_GameBoardState.CountSignAppearances('O');
            blackCharsInBoard = i_GameBoardState.CountSignAppearances('X');
            difference = whiteCharsInBoard - blackCharsInBoard;

            return difference;
        }

        private static bool isGameOver(Board i_GameBoardState, GameUtilities.ePlayerColor i_MaximizingPlayer)
        {
            //this method passing all cell in the list and check if their is an option for maximizingPlayer
            GameManager gm = new GameManager(i_GameBoardState, i_MaximizingPlayer);
            List<Cell> cellLists = new List<Cell>();
            bool addToCellsList = false;
            UI.Draw(i_GameBoardState); // DELETE
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
            int minmaxOutput;
            Cell chosenCell = new Cell();

            minmaxOutput = Minmax(i_GameBoard, 3, GameUtilities.ePlayerColor.BlackPlayer, ref chosenCell);

            io_CurrentMoveRowIndex = chosenCell.Row;
            io_CurrentMoveColumnIndex = chosenCell.Column;

        }
    }
}
