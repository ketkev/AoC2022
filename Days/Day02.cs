using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AoC2022.utils;
using AoCHelper;

namespace AoC2022.Days;

internal enum Move
{
    Rock,
    Paper,
    Scissor
}

struct Round
{
    public readonly Move Opponent;
    public readonly Move Yours;

    public Round(string opponent, String yours)
    {
        Opponent = opponent switch
        {
            "A" => Move.Rock,
            "B" => Move.Paper,
            "C" => Move.Scissor,
            _ => throw new ArgumentOutOfRangeException()
        };

        Yours = yours switch
        {
            "X" => Move.Rock,
            "Y" => Move.Paper,
            "Z" => Move.Scissor,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public sealed class Day02 : BaseDay
{
    private readonly List<Round> _rounds = new();

    public Day02()
    {
        var input = File.ReadAllLines(InputFilePath);

        foreach (var line in input)
        {
            var (opponent, yours, _) = line.Split(" ");
            _rounds.Add(new Round(opponent, yours));
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var score = 0L;

        foreach (var round in _rounds)
        {
            score += round.Yours switch
            {
                Move.Rock => 1,
                Move.Paper => 2,
                Move.Scissor => 3,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (round.Opponent == round.Yours)
            {
                score += 3;
                continue;
            }

            if ((round.Yours == Move.Rock && round.Opponent == Move.Scissor)
                || (round.Yours == Move.Paper && round.Opponent == Move.Rock)
                || (round.Yours == Move.Scissor && round.Opponent == Move.Paper))
            {
                score += 6;
            }
        }

        return new ValueTask<string>($"{score}");
    }

    public override ValueTask<string> Solve_2()
    {
        var score = 0L;
            
        foreach (var round in _rounds)
        {
            Move move;
                
            switch (round.Yours)
            {
                case Move.Rock:
                    // Lose
                        
                    move = round.Opponent switch
                    {
                        Move.Rock => Move.Scissor,
                        Move.Paper => Move.Rock,
                        Move.Scissor => Move.Paper,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    break;
                case Move.Paper:
                    // Draw
                    score += 3;

                    move = round.Opponent;
                    break;
                case Move.Scissor:
                    // Win
                    score += 6;

                    move = round.Opponent switch
                    {
                        Move.Rock => Move.Paper,
                        Move.Paper => Move.Scissor,
                        Move.Scissor => Move.Rock,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
                
            score += move switch
            {
                Move.Rock => 1,
                Move.Paper => 2,
                Move.Scissor => 3,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
            
        return new ValueTask<string>($"{score}");
    }
}