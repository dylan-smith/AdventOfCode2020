using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 5)]
    public class Day05 : BaseDay
    {
        public override string PartOne(string input)
        {
            return input.Lines().Max(x => CalcSeatId(x)).ToString();
        }

        private int CalcSeatId(string input)
        {
            var binary = input.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1');
            return Convert.ToInt32(binary, 2);
        }

        public override string PartTwo(string input)
        {
            var seats = input.Lines().Select(CalcSeatId).OrderBy(x => x).ToList();

            for (var i = 1; i < seats.Count; i++)
            {
                if (seats[i] != seats[i - 1] + 1)
                {
                    return (seats[i] - 1).ToString();
                }
            }

            throw new Exception();
        }
    }
}
