using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 24)]
    public class Day24 : BaseDay
    {
        public override string PartOne(string input)
        {
            var tilePaths = input.ParseLines(x => ParseTilePath(x));
            var blackTiles = RenovateTiles(tilePaths);

            return blackTiles.Count.ToString();
        }

        private HashSet<(double x, double y)> RenovateTiles(IEnumerable<IEnumerable<string>> tilePaths)
        {
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

            return blackTiles;
        }

        private (double x, double y) FollowPath(IEnumerable<string> path)
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
            var tilePaths = input.ParseLines(x => ParseTilePath(x));
            var blackTiles = RenovateTiles(tilePaths);

            for (var i = 0; i < 100; i++)
            {
                blackTiles = ProcessDay(blackTiles);
            }

            return blackTiles.Count.ToString();
        }

        private IEnumerable<(double x, double y)> GetHexNeighbors((double x, double y) tile)
        {
            yield return (tile.x - 1, tile.y);
            yield return (tile.x + 1, tile.y);
            yield return (tile.x + 0.5, tile.y + 1);
            yield return (tile.x - 0.5, tile.y + 1);
            yield return (tile.x + 0.5, tile.y - 1);
            yield return (tile.x - 0.5, tile.y - 1);
        }

        private HashSet<(double x, double y)> ProcessDay(HashSet<(double x, double y)> blackTiles)
        {
            var result = new HashSet<(double x, double y)>();

            var allTiles = blackTiles.SelectMany(GetHexNeighbors).Concat(blackTiles);

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

        private bool ShouldFlipWhiteTile((double x, double y) tile, HashSet<(double x, double y)> blackTiles)
        {
            var adjacent = GetHexNeighbors(tile).Count(n => blackTiles.Contains(n));

            return adjacent == 2;
        }

        private bool ShouldFlipBlackTile((double x, double y) tile, HashSet<(double x, double y)> blackTiles)
        {
            var adjacent = GetHexNeighbors(tile).Count(n => blackTiles.Contains(n));

            return adjacent == 0 || adjacent > 2;
        }
    }
}
