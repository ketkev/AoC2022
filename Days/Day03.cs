using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoCHelper;

namespace AoC2022.Days
{
    public enum State
    {
        FirstOnly,
        SecondOnly,
        Both
    }
    
    public sealed class Day03 : BaseDay
    {
        private readonly List<String> _input;
        public Day03()
        {
            _input = File.ReadAllLines(InputFilePath).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            List<Dictionary<char, State>> items = new();
            
            foreach (var line in _input)
            {
                var first = line.Substring(0, line.Length / 2);
                var second = line.Substring(line.Length / 2);

                var dict = new Dictionary<char, State>();
                
                foreach (var c in first)
                {
                    if (!dict.ContainsKey(c))
                    {
                        dict.Add(c, State.FirstOnly);
                    }
                }
                
                foreach (var c in second)
                {
                    if (dict.ContainsKey(c) && dict[c] == State.FirstOnly)
                    {
                        dict[c] = State.Both;
                    }
                    else if (!dict.ContainsKey(c))
                    {
                        dict.Add(c, State.SecondOnly);
                    }
                }

                items.Add(dict);
            }
            
            var result = 0L;
            
            foreach (var dict in items)
            {
                foreach (var (c, state) in dict)
                {
                    if (state == State.Both)
                    {
                        result += calculateScore(c);
                    }
                }
            }

            return new ValueTask<string>($"{result}");
        }
        
        public override ValueTask<string> Solve_2()
        {
            
            List<char> badges = new List<char>();
            
            foreach (var strings in _input.Chunk(3))
            {
                var dict = new Dictionary<char, int>();
                
                var bags = strings.ToList();

                int i = 0;
                foreach (var bag in bags)
                {
                    i += 1;
                    
                    foreach (var c in bag)
                    {
                        if (i == 1 && !dict.ContainsKey(c))
                        {
                            dict.Add(c, i);
                        }
                        
                        else if (i != 1 && dict.ContainsKey(c))
                        {
                            if (dict[c] == i - 1)
                            {
                                dict[c] = i;
                            }
                        }
                    }
                }
                
                foreach (var (c, value) in dict)
                {
                    if (value == 3)
                    {
                        badges.Add(c);
                    }
                }
            }

            var result = badges.Select(calculateScore).Sum();

            return new ValueTask<string>($"{result}");
        }

        private long calculateScore(char c)
        {
            int ascii = c;
            
            if (char.IsLower(c))
            {
                ascii -= 96;
            }
            else
            {
                ascii -= 38;
            }
            
            return ascii;
        }
    }
}