using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2022.utils;
using AoCHelper;

namespace AoC2022.Days;

struct Instruction
{
    public long Count;
    public int From;
    public int To;

    public Instruction(long count, int from, int to)
    {
        Count = count;
        From = from;
        To = to;
    }
}

public sealed class Day05 : BaseDay
{
    private List<Stack<char>> _stacks;
    private List<string> _input;
    private List<Instruction> _instructions;

    public Day05()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
        _instructions = new List<Instruction>();

        var regex = new RegexHelper(@"move (\d+) from (\d+) to (\d+)");

        foreach (var line in _input)
        {
            var matches = regex.GetMatches(line);

            var (_, count, from, to, _) = matches.First().Groups.Values;

            _instructions.Add(new Instruction(
                long.Parse(count.Value),
                int.Parse(from.Value),
                int.Parse(to.Value)
            ));
        }

        CreateStacks();
    }

    private void CreateStacks()
    {
        _stacks = new List<Stack<char>>
        {
            new(new[] { 'S', 'Z', 'P', 'D', 'L', 'B', 'F', 'C' }),
            new(new[] { 'N', 'V', 'G', 'P', 'H', 'W', 'B' }),
            new(new[] { 'F', 'W', 'B', 'J', 'G' }),
            new(new[] { 'G', 'J', 'N', 'F', 'L', 'W', 'C', 'S' }),
            new(new[] { 'W', 'J', 'L', 'T', 'P', 'M', 'S', 'H' }),
            new(new[] { 'B', 'C', 'W', 'G', 'F', 'S' }),
            new(new[] { 'H', 'T', 'P', 'M', 'Q', 'B', 'W' }),
            new(new[] { 'F', 'S', 'W', 'T' }),
            new(new[] { 'N', 'C', 'R' })
        };
    }

    public override ValueTask<string> Solve_1()
    {
        foreach (var instruction in _instructions)
        {
            for (int i = 0; i < instruction.Count; i++)
            {
                _stacks[instruction.To - 1].Push(_stacks[instruction.From - 1].Pop());
            }
        }

        var result = new string(_stacks.Select(stack => stack.Peek()).ToArray());

        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        CreateStacks();
            
        var holding = new Stack<char>();

        foreach (var instruction in _instructions)
        {
            for (int i = 0; i < instruction.Count; i++)
            {
                holding.Push(_stacks[instruction.From - 1].Pop());
            }

            while (holding.Count != 0)
            {
                _stacks[instruction.To - 1].Push(holding.Pop());
            }
        }

        var result = new string(_stacks.Select(stack => stack.Peek()).ToArray());

        return new ValueTask<string>($"{result}");
    }
}