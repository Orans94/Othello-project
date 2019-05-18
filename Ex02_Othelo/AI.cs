using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    public class AI
    {

        public static int Minmax(Board i_GameBoardState, int i_Depth, GameUtilities.ePlayerColor i_MaximizingPlayer, ref Cell io_Cell,ref List<KeyValuePair<int, List<Cell>>> listOfKeyValue) 
        {
            GameManager gameMangaerAI = new GameManager(i_GameBoardState, i_MaximizingPlayer);
            Board copiedBoard;
            List<Cell> playerOptionList = new List<Cell>();
            List<Cell> playerMoveList = new List<Cell>();

            KeyValuePair<int, List<Cell>> pair;
            //UI.Draw(i_GameBoardState); // DELETE
 

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
                        eval = Minmax(copiedBoard, i_Depth - 1, GameUtilities.ePlayerColor.WhitePlayer,ref io_Cell, ref listOfKeyValue);
                        if (eval > maxEval)
                        {
                            playerMoveList.Add(io_Cell);
                            pair = new KeyValuePair<int, List<Cell>>(eval,playerMoveList);
                            listOfKeyValue.Add(pair);

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

                        eval = Minmax(copiedBoard, i_Depth - 1, GameUtilities.ePlayerColor.BlackPlayer, ref io_Cell, ref listOfKeyValue);
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
            difference = blackCharsInBoard - whiteCharsInBoard;

            return difference;
        }

        private static bool isGameOver(Board i_GameBoardState, GameUtilities.ePlayerColor i_MaximizingPlayer)
        {
            //this method passing all cell in the list and check if their is an option for maximizingPlayer
            GameManager gm = new GameManager(i_GameBoardState, i_MaximizingPlayer);
            List<Cell> cellLists = new List<Cell>();
            bool addToCellsList = false;
            //UI.Draw(i_GameBoardState); // DELETE
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
            List<KeyValuePair<int, List<Cell>>> listOfKeyValue = new List<KeyValuePair<int, List<Cell>>>();

            minmaxOutput = Minmax(i_GameBoard, 5, GameUtilities.ePlayerColor.BlackPlayer, ref chosenCell, ref listOfKeyValue);

            listOfKeyValue.Sort((x, y) => x.Key.CompareTo(y.Key));

            io_CurrentMoveRowIndex = chosenCell.Row;
            io_CurrentMoveColumnIndex = chosenCell.Column;

            io_CurrentMoveRowIndex = listOfKeyValue[listOfKeyValue.Count-1].Value[0].Row;
            io_CurrentMoveColumnIndex = listOfKeyValue[listOfKeyValue.Count-1].Value[0].Column;

        }
    }
}
