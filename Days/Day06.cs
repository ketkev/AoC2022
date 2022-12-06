using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2022.utils;
using AoCHelper;

namespace AoC2022.Days
{
    public sealed class Day06 : BaseDay
    {
        private string _input;

        public Day06()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            var position = 4;

            for (; position < _input.Length; position++)
            {
                if (isUnique(_input.Substring(position - 4, 4)))
                    break;
            }

            return new ValueTask<string>($"{position}");
        }

        private bool isUnique(string input)
        {
            var seen = new List<char>();

            foreach (var c in input)
            {
                if (seen.Contains(c))
                {
                    return false;
                }

                seen.Add(c);
            }

            return true;
        }

        public override ValueTask<string> Solve_2()
        {
            var position = 14;

            for (; position < _input.Length; position++)
            {
                if (isUnique(_input.Substring(position - 14, 14)))
                    break;
            }

            return new ValueTask<string>($"{position}");
        }
    }
}