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
            return GetRow(input.Substring(0, 7)) * 8 + GetColumn(input.Substring(7));
        }

        private int GetRow(string input)
        {
            var min = 0;
            var max = 127;

            foreach (var c in input)
            {
                if (c == 'F')
                {
                    max -= (max - min + 1) / 2;
                }

                if (c == 'B')
                {
                    min += (max - min + 1) / 2;
                }
            }

            return min;
        }

        private int GetColumn(string input)
        {
            var min = 0;
            var max = 8;

            foreach (var c in input)
            {
                if (c == 'L')
                {
                    max -= (max - min + 1) / 2;
                }

                if (c == 'R')
                {
                    min += (max - min + 1) / 2;
                }
            }

            return min;
        }

        public override string PartTwo(string input)
        {
            var seats = input.Lines().Select(CalcSeatId).OrderBy(x => x).ToList();

            for (var i = 0; i < seats.Count; i++)
            {
                if (seats[i] != seats[0] + i)
                {
                    return (seats[0] + i).ToString();
                }
            }

            throw new Exception();
        }
    }
}
