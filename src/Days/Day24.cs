using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 24)]
    public class Day24 : BaseDay
    {
        public override string PartOne(string input)
        {
            var tilePaths = input.ParseLines(x => ParseTilePath(x).ToList()).ToList();

            var blackTiles = new HashSet<(double x, double y)>();

            foreach (var path in tilePaths)
            {
                var pos = FollowPath(path);

                if (blackTiles.Contains(pos))
                {
                    blackTiles.Remove(pos);
                }
                else
                {
                    blackTiles.Add(pos);
                }
            }

            return blackTiles.Count.ToString();
        }

        private (double x, double y) FollowPath(List<string> path)
        {
            var pos = (x: 0.0, y: 0.0);

            foreach (var dir in path)
            {
                if (dir == "e")
                {
                    pos = (pos.x + 1, pos.y);
                }

                if (dir == "w")
                {
                    pos = (pos.x - 1, pos.y);
                }

                if (dir == "ne")
                {
                    pos = (pos.x + 0.5, pos.y + 1);
                }

                if (dir == "nw")
                {
                    pos = (pos.x - 0.5, pos.y + 1);
                }

                if (dir == "se")
                {
                    pos = (pos.x + 0.5, pos.y - 1);
                }

                if (dir == "sw")
                {
                    pos = (pos.x - 0.5, pos.y - 1);
                }
            }

            return pos;
        }

        private IEnumerable<string> ParseTilePath(string line)
        {
            var pos = 0;

            while (pos < line.Length)
            {
                switch (line[pos])
                {
                    case 'e':
                        pos++;
                        yield return "e";
                        break;
                    case 'w':
                        pos++;
                        yield return "w";
                        break;
                    case 'n':
                        if (line[pos + 1] == 'e')
                        {
                            yield return "ne";
                        }
                        else
                        {
                            yield return "nw";
                        }

                        pos += 2;
                        break;
                    case 's':
                        if (line[pos + 1] == 'e')
                        {
                            yield return "se";
                        }
                        else
                        {
                            yield return "sw";
                        }

                        pos += 2;
                        break;
                }
            }
        }

        public override string PartTwo(string input)
        {
            var tilePaths = input.ParseLines(x => ParseTilePath(x).ToList()).ToList();

            var blackTiles = new HashSet<(double x, double y)>();

            foreach (var path in tilePaths)
            {
                var pos = FollowPath(path);

                if (blackTiles.Contains(pos))
                {
                    blackTiles.Remove(pos);
                }
                else
                {
                    blackTiles.Add(pos);
                }
            }

            for (var i = 0; i < 100; i++)
            {
                blackTiles = ProcessDay(blackTiles);
            }

            return blackTiles.Count.ToString();
        }

        private HashSet<(double x, double y)> ProcessDay(HashSet<(double x, double y)> blackTiles)
        {
            var result = new HashSet<(double x, double y)>();

            var allTiles = GetAllTiles(blackTiles).ToList();

            foreach (var tile in allTiles)
            {
                if (blackTiles.Contains(tile))
                {
                    if (!ShouldFlipBlackTile(tile, blackTiles))
                    {
                        result.Add(tile);
                    }
                }
                else
                {
                    if (ShouldFlipWhiteTile(tile, blackTiles))
                    {
                        result.Add(tile);
                    }
                }
            }

            return result;
        }

        private IEnumerable<(double x, double y)> GetAllTiles(HashSet<(double x, double y)> blackTiles)
        {
            foreach (var tile in blackTiles)
            {
                yield return tile;

                yield return (tile.x - 1, tile.y);
                yield return (tile.x + 1, tile.y);
                yield return (tile.x + 0.5, tile.y + 1);
                yield return (tile.x - 0.5, tile.y + 1);
                yield return (tile.x + 0.5, tile.y - 1);
                yield return (tile.x - 0.5, tile.y - 1);
            }
        }

        private bool ShouldFlipWhiteTile((double x, double y) tile, HashSet<(double x, double y)> blackTiles)
        {
            var adjacent = 0;

            if (blackTiles.Contains((tile.x - 1, tile.y)))
            {
                adjacent++;
            }

            if (blackTiles.Contains((tile.x + 1, tile.y)))
            {
                adjacent++;
            }

            if (blackTiles.Contains((tile.x + 0.5, tile.y + 1)))
            {
                adjacent++;
            }

            if (blackTiles.Contains((tile.x - 0.5, tile.y + 1)))
            {
                adjacent++;
            }

            if (blackTiles.Contains((tile.x + 0.5, tile.y - 1)))
            {
                adjacent++;
            }

            if (blackTiles.Contains((tile.x - 0.5, tile.y - 1)))
            {
                adjacent++;
            }

            return adjacent == 2;
        }

        private bool ShouldFlipBlackTile((double x, double y) tile, HashSet<(double x, double y)> blackTiles)
        {
            var adjacent = 0;

            if (blackTiles.Contains((tile.x - 1, tile.y)))
            {
                adjacent++;
            }

            if (blackTiles.Contains((tile.x + 1, tile.y)))
            {
                adjacent++;
            }

            if (blackTiles.Contains((tile.x + 0.5, tile.y + 1)))
            {
                adjacent++;
            }

            if (blackTiles.Contains((tile.x - 0.5, tile.y + 1)))
            {
                adjacent++;
            }

            if (blackTiles.Contains((tile.x + 0.5, tile.y - 1)))
            {
                adjacent++;
            }

            if (blackTiles.Contains((tile.x - 0.5, tile.y - 1)))
            {
                adjacent++;
            }

            return adjacent == 0 || adjacent > 2;
        }
    }
}
