using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 10)]
    public class Day10 : BaseDay
    {
        private Dictionary<int, long> _counts = new Dictionary<int, long>();

        public override string PartOne(string input)
        {
            var joltages = input.Integers().OrderBy(x => x). ToList();
            var ones = 0;
            var threes = 0;
            var curJoltage = 0;

            foreach (var j in joltages)
            {
                if (j == curJoltage + 1)
                {
                    ones++;
                }
                
                if (j == curJoltage + 3)
                {
                    threes++;
                }

                curJoltage = j;
            }

            return (ones * (threes + 1)).ToString();
        }

        public override string PartTwo(string input)
        {
            var joltages = input.Integers().OrderBy(x => x).ToList();

            var device = joltages.Last() + 3;

            var result = CountArrangements(-1, joltages);

            return result.ToString();
        }

        //private long CountArrangements(int start, int end, List<int> joltages)
        //{
        //    var newJolts = joltages.Where(j => j > start).ToList();
        //    var candidates = joltages.Where(j => j <= start + 3).ToList();
        //    var result = 0L;

        //    if (candidates.Count == 0)
        //    {
        //        return 1;
        //    }

        //    foreach (var j in candidates)
        //    {
        //        result += CountArrangements(j, end, newJolts);
        //    }

        //    return result;
        //}

        private long CountArrangements(int startIdx, List<int> joltages)
        {
            if (_counts.ContainsKey(startIdx))
            {
                return _counts[startIdx];
            }

            var result = 0L;

            if (startIdx == joltages.Count - 1)
            {
                return 1;
            }

            var i = startIdx + 1;
            var prev = startIdx < 0 ? 0 : joltages[startIdx];

            while (i < joltages.Count && joltages[i] <= prev + 3)
            {
                result += CountArrangements(i, joltages);
                i++;
            }

            _counts.Add(startIdx, result);
            return result;
        }
    }
}
