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
            var x = 0;
            var y = 0;
            var count = 0L;

            while (y < grid.GetUpperBound(1))
            {
                x += right;
                y += down;

                x %= grid.GetUpperBound(0) + 1;

                if (grid[x, y] == '#')
                {
                    count++;
                }
            }

            return count;
        }

        public override string PartTwo(string input)
        {
            var grid = input.CreateCharGrid();

            var a = CountTrees(grid, 1, 1);
            var b = CountTrees(grid, 3, 1);
            var c = CountTrees(grid, 5, 1);
            var d = CountTrees(grid, 7, 1);
            var e = CountTrees(grid, 1, 2);

            return (a * b * c * d * e).ToString();
        }
    }
}
