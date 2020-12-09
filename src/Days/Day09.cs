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

            for (var i = 25; i < numbers.Count; i++)
            {
                var combos = numbers.Skip(i - 25).Take(25).GetCombinations(2);
                var result = combos.FirstOrDefault(x => x.Sum() == numbers[i]);

                if (result == null)
                {
                    return numbers[i].ToString();
                }
            }

            throw new Exception();

        }

        public override string PartTwo(string input)
        {
            var target = 1038347917L;

            var numbers = input.Longs().ToList();

            for (var size = 2; size < numbers.Count; size++)
            {
                var sum = 0L;

                for (var i = 0; i < size; i++)
                {
                    sum += numbers[i];
                }

                for (var i = size; i < numbers.Count; i++)
                {
                    sum += numbers[i];
                    sum -= numbers[i - size];

                    if (sum == target)
                    {
                        return GetSmallBig(numbers, size, i).ToString();
                    }
                }
            }

            throw new Exception();
        }

        private long GetSmallBig(List<long> numbers, int size, int last)
        {
            var all = new List<long>();

            for (var i = 0; i < size; i++)
            {
                all.Add(numbers[last - i]);
            }

            return all.Min() + all.Max();
        }
    }
}
