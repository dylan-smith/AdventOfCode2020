using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 9)]
    public class Day09 : BaseDay
    {
        public override string PartOne(string input)
        {
            var numbers = input.Longs().ToList();

            return FindBadXMAS(numbers).ToString();
        }

        private long FindBadXMAS(List<long> numbers)
        {
            for (var i = 25; i < numbers.Count; i++)
            {
                var combos = numbers.Skip(i - 25).Take(25).GetCombinations(2);
                var result = combos.FirstOrDefault(x => x.Sum() == numbers[i]);

                if (result == null)
                {
                    return numbers[i];
                }
            }

            throw new Exception();
        }

        public override string PartTwo(string input)
        {
            var numbers = input.Longs().ToList();
            var target = FindBadXMAS(numbers);

            for (var start = 0; start < numbers.Count; start++)
            {
                var sum = numbers[start];

                for (var end = start + 1; end < numbers.Count && sum < target; end++)
                {
                    sum += numbers[end];

                    if (sum == target)
                    {
                        return CalcWeakness(numbers, start, end).ToString();
                    }
                }
            }

            throw new Exception();
        }

        private long CalcWeakness(List<long> numbers, int start, int end)
        {
            var result = numbers.Skip(start).Take(end - start).ToList();

            return result.Min() + result.Max();
        }
    }
}
