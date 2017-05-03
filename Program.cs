using System;
using System.Collections.Generic; //to allow for lists
using System.Threading; //to allow pauses for the AI to seem more real

namespace CMP1127M {

    class Program {

        public class Game {
            static Player[] players;
            static int maxScore = 50;
            static int turn = 0;
            public bool isGameOver = false;

            public Game(int playerCount, int newMaxScore) {

              if (playerCount == 1) {
                players = new Player[playerCount+1];
                players[0] = new Player(false);
                players[1] = new Player(true); //set up AI
              } else {
                players = new Player[playerCount];
                for (int i = 0; i < playerCount; i++) {
                  players[i] = new Player(false);
                }
              }

              maxScore = newMaxScore;
            }

            public void nextTurn() {
              //display header
              Console.BackgroundColor = ConsoleColor.Blue;
              Console.ForegroundColor = ConsoleColor.White;
              Console.WriteLine("Dice Game");
              Console.BackgroundColor = ConsoleColor.Gray;
              Console.ForegroundColor = ConsoleColor.Black;
              Console.WriteLine("Players: " + players.Length);
              Console.WriteLine("Playing to " + maxScore);
              Console.ResetColor();

              players[turn].turn();
              if (players[turn].getScore() >= maxScore) {
                endGame(turn);
              } else {
                turn++;
                if (turn > players.Length-1) {
                  turn = 0;
                }
              }
            }

            public void endGame(int winner) {
              Console.Clear();
              Console.BackgroundColor = ConsoleColor.White;
              Console.ForegroundColor = ConsoleColor.Black;
              Console.WriteLine("!!!" + players[winner].getName().ToUpper() + " WINS!!!\n");
              Console.ResetColor();

              Console.ReadKey(true);

              Console.WriteLine("Game stats: ");
              int totalRolls = 0;
              int[] rollValues = {0, 0, 0, 0, 0, 0};
              int totalNumber = 0;

              for (int i = 0; i < players.Length; i++) {
                Console.WriteLine(i);
                totalRolls += players[i].previousRolls.Count;
                foreach (int roll in players[i].previousRolls) {
                  rollValues[roll-1]++;
                  totalNumber = totalNumber + roll;
                }

                isGameOver = true;
              }

              int averageRoll = totalNumber/totalRolls;

              Console.WriteLine("Total rolls: " + totalRolls);
              Console.WriteLine("1 was rolled " + rollValues[0] + " times.");
              Console.WriteLine("2 was rolled " + rollValues[1] + " times.");
              Console.WriteLine("3 was rolled " + rollValues[2] + " times.");
              Console.WriteLine("4 was rolled " + rollValues[3] + " times.");
              Console.WriteLine("5 was rolled " + rollValues[4] + " times.");
              Console.WriteLine("6 was rolled " + rollValues[5] + " times.");
              Console.WriteLine("The average roll was " + averageRoll);
              Console.WriteLine("A total of " + totalNumber + " was rolled up.");
              Console.ReadLine();

            }
        }

        public class Die {
            int faceValue;
            static Random rnd = new Random();

            public Die() {
              //Console.WriteLine("New die created!");
            }

            public void roll() {
                faceValue = rnd.Next(1, 7);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(faceValue);
                Console.ResetColor();
            }

            public int getValue() {
                //Console.WriteLine("Returning value " + faceValue);
                return faceValue;
            }
        }

        public class Player {
            string name = "Guest";
            int score = 0;
            bool AI;

            public List<int> previousRolls = new List<int>(); //this is a list so that we can store a potentially infinite set of values

            Die[] dice = new Die[5];

            public Player(bool isAI) {
                AI = isAI;

                for (int i = 0; i < 5; i++) {
                  dice[i] = new Die();
                }

                if (AI == false) {
                  Console.WriteLine("Welcome, player! What's your name?");
                  name = Console.ReadLine();
                } else {
                  name = @"MU/TH/UR 6000";
                }
            }

