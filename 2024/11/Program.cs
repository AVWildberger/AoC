using System.Diagnostics;
using TextCopy; //Using NuGet Package TextCopy (https://github.com/CopyText/TextCopy)

// CONFIG //

const Copy COPY = Copy.STAR_TWO;
const string FILE = "../../../input.txt";

// CHECK & READ FILE INPUTS //

if (!File.Exists(FILE))
{
    Console.Write($"Please ensure that the correct input file named \"{FILE.Split('/').Last()}\" is in the same directory as Program.cs!");
    Console.ReadKey(true);
    return;
}

string input = File.ReadAllText(FILE);

// MAIN LOGIC //

long star1 = Star1(input);
long star2 = Star2(input);

// PRINT (& COPY) RESULTS //

Console.Clear();
Console.WriteLine($"Star 1: {star1}");
Console.WriteLine($"Star 2: {star2}");

switch (COPY)
{
    case Copy.STAR_ONE:
        await ClipboardService.SetTextAsync(star1.ToString());
        break;
    case Copy.STAR_TWO:
        await ClipboardService.SetTextAsync(star2.ToString());
        break;
};

// STARS //

static long Star1(string input)
{
    return BlinkXTimesThenGetLength(input, 25);
}

static long Star2(string input)
{
    return 0;
}

// UTIL //

static long BlinkXTimesThenGetLength(string input, int repetitions)
{
    var stones = input.Split(' ').Select(long.Parse).ToList();

    for (int i = 0; i < repetitions; i++)
    {
        stones = Blink(stones);
        Console.WriteLine($"{i + 1}/{repetitions}");
    }

    return stones.Count;
}

static List<long> Blink(List<long> stones)
{
    List<long> newStones = new(stones);
    int offset = 0;

    for (int i = 0; i < stones.Count; i++)
    {
        if (newStones[i + offset] == 0)
        {
            newStones[i + offset] = 1;
        }
        else if (stones[i].ToString().Length % 2 == 0)
        {
            string stoneStr = newStones[i + offset].ToString();
            newStones.Insert(i + 1 + offset, int.Parse(stoneStr.Substring(stoneStr.Length / 2, stoneStr.Length / 2)));
            newStones[i + offset] = int.Parse(stoneStr.Substring(0, stoneStr.Length / 2));
            offset++;
        }
        else
        {
            newStones[i + offset] *= 2024;
        }
    }

    return newStones;
}

enum Copy
{
    NONE,
    STAR_ONE,
    STAR_TWO
};