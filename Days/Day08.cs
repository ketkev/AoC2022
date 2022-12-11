using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AoC2022.utils;
using AoCHelper;

namespace AoC2022.Days;

internal class Tree
{
    public int Height;
    public bool Visible;

    public Tree(int height)
    {
        Height = height;
        Visible = true;
    }
}

public sealed class Day08 : BaseDay
{
    private List<List<Tree>> _map;
    private Vector2Int _bottomRight;

    public Day08()
    {
        var input = File.ReadAllLines(InputFilePath);

        _map = new List<List<Tree>>();

        foreach (var line in input)
        {
            var mapLine = new List<Tree>();

            foreach (var height in line)
            {
                mapLine.Add(new Tree(height - '0'));
            }

            _map.Add(mapLine);
        }

        _bottomRight = new Vector2Int(_map[0].Count - 1, _map.Count - 1);
    }

    public override ValueTask<string> Solve_1()
    {
        var visibleCount = 0;

        for (var y = 0; y < _map.Count; y++)
        {
            for (var x = 0; x < _map[y].Count; x++)
            {
                var height = _map[y][x].Height;

                var visibleT = CanSeeBetween(new Vector2Int(x, y), new Vector2Int(x, 0), height);
                var visibleR = CanSeeBetween(new Vector2Int(x, y), new Vector2Int(_bottomRight.x, y), height);
                var visibleB = CanSeeBetween(new Vector2Int(x, y), new Vector2Int(x, _bottomRight.y), height);
                var visibleL = CanSeeBetween(new Vector2Int(x, y), new Vector2Int(0, y), height);

                _map[y][x].Visible = visibleT || visibleR || visibleB || visibleL;

                if (_map[y][x].Visible)
                {
                    visibleCount++;
                }
            }
        }

        return new ValueTask<string>($"{visibleCount}");
    }

    public override ValueTask<string> Solve_2()
    {
        var highestScenicScore = 0L;

        for (var y = 0; y < _map.Count; y++)
        {
            for (var x = 0; x < _map[y].Count; x++)
            {
                var height = _map[y][x].Height;

                var viewDistanceT = ViewDistance(new Vector2Int(x, y), new Vector2Int(x, 0), height, true);
                var viewDistanceR = ViewDistance(new Vector2Int(x, y), new Vector2Int(_bottomRight.x, y), height, true);
                var viewDistanceB = ViewDistance(new Vector2Int(x, y), new Vector2Int(x, _bottomRight.y), height, true);
                var viewDistanceL = ViewDistance(new Vector2Int(x, y), new Vector2Int(0, y), height, true);

                var scenicScore = viewDistanceT * viewDistanceR * viewDistanceB * viewDistanceL;

                if (scenicScore > highestScenicScore)
                {
                    highestScenicScore = scenicScore;
                }
            }
        }

        return new ValueTask<string>($"{highestScenicScore}");
    }

    private int ViewDistance(Vector2Int from, Vector2Int to, int height, bool inclusive = false)
    {
        if (from == to)
            return 0;

        var curr = from;
        var steps = 0;

        var moveVec = new Vector2Int(
            to.x - from.x,
            to.y - from.y
        ).Normalize();

        while (curr != to)
        {
            curr += moveVec;

            if (inclusive)
                steps++;

            if (_map[curr.y][curr.x].Height >= height)
                break;

            if (!inclusive)
                steps++;
        }

        return steps;
    }

    private bool CanSeeBetween(Vector2Int from, Vector2Int to, int height)
    {
        var steps = ViewDistance(from, to, height);

        return steps == from.ManhattanDistance(to);
    }
}