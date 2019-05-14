using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{

    //TODO: after every time i recieve input from user, check if its valid. if not, ask again(also use TryParse().
    //When you make a class(not a struct) an object of that class is actually a ref type(like a pointer).
    class UI
    {

        public static void Clear()
        {
            //TODO:reference the dll
            //this method clears the screen using the dll referenced.
            Ex02.ConsoleUtils.Screen.Clear();
        }

        public static void Draw(ref Board i_GameBoard,Board.eBoardSize i_BoardSize)
        {
            //this method recieves a board and drawing it.
            StringBuilder stringBuilder = new StringBuilder("", 36);

            printFirstLine(i_BoardSize);
            printLineOfEqualSign(i_BoardSize);
            for(int i=1; i<(int)i_BoardSize; i++)
            {
                printBoardRowData(i_GameBoard, i);
            }
        }

        private static void printBoardRowData(Board i_GameBoard, int i_RowIndex)
        {
            StringBuilder sb = new StringBuilder("", 36);
            if(i_GameBoard.Size == Board.eBoardSize.bigBoard)
            {
                sb.AppendFormat("{0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} | {8} |", i_RowIndex, i_GameBoard.Matrix[i_RowIndex, 1],
                     i_GameBoard.Matrix[i_RowIndex, 2], i_GameBoard.Matrix[i_RowIndex, 3], i_GameBoard.Matrix[i_RowIndex, 4], i_GameBoard.Matrix[i_RowIndex, 5],
                     i_GameBoard.Matrix[i_RowIndex, 6], i_GameBoard.Matrix[i_RowIndex, 7], i_GameBoard.Matrix[i_RowIndex, 8]);
            }
            else
            {
                sb.AppendFormat("{0} | {1} | {2} | {3} | {4} | {5} | {6} |", i_RowIndex);
            }
        }
        private static void printFirstLine(Board.eBoardSize boardSize)
        {
            if(boardSize == Board.eBoardSize.bigBoard)
            {
                Console.WriteLine("    A   B   C   D   E   F   G   H  ");
            }
            else
            {
                Console.WriteLine("    A   B   C   D   E   F  ");
            }
        }
        private static void printLineOfEqualSign(Board.eBoardSize boardSize)
        {
            if(boardSize == Board.eBoardSize.bigBoard)
            {
                Console.WriteLine("=================================");
            }
            else
            {
                Console.WriteLine("======================== ");
            }
        }

        public static string AskForUserName()
        {
            //this method is asking the user to enter his name and return the answer as a string.
            string userName;

            Console.WriteLine("Please enter user name");
            userName = Console.ReadLine();

            return userName;
        }

        public static string ShowGameModeMenu()
        {
            //this method is printing a menu for the user to choose the game mode and return the answer as a string.
            string userGameModeChoice;

            Console.WriteLine("Please choose game mode:{0}1.Human VS Human{0}2.Human VS Pc", Environment.NewLine);
            userGameModeChoice = Console.ReadLine();

            return userGameModeChoice;
        }

        public static string ShowBoardSizeMenu()
        {
            //this method is printing a menu for the user to choose the board size and return the answer as a string.
            string userBoardSizeChoice;

            Console.WriteLine("Please choose board size:{0}1.6x6{0}2.8x8", Environment.NewLine);
            userBoardSizeChoice = Console.ReadLine();

            return userBoardSizeChoice;
        }

        public static string RequestPlayerToPlay(GameUtilities.PlayerColor i_PlayerTurn)
        {
            //this method is recieving the player that should play now and asking the player to play
            bool isMoveValidate;
            string playerMoveInput, currentPlayer;
            

            if (i_PlayerTurn == GameUtilities.PlayerColor.BLACK_PLAYER)
            {
                currentPlayer = "Black player";
            }
            else
            {
                currentPlayer = "White player";
            }


            Console.WriteLine(string.Format("{0}, please play your turn.", currentPlayer));
            playerMoveInput = Console.ReadLine();
            
            

        }

        public static void RequestValidMove()
        {
            //this method is informing the user that the move isn't valid and asking the user for a valid move
            Console.WriteLine("The move is not valid, please enter a valid move");
        }

        public static void InformTurnHasBeenChanged(GameUtilities.PlayerColor i_PlayerTurn)
        {
            //this method is informing the players that the turn has been changed.
        }

        public static void DeclareWinner(int i_WhiteScore, int i_BlackScore, GameUtilities.PlayerColor i_WinnerColor)
        {
            //this method is printing a game over message, which contains the scores of both of the players and the winner name and color.
        }

        public static string AskForRematchOrExit()
        {
            //this method is asking user whether he wants a rematch or to exit and return the answer as a string.
            string userRematchOrExitChoice;

            Console.WriteLine("Would you like to play another game or to exit?{0}1.Rematch{0}2.Exit{0}", Environment.NewLine);
            userRematchOrExitChoice = Console.ReadLine();

            return userRematchOrExitChoice;
        }

        public static void ShowExitMessage()
        {
            //this method is printing an exit message
            Console.WriteLine("Thank you for playing Othello{0}", Environment.NewLine);
        }

    }
}
