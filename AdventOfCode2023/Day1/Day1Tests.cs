using System.Diagnostics;
using System.Text.RegularExpressions;
using Shouldly;

namespace AdventOfCode2023.Day1;

public class Day1Tests
{
    private readonly string input;
    private readonly string example;
    private readonly string example2;

    public Day1Tests()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var inputPath = Path.Combine(currentDirectory, "Day1", "input.txt");
        var examplePath = Path.Combine(currentDirectory, "Day1", "example.txt");
        input = File.ReadAllText(inputPath);
        example = File.ReadAllText(examplePath);
        example2 = File.ReadAllText(Path.Combine(currentDirectory, "Day1", "example2.txt"));
    }

    [SetUp]
    public void Setup()
    {
    }

    
    [Test]
    public void ShouldGetNumbersFromLineAndAddTogether()
    {
        var lines = example.Split(Environment.NewLine);

        TurnIntoInt(GetNumbersFromLine(lines.First())).ShouldBe(12);
        TurnIntoInt(GetNumbersFromLine(lines.Last())).ShouldBe(77);
    }

    [Test]
    public void ShouldAddAllTheNumbersTogether()
    {
        var lines = example.Split(Environment.NewLine);
        var result = AddAllTheLinesTogether(lines);
        result.ShouldBe(142);
    }

    [Test]
    public void ShouldGetTheAnswer()
    {
        var lines = input.Split(Environment.NewLine);
        var result = AddAllTheLinesTogether(lines);
        result.ShouldBe(54239);
    }

    [Test]
    public void Part2_ShouldGetNumbersFromLine()
    {
        var lines = example2.Split(Environment.NewLine);
        var result = GetNumbersFromLine2(lines.First());
        result.Count().ShouldBe(3);
    }

    [Test]
    public void Part2_ShouldGetValueForLineBasedOnFirstAndLastNumber()
    {
        var lines = example2.Split(Environment.NewLine);
        var result = TurnIntoInt(GetNumbersFromLine2(lines.First()));
        result.ShouldBe(29);
        result = TurnIntoInt(GetNumbersFromLine2(lines.Last()));
        result.ShouldBe(76);
    }

    [Test]
    public void Part2_ShouldSumThemALl()
    {
        var lines = example2.Split(Environment.NewLine);
        var result = AddAllTheLinesTogether2(lines);
        result.ShouldBe(281);
    }

    [Test]
    public void Part2_ShouldGetTheAnswer()
    {
        var lines = input.Split(Environment.NewLine);
        var result = AddAllTheLinesTogether2(lines);
        result.ShouldBe(55330);
    }

    private int AddAllTheLinesTogether2(string[] lines)
    {
        return lines.Select(GetNumbersFromLine2).Select(TurnIntoInt).Sum();
    }

    private static IEnumerable<int> GetNumbersFromLine2(string line)
    {
        var matches = Regex.Matches(line);
        var results = new List<int>();
        foreach (var match in matches)
        {
            var matchString = match.ToString();
            if (int.TryParse(matchString, out var result))
                yield return result;
            else if (WordsZeroThroughNine.Contains(matchString))
                yield return Array.IndexOf(WordsZeroThroughNine, matchString);
        }
    }

    private int AddAllTheLinesTogether(IEnumerable<string> lines)
    {
        return lines.Select(GetNumbersFromLine).Select(TurnIntoInt).Sum();
    }


    private static IEnumerable<int> GetNumbersFromLine(string line)
    {
        foreach (var c in line)
        {
            if (int.TryParse(c.ToString(), out var result))
            {
                yield return result;
            }
        }
    }
    
    private static int TurnIntoInt(IEnumerable<int> numbers)
    {
        var first = numbers.First();
        var last = numbers.Last();
        return int.Parse($"{first}{last}");
    }
    //regex to find all single digit numbers and words zero through nine
    static Regex Regex = new Regex(@"\d|(one|two|three|four|five|six|seven|eight|nine)");
    
    static string[] WordsZeroThroughNine = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    
}