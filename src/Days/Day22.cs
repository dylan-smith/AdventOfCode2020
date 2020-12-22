using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 22)]
    public class Day22 : BaseDay
    {
        public override string PartOne(string input)
        {
            var (player1, player2) = ParseDecks(input);

            while (player1.Any() && player2.Any())
            {
                (player1, player2) = PlayRound(player1, player2);
            }

            var winner = player1.Any() ? player1 : player2;

            return CalcScore(winner).ToString();
        }

        private int CalcScore(Queue<int> winner)
        {
            var pos = winner.Count;
            var result = 0;

            while (winner.Any())
            {
                var card = winner.Dequeue();
                result += card * pos--;
            }

            return result;
        }

        private (Queue<int> player1, Queue<int> player2) PlayRound(Queue<int> player1, Queue<int> player2)
        {
            var card1 = player1.Dequeue();
            var card2 = player2.Dequeue();

            if (card1 > card2)
            {
                player1.Enqueue(card1);
                player1.Enqueue(card2);
            }
            else
            {
                player2.Enqueue(card2);
                player2.Enqueue(card1);
            }

            return (player1, player2);
        }

        private (Queue<int> player1, Queue<int> player2) ParseDecks(string input)
        {
            var player1 = ParsePlayer(input.Paragraphs().First());
            var player2 = ParsePlayer(input.Paragraphs().Last());

            return (player1, player2);
        }

        private Queue<int> ParsePlayer(string input)
        {
            return new Queue<int>(input.Lines().Skip(1).Select(int.Parse));
        }

        public override string PartTwo(string input)
        {
            var (player1, player2) = ParseDecks(input);

            var (_, deck) = PlayRecursiveGame(player1, player2);

            return CalcScore(deck).ToString();
        }

        private (int winner, Queue<int> deck) PlayRecursiveGame(Queue<int> player1, Queue<int> player2)
        {
            var seen = new HashSet<(HashableList<int>, HashableList<int>)>();
            
            while (player1.Any() && player2.Any())
            {
                var state1 = new HashableList<int>(player1.ToArray());
                var state2 = new HashableList<int>(player2.ToArray());

                if (!seen.Add((state1, state2)))
                {
                    return (1, player1);
                }

                (player1, player2) = PlayRecursiveRound(player1, player2);
            }

            return player1.Any() ? (1, player1) : (2, player2);
        }

        private (Queue<int> player1, Queue<int> player2) PlayRecursiveRound(Queue<int> player1, Queue<int> player2)
        {
            var card1 = player1.Dequeue();
            var card2 = player2.Dequeue();
            int winner;

            if (player1.Count < card1 || player2.Count < card2)
            {
                winner = card1 > card2 ? 1 : 2;
            }
            else
            {
                var sub1 = new Queue<int>(player1.Take(card1));
                var sub2 = new Queue<int>(player2.Take(card2));

                (winner, _) = PlayRecursiveGame(sub1, sub2);
            }

            if (winner == 1)
            {
                player1.Enqueue(card1);
                player1.Enqueue(card2);
            }
            else
            {
                player2.Enqueue(card2);
                player2.Enqueue(card1);
            }

            return (player1, player2);
        }
    }

    public class GameState
    {
        public int[] Player1 { get; set; }
        public int[] Player2 { get; set; }

        public GameState(Queue<int> player1, Queue<int> player2)
        {
            Player1 = player1.ToArray();
            Player2 = player2.ToArray();
        }

        public override bool Equals(object obj)
        {
            var state = (GameState)obj;
            return this.Player1.SequenceEqual(state.Player1) && this.Player2.SequenceEqual(state.Player2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                foreach (var x in Player1)
                {
                    hash = hash * 31 + x.GetHashCode();
                }
                foreach (var x in Player2)
                {
                    hash = hash * 31 + x.GetHashCode();
                }
                return hash;
            }
        }
    }
}
