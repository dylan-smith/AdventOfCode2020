using System;

namespace AdventOfCode
{
    public sealed class DayAttribute : Attribute
    {
        public int Year { get; set; }
        public int Day { get; set; }

        public DayAttribute(int year, int day)
        {
            Year = year;
            Day = day;
        }

        public override string ToString()
        {
            return $"{Year} - Day {Day}";
        }
    }
}
