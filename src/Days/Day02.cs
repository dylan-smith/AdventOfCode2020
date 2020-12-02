using System;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 2)]
    public class Day02 : BaseDay
    {
        public override string PartOne(string input)
        {
            var passwords = input.Lines().Select(ParseInput);

            return passwords.Count(IsPasswordValid1).ToString();
        }

        private bool IsPasswordValid1((int min, int max, char letter, string password) input)
        {
            var count = input.password.Count(c => c == input.letter);

            return count >= input.min && count <= input.max;
        }

        private bool IsPasswordValid2((int min, int max, char letter, string password) input)
        {
            var first = input.password[input.min - 1] == input.letter;
            var second = input.password[input.max - 1] == input.letter;

            return first ^ second;
        }

        private (int min, int max, char letter, string password) ParseInput(string input)
        {
            var segments = input.Split(new char[] { '-', ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);

            var min = int.Parse(segments[0]);
            var max = int.Parse(segments[1]);
            var letter = segments[2][0];
            var password = segments.Last();

            return (min, max, letter, password);
        }

        public override string PartTwo(string input)
        {
            var passwords = input.Lines().Select(ParseInput);

            return passwords.Count(IsPasswordValid2).ToString();
        }
    }
}
