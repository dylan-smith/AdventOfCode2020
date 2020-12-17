using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 17)]
    public class Day17 : BaseDay
    {
        public override string PartOne(string input)
        {
            var grid = CreateGrid3D(input);

            for (var i = 0; i < 6; i++)
            {
                grid = CycleGrid3D(grid);
            }
            
            return grid.Count.ToString();
        }

        private HashSet<Point3D> CreateGrid3D(string input)
        {
            var rows = input.Lines().ToList();
            var result = new HashSet<Point3D>();

            for (var y = 0; y < rows.Count; y++)
            {
                for (var x = 0; x < rows[y].Length; x++)
                {
                    if (rows[y][x] == '#')
                    {
                        result.Add(new Point3D(x, y, 0));
                    }
                }
            }

            return result;
        }

        private HashSet<Point4D> CreateGrid4D(string input)
        {
            var rows = input.Lines().ToList();
            var result = new HashSet<Point4D>();

            for (var y = 0; y < rows.Count; y++)
            {
                for (var x = 0; x < rows[y].Length; x++)
                {
                    if (rows[y][x] == '#')
                    {
                        result.Add(new Point4D(x, y, 0, 0));
                    }
                }
            }

            return result;
        }

        private HashSet<Point3D> CycleGrid3D(HashSet<Point3D> grid)
        {
            var result = new HashSet<Point3D>();
            var cubes = GetAllCubes3D(grid);

            foreach (var cube in cubes)
            {
                var neighbors = cube.GetNeighbors(includeDiagonals: true);
                var activeNeighbors = neighbors.Count(n => grid.Contains(n));

                if (grid.Contains(cube) && (activeNeighbors == 2 || activeNeighbors == 3))
                {
                    result.Add(cube);
                }

                if (!grid.Contains(cube) && activeNeighbors == 3)
                {
                    result.Add(cube);
                }
            }

            return result;
        }

        private HashSet<Point4D> CycleGrid4D(HashSet<Point4D> grid)
        {
            var result = new HashSet<Point4D>();
            var cubes = GetAllCubes4D(grid);

            foreach (var cube in cubes)
            {
                var neighbors = cube.GetNeighbors(includeDiagonals: true);
                var activeNeighbors = neighbors.Count(n => grid.Contains(n));

                if (grid.Contains(cube) && (activeNeighbors == 2 || activeNeighbors == 3))
                {
                    result.Add(cube);
                }

                if (!grid.Contains(cube) && activeNeighbors == 3)
                {
                    result.Add(cube);
                }
            }

            return result;
        }

        private HashSet<Point3D> GetAllCubes3D(HashSet<Point3D> grid)
        {
            var result = new HashSet<Point3D>();

            foreach (var cube in grid)
            {
                result.Add(cube);
                result.AddRange(cube.GetNeighbors(includeDiagonals: true));
            }

            return result;
        }

        private HashSet<Point4D> GetAllCubes4D(HashSet<Point4D> grid)
        {
            var result = new HashSet<Point4D>();

            foreach (var cube in grid)
            {
                result.Add(cube);
                result.AddRange(cube.GetNeighbors(includeDiagonals: true));
            }

            return result;
        }

        public override string PartTwo(string input)
        {
            var grid = CreateGrid4D(input);

            for (var i = 0; i < 6; i++)
            {
                grid = CycleGrid4D(grid);
            }

            return grid.Count.ToString();
        }
    }
}
