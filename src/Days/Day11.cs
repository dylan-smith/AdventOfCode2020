using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 11)]
    public class Day11 : BaseDay
    {
        public override string PartOne(string input)
        {
            var grid = input.CreateCharGrid();

            var occupied = grid.Count('#');
            var floor = grid.Count('.');
            var empty = grid.Count('L');

            grid = UpdateSeats(grid);
            Log(grid.GetString());
            Log("==================");

            while (grid.Count('#') != occupied || grid.Count('.') != floor || grid.Count('L') != empty)
            {
                occupied = grid.Count('#');
                floor = grid.Count('.');
                empty = grid.Count('L');

                grid = UpdateSeats(grid);
                Log(grid.GetString());
                Log("==================");
            }
                        
            return occupied.ToString();
        }

        private char[,] UpdateSeats(char[,] grid)
        {
            return grid.Clone((x, y, c) => UpdateSeat(x, y, c, grid));
        }

        private char[,] UpdateSeats2(char[,] grid)
        {
            return grid.Clone((x, y, c) => UpdateSeat2(x, y, c, grid));
        }

        private char UpdateSeat(int x, int y, char c, char[,] grid)
        {
            if (c == 'L' && grid.GetNeighbors(x, y).Count(c => c == '#') == 0)
            {
                return '#';
            }

            if (c == '#' && grid.GetNeighbors(x, y).Count(c => c == '#') >= 4)
            {
                return 'L';
            }

            return c;
        }

        private char UpdateSeat2(int x, int y, char c, char[,] grid)
        {
            if (c == 'L' && GetVisibleSeats(grid, x, y).Count(c => c == '#') == 0)
            {
                return '#';
            }

            if (c == '#' && GetVisibleSeats(grid, x, y).Count(c => c == '#') >= 5)
            {
                return 'L';
            }

            return c;
        }

        private IEnumerable<char> GetVisibleSeats(char[,] grid, int x, int y)
        {
            var directions = new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0), (1, 1), (-1, -1), (-1, 1), (1, -1) };

            foreach (var dir in directions)
            {
                var xx = x + dir.x;
                var yy = y + dir.y;
                var found = false;

                while (xx < grid.Width() && xx >= 0 && yy < grid.Height() && yy >= 0 && !found)
                {
                    if (grid[xx, yy] != '.')
                    {
                        found = true;
                        yield return grid[xx, yy];
                    }

                    xx = xx + dir.x;
                    yy = yy + dir.y;
                }
            }
        }

        public override string PartTwo(string input)
        {
            var grid = input.CreateCharGrid();

            var occupied = grid.Count('#');
            var floor = grid.Count('.');
            var empty = grid.Count('L');

            grid = UpdateSeats(grid);
            Log(grid.GetString());
            Log("==================");

            while (grid.Count('#') != occupied || grid.Count('.') != floor || grid.Count('L') != empty)
            {
                occupied = grid.Count('#');
                floor = grid.Count('.');
                empty = grid.Count('L');

                grid = UpdateSeats2(grid);
                Log(grid.GetString());
                Log("==================");
            }

            return occupied.ToString();
        }
    }
}
