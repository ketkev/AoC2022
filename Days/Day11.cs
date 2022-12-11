using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using AoCHelper;

namespace AoC2022.Days;

class Monkey
{
    private LinkedList<BigInteger> _items;
    public long InspectionCount { get; private set; }
    private Func<BigInteger, BigInteger> _operation;
    private Predicate<BigInteger> _test;
    public int ThrowToMonkeyWhenTrue { get; }
    public int ThrowToMonkeyWhenFalse { get; }

    public Monkey(LinkedList<BigInteger> items, Func<BigInteger, BigInteger> operation, Predicate<BigInteger> test, int throwToMonkeyWhenTrue,
        int throwToMonkeyWhenFalse)
    {
        _items = items;
        _operation = operation;
        _test = test;
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

        return _test(val);
    }
    
    public bool InspectWhileWorried()
    {
        InspectionCount++;

        var val = _items.First!.Value;

        val = _operation(val);

        _items.First.Value = val;

        return _test(val);
    }

    public bool HasItem()
    {
        return _items.Count != 0;
    }

    public BigInteger Throw()
    {
        var val = _items.First.Value;

        _items.RemoveFirst();

        return val;
    }

    public void Catch(BigInteger item)
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
        _monkeys = new List<Monkey>
        {
            new(
                new LinkedList<BigInteger>(new BigInteger[] { 79, 98 }),
                item => item * 19,
                item => item % 23 == 0,
                2,
                3
            ),
            new(
                new LinkedList<BigInteger>(new BigInteger[] { 54, 65, 75, 74 }),
                item => item + 6,
                item => item % 19 == 0,
                2,
                0
            ),
            new(
                new LinkedList<BigInteger>(new BigInteger[] { 79, 60, 97 }),
                item => item * item,
                item => item % 13 == 0,
                1,
                3
            ),
            new(
                new LinkedList<BigInteger>(new BigInteger[] { 74 }),
                item => item + 3,
                item => item % 17 == 0,
                0,
                1
            )
        };

        // _monkeys = new List<Monkey>
        // {
        //     new(
        //         new LinkedList<BigInteger>(new BigInteger[] { 89, 73, 66, 57, 64, 80 }),
        //         item => item * 3,
        //         item => item % 13 == 0,
        //         6,
        //         2
        //     ),
        //     new(
        //         new LinkedList<BigInteger>(new BigInteger[] { 83, 78, 81, 55, 81, 59, 69 }),
        //         item => item + 1,
        //         item => item % 3 == 0,
        //         7,
        //         4
        //     ),
        //     new(
        //         new LinkedList<BigInteger>(new BigInteger[] { 76, 91, 58, 85 }),
        //         item => item * 13,
        //         item => item % 7 == 0,
        //         1,
        //         4
        //     ),
        //     new(
        //         new LinkedList<BigInteger>(new BigInteger[] { 71, 72, 74, 76, 68 }),
        //         item => item * item,
        //         item => item % 2 == 0,
        //         6,
        //         0
        //     ),
        //     new(
        //         new LinkedList<BigInteger>(new BigInteger[] { 98, 85, 84 }),
        //         item => item + 7,
        //         item => item % 19 == 0,
        //         5,
        //         7
        //     ),
        //     new(
        //         new LinkedList<BigInteger>(new BigInteger[] { 78 }),
        //         item => item + 8,
        //         item => item % 5 == 0,
        //         3,
        //         0
        //     ),
        //     new(
        //         new LinkedList<BigInteger>(new BigInteger[] { 86, 70, 60, 88, 88, 78, 74, 83 }),
        //         item => item + 4,
        //         item => item % 11 == 0,
        //         1,
        //         2
        //     ),
        //     new(
        //         new LinkedList<BigInteger>(new BigInteger[] { 81, 58 }),
        //         item => item + 5,
        //         item => item % 17 == 0,
        //         3,
        //         5
        //     )
        // };
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

            if (round % 100 == 0)
            {
                Console.WriteLine($"Did round {round}");
            }
        }

        var busiestMonkeys = _monkeys.OrderByDescending(monkey => monkey.InspectionCount).Take(2).ToList();
        var monkeyBusiness = busiestMonkeys[0].InspectionCount * busiestMonkeys[1].InspectionCount;

        return new ValueTask<string>($"{monkeyBusiness}");
    }
}