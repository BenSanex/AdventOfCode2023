using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using Shouldly;

namespace AdventOfCode2023.Day4;

public class Day4
{
    private string _example = @"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";

    private string _input => File.ReadAllText("Day4/input.txt");

    [Test]
    public void ShouldBreakLineIntoCardAndWinners()
    {
        var lines = _example.Split(Environment.NewLine);
        var one = BreakIntoGame(lines[0]);
        one.winners.Count().ShouldBe(5);
        one.draws.Count().ShouldBe(8);
    }

    [Test]
    public void ShouldGetPoints()
    {
        var lines = _example.Split(Environment.NewLine);
        var one = BreakIntoGame(lines[0]);
        var points = GetScore(one);

        points.ShouldBe(8);
    }

    [Test]
    public void ShouldGetTotal()
    {
        var lines = _example.Split(Environment.NewLine);
        lines.Select(BreakIntoGame).Select(GetScore).Sum().ShouldBe(13);
    }

    [Test]
    public void Part1()
    {
        var lines = _input.Split(Environment.NewLine);
        lines.Select(BreakIntoGame).Select(GetScore).Sum().ShouldBe(23673);
    }

    [Test]
    public void ShouldGetCountFromCard()
    {
        var lines = _example.Split(Environment.NewLine);
        var one = BreakIntoGame(lines[0]);
        var number = GetNummatches(one.winners, one.draws);
        number.ShouldBe(4);
    }

    [Test]
    public void ShouldAddToCardCounts()
    {
        var lines = _example.Split(Environment.NewLine);
        var cardCount = NewGame(lines.Length);
        var one = BreakIntoGame(lines[0]);
        var updates = GetNummatches(one.winners, one.draws);
        var result = UpdateCardCount(cardCount, 0, updates);
        result[0].ShouldBe(1);
        result[1].ShouldBe(2);
        result[2].ShouldBe(2);
        result[3].ShouldBe(2);
    }

    [Test]
    public void PutItAllTogether()
    {
        var lines = _example.Split(Environment.NewLine);
        var cardCount = NewGame(lines.Length);
        var result = DoPart2(cardCount, lines);
        result.Sum().ShouldBe(30);
    }

    [Test]
    public void Part2()
    {
        var lines = _input.Split(Environment.NewLine);
        var cardCount = NewGame(lines.Length);
        var result = DoPart2(cardCount, lines);
        result.Sum().ShouldBe(12263631);
    }

    private IEnumerable<int> DoPart2(int[] cardCount, string[] lines)
    {
        for (var i = 0; i < cardCount.Length; i++)
        {
            var line = BreakIntoGame(lines[i]);
            var updates = GetNummatches(line.winners, line.draws);
            cardCount = UpdateCardCount(cardCount, i, updates);
        }

        return cardCount;
    }

    [Test]
    public void ShouldInitializeGame()
    {
        var counts = NewGame(_example.Split(Environment.NewLine).Length);
        counts.Length.ShouldBe(6);
        counts.All(c => c == 1).ShouldBeTrue();
    }
    
    

    private int[] NewGame(int length)
    {
        //new array with length that has a one in every spot
        var cardCount = new int[length];
        for (var i = 0; i < length; i++)
        {
            cardCount[i] = 1;
        }

        return cardCount;
    }

    private int[] UpdateCardCount(int[] cardCount, int index, int updates)
    {
        for (var i = index+1; i <= updates+index; i++)
        {
            cardCount[i]+=cardCount[index];
        }

        return cardCount;
    }


    private int GetNummatches(IEnumerable<int> oneWinners, IEnumerable<int> oneDraws)
    {
        return oneWinners.Intersect(oneDraws).Count();
    }


    private static int GetScore((IEnumerable<int> winners, IEnumerable<int> draws) card)
    {
        return (int)Math.Pow(2, card.winners.Intersect(card.draws).Count() - 1);
    }


    private static (IEnumerable<int> winners, IEnumerable<int> draws) BreakIntoGame(string line)
    {
        var card = line.Split(":")[1].Trim();

        var split = card.Split("|");

        var winners = GetNumbers(split[0]);
        var draws = GetNumbers(split[1]);
        return (winners, draws);
    }

    private static IEnumerable<int> GetNumbers(string s)
    {
        foreach (var thing in s.Split(" "))
        {
            if (int.TryParse(thing, out var number))
            {
                yield return number;
            }
        }
    }
}