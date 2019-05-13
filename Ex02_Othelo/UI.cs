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

        public static void Draw(ref Board i_GameBoard)
        {
            //this method recieves a board and drawing it.
            
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

        public static void RequestPlayerToPlay(PlayerTurn i_PlayerTurn)
        {
            //this method is recieving the player that should play now and asking the player to play
        }

        public static void RequestValidMove()
        {
            //this method is informing the user that the move isn't valid and asking the user for a valid move
            Console.WriteLine("The move is not valid, please enter a valid move", Environment.NewLine);
        }

        public static void InformTurnHasBeenChanged(PlayerColor i_PlayerTurn)
        {
            //this method is informing the players that the turn has been changed.
        }

        public static void DeclareWinner(int i_WhiteScore, int i_BlackScore, PlayerColor i_WinnerColor, string i_WinnerName)
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
