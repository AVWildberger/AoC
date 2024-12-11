using System.Text.RegularExpressions;
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

var input = File.ReadAllText(FILE);

// MAIN LOGIC //

int star1 = Star1(input);
int star2 = Star2(input);

// PRINT (& COPY) RESULTS //

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

static int Star1(string input)
{
    int sum = 0;
    string pattern = @"mul\(\d{1,3},\d{1,3}\)";

    MatchCollection matches = Regex.Matches(input, pattern);
    foreach (Match match in matches)
    {
        sum += Multiply(match);
    }

    return sum;
}

static int Star2(string input)
{
    int sum = 0;
    bool mulEnabled = true;
    string pattern = @"(mul\(\d{1,3},\d{1,3}\)|do\(\)|don't\(\))";

    MatchCollection matches = Regex.Matches(input, pattern);
    foreach (Match match in matches)
    {
        if (match.Value.StartsWith("mul") && mulEnabled)
        {
            sum += Multiply(match);
        }
        else if (match.Value == "do()")
        {
            mulEnabled = true;
        }
        else if (match.Value == "don't()")
        {
            mulEnabled = false;
        }
    }

    return sum;
}

// UTIL //

static int Multiply(Match match)
{
    //mul(x,y)
    return match.Value
            .Substring(0, match.Length - 1)
            .Remove(0, 4)
            .Split(',')
            .Select(int.Parse)
            .ToArray()
            .Aggregate((num1, num2) => num1 * num2);
}

enum Copy
{
    NONE,
    STAR_ONE,
    STAR_TWO
}