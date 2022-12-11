using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2022.utils;
using AoCHelper;

namespace AoC2022.Days;

public sealed class Day03 : BaseDay
{
    private readonly List<String> _input;
    public Day03()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var result = 0L;
            
        foreach (var bag in _input)
        {
            var first = bag.Substring(0, bag.Length / 2);
            var second = bag.Substring(bag.Length / 2);

            var intersect = first.Intersect(second).First();

            result += CalculateScore(intersect);
        }

        return new ValueTask<string>($"{result}");
    }
        
    public override ValueTask<string> Solve_2()
    {

        var result = 0L;
            
        foreach (var (bagOne, bagTwo, bagThree, _) in _input.Chunk(3))
        {
            var badge = bagOne.Intersect(bagTwo).Intersect(bagThree).First();

            result += CalculateScore(badge);
        }

        return new ValueTask<string>($"{result}");
    }

    private long CalculateScore(char c)
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