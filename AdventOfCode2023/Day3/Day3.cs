using System.Text.RegularExpressions;
using Shouldly;

namespace AdventOfCode2023.Day3;

public class Day3
{
    private readonly string _example;
    private readonly string _input;

    public Day3()
    {
        _example = @"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";

        _input = File.ReadAllText("Day3/input.txt");
    }

    [Test]
    public void ShouldGetNumberCoordinates()
    {
        var thing = new Map(_example);
        var number = thing.GetWholeNumberByCoords(0, 0);
        number.ShouldBe(467);
    }

    [Test]
    public void ShouldSeeIfSurroundingContainsSymbol()
    {
        var thing = new Map(_example);
        var coords = thing.GetSurroundingCoords(0, 0);
        var containsSymbol = thing.Symbolic(coords);
        containsSymbol.ShouldBeFalse();

        coords = thing.GetSurroundingCoords(3, 0);
        thing.Symbolic(coords).ShouldBeTrue();
        coords = thing.GetSurroundingCoords(4, 1);
        thing.Symbolic(coords).ShouldBeTrue();
    }

    [Test]
    public void ShouldTellIfContainsASymbol()
    {
        var thing = new Map(_example);
        var coords = (3, 1);
        var containsSymbol = thing.CoordContainsSymbol(coords, thing.GetMap());
        containsSymbol.ShouldBeTrue();
    }

    [Test]
    public void ShouldGetListOfNumbers()
    {
        var thing = new Map(_example);
        var numbers = thing.GetAllSchematicNumbers();
        numbers.Sum().ShouldBe(4361);
    }

    [Test]
    public void ShouldAnswerPart1()
    {
        var thing = new Map(_input);
        var numbers = thing.GetAllSchematicNumbers().Sum();
        numbers.ShouldBe(536202);
    }

    [Test]
    public void ShouldGetGearRatio()
    {
        var example = new Map(_example);
        var coords = example.GetSurroundingCoords(3, 1);
        var numbers = example.GetSurroundingNumbers(coords);
        numbers.Count().ShouldBe(2);
        (numbers[0] * numbers[1]).ShouldBe(16345);
    }

    [Test]
    public void ShouldSumGearRatios()
    {
        var example = new Map(_example);
        var ratios = example.GetAllGearRatios();
        ratios.Sum().ShouldBe(467835);
    }

    [Test]
    public void ShouldAnswerPart2()
    {
        var input = new Map(_input);
        input.GetAllGearRatios().Sum().ShouldBe(78272573);
    }
}

public class Map
{
    private readonly string _map;

    public Map(string input)
    {
        _map = input;
    }

    public List<List<char>> GetMap()
    {
        var lines = _map.Split(Environment.NewLine);
        return lines.Select(m => m.ToCharArray().ToList()).ToList();
    }

    public int MapWidth => GetMap()[0].Count;

    public int MapHeight => GetMap().Count;


    public List<(int, int)> GetSurroundingCoords(int x, int y)
    {
        var coords = new List<(int, int)>
        {
            (x - 1, y - 1),
            (x + 1, y - 1),
            (x - 1, y),
            (x + 1, y),
            (x - 1, y + 1),
            (x, y + 1),
            (x, y - 1),
            (x + 1, y + 1)
        };
        return coords;
    }

    public int GetWholeNumberByCoords(int x, int y)
    {
        var map = GetMap();
        var number = new List<char> { map[y][x] };
        //go backwards, adding the characters to number until you hit a non-number
        for (var i = x - 1; i >= 0; i--)
        {
            if (char.IsNumber(map[y][i]))
            {
                number = number.Prepend(map[y][i]).ToList();
            }
            else
            {
                break;
            }
        }

        for (var i = x + 1; i >= 0; i++)
        {
            if (i < MapWidth && char.IsNumber(map[y][i]))
            {
                number = number.Append(map[y][i]).ToList();
            }
            else
            {
                break;
            }
        }

        return int.Parse(string.Join("", number));
    }


    public bool Symbolic(List<(int, int)> coords)
    {
        var map = GetMap();
        return coords.Any(coord => CoordContainsSymbol(coord, map));
    }

    public bool CoordContainsSymbol((int, int) coord, List<List<char>> map)
    {
        return coord is { Item2: >= 0, Item1: >= 0 } && MapHeight > coord.Item2 && MapWidth > coord.Item1 && Symbols.Contains(map[coord.Item2][coord.Item1]);
    }

    //Array of all realistic symbols excluding periods
    private List<char> Symbols = new List<char> { '#', '$', '*', '+', '@', '~', '^', '&', '!', '%', '?', '>', '<', '(', ')', '[', ']', '{', '}', '/', '\\', '|', '-', '_', '=', '"', '\'' };

    public List<int> GetAllSchematicNumbers()
    {
        var map = GetMap();
        var numbers = new List<int>();
        for (var y = 0; y < MapHeight; y++)
        {
            for (var x = 0; x < MapWidth; x++)
            {
                if (char.IsNumber(map[y][x]) && Symbolic(GetSurroundingCoords(x, y)))
                {
                    var numToAdd = GetWholeNumberByCoords(x, y);
                    numbers.Add(numToAdd);

                    //move to next non number
                    while (char.IsNumber(map[y][x]))
                    {
                        x++;
                        if (x >= MapWidth)
                        {
                            x = 0;
                            y++;
                        }
                    }
                }
            }
        }

        return numbers;
    }

    public List<int> GetSurroundingNumbers(List<(int, int)> coords)
    {
        var map = GetMap();
        var numbers = new List<int>();
        foreach (var coord in coords)
        {
            if (int.TryParse(map[coord.Item2][coord.Item1].ToString(), out var num))
            {
                numbers.Add(GetWholeNumberByCoords(coord.Item1, coord.Item2));
            }
        }

        return numbers.Distinct().ToList();
    }

    public List<int> GetAllGearRatios()
    {
        var map = GetMap();
        var numbers = new List<int>();
        for (var y = 0; y < MapHeight; y++)
        {
            for (var x = 0; x < MapWidth; x++)
            {
                var current = map[y][x];
                if (map[y][x] == '*')
                {
                    var coords = GetSurroundingCoords(x, y);
                    var ratios = GetSurroundingNumbers(coords);
                    if (ratios.Count == 2)
                    {
                        numbers.Add(ratios[0] * ratios[1]);
                    }
                }
            }
        }

        return numbers;
    }
}