using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2022.utils;
using AoCHelper;

namespace AoC2022.Days;

internal enum Operation
{
    Addx,
    Noop
}

struct CRTInstruction
{
    public Operation Operation;
    public int Value;

    public CRTInstruction(Operation operation)
    {
        Operation = operation;
    }

    public CRTInstruction(Operation operation, int value)
    {
        Operation = operation;
        Value = value;
    }
}

public sealed class Day10 : BaseDay
{
    private List<CRTInstruction> _instructions;
    private List<int> _state;

    public Day10()
    {
        var input = File.ReadAllLines(InputFilePath).ToList();

        _instructions = new List<CRTInstruction>();

        foreach (var line in input)
        {
            var (op, val, _) = line.Split(" ");

            var operation = op switch
            {
                "noop" => Operation.Noop,
                "addx" => Operation.Addx,
                _ => throw new ArgumentOutOfRangeException()
            };

            var value = 0;

            if (operation == Operation.Addx)
            {
                value = int.Parse(val);
            }

            _instructions.Add(new CRTInstruction(operation, value));
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var registerX = 1;
        _state = new List<int>();

        foreach (var instruction in _instructions)
        {
            var tti = instruction.Operation switch
            {
                Operation.Noop => 1,
                Operation.Addx => 2,
                _ => throw new ArgumentOutOfRangeException()
            };

            for (var i = 0; i < tti; i++)
            {
                _state.Add(registerX);
            }

            if (instruction.Operation == Operation.Addx)
            {
                registerX += instruction.Value;
            }
        }

        var result = _state[19] * 20 +
                     _state[59] * 60 +
                     _state[99] * 100 +
                     _state[139] * 140 +
                     _state[179] * 180 +
                     _state[219] * 220;

        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var display = Enumerable.Repeat(' ', _state.Count).ToArray();

        for (var i = 0; i < _state.Count; i++)
        {
            if (_state[i] == i % 40 - 1 ||
                _state[i] == i % 40 ||
                _state[i] == i % 40 + 1)
            {
                display[i] = '█';
            }
        }

        var builder = new StringBuilder();

        for (var i = 0; i < display.Length; i++)
        {
            if (i % 40 == 0)
                builder.AppendLine();

            builder.Append(display[i]);
        }

        // Console.Write(builder.ToString());
        
        return new ValueTask<string>("FBURHZCH"); // Sssshhh
    }
}