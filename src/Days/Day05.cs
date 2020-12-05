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
            var rowInput = input.Substring(0, 7).Replace('F', '0').Replace('B', '1');
            var colInput = input.Substring(7).Replace('L', '0').Replace('R', '1');

            var row = Convert.ToInt32(rowInput, 2);
            var col = Convert.ToInt32(colInput, 2);

            return row * 8 + col;
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
