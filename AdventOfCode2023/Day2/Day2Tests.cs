using System.Reflection;
using Shouldly;

namespace AdventOfCode2023.Day2;

public class Day2Tests
{
    private readonly string example;
    private readonly string input;

    public Day2Tests()
    {
        var dir = Directory.GetCurrentDirectory();
        var inputPath = Path.Combine(dir, "Day2", "input.txt");
        var examplePath = Path.Combine(dir, "Day2", "example.txt");
        example = File.ReadAllText(examplePath);
        input = File.ReadAllText(inputPath);
    }

    [Test]
    public void ShouldSplitLineIntoPulls()
    {
        var lines = example.Split(Environment.NewLine);

        var pulls = SplitIntoPulls(lines.First());
        pulls.Length.ShouldBe(3);
        pulls.First().ShouldBe("3 blue, 4 red");
    }

    [Test]
    public void ShouldReadCounts()
    {
        var example = "1 red, 2 blue";
        var result = ReadColors(example);
        result["red"].ShouldBe(1);
        result["blue"].ShouldBe(2);
    }

    [Test]
    public void ShouldUpdateNewMaxes()
    {
        var exampleSTring = "1 red, 2 blue";
        var existing = new Dictionary<string, int> { { "red", 1 }, { "blue", 1 } };
        var result = UpdateMaxes(exampleSTring, existing);
        result["red"].ShouldBe(1);
        result["blue"].ShouldBe(2);
    }

    [Test]
    public void ShouldGetMaxesForAGame()
    {
        var line = example.Split(Environment.NewLine).First();
        var result = GameMaxes(line);
        result.Count.ShouldBe(3);
        result["red"].ShouldBe(4);
        result["blue"].ShouldBe(6);
        result["green"].ShouldBe(2);
    }

    [Test]
    public void ShouldGetAllTheGames()
    {
        var lines = example.Split(Environment.NewLine);
        var result = AllGamesMaxes(lines);
        result.Count().ShouldBe(5);
        result.All(r => r.Count == 3).ShouldBeTrue();
        result.Last()["red"].ShouldBe(6);
        result.First()["blue"].ShouldBe(6);
    }

    [Test]
    public void ShouldGetAllTheGamesWithTooMany()
    {
        Dictionary<string, int> max = new()
        {
            { "red", 12 },
            { "blue", 14 },
            { "green", 13 }
        };
        var lines = example.Split(Environment.NewLine);
        var maxes = AllGamesMaxes(lines);
        var result = GetPossibles(maxes.ToList(), max);
        result.Sum().ShouldBe(8);
    }

    [Test]
    public void ShouldGetThePower()
    {
        var lines = example.Split(Environment.NewLine);
        var maxes = AllGamesMaxes(lines);
        var powers = GetPowers(maxes.ToList());
        powers.Sum().ShouldBe(2286);
    }

    [Test]
    public void ShouldGetTheRealPower()
    {
        var lines = input.Split(Environment.NewLine);
        var maxes = AllGamesMaxes(lines);
        var powers = GetPowers(maxes.ToList());
        powers.Sum().ShouldBe(66681);
    }

    private List<int> GetPowers(List<Dictionary<string, int>> toList)
    {
        return toList.Select(game => game["red"] * game["blue"] * game["green"]).ToList();
    }

    [Test]
    public void ShouldWork()
    {
        Dictionary<string, int> max = new()
        {
            { "red", 12 },
            { "blue", 14 },
            { "green", 13 }
        };
        var lines = input.Split(Environment.NewLine);
        var maxes = AllGamesMaxes(lines);
        var result = GetPossibles(maxes.ToList(), max).ToList();
        Console.WriteLine(string.Join(',', result));
        result.Sum().ShouldBe(2237);
    }

    private IEnumerable<int> GetPossibles(List<Dictionary<string, int>> maxes, Dictionary<string, int> max)
    {
        for (var i = 0; i < maxes.Count; i++)
        {
            var game = maxes[i];
            var possible = game.All(color => color.Value <= max[color.Key]);

            if (possible)
            {
                yield return i + 1;
            }
        }
    }

    private IEnumerable<Dictionary<string, int>> AllGamesMaxes(IEnumerable<string> lines)
    {
        return lines.Select(GameMaxes);
    }

    private Dictionary<string, int> GameMaxes(string line)
    {
        var pulls = SplitIntoPulls(line);
        var result = new Dictionary<string, int>();

        return pulls.Aggregate(result, (current, pull) => UpdateMaxes(pull, current));
    }

    private Dictionary<string, int> UpdateMaxes(string colorPull, Dictionary<string, int> existing)
    {
        var colors = ReadColors(colorPull);
        foreach (var color in colors)
        {
            if (existing.TryGetValue(color.Key, out var existingCount))
            {
                if (color.Value > existingCount)
                {
                    existing[color.Key] = color.Value;
                }
            }
            else
            {
                existing.Add(color.Key, color.Value);
            }
        }

        return existing;
    }


    private Dictionary<string, int> ReadColors(string s)
    {
        var result = new Dictionary<string, int>();
        var colors = s.Split(',');
        foreach (var color in colors)
        {
            var split = color.Trim().Split(' ');
            var count = int.Parse(split.First());
            var colorName = split.Last();
            result.Add(colorName, count);
        }

        return result;
    }


    [Test]
    public void ShouldGetCountsPerPull()
    {
        var lines = example.Split(Environment.NewLine);
        var result = ShouldGetMinCountsOfColor(lines.First());
        result.Count.ShouldBe(3);
    }


    private static string[] SplitIntoPulls(string line)
    {
        var pulls = line[(line.IndexOf(':') + 1)..].Trim().Split(';');
        return pulls;
    }

    private Dictionary<string, int> ShouldGetMinCountsOfColor(string line)
    {
        var result = new Dictionary<string, int>();
        var pulls = SplitIntoPulls(line);
        var minCounts = new Dictionary<string, int>();
        foreach (var pull in pulls)
        {
        }

        return minCounts;
    }
}