            public int turn() {
                int roundScore = 0;
                Console.WriteLine("Your turn, " + name + ".\nScore: " + score + "\n(1) - Throw, reroll three\n(2) - Throw All, double points");
                char option;
                if (AI == true) {
                  Thread.Sleep(500);
                  option = '1';
                } else {
                  option = Console.ReadKey().KeyChar;
                }

                if (option == '1') {
                  Console.WriteLine("\nPress any key to roll!");
                  for (int i = 0; i < 5; i++) {
                    if (AI == true) {
                      Thread.Sleep(500);
                    } else {
                      Console.ReadKey(true);
                    }
                    dice[i].roll();
                    Console.Write(", ");
                    previousRolls.Add(dice[i].getValue());
                  }

                  Console.WriteLine("\nAll dice rolled. Press any key to calculate score!");
                  if (AI == true) {
                    Thread.Sleep(1000);
                  } else {
                    Console.ReadKey(true);
                  }

                  roundScore = calculateScore();

                  addScore(roundScore);

                } else if (option == '2') {

                  Console.WriteLine("\nPress any key to roll all dice!");

                  for (int i = 0; i < 5; i++) {
                    dice[i].roll();
                    Console.Write(", ");
                    previousRolls.Add(dice[i].getValue());
                  }

                  Console.WriteLine("\nAll dice rolled. Press any key to calculate score!");
                  if (AI == true) {
                    Thread.Sleep(1000);
                  } else {
                    Console.ReadKey(true);
                  }

                  roundScore = calculateScore(true)*2;

                  addScore(roundScore);
                }

                Console.WriteLine("You scored " + roundScore + " points!\n\nPress any key to continue...");
                if (AI == true) {
                  Thread.Sleep(4000);
                } else {
                  Console.ReadKey(true);
                }

                return 0;
            }

            public int calculateScore(bool threwAll = false) { //
              Console.WriteLine("Calculating score...");
              int[] results = {0, 0, 0, 0, 0, 0};
              for (int i = 0; i < 5; i++) { //cycle through all dice
                results[dice[i].getValue()-1]++;
              }

              int[] bestResult = calculateBestResult(results);

              Console.WriteLine(bestResult[0] + " of a kind! (" + bestResult[1] + ")");

              if (bestResult[0] == 2 && threwAll == false) {
                Console.WriteLine("Reroll the remaining 3 dice! (Press any key to roll)");

                for (int i = 0; i < 5; i++) {
                  if (dice[i].getValue() != bestResult[1]) {
                    if (AI == true) {
                      Thread.Sleep(500);
                    } else {
                      Console.ReadKey(true);
                    }
                    dice[i].roll();
                    results[dice[i].getValue()-1]++;
                    Console.Write(", ");
                    previousRolls.Add(dice[i].getValue());
                  }
                }
                Console.WriteLine("\nAll dice rolled. Press any key to calculate score!");
                if (AI == true) {
                  Thread.Sleep(1000);
                } else {
                  Console.ReadKey(true);
                }
              }

              bestResult = calculateBestResult(results);

              switch (bestResult[0]) {
                case 3:
                  return 3;
                case 4:
                  return 6;
                case 5:
                  return 12;
                default:
                  return 0;
              }
            }

            public int[] calculateBestResult(int[] results) {
              int[] bestResult = {1, 0}; //value, dice number

              for (int i = 0; i < 6; i++) {
                if (results[i] >= bestResult[0]) {
                  bestResult[0] = results[i];
                  bestResult[1] = i+1;
                }
              }

              return bestResult;
            }

            public void addScore(int toAdd) {
              score += toAdd;
            }

            public int getScore() {
              return score;
            }

            public string getName() {
              return name;
            }
        }

        static void Main(string[] args) {
          bool isPlaying = true;
          while (isPlaying == true) {
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Dice Game");
            Console.ResetColor();

            Console.Write("Number of players (if 1 then an AI will be used): ");
            int playerCount = 1;

            try {
              playerCount = Convert.ToInt32(Console.ReadLine());
            } catch (FormatException e) {
              Console.WriteLine("Error: " + e);
            }

            Console.Write("Score to play to: ");

            int maxScore = 50;

            try {
              maxScore = Convert.ToInt32(Console.ReadLine());
            } catch (FormatException e) {
              Console.WriteLine("Error: " + e);
            }

            Game game = new Game(playerCount, maxScore);
            while (game.isGameOver == false) {
              Console.Clear();
              game.nextTurn();
            }

            while (1 == 1) {
              Console.WriteLine("\nWould you like to play again? y/n");
              char option = Console.ReadKey().KeyChar;
              if (option == 'n') {
                isPlaying = false;
                break;
              } else if (option == 'y') {
                break;
              }
            }
          }
        }

    }

}
