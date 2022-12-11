using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2022.utils;
using AoCHelper;

namespace AoC2022.Days;

class AocFile
{
    private string _name;
    private long? _size;
    private List<AocFile> _children;
    private AocFile _parent;

    public AocFile(string name, AocFile parent)
    {
        _name = name;
        _parent = parent;
        _size = null;

        _children = new List<AocFile>();
    }

    public AocFile(string name, long size, AocFile parent) : this(name, parent)
    {
        _size = size;
    }

    public void AddChild(AocFile child)
    {
        if (_children.Exists(file => file._name == child._name))
        {
            return;
        }

        _children.Add(child);
    }

    public AocFile FindChild(string name)
    {
        if (_size != null)
        {
            return null;
        }

        return _children.Find(child => child._name == name);
    }

    public AocFile GetParent()
    {
        return _parent;
    }

    public long GetSize()
    {
        if (_size is { } size)
        {
            return size;
        }
        else
        {
            return _children.Sum(child => child.GetSize());
        }
    }
}

public sealed class Day07 : BaseDay
{
    private AocFile _root;

    private AocFile _current;

    private List<AocFile> _directories;

    public Day07()
    {
        _directories = new List<AocFile>();
            
        _root = new AocFile("/", null);
        _current = _root;

        _directories.Add(_root);

        var input = File.ReadAllLines(InputFilePath);

        // Skip first line because of root creation
        for (var i = 1; i < input.Length; i++)
        {
            var line = input[i];

            if (line.StartsWith("$"))
            {
                if (line.StartsWith("$ ls"))
                {
                    continue;
                }

                if (line.StartsWith("$ cd"))
                {
                    var (_, _, dir, _) = line.Split(" ");

                    switch (dir)
                    {
                        case "/":
                            _current = _root;
                            break;
                        case "..":
                            _current = _current.GetParent();
                            break;
                        default:
                        {
                            var newDir = _current.FindChild(dir);
                            _current = newDir;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (line.StartsWith("dir"))
                {
                    var (_, name, _) = line.Split(" ");

                    var dir = new AocFile(name, _current);
                        
                    _current.AddChild(dir);
                    _directories.Add(dir);
                }
                else
                {
                    var (size, name, _) = line.Split(" ");

                    var parsedSize = long.Parse(size);
                        
                    _current.AddChild(new AocFile(name, parsedSize, _current));
                }
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var sum = _directories.Select(dir => dir.GetSize()).Where(size => size <= 100_000).Sum();

        return new ValueTask<string>($"{sum}");
    }

    public override ValueTask<string> Solve_2()
    {
        const long totalSize = 70_000_000L;
        const long sizeRequired = 30_000_000L;

        var sizeUsed = _root.GetSize();

        var required = sizeRequired - (totalSize - sizeUsed);

        var smallest = _directories.Select(dir => dir.GetSize()).Where(size => size >= required).MinBy(size => size);
            
        return new ValueTask<string>($"{smallest}");
    }
}