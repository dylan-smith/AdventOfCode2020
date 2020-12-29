using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 25)]
    public class Day25 : BaseDay
    {
        public override string PartOne(string input)
        {
            var doorPublicKey = input.Integers().First();
            var cardPublicKey = input.Integers().Last();

            var value = 1L;
            var subject = 7;
            var doorLoop = 0;

            while (value != doorPublicKey)
            {
                value *= subject;
                value %= 20201227;
                doorLoop++;
            }

            subject = cardPublicKey;
            value = 1L;

            for (var i = 0; i < doorLoop; i++)
            {
                value *= subject;
                value %= 20201227;
            }

            return value.ToString();
        }

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }
    }
}
