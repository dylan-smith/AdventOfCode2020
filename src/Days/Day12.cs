using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 12)]
    public class Day12 : BaseDay
    {
        public override string PartOne(string input)
        {
            var instructions = input.ParseLines(ParseInstruction).ToList();

            var pos = new Point(0, 0);
            var direction = Compass.East;

            foreach (var i in instructions)
            {
                pos = UpdatePos(i, pos, direction);
                direction = UpdateDirection(i, direction);
            }

            return pos.ManhattanDistance().ToString();
        }

        private Compass UpdateDirection((char movement, int value) i, Compass direction)
        {
            if (i.movement == 'L')
            {
                return IncrementDirectionCounterClockwise(direction, i.value / 90);
            }

            if (i.movement == 'R')
            {
                return IncrementDirectionClockwise(direction, i.value / 90);
            }

            return direction;
        }

        private Compass IncrementDirectionClockwise(Compass direction, int count)
        {
            var result = direction;

            for (var i = 0; i < count; i++)
            {
                result = result switch
                {
                    Compass.North => Compass.East,
                    Compass.East => Compass.South,
                    Compass.South => Compass.West,
                    Compass.West => Compass.North
                };
            }

            return result;
        }

        private Compass IncrementDirectionCounterClockwise(Compass direction, int count)
        {
            var result = direction;

            for (var i = 0; i < count; i++)
            {
                result = result switch
                {
                    Compass.North => Compass.West,
                    Compass.East => Compass.North,
                    Compass.South => Compass.East,
                    Compass.West => Compass.South
                };
            }

            return result;
        }

        private Point UpdatePos((char movement, int value) i, Point pos, Compass direction)
        {
            var result = pos;

            if (i.movement == 'F')
            {
                result = MoveForward(pos, direction, i.value);
            }

            if (i.movement == 'N')
            {
                result.Y += i.value;
            }

            if (i.movement == 'E')
            {
                result.X += i.value;
            }

            if (i.movement == 'S')
            {
                result.Y -= i.value;
            }

            if (i.movement == 'W')
            {
                result.X -= i.value;
            }

            return result;
        }

        private Point MoveForward(Point pos, Compass direction, int value)
        {
            if (direction == Compass.North)
            {
                return new Point(pos.X, pos.Y + value);
            }

            if (direction == Compass.East)
            {
                return new Point(pos.X + value, pos.Y);
            }

            if (direction == Compass.South)
            {
                return new Point(pos.X, pos.Y - value);
            }

            if (direction == Compass.West)
            {
                return new Point(pos.X - value, pos.Y);
            }

            throw new Exception();
        }

        private (char movement, int value) ParseInstruction(string line)
        {
            return (line[0], int.Parse(line.ShaveLeft(1)));
        }

        public override string PartTwo(string input)
        {
            var waypoint = new Point(10, 1);
            var instructions = input.ParseLines(ParseInstruction).ToList();

            var ship = new Point(0, 0);
            var direction = Compass.East;

            foreach (var i in instructions)
            {
                (ship, waypoint, direction) = ProcessInstruction(i, ship, waypoint, direction);
            }

            return ship.ManhattanDistance().ToString();
        }

        private (Point ship, Point waypoint, Compass direction) ProcessInstruction((char movement, int value) i, Point ship, Point waypoint, Compass direction)
        {
            switch (i.movement)
            {
                case 'N':
                    waypoint.Y += i.value;
                    break;
                case 'E':
                    waypoint.X += i.value;
                    break;
                case 'S':
                    waypoint.Y -= i.value;
                    break;
                case 'W':
                    waypoint.X -= i.value;
                    break;
                case 'F':
                    ship.X += waypoint.X * i.value;
                    ship.Y += waypoint.Y * i.value;
                    break;
                case 'L':
                    for (var rotate = 0; rotate < (i.value / 90); rotate++)
                    {
                        waypoint = new Point(-waypoint.Y, waypoint.X);
                    }
                    break;
                case 'R':
                    for (var rotate = 0; rotate < (i.value / 90); rotate++)
                    {
                        waypoint = new Point(waypoint.Y, -waypoint.X);
                    }
                    break;
            }

            return (ship, waypoint, direction);
        }
    }
}
