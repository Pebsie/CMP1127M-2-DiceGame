using System;

namespace CMP1127M {

    class Program {

        public class Game {
            Player[] players;
            int maxScore = 50;
            int turn = 0;
        }

        public class Die {
            int value;
            Random rnd = new Random()

            public Die() {
            }

            public static void roll() {
                value = rnd.Next(1, 6);
            }

            public int getValue() {
                return value;
            }
        }

        public class Player {
            string name;
            int score = 0;
            
            
            Die[] dice = new Die[4];

            public Player(string name = "Guest") {

                for (int i = 0; i < 5; i++) { //create dice
                    dice[i] = new Die();
                }

            }

            public int turn() {
                Console.WriteLine("Your turn, " + name + ".\nScore: " + score + "(1) - Throw, reroll three\n(2) - Throw All, double points");
            }
        }

        static void Main(string[] args) {

        }

    }

}