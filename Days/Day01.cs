using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoCHelper;

namespace AoC2022.Days
{
    public sealed class Day01 : BaseDay
    {
        private readonly List<long> _elves;

        public Day01()
        {
            var input = File.ReadAllLines(InputFilePath);

            _elves = new();

            var sum = 0L;
            
            foreach (var line in input)
            {
                if (line.Length == 0)
                {
                    _elves.Add(sum);
                    sum = 0L;
                }
                else
                {
                    sum += long.Parse(line);
                }
            }
        }

        public override ValueTask<string> Solve_1()
        {
            return new ValueTask<string>($"{_elves.Max()}");
        }
        
        public override ValueTask<string> Solve_2()
        {
            _elves.Sort();
            _elves.Reverse();
            
            return new ValueTask<string>($"{_elves.Take(3).Sum()}");
        }
    }
}