using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 20)]
    public class Day20 : BaseDay
    {
        public override string PartOne(string input)
        {
            var tiles = ParseTiles(input);

            var corners = ArrangeTiles(tiles);

            return corners.Multiply().ToString();
        }

        private List<long> ArrangeTiles(Dictionary<int, char[,]> tiles)
        {
            var corners = new List<long>();

            foreach (var a in tiles)
            {
                var sides = GetSides(a.Value);

                foreach (var b in tiles)
                {
                    if (a.Key != b.Key)
                    {
                        var newSides = new List<string>();

                        foreach (var side in sides)
                        {
                            if (!SidesMatch(side, b.Value))
                            {
                                newSides.Add(side);
                            }
                        }

                        sides = newSides;
                    }
                }

                if (sides.Count() == 2)
                {
                    corners.Add((long)a.Key);
                }
            }

            return corners;
        }

        private bool SidesMatch(string compare, char[,] value)
        {
            var flipped = compare.ReverseString();
            var sides = GetSides(value);

            foreach (var side in sides)
            {
                if (compare == side || flipped == side)
                {
                    return true;
                }
            }

            return false;
        }

        private List<string> GetSides(char[,] value)
        {
            var top = string.Empty;
            var bottom = string.Empty;
            var left = string.Empty;
            var right = string.Empty;

            for (var x = 0; x < value.Width(); x++)
            {
                top += value[x, 0];
            }

            for (var x = 0; x < value.Width(); x++)
            {
                bottom += value[x, value.GetUpperBound(1)];
            }

            for (var y = 0; y < value.Height(); y++)
            {
                left += value[0, y];
            }

            for (var y = 0; y < value.Height(); y++)
            {
                right += value[value.GetUpperBound(0), y];
            }

            return new List<string>() { top, bottom, left, right };
        }

        private Dictionary<int, char[,]> ParseTiles(string input)
        {
            var tiles = input.Paragraphs();
            var result = new Dictionary<int, char[,]>();

            foreach (var tile in tiles)
            {
                var id = int.Parse(tile.Lines().First().Split(new string[] { "Tile ", ":" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                var grid = string.Join("\n", tile.Lines().Skip(1)).CreateCharGrid();

                result.Add(id, grid);
            }

            return result;
        }

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }
    }
}
