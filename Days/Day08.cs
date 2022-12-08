using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2022.utils;
using AoCHelper;

namespace AoC2022.Days
{
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

                    var visibleT = IsVisibleFromTop(x, y, height);
                    var visibleR = IsVisibleFromRight(x, y, height);
                    var visibleB = IsVisibleFromBottom(x, y, height);
                    var visibleL = IsVisibleFromLeft(x, y, height);

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
            return new ValueTask<string>($"TODO");
        }

        private bool IsVisibleFromTop(int x, int y, int height)
        {
            var searchX = x;
            var searchY = 0;


            while (!(searchX == x && searchY == y))
            {
                if (_map[searchY][searchX].Height >= height)
                {
                    return false;
                }

                searchY++;
            }

            return true;
        }

        private bool IsVisibleFromRight(int x, int y, int height)
        {
            var searchX = _bottomRight.x;
            var searchY = y;


            while (!(searchX == x && searchY == y))
            {
                if (_map[searchY][searchX].Height >= height)
                {
                    return false;
                }

                searchX--;
            }

            return true;
        }

        private bool IsVisibleFromBottom(int x, int y, int height)
        {
            var searchX = x;
            var searchY = _bottomRight.y;


            while (!(searchX == x && searchY == y))
            {
                if (_map[searchY][searchX].Height >= height)
                {
                    return false;
                }

                searchY--;
            }

            return true;
        }

        private bool IsVisibleFromLeft(int x, int y, int height)
        {
            var searchX = 0;
            var searchY = y;


            while (!(searchX == x && searchY == y))
            {
                if (_map[searchY][searchX].Height >= height)
                {
                    return false;
                }

                searchX++;
            }

            return true;
        }
    }
}