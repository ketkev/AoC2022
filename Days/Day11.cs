using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using AoCHelper;

namespace AoC2022.Days;

class Monkey
{
    public static long LCD = 0;
    private LinkedList<long> _items;
    public long InspectionCount { get; private set; }
    private Func<long, long> _operation;
    public readonly int Test;
    public int ThrowToMonkeyWhenTrue { get; }
    public int ThrowToMonkeyWhenFalse { get; }

    public Monkey(LinkedList<long> items, Func<long, long> operation, int test,
        int throwToMonkeyWhenTrue,
        int throwToMonkeyWhenFalse)
    {
        _items = items;
        _operation = operation;
        Test = test;
        ThrowToMonkeyWhenTrue = throwToMonkeyWhenTrue;
        ThrowToMonkeyWhenFalse = throwToMonkeyWhenFalse;

        InspectionCount = 0;
    }

    public bool Inspect()
    {
        InspectionCount++;

        var val = _items.First!.Value;

        val = _operation(val);
        val /= 3;

        _items.First.Value = val;

        return val % Test == 0;
    }

    public bool InspectWhileWorried()
    {
        InspectionCount++;

        var val = _items.First!.Value;

        val = _operation(val);

        _items.First.Value = val;

        return val % Test == 0;
    }

    public bool HasItem()
    {
        return _items.Count != 0;
    }

    public void ReduceStress()
    {
        var num = _items.First.Value / LCD;
        _items.First.Value -= num * LCD;
    }
    
    public long Throw()
    {
        ReduceStress();

        var val = _items.First.Value;

        _items.RemoveFirst();

        return val;
    }

    public void Catch(long item)
    {
        _items.AddLast(item);
    }
}

public sealed class Day11 : BaseDay
{
    private List<Monkey> _monkeys;

    public Day11()
    {
        InitialiseMonkeys();
    }

    private void InitialiseMonkeys()
    {
        // _monkeys = new List<Monkey>
        // {
        //     new(
        //         new LinkedList<long>(new long[] { 79, 98 }),
        //         item => item * 19,
        //         23,
        //         2,
        //         3
        //     ),
        //     new(
        //         new LinkedList<long>(new long[] { 54, 65, 75, 74 }),
        //         item => item + 6,
        //         19,
        //         2,
        //         0
        //     ),
        //     new(
        //         new LinkedList<long>(new long[] { 79, 60, 97 }),
        //         item => item * item,
        //         13,
        //         1,
        //         3
        //     ),
        //     new(
        //         new LinkedList<long>(new long[] { 74 }),
        //         item => item + 3,
        //         17,
        //         0,
        //         1
        //     )
        // };

        _monkeys = new List<Monkey>
        {
            new(
                new LinkedList<long>(new long[] { 89, 73, 66, 57, 64, 80 }),
                item => item * 3,
                13,
                6,
                2
            ),
            new(
                new LinkedList<long>(new long[] { 83, 78, 81, 55, 81, 59, 69 }),
                item => item + 1,
                3,
                7,
                4
            ),
            new(
                new LinkedList<long>(new long[] { 76, 91, 58, 85 }),
                item => item * 13,
                7,
                1,
                4
            ),
            new(
                new LinkedList<long>(new long[] { 71, 72, 74, 76, 68 }),
                item => item * item,
                2,
                6,
                0
            ),
            new(
                new LinkedList<long>(new long[] { 98, 85, 84 }),
                item => item + 7,
                19,
                5,
                7
            ),
            new(
                new LinkedList<long>(new long[] { 78 }),
                item => item + 8,
                5,
                3,
                0
            ),
            new(
                new LinkedList<long>(new long[] { 86, 70, 60, 88, 88, 78, 74, 83 }),
                item => item + 4,
                11,
                1,
                2
            ),
            new(
                new LinkedList<long>(new long[] { 81, 58 }),
                item => item + 5,
                17,
                3,
                5
            )
        };

        var lcd = _monkeys.Aggregate(1, (current, monkey) => current * monkey.Test);

        Monkey.LCD = lcd;
    }

    public override ValueTask<string> Solve_1()
    {
        InitialiseMonkeys();

        var roundsToDo = 20;

        for (var round = 0; round < roundsToDo; round++)
        {
            foreach (var monkey in _monkeys)
            {
                while (monkey.HasItem())
                {
                    if (monkey.Inspect())
                    {
                        _monkeys[monkey.ThrowToMonkeyWhenTrue].Catch(monkey.Throw());
                    }
                    else
                    {
                        _monkeys[monkey.ThrowToMonkeyWhenFalse].Catch(monkey.Throw());
                    }
                }
            }
        }

        var busiestMonkeys = _monkeys.OrderByDescending(monkey => monkey.InspectionCount).Take(2).ToList();
        var monkeyBusiness = busiestMonkeys[0].InspectionCount * busiestMonkeys[1].InspectionCount;

        return new ValueTask<string>($"{monkeyBusiness}");
    }

    public override ValueTask<string> Solve_2()
    {
        InitialiseMonkeys();

        var roundsToDo = 10_000;

        for (var round = 0; round < roundsToDo; round++)
        {
            foreach (var monkey in _monkeys)
            {
                while (monkey.HasItem())
                {
                    if (monkey.InspectWhileWorried())
                    {
                        _monkeys[monkey.ThrowToMonkeyWhenTrue].Catch(monkey.Throw());
                    }
                    else
                    {
                        _monkeys[monkey.ThrowToMonkeyWhenFalse].Catch(monkey.Throw());
                    }
                }
            }
        }

        var busiestMonkeys = _monkeys.OrderByDescending(monkey => monkey.InspectionCount).Take(2).ToList();
        var monkeyBusiness = busiestMonkeys[0].InspectionCount * busiestMonkeys[1].InspectionCount;

        return new ValueTask<string>($"{monkeyBusiness}");
    }
}