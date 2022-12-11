using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2022.utils;
using AoCHelper;

namespace AoC2022.Days;

internal struct Step
{
    public Direction Direction;
    public int Distance;

    public Step(Direction direction, int distance)
    {
        Direction = direction;
        Distance = distance;
    }
}

public sealed class Day09 : BaseDay
{
    private List<Step> _steps;

    public Day09()
    {
        var input = File.ReadAllLines(InputFilePath);

        _steps = new List<Step>();

        foreach (var step in input)
        {
            var (dist, distance, _) = step.Split(" ");

            var direction = dist switch
            {
                "U" => Direction.Up,
                "R" => Direction.Right,
                "D" => Direction.Down,
                "L" => Direction.Left,
                _ => throw new ArgumentOutOfRangeException()
            };

            _steps.Add(new Step(direction, int.Parse(distance)));
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var head = new Vector2Int(0, 0);
        var tail = new Vector2Int(0, 0);

        var visitedPositions = new HashSet<Vector2Int>();

        foreach (var step in _steps)
        {
            for (int i = 0; i < step.Distance; i++)
            {
                var moveVec = step.Direction switch
                {
                    Direction.Up => Vector2Int.Up(),
                    Direction.Right => Vector2Int.Right(),
                    Direction.Down => Vector2Int.Down(),
                    Direction.Left => Vector2Int.Left(),
                    _ => throw new ArgumentOutOfRangeException()
                };

                head += moveVec;

                tail = FixTail(head, tail);

                visitedPositions.Add(tail);
            }
        }

        return new ValueTask<string>($"{visitedPositions.Count}");
    }

    public override ValueTask<string> Solve_2()
    {
        var visitedPositions = new HashSet<Vector2Int>();

        List<Vector2Int> rope = Enumerable.Repeat(new Vector2Int(0, 0), 10).ToList();

        foreach (var step in _steps)
        {
            for (int i = 0; i < step.Distance; i++)
            {
                var moveVec = step.Direction switch
                {
                    Direction.Up => Vector2Int.Up(),
                    Direction.Right => Vector2Int.Right(),
                    Direction.Down => Vector2Int.Down(),
                    Direction.Left => Vector2Int.Left(),
                    _ => throw new ArgumentOutOfRangeException()
                };

                rope[0] += moveVec;

                for (var segment = 1; segment < rope.Count; segment++)
                {
                    rope[segment] = FixTail(rope[segment - 1], rope[segment]);
                }


                visitedPositions.Add(rope.Last());
            }
        }

        return new ValueTask<string>($"{visitedPositions.Count}");
    }

    private static Vector2Int FixTail(Vector2Int head, Vector2Int tail)
    {
        if (IsTouching(head, tail))
            return tail;

        var moveVec = new Vector2Int(
            int.Clamp(head.x - tail.x, -1, 1),
            int.Clamp(head.y - tail.y, -1, 1)
        );

        return tail + moveVec;
    }

    private static bool IsTouching(Vector2Int head, Vector2Int tail)
    {
        return int.Abs(head.x - tail.x) <= 1 && int.Abs(head.y - tail.y) <= 1;
    }
}