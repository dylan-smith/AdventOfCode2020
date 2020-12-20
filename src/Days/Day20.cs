using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 20)]
    public class Day20 : BaseDay
    {
        public override string PartOne(string input)
        {
            var tiles = ParseTiles(input);

            var corners = FindCorners(tiles);

            corners.ForEach(c => Log($"{c}"));

            return corners.Multiply().ToString();
        }

        private List<long> FindCorners(Dictionary<int, char[,]> tiles)
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
            var tiles = ParseTiles(input);

            var map = ArrangeTiles(tiles);

            var variations = GetVariations(map);

            foreach (var variation in variations)
            {
                var monsterPoints = CountSeaMonsters(variation);

                if (monsterPoints.Any())
                {
                    var result = variation.GetPoints('#').Count(p => !monsterPoints.Contains(p));
                    return result.ToString();
                }
            }

            return "foo";
        }

        private List<Point> CountSeaMonsters(char[,] map)
        {
            var result = new List<Point>();

            var monsterPoints = new List<Point>()
            {
                new Point(0, 1),
                new Point(1, 2),
                new Point(4, 2),
                new Point(5, 1),
                new Point(6, 1),
                new Point(7, 2),
                new Point(10, 2),
                new Point(11, 1),
                new Point(12, 1),
                new Point(13, 2),
                new Point(16, 2),
                new Point(17, 1),
                new Point(18, 1),
                new Point(18, 0),
                new Point(19, 1)
            };

            for (var y = 0; y < map.Height(); y++)
            {
                for (var x = 0; x < map.Width(); x++)
                {
                    if (CheckForSeaMonster(map, x, y))
                    {
                        foreach (var p in monsterPoints)
                        {
                            result.Add(new Point(x + p.X, y + p.Y));
                        }
                    }
                }
            }

            return result;
        }

        private bool CheckForSeaMonster(char[,] map, int x, int y)
        {
            var monsterPoints = new List<Point>()
            {
                new Point(0, 1),
                new Point(1, 2),
                new Point(4, 2),
                new Point(5, 1),
                new Point(6, 1),
                new Point(7, 2),
                new Point(10, 2),
                new Point(11, 1),
                new Point(12, 1),
                new Point(13, 2),
                new Point(16, 2),
                new Point(17, 1),
                new Point(18, 1),
                new Point(18, 0),
                new Point(19, 1)
            };

            foreach (var p in monsterPoints)
            {
                if (x + p.X >= map.Width())
                {
                    return false;
                }

                if (y + p.Y >= map.Height())
                {
                    return false;
                }

                if (map[x + p.X, y + p.Y] != '#')
                {
                    return false;
                }
            }

            return true;
        }

        private List<char[,]> GetVariations(char[,] map)
        {
            var result = new List<char[,]>();

            result.Add(map);
            result.Add(map.Rotate(1));
            result.Add(map.Rotate(2));
            result.Add(map.Rotate(3));

            result.Add(map.FlipVertical());
            result.Add(map.FlipVertical().Rotate(1));
            result.Add(map.FlipVertical().Rotate(2));
            result.Add(map.FlipVertical().Rotate(3));

            return result;
        }

        private char[,] ArrangeTiles(Dictionary<int, char[,]> tiles)
        {
            var variations = GetVariations(tiles);
            var size = (int)Math.Sqrt(tiles.Count);
            var map = new char[size, size][,];
            var foundId = 0;

            foreach (var id in variations)
            {
                foreach (var variation in id.Value)
                {
                    if (!HasMatch(GetTop(variation), variations, id.Key) && !HasMatch(GetLeft(variation), variations, id.Key))
                    {
                        map[0, 0] = variation;
                        foundId = id.Key;
                        break;
                    }
                }
            }

            variations.Remove(foundId);

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    if (y == 0 && x > 0)
                    {
                        var matchLeft = GetRight(map[x - 1, y]);

                        foreach (var id in variations)
                        {
                            foreach (var variation in id.Value)
                            {
                                if (GetLeft(variation) == matchLeft && !HasMatch(GetTop(variation), variations, id.Key))
                                {
                                    map[x, y] = variation;
                                    foundId = id.Key;
                                    break;
                                }
                            }
                        }

                        variations.Remove(foundId);
                    }

                    if (x == 0 && y > 0)
                    {
                        var matchTop = GetBottom(map[x, y - 1]);

                        foreach (var id in variations)
                        {
                            foreach (var variation in id.Value)
                            {
                                if (GetTop(variation) == matchTop && !HasMatch(GetLeft(variation), variations, id.Key))
                                {
                                    map[x, y] = variation;
                                    foundId = id.Key;
                                    break;
                                }
                            }
                        }

                        variations.Remove(foundId);
                    }

                    if (x > 0 && y > 0)
                    {
                        var matchTop = GetBottom(map[x, y - 1]);
                        var matchLeft = GetRight(map[x - 1, y]);

                        foreach (var id in variations)
                        {
                            foreach (var variation in id.Value)
                            {
                                if (GetTop(variation) == matchTop && GetLeft(variation) == matchLeft)
                                {
                                    map[x, y] = variation;
                                    foundId = id.Key;
                                    break;
                                }
                            }
                        }

                        variations.Remove(foundId);
                    }
                }
            }

            var finalMap = RemoveBorders(map);

            return finalMap;
        }

        private char[,] RemoveBorders(char[,][,] map)
        {
            var tilesWide = map.GetUpperBound(0) + 1;
            var tilesHigh = map.GetUpperBound(1) + 1;
            var tileWidth = map[0, 0].Width();
            var tileHeight = map[0, 0].Height();

            var finalWidth = (tileWidth - 2) * tilesWide;
            var finalHeight = (tileHeight - 2) * tilesHigh;

            var result = new char[finalWidth, finalHeight];

            for (var y = 0; y < tilesHigh; y++)
            {
                for (var x = 0; x < tilesWide; x++)
                {
                    for (var yy = 1; yy < (tileHeight - 1); yy++)
                    {
                        for (var xx = 1; xx < (tileWidth - 1); xx++)
                        {
                            var fx = (x * (tileWidth - 2)) + xx - 1;
                            var fy = (y * (tileHeight - 2)) + yy - 1;

                            result[fx, fy] = map[x, y][xx, yy];
                        }
                    }
                }
            }

            return result;
        }

        private bool HasMatch(string compare, Dictionary<int, List<char[,]>> variations, int key)
        {
            foreach (var id in variations)
            {
                if (id.Key != key)
                {
                    foreach (var variation in id.Value)
                    {
                        if (GetTop(variation) == compare)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private string GetTop(char[,] value)
        {
            var top = string.Empty;

            for (var x = 0; x < value.Width(); x++)
            {
                top += value[x, 0];
            }

            return top;
        }

        private string GetBottom(char[,] value)
        {
            var bottom = string.Empty;

            for (var x = 0; x < value.Width(); x++)
            {
                bottom += value[x, value.GetUpperBound(1)];
            }

            return bottom;
        }

        private string GetLeft(char[,] value)
        {
            var left = string.Empty;

            for (var y = 0; y < value.Height(); y++)
            {
                left += value[0, y];
            }

            return left;
        }

        private string GetRight(char[,] value)
        {
            var right = string.Empty;

            for (var y = 0; y < value.Height(); y++)
            {
                right += value[value.GetUpperBound(0), y];
            }

            return right;
        }

        private Dictionary<int, List<char[,]>> GetVariations(Dictionary<int, char[,]> tiles)
        {
            var result = new Dictionary<int, List<char[,]>>();

            foreach (var tile in tiles)
            {
                var variations = new List<char[,]>();

                variations.Add(tile.Value);
                variations.Add(tile.Value.Rotate(1));
                variations.Add(tile.Value.Rotate(2));
                variations.Add(tile.Value.Rotate(3));

                variations.Add(tile.Value.FlipVertical());
                variations.Add(tile.Value.FlipVertical().Rotate(1));
                variations.Add(tile.Value.FlipVertical().Rotate(2));
                variations.Add(tile.Value.FlipVertical().Rotate(3));

                result.Add(tile.Key, variations);
            }

            return result;
        }
    }
}
