﻿using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 1)]
    public class Day01 : BaseDay
    {
        public override string PartOne(string input)
        {
            var entries = input.Integers().OrderBy(x => x).ToList();
            var result = entries.GetCombinations(2).First(c => c.Sum() == 2020);

            return result.Multiply().ToString();
        }

        public override string PartTwo(string input)
        {
            var entries = input.Integers().OrderBy(x => x).ToList();
            var result = entries.GetCombinations(3).First(c => c.Sum() == 2020).ToList();

            return result.Multiply().ToString();
        }
    }
}
