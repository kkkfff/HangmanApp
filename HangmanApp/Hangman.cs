using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace HangmanApp
{
    class Hangman
    {
        string capital;
        string country;
        int capitalLength;
        string uniqueCharacterCapital;
        int uniqueCharacterCapitalLength;
        int attemptNr = 0;
        int lifePoints = 5;
        string pathCC = @"..\\countries_and_capitals.txt";
        string pathHS = @"..\\highscores.txt";
        List<char> inWordList = new List<char>();
        List<char> notInWordList = new List<char>();
        Stopwatch Timer = new Stopwatch();
        string[] hangPicture = {    "  ____  \n |/  |  \n |  (-) \n |  /|\\ \n |   |  \n |  / \\ \n_|_    ",
                                    "  ____  \n |/  |  \n |  (-) \n |  /|\\ \n |      \n |      \n_|_    ",
                                    "  ____  \n |/  |  \n |  (-) \n |      \n |      \n |      \n_|_    ",
                                    "  ____  \n |/  |  \n |      \n |      \n |      \n |      \n_|_    ",
                                    "  ___  \n |/     \n |      \n |      \n |      \n |      \n_|_    ",
                                    "       \n |      \n |      \n |      \n |      \n |      \n_|_    "};

        public void Run()
        {
            this.ProgramInitialization();
            this.PrepareWord();
            while (this.lifePoints > 0 && (this.inWordList.Count != this.uniqueCharacterCapitalLength))
            {
                Console.Clear();
                this.PrintStatus();
                char chooseOption = '\0';
                while (chooseOption != '1' && chooseOption != '2')
                {
                    Console.Clear();
                    this.PrintStatus();
                    if (this.lifePoints == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"\n\n Hint: The Capital of {this.country}");
                        Console.ResetColor();
                    }
                    Console.WriteLine("\n\n1. Whole word");
                    Console.WriteLine("2. Single letter");
                    chooseOption = Console.ReadKey().KeyChar;
                }
                this.attemptNr++;
                if (chooseOption == '1')
                {
                    CheckWholeWordGuess();
                }
                else
                {
                    CheckSingleLetterGuess();
                }
            }
            Console.Clear();
            if (this.inWordList.Count == this.uniqueCharacterCapitalLength)
            {
                this.Timer.Stop();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\nWell done! You did it in ");
                TimeSpan ts = this.Timer.Elapsed;
                Console.Write(ts.ToString("mm\\:ss"));
                Console.Write(" min. (");
                Console.Write(attemptNr);
                if (attemptNr == 1) { Console.WriteLine(" attempt)"); } else { Console.WriteLine(" attempts)"); }
                Console.ResetColor();
                Console.Write(" Please write your name: ");
                string userName = Console.ReadLine();

                using (StreamWriter sw = File.AppendText(this.pathHS))
                {
                    sw.WriteLine($"{userName} | {DateTime.Now} | {ts.ToString("mm\\:ss")} | {this.attemptNr} | {this.capital}");
                }
                this.PrintScore();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(this.hangPicture[this.lifePoints]);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n..:: G A M E   O V E R ::..");
                Console.WriteLine($"{this.capital} was the right answer.");
                Console.ResetColor();
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
                this.PrintScore();
            }
        }

        void ProgramInitialization()
        {
            if (!File.Exists(this.pathHS))
            {
                using (StreamWriter sw = File.CreateText(this.pathHS)) ;
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(this.hangPicture[0]);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Welcome to HANGMAN game.");
            Console.ResetColor();
            Console.WriteLine("\nThe rules are very simple:\n1. You have to guess the Capital of randomly chose Country of the World.");
            Console.WriteLine("2. You can choose the way you gonna win. Guess full Capital name or do it step-by-step, letter-by-letter.");
            Console.WriteLine($"3. You have {this.lifePoints} Life Points at the beginning of the game.");
            Console.WriteLine("4. If You guess wrong letter you lose 1 Life Point. If you write wrong Capital you lose 2 Life Points.");
            Console.WriteLine("5. The Game is over when you lose all Life Points or when you find the right Capital name - then you are the winner!");
            Console.WriteLine("\nAre you ready to START the Hangman game? Press any key...");
            Console.ReadKey();
            this.Timer.Start();
        }

        void PrepareWord()
        {
            string[] allLines = File.ReadAllLines(this.pathCC);
            Random rnd = new Random();
            string line = allLines[rnd.Next(0, allLines.Length - 1)];
            string[] stringSeparators = new string[] { " | " };
            string[] lineParts = line.Split(stringSeparators, StringSplitOptions.None);
            this.country = lineParts[0];
            this.capital = lineParts[1];

            this.capitalLength = this.capital.Length;

            this.uniqueCharacterCapital = new String(capital.ToUpper().Distinct().ToArray());
            this.uniqueCharacterCapitalLength = uniqueCharacterCapital.Length;

            if (this.capital.Contains(' '))
            { inWordList.Add(' '); }
        }

        void CheckWholeWordGuess()
        {
            Console.Write("\nGuess the Capital: ");
            string guessWord = Console.ReadLine();
            Console.Clear();
            if (guessWord.ToUpper() == this.capital.ToUpper())
            {
                this.inWordList = this.uniqueCharacterCapital.ToList();
            }
            else
            {
                if (this.lifePoints == 1)
                {
                    Console.Write("\nWrong Capital. You lose your last life point. Press any key to continue...");
                    this.lifePoints = this.lifePoints - 1;
                }
                else
                {
                    Console.Write("\nWrong Capital. You lose 2 life points. Press any key to continue...");
                    this.lifePoints = this.lifePoints - 2;
                }
                Console.ReadKey();
            }
        }

        void CheckSingleLetterGuess()
        {
            Console.Write("\nGuess a letter: ");
            char guessLetter = char.ToUpper(Console.ReadKey().KeyChar); //TODO: validate special characters and numbers
            Console.Clear();
            if ((this.uniqueCharacterCapital.Contains(guessLetter)))
            {
                if (!this.inWordList.Contains(guessLetter))
                {
                    Console.WriteLine("\n You have guessed a letter! Press any key to continue...");
                    inWordList.Add(guessLetter);
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Write("\nThere is no \"");
                Console.Write(guessLetter);
                Console.Write("\" in the word. You lose 1 life point. Press any key to continue...");
                this.lifePoints--;
                notInWordList.Add(guessLetter);
                Console.ReadKey();
            }
        }

        void PrintGuessingWord()
        {
            Console.WriteLine("");
            for (int i = 0; i < this.capitalLength; i++)
            {
                if (inWordList.Contains(char.ToUpper(this.capital[i])))
                {
                    Console.Write(" ");
                    Console.Write(char.ToUpper(this.capital[i]));
                }
                else
                {
                    Console.Write(" _");
                }
            }
        }

        void PrintStatus()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(this.hangPicture[this.lifePoints]);
            Console.ResetColor();
            Console.Write("\nGuess the Capital:       Life points left: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(this.lifePoints);
            Console.ResetColor();
            Console.Write("     Not in word letters: ");
            Console.Write(string.Join(" ", this.notInWordList));
            this.PrintGuessingWord();
        }

        void PrintScore()   //TODO: sort and format HS
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("         ..:: H I G H   S C O R E S ::..");
            Console.ResetColor();
            using (StreamReader sr = File.OpenText(this.pathHS))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();
        }
    }
}
