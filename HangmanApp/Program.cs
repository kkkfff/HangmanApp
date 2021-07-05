using System;

namespace HangmanApp
{
    class Program
    {
        static void Main(string[] args)
        {
            /*C# Console Application - The Hangman Game was coded by Krzysztof Fudro
            between 03.07.2021 - 05.07.2021 as Motorola Solutions Academy's Recruitment Task*/

            char newGameOpt = 'Y';
            while (newGameOpt == 'Y')
            {
                Hangman game = new Hangman();
                game.Run();
                Console.Clear();
                Console.WriteLine("\nWould You like to play again? (Y/N)");
                newGameOpt = char.ToUpper(Console.ReadKey().KeyChar);
            }
        }
    }
}
