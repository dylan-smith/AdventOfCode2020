using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 15)]
    public class Day15 : BaseDay
    {
        public override string PartOne(string input)
        {
            var numbers = input.Integers().ToList();

            return MemoryGame(numbers, 2020).ToString();
        }

        public int MemoryGame(IEnumerable<int> starting, int position)
        {
            var lastSpoken = new Dictionary<int, int>();

            for (var i = 0; i < starting.Count() - 1; i++)
            {
                lastSpoken.SafeSet(starting.ElementAt(i), i);
            }

            var target = starting.Last();

            for (var pos = starting.Count(); pos < position; pos++)
            {
                if (lastSpoken.ContainsKey(target))
                {
                    var age = pos - lastSpoken[target] - 1;
                    lastSpoken[target] = pos - 1;
                    target = age;
                }
                else
                {
                    lastSpoken[target] = pos - 1;
                    target = 0;
                }
            }

            return target;
        }

        public override string PartTwo(string input)
        {
            var numbers = input.Integers().ToList();
            
            return MemoryGame(numbers, 30000000).ToString();
        }
    }
}
