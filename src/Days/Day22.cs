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

            var result = CalcScore(winner);

            return result.ToString();
        }

        private long CalcScore(Queue<int> winner)
        {
            var pos = winner.Count;
            var result = 0L;

            while (winner.Any())
            {
                var card = winner.Dequeue();
                result += card * pos;
                pos--;
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
            return new Queue<int>(input.Lines().Skip(1).Select(x => int.Parse(x)));
        }

        public override string PartTwo(string input)
        {
            var (player1, player2) = ParseDecks(input);

            var (_, deck) = PlayGame(player1, player2);

            var result = CalcScore(deck);

            return result.ToString();
        }

        private (int winner, Queue<int> deck) PlayGame(Queue<int> player1, Queue<int> player2)
        {
            var seen = new List<(List<int> player1, List<int> player2)>();

            while (player1.Any() && player2.Any())
            {
                if (seen.Any(s => s.player1.SequenceEqual(player1) && s.player2.SequenceEqual(player2)))
                {
                    return (1, player1);
                }

                seen.Add((player1.ToList(), player2.ToList()));

                (player1, player2) = PlayRound2(player1, player2);
            }

            return player1.Any() ? (1, player1) : (2, player2);
        }

        private (Queue<int> player1, Queue<int> player2) PlayRound2(Queue<int> player1, Queue<int> player2)
        {
            var card1 = player1.Dequeue();
            var card2 = player2.Dequeue();

            if (player1.Count < card1 || player2.Count < card2)
            {
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
            }
            else
            {
                var sub1 = new Queue<int>(player1.Take(card1));
                var sub2 = new Queue<int>(player2.Take(card2));

                var (winner, _) = PlayGame(sub1, sub2);

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
            }

            return (player1, player2);
        }
    }
}
