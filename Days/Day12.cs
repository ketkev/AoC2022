using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2022.utils;
using AoC2022.utils.graph;
using AoCHelper;

namespace AoC2022.Days;

public sealed class Day12 : BaseDay
{
    private List<string> _input;
    private Graph _graph;

    private Vector2Int _start;
    private Vector2Int _goal;

    private List<Vector2Int> _lowPoints;

    public Day12()
    {
        _graph = new Graph();
        _input = File.ReadAllLines(InputFilePath).ToList();
        _lowPoints = new List<Vector2Int>();

        var directions = new List<Vector2Int>
        {
            Vector2Int.Up(),
            Vector2Int.Right(),
            Vector2Int.Down(),
            Vector2Int.Left()
        };

        for (var y = 0; y < _input.Count; y++)
        {
            for (var x = 0; x < _input[0].Length; x++)
            {
                var pos = new Vector2Int(x, y);

                if (_input[y][x] == 'S')
                {
                    _start = pos;
                    _lowPoints.Add(pos);
                }

                if (_input[y][x] == 'E')
                {
                    _goal = pos;
                }

                if (_input[y][x] == 'a')
                {
                    _lowPoints.Add(pos);
                }

                _graph.AddVertex(pos.ToString());

                foreach (var direction in directions)
                {
                    var dest = pos + direction;

                    if (IsConnected(pos, dest))
                    {
                        _graph.AddEdge(pos.ToString(), dest.ToString());
                    }
                }
            }
        }
    }

    private bool IsConnected(Vector2Int from, Vector2Int to)
    {
        if (to.x < 0 || to.x >= _input[0].Length ||
            to.y < 0 || to.y >= _input.Count)
            return false;

        return GetHeight(_input[to.y][to.x]) - GetHeight(_input[from.y][from.x]) <= 1;
    }

    private int GetHeight(char c)
    {
        switch (c)
        {
            case 'S':
                return 0;
            case 'E':
                return 25;
            default:
            {
                var pos = "abcdefghijklmnopqrstuvwxyz";
                return pos.IndexOf(c);
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        _graph.Dijkstra(_start.ToString());

        var steps = _graph.GetVertex(_goal.ToString()).GetDistance();

        return new ValueTask<string>($"{steps}");
    }

    public override ValueTask<string> Solve_2()
    {
        var smallest = double.MaxValue;

        foreach (var point in _lowPoints)
        {
            _graph.ClearAll();
            _graph.Dijkstra(point.ToString());
            var steps = _graph.GetVertex(_goal.ToString()).GetDistance();

            if (steps < smallest)
            {
                smallest = steps;
            }
        }

        return new ValueTask<string>($"{smallest}");
    }
}