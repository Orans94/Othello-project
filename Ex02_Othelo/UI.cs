using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    //TODO: after every time i recieve input from user, check if its valid. if not, ask again(also use TryParse().
    //When you make a class(not a struct) an object of that class is actually a ref type(like a pointer).
    public class UI
    {
        public static void Clear()
        {
            //this method clears the screen using the dll referenced.
            Ex02.ConsoleUtils.Screen.Clear();
        }

        public static void Draw(Board i_GameBoard,HumanPlayer i_WhiteHumanPlayer,HumanPlayer i_BlackHumanPlayer, PcPlayer i_BlackPCPlayer)
        {
            //this method recieves a board and drawing it.
            StringBuilder stringBuilder = new StringBuilder("", 36);
            printPlayersScore(i_WhiteHumanPlayer, i_BlackHumanPlayer, i_BlackPCPlayer);
            printFirstLine(i_GameBoard.Size);
            printLineOfEqualSign(i_GameBoard.Size);
            for (int i = 0; i < (int)i_GameBoard.Size; i++)
            {
                printBoardRowData(i_GameBoard, i);
                printLineOfEqualSign(i_GameBoard.Size);
            }
        }
        public static void Draw(Board i_GameBoard) // DELETE
        {
            //this method recieves a board and drawing it.
            StringBuilder stringBuilder = new StringBuilder("", 36);
            printFirstLine(i_GameBoard.Size);
            printLineOfEqualSign(i_GameBoard.Size);
            for (int i = 0; i < (int)i_GameBoard.Size; i++)
            {
                printBoardRowData(i_GameBoard, i);
                printLineOfEqualSign(i_GameBoard.Size);
            }
        }

        private static void printPlayersScore(HumanPlayer i_WhiteHumanPlayer, HumanPlayer i_BlackHumanPlayer, PcPlayer i_BlackPCPlayer)
        {
            string whitePlayerName, blackPlayerName;
            int whitePlayerScore, blackPlayerScore;

            whitePlayerName = i_WhiteHumanPlayer.Name;
            blackPlayerName = i_BlackHumanPlayer.Active == true ? i_BlackHumanPlayer.Name : i_BlackPCPlayer.Name;
            whitePlayerScore = i_WhiteHumanPlayer.Score;
            blackPlayerScore = i_BlackHumanPlayer.Active == true ? i_BlackHumanPlayer.Score : i_BlackPCPlayer.Score;

            String scoreMessage = string.Format("{0} : {1}              {2} : {3}",
                whitePlayerName, whitePlayerScore, blackPlayerName, blackPlayerScore);
            Console.WriteLine(scoreMessage);
        }

        public static void PCIsThinkingMessage()
        {
            // this method print to console the PC is thinking right now
            string message = "PC is thinking";
            string dot = ".";
            Console.Write(message);
            System.Threading.Thread.Sleep(700);
            Console.Write(dot);
            System.Threading.Thread.Sleep(700);
            Console.Write(dot);
            System.Threading.Thread.Sleep(700);
            Console.Write(dot);
            System.Threading.Thread.Sleep(700);
        }

        private static void printBoardRowData(Board i_GameBoard, int i_RowIndex)
        {
            //this method is recieving a game board and row index, and prints the board data of that row
            StringBuilder lineOfMatrixData = new StringBuilder("", 36);
            if (i_GameBoard.Size == Board.eBoardSize.bigBoard)
            {
                lineOfMatrixData.AppendFormat("{0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} | {8} |", i_RowIndex + 1, i_GameBoard.Matrix[i_RowIndex, 0].Sign,
                     i_GameBoard.Matrix[i_RowIndex, 1].Sign, i_GameBoard.Matrix[i_RowIndex, 2].Sign, i_GameBoard.Matrix[i_RowIndex, 3].Sign, i_GameBoard.Matrix[i_RowIndex, 4].Sign,
                     i_GameBoard.Matrix[i_RowIndex, 5].Sign, i_GameBoard.Matrix[i_RowIndex, 6].Sign, i_GameBoard.Matrix[i_RowIndex, 7].Sign);
            }
            else
            {
                lineOfMatrixData.AppendFormat("{0} | {1} | {2} | {3} | {4} | {5} | {6} |", i_RowIndex + 1, i_GameBoard.Matrix[i_RowIndex, 0].Sign,
                     i_GameBoard.Matrix[i_RowIndex, 1].Sign, i_GameBoard.Matrix[i_RowIndex, 2].Sign, i_GameBoard.Matrix[i_RowIndex, 3].Sign, i_GameBoard.Matrix[i_RowIndex, 4].Sign,
                     i_GameBoard.Matrix[i_RowIndex, 5].Sign);
            }

            Console.WriteLine(lineOfMatrixData);
        }

        private static void printFirstLine(Board.eBoardSize boardSize)
        {
            //this method prints the first line
            if (boardSize == Board.eBoardSize.bigBoard)
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
            //this method prints a line of equal signs according to the board size
            if (boardSize == Board.eBoardSize.bigBoard)
            {
                Console.WriteLine("  =================================");
            }
            else
            {
                Console.WriteLine("  ======================== ");
            }
        }

        public static string AskUserForUserName()
        {
            //this method is asking the user to enter his name and return the answer as a string.
            string userName;

            Console.WriteLine("Please enter user name");
            userName = Console.ReadLine();
            Ex02.ConsoleUtils.Screen.Clear();

            return userName;
        }

        public static GameManager.eGameMode AskUserForGameMode()
        {
            //TODO: convert string to GameMode check input and demand legal input.
            //this method is printing a menu for the user to choose the game mode and return the answer as a string.
            string userGameModeChoiceString;
            GameManager.eGameMode gameMode;
            bool isChoiceValid;
            int userGameModeChoiceInt;

            Console.WriteLine("Please choose game mode:{0}1.Human VS Human{0}2.Human VS PC", Environment.NewLine);
            userGameModeChoiceString = Console.ReadLine();
            Ex02.ConsoleUtils.Screen.Clear();
            isChoiceValid = isUserChoiceValid(userGameModeChoiceString);
            while (!isChoiceValid)
            {
                Console.WriteLine("Invalid input, Please choose game mode:{0}1.Human VS Human{0}2.Human VS PC", Environment.NewLine);
                userGameModeChoiceString = Console.ReadLine();
                isChoiceValid = isUserChoiceValid(userGameModeChoiceString);
                Ex02.ConsoleUtils.Screen.Clear();
            }

            userGameModeChoiceInt = userGameModeChoiceString[0] - '0';
            gameMode = (GameManager.eGameMode)userGameModeChoiceInt;

            return gameMode;
        }

        private static bool isUserChoiceValid(string userChoiceString)
        {
            bool isValidLength, isValidChar, result;

            isValidLength = userChoiceString.Length == 1;
            isValidChar = userChoiceString[0] == '1' || userChoiceString[0] == '2';
            result = isValidLength && isValidChar;

            return result;
        }

        public static Board.eBoardSize AskUserForBoardSize()
        {
            //this method is printing a menu for the user to choose the board size and return the answer as a string.
            string userBoardSizeChoiceString;
            Board.eBoardSize boardSize;
            bool isChoiceValid;
            int userBoardSizeChoiceInt;

            Console.WriteLine("Please choose board size:{0}1.6x6{0}2.8x8", Environment.NewLine);
            userBoardSizeChoiceString = Console.ReadLine();
            Ex02.ConsoleUtils.Screen.Clear();
            isChoiceValid = isUserChoiceValid(userBoardSizeChoiceString);
            while (!isChoiceValid)
            {
                Console.WriteLine("Invalid input, Please choose board size:{0}1.6x6{0}2.8x8", Environment.NewLine);
                userBoardSizeChoiceString = Console.ReadLine();
                isChoiceValid = isUserChoiceValid(userBoardSizeChoiceString);
                Ex02.ConsoleUtils.Screen.Clear();
            }

            userBoardSizeChoiceInt = userBoardSizeChoiceString[0] - '0';
            if (userBoardSizeChoiceInt == 2)
            {
                boardSize = Board.eBoardSize.bigBoard;
            }
            else
            {
                boardSize = Board.eBoardSize.smallBoard;
            }

            return boardSize;
        }

        public static string RequestPlayerToPlay(string i_PlayerName, GameUtilities.ePlayerColor i_PlayerTurn, Board.eBoardSize i_CurrentBoardSize)
        {
            //this method is recieving the player that should play now and asking the player to play
            bool isMoveValidate;
            string playerMoveInput, currentPlayerName, currentPlayerColor, currentPlayerSign;

            currentPlayerName = i_PlayerName;
            if (i_PlayerTurn == GameUtilities.ePlayerColor.BlackPlayer)
            {
                currentPlayerColor = "Black";
                currentPlayerSign = "X";
            }
            else
            {
                currentPlayerColor = "White";
                currentPlayerSign = "O";
            }

            Console.WriteLine(string.Format("{0} player {1}, please play your turn => {2}.", currentPlayerColor,currentPlayerName,currentPlayerSign));
            playerMoveInput = Console.ReadLine();
            playerMoveInput = playerMoveInput.ToUpper();
            isMoveValidate = isPlayerStringValid(playerMoveInput, i_CurrentBoardSize);
            while (!isMoveValidate)
            {
                SyntaxIsntValid();
                playerMoveInput = Console.ReadLine();
                playerMoveInput = playerMoveInput.ToUpper();
                isMoveValidate = isPlayerStringValid(playerMoveInput, i_CurrentBoardSize);
            }

            return playerMoveInput;
        }

        private static bool isPlayerStringValid(string i_PlayerMoveInput, Board.eBoardSize i_CurrentBoardSize)
        {
            bool isFirstCharValid, isSecondCharValid, isValidLength, result;

            if (i_PlayerMoveInput == "Q")
            {
                result = true;
            }
            else
            {
                isValidLength = i_PlayerMoveInput.Length == 2;
                if (isValidLength)
                {
                    isFirstCharValid = isFirstCharIsAValidLetter(i_PlayerMoveInput[0], i_CurrentBoardSize); // first char need to be letter and in board range
                    isSecondCharValid = isSecondCharIsAValidNumber(i_PlayerMoveInput[1], i_CurrentBoardSize); // second char need to be number and in board range
                    result = isFirstCharValid && isSecondCharValid;
                }
                else
                {
                    result = false;

                }
            }

            return result;
        }

        private static bool isSecondCharIsAValidNumber(char i_CharToValidate, Board.eBoardSize i_CurrentBoardSize)
        {
            bool result;

            if (i_CurrentBoardSize == Board.eBoardSize.bigBoard)
            {
                result = i_CharToValidate >= '1' && i_CharToValidate <= '8';
            }
            else
            {
                result = i_CharToValidate >= '1' && i_CharToValidate <= '6';
            }

            return result;
        }

        private static bool isFirstCharIsAValidLetter(char i_CharToValidate, Board.eBoardSize i_CurrentBoardSize)
        {
            bool result;

            if (i_CurrentBoardSize == Board.eBoardSize.bigBoard)
            {
                result = i_CharToValidate >= 'A' && i_CharToValidate <= 'H';
            }
            else
            {
                result = i_CharToValidate >= 'A' && i_CharToValidate <= 'F';
            }

            return result;
        }

        public static void SyntaxIsntValid()
        {
            //this method is informing the user that the move isn't valid and asking the user for a valid move
            Console.WriteLine("The move is not syntax valid, please enter a valid move");
        }

        public static void InformTurnHasBeenChanged(GameUtilities.ePlayerColor i_PlayerTurn)
        {
            //this method is informing the players that the turn has been changed.
        }

        public static void DeclareWinner(int i_WhitePlayerScore, int i_BlackPlayerScore, GameUtilities.ePlayerColor i_WinnerColor)
        {
            //this method is printing a game over message, which contains the scores of both of the players and the winner name and color.
            StringBuilder winnerDeclerationMessage = new StringBuilder("", 60);
            string winnerColor;

            if(i_WinnerColor == GameUtilities.ePlayerColor.BlackPlayer)
            {
                winnerColor = "Black player";
            }
            else
            {
                winnerColor = "White player";
            }

            winnerDeclerationMessage.AppendFormat("White player score: {1}{0}Black player score: {2}{0}The winner is: {3}!",
                Environment.NewLine, i_WhitePlayerScore, i_BlackPlayerScore, winnerColor);
            Console.WriteLine(winnerDeclerationMessage);
        }

        public static GameManager.eGameDecision AskUserForRematchOrExit()
        {
            //this method is asking user whether he wants a rematch or to exit and return the answer as a string.
            string userRematchOrExitChoiceString;
            GameManager.eGameDecision rematchOrExit;
            bool isChoiceValid;
            int userRematchOrExitChoiceInt;

            Console.WriteLine("Would you like to play another game or to exit?{0}1.Rematch{0}2.Exit{0}", Environment.NewLine);
            userRematchOrExitChoiceString = Console.ReadLine();
            Ex02.ConsoleUtils.Screen.Clear();
            isChoiceValid = isUserChoiceValid(userRematchOrExitChoiceString);
            while (!isChoiceValid)
            {
                Console.WriteLine("Invalid input, Would you like to play another game or to exit?{0}1.Rematch{0}2.Exit{0}", Environment.NewLine);
                userRematchOrExitChoiceString = Console.ReadLine();
                isChoiceValid = isUserChoiceValid(userRematchOrExitChoiceString);
            }

            userRematchOrExitChoiceInt = userRematchOrExitChoiceString[0] - '0';
            rematchOrExit = (GameManager.eGameDecision)userRematchOrExitChoiceInt;

            return rematchOrExit;
        }

        public static void ShowExitMessage()
        {
            //this method is printing an exit message
            Console.WriteLine("Thank you for playing Othello!");
        }

        public static void DeclareDraw(int i_WhitePlayerScore, int i_BlackPlayerScore)
        {
            // this method declare on draw score situation
            string scoreAndDeclareDrawMessage = string.Format("White player score: {1}{0}Black player score: {2}{0} Their is a DRAW!",
                Environment.NewLine, i_WhitePlayerScore, i_BlackPlayerScore);
            Console.WriteLine(scoreAndDeclareDrawMessage);
        }
    }
}
