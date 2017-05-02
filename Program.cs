using System;
using System.Collections.Generic; //to allow for lists

namespace CMP1127M {

    class Program {

        public class Game {
            static Player[] players;
            static int maxScore = 50;
            static int turn = 0;

            public Game(int playerCount) {

              players = new Player[playerCount];
              for (int i = 0; i < playerCount; i++) {
                players[i] = new Player();
              }

            }

            public void nextTurn() {
              players[turn].turn();
              turn++;
              if (turn > players.Length) {
                turn = 0;
              }
            }
        }

        public class Die {
            static int faceValue;
            static Random rnd = new Random();

            public Die() {
            }

            public void roll() {
                faceValue = rnd.Next(1, 6);
                Console.Write(faceValue);
            }

            public int getValue() {
                return faceValue;
            }
        }

        public class Player {
            static string name;
            static int score = 0;

            public List<int> previousRolls = new List<int>(); //this is a list so that we can store a potentially infinite set of values

            Die[] dice = new Die[4];

            public Player(string name = "Guest") {

                for (int i = 0; i < 5; i++) { //create dice
                    dice[i] = new Die();
                }

            }

            public int turn() {

                Console.WriteLine("Your turn, " + name + ".\nScore: " + score + "(1) - Throw, reroll three\n(2) - Throw All, double points");
                char option = Console.ReadKey().KeyChar;

                if (option == '1') {
                  Console.WriteLine("\nPress any key to roll!");
                  for (int i = 0; i < 5; i++) {
                    Console.ReadKey();
                    dice[i].roll();
                    Console.Write(dice[i].getValue() + " ");
                  }
                }

                return 0;
            }
        }

        static void Main(string[] args) {
          Game game = new Game(3);
          game.nextTurn();
          game.nextTurn();
          game.nextTurn();
          game.nextTurn();
          game.nextTurn();
          game.nextTurn();
          game.nextTurn();
        }

    }

}
