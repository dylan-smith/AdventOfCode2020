using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 13)]
    public class Day13 : BaseDay
    {
        public override string PartOne(string input)
        {
            var timeLeaving = int.Parse(input.Lines().First());
            var buses = input.Lines().Last().Words().Where(b => b != "x").Select(int.Parse).ToList();
            var minWait = int.MaxValue;
            var nextBus = 0;
            
            foreach (var bus in buses)
            {
                var time = 0;

                while (time < timeLeaving)
                {
                    time += bus;
                }

                if (time < minWait)
                {
                    minWait = time;
                    nextBus = bus;
                }
            }

            return (nextBus * (minWait - timeLeaving)).ToString();
        }

        public override string PartTwo(string input)
        {
            var schedule = input.Lines().Last().Words().ToList();

            // 23 @ 0 ***
            // 41 @ 13
            // 647 @ 23  ***
            // 13 @ 41
            // 19 @ 42 ***
            // 29 @ 52 ***
            // 557 @ 54
            // 37 @ 60 ***
            // 17 @ 71

            var result = GenerateNums().Where(n => ((n + 31) % 557) == 0)
                          .Where(n => ((n - 10) % 41) == 0)
                          .Where(n => ((n + 37) % 37) == 0)
                          .Where(n => ((n + 29) % 29) == 0)
                          .Where(n => ((n - 23) % 23) == 0)
                          .Where(n => ((n + 19) % 19) == 0)
                          .Where(n => ((n + 48) % 17) == 0)
                          .Where(n => ((n + 18) % 13) == 0)
                          .First() - 23;

            return result.ToString();
        }

        private IEnumerable<long> GenerateNums()
        {
            var result = 0L;

            while (true)
            {
                result += 303378947;
                yield return result;
            }
        }
    }
}
