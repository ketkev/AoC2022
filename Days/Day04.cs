using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2022.utils;
using AoCHelper;

namespace AoC2022.Days
{
    struct Section
    {
        public long From;
        public long To;

        public Section(string from, string to)
        {
            From = long.Parse(from);
            To = long.Parse(to);
        }
    }
    
    public sealed class Day04 : BaseDay
    {
        private readonly List<(Section, Section)> _pairs;

        public Day04()
        {
            var input = File.ReadAllLines(InputFilePath).ToList();

            _pairs = new List<(Section, Section)>();
            
            foreach (var line in input)
            {
                var (first, second, _) = line.Split(",");

                var (firstFrom, firstTo, _) = first.Split("-");
                var (secondFrom, secondTo, _) = second.Split("-");
                
                _pairs.Add((new Section(firstFrom, firstTo), new Section(secondFrom, secondTo)));
            }
        }

        public override ValueTask<string> Solve_1()
        {
            var count = 0L;
            
            foreach (var (first, second) in _pairs)
            {
                if (ContainsOther(first, second) || ContainsOther(second, first))
                {
                    count++;
                }
            }
            
            return new ValueTask<string>($"{count}");
        }

        public override ValueTask<string> Solve_2()
        {
            var count = 0L;

            foreach (var (first, second) in _pairs)
            {
                if (Overlaps(first, second) || Overlaps(second, first))
                {
                    count++;
                }
            }
            
            return new ValueTask<string>($"{count}");
        }

        private static bool ContainsOther(Section a, Section b)
        {
            return a.From <= b.From && a.To >= b.To;
        }

        private static bool Overlaps(Section a, Section b)
        {
            return (a.From <= b.From && a.To >= b.From) 
                   || (a.To >= b.To && a.From <= b.To)
                   || ContainsOther(a, b);
        }
    }
}