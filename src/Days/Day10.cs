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
            var joltages = input.Integers().OrderBy(x => x).ToList();
            var curJoltage = 0;
            var diffCounts = new Dictionary<int, int>();

            foreach (var j in joltages)
            {
                diffCounts.SafeIncrement(j - curJoltage);
                curJoltage = j;
            }

            return (diffCounts[1] * (diffCounts[3] + 1)).ToString();
        }

        public override string PartTwo(string input)
        {
            var joltages = input.Integers().OrderBy(x => x).ToList();
            joltages.Insert(0, 0);

            return CountArrangements(0, joltages).ToString();
        }

        private long CountArrangements(int startIdx, List<int> joltages)
        {
            if (_counts.ContainsKey(startIdx))
            {
                return _counts[startIdx];
            }

            if (startIdx == joltages.Count - 1)
            {
                return 1;
            }

            var result = 0L;

            for (var i = startIdx + 1; i < joltages.Count && joltages[i] <= joltages[startIdx] + 3; i++)
            {
                result += CountArrangements(i, joltages);
            }

            _counts.Add(startIdx, result);
            return result;
        }
    }
}
