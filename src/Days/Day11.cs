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
            char[,] prev;

            do
            {
                prev = grid;
                grid = UpdateSeats(grid);
            } while (grid.Count('#') != prev.Count('#') || grid.Count('.') != prev.Count('.') || grid.Count('L') != prev.Count('L'));

            return grid.Count('#').ToString();
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
            if (c == 'L' && !grid.GetNeighbors(x, y).Any(c => c == '#'))
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
            if (c == 'L' && !GetVisibleSeats(grid, x, y).Any(c => c == '#'))
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

                while (xx < grid.Width() && xx >= 0 && yy < grid.Height() && yy >= 0)
                {
                    if (grid[xx, yy] != '.')
                    {
                        yield return grid[xx, yy];
                        break;
                    }

                    xx += dir.x;
                    yy += dir.y;
                }
            }
        }

        public override string PartTwo(string input)
        {
            var grid = input.CreateCharGrid();
            char[,] prev;

            do
            {
                prev = grid;
                grid = UpdateSeats2(grid);
            } while (grid.Count('#') != prev.Count('#') || grid.Count('.') != prev.Count('.') || grid.Count('L') != prev.Count('L'));

            return grid.Count('#').ToString();
        }
    }
}
