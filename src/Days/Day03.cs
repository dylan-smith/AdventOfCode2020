using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 3)]
    public class Day03 : BaseDay
    {
        public override string PartOne(string input)
        {
            var grid = input.CreateCharGrid();

            return CountTrees(grid, 3, 1).ToString();
        }

        private long CountTrees(char[,] grid, int right, int down)
        {
            var x = right;
            var y = down;
            var count = 0L;

            while (y < grid.Height())
            {
                if (grid.GetCharWithWrapping(x, y) == '#')
                {
                    count++;
                }

                x += right;
                y += down;
            }

            return count;
        }

        public override string PartTwo(string input)
        {
            var grid = input.CreateCharGrid();
            var slopes = new (int x, int y)[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };
            
            var trees = slopes.Select(slope => CountTrees(grid, slope.x, slope.y));

            return trees.Multiply().ToString();
        }
    }
}
