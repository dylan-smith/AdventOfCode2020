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
            var orientations = GetAllOrientations(tiles);

            var corners = orientations.Where(tile => tile.Value.Take(4).Count(o => !HasMatchingSide(o.GetTopRow(), orientations, tile.Key)) == 2);

            return corners.Multiply(c => (long)c.Key).ToString();
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
            var map = BuildMap(tiles);
            var orientations = GetOrientations(map);

            foreach (var orientation in orientations)
            {
                var monsters = CountSeaMonsters(orientation);

                if (monsters > 0)
                {
                    var result = orientation.Count('#') - (monsters * 15);
                    return result.ToString();
                }
            }

            throw new Exception();
        }

        private Dictionary<int, List<char[,]>> GetAllOrientations(Dictionary<int, char[,]> tiles)
        {
            var result = new Dictionary<int, List<char[,]>>();

            foreach (var tile in tiles)
            {
                result.Add(tile.Key, GetOrientations(tile.Value).ToList());
            }

            return result;
        }

        private int CountSeaMonsters(char[,] map)
        {
            var result = 0;

            for (var y = 0; y < map.Height(); y++)
            {
                for (var x = 0; x < map.Width(); x++)
                {
                    if (CheckForSeaMonster(map, x, y))
                    {
                        result++;
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

        private IEnumerable<char[,]> GetOrientations(char[,] map)
        {
            yield return map;
            yield return map.RotateClockwise(1);
            yield return map.RotateClockwise(2);
            yield return map.RotateClockwise(3);

            yield return map.FlipVertical();
            yield return map.FlipVertical().RotateClockwise(1);
            yield return map.FlipVertical().RotateClockwise(2);
            yield return map.FlipVertical().RotateClockwise(3);
        }

        private char[,] BuildMap(Dictionary<int, char[,]> tiles)
        {
            var orientations = GetAllOrientations(tiles);
            var size = (int)Math.Sqrt(tiles.Count);
            var map = new char[size, size][,];

            var (topLeftCornerId, topLeftMap) = FindTopLeftCorner(orientations);
            map[0, 0] = topLeftMap;
            orientations.Remove(topLeftCornerId);

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    if (x > 0 || y > 0)
                    {
                        var (id, tile) = FindMapTile(x, y, map, orientations);
                        map[x, y] = tile;
                        orientations.Remove(id);
                    }
                }
            }

            return RemoveBorders(map);
        }

        private (int id, char[,] tile) FindMapTile(int x, int y, char[,][,] map, Dictionary<int, List<char[,]>> orientations)
        {
            var matchLeft = string.Empty;
            var matchTop = string.Empty;

            // top row
            if (y == 0)
            {
                matchLeft = map[x - 1, y].GetRightColumn();
                matchTop = "none";
            }

            // left column
            if (x == 0)
            {
                matchTop = map[x, y - 1].GetBottomRow();
                matchLeft = "none";
            }

            // everything else
            if (y > 0 && x > 0)
            {
                matchTop = map[x, y - 1].GetBottomRow();
                matchLeft = map[x - 1, y].GetRightColumn();
            }

            foreach (var id in orientations)
            {
                foreach (var orientation in id.Value)
                {
                    if (IsMatch(orientation, matchLeft, matchTop, orientations, id.Key))
                    {
                        return (id.Key, orientation);
                    }
                }
            }

            throw new Exception();
        }

        private bool IsMatch(char[,] orientation, string matchLeft, string matchTop, Dictionary<int, List<char[,]>> orientations, int id)
        {
            if (matchLeft == "none")
            {
                if (HasMatchingSide(orientation.GetLeftColumn(), orientations, id))
                {
                    return false;
                }
            }

            if (matchTop == "none")
            {
                if (HasMatchingSide(orientation.GetTopRow(), orientations, id))
                {
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(matchLeft) && matchLeft != "none")
            {
                if (orientation.GetLeftColumn() != matchLeft)
                {
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(matchTop) && matchTop != "none")
            {
                if (orientation.GetTopRow() != matchTop)
                {
                    return false;
                }
            }

            return true;
        }

        private (int id, char[,] orientation) FindTopLeftCorner(Dictionary<int, List<char[,]>> orientations)
        {
            foreach (var tile in orientations)
            {
                foreach (var orientation in tile.Value)
                {
                    if (!HasMatchingSide(orientation.GetTopRow(), orientations, tile.Key) && 
                        !HasMatchingSide(orientation.GetLeftColumn(), orientations, tile.Key))
                    {
                        return (tile.Key, orientation);
                    }
                }
            }

            throw new Exception();
        }

        private char[,] RemoveBorders(char[,][,] map)
        {
            var finalWidth = (map[0, 0].Width() - 2) * map.Width();
            var finalHeight = (map[0, 0].Height() - 2) * map.Height();

            var result = new char[finalWidth, finalHeight];

            for (var y = 0; y < map.Height(); y++)
            {
                for (var x = 0; x < map.Width(); x++)
                {
                    for (var yy = 1; yy < (map[x, y].Height() - 1); yy++)
                    {
                        for (var xx = 1; xx < (map[x, y].Width() - 1); xx++)
                        {
                            var fx = (x * (map[x, y].Width() - 2)) + xx - 1;
                            var fy = (y * (map[x, y].Height() - 2)) + yy - 1;

                            result[fx, fy] = map[x, y][xx, yy];
                        }
                    }
                }
            }

            return result;
        }

        private bool HasMatchingSide(string side, Dictionary<int, List<char[,]>> orientations, int id)
        {
            foreach (var tile in orientations)
            {
                if (tile.Key != id)
                {
                    foreach (var orientation in tile.Value)
                    {
                        if (orientation.GetTopRow() == side)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
