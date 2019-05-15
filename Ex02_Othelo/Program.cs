using System;
using System.Collections.Generic;
using System.Text;

// The black player marked as 'X'
// The white player marked as 'O'
// When the game mode is human vs pc than the black player will be the PC and the white player will be the human.

//IMPROVEMENTS:
//1. make rowindex and columnindex as one package


namespace Ex02_Othelo
{
    class Program
    {
        public static void Main(string[] args)
        {
            GameManager othelo = new GameManager();
            othelo.Run();

            Console.ReadLine();
        }
    }
}
