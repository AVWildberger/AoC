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

var input = File.ReadAllLines(FILE);

// MAIN LOGIC //

long star1 = Star1(input);
long star2 = Star2(input);

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

static long Star1(string[] input)
{
    return GetSumOfCorrectLines(input, false);
}

static long Star2(string[] input)
{
    return GetSumOfCorrectLines(input, true);
}

// UTIL //

static long GetSumOfCorrectLines(string[] lines, bool starTwo)
{
    long sum = 0;

    foreach (var line in lines)
    {
        (long result, int[] terms) calc = ProcessLine(line);

        if (CheckIfCorrect(calc, starTwo))
        {
            sum += calc.result;
        }
    }

    return sum;
}

static (long result, int[] terms) ProcessLine(string input)
{
    (long result, int[] terms) calc;

    var arr = input.Split(": ");
    calc.result = long.Parse(arr[0]);
    calc.terms = arr[1].Split(' ').Select(int.Parse).ToArray();

    return calc;
}

static bool CheckIfCorrect((long result, int[] terms) calc, bool concatOp)
{
    long[] currentArr = { calc.terms[0] };

    for (int i = 1; i < calc.terms.Length; i++)
    {
        currentArr = AddOptions(currentArr, calc.terms[i], concatOp);
    }

    return currentArr.Contains(calc.result);
}

static long[] AddOptions(long[] options, int term, bool concatOp)
{
    List<long> newOptions = new List<long>();

    foreach (var option in options)
    {
        newOptions.Add(option * term);
        newOptions.Add(option + term);

        if (concatOp)
        {
            newOptions.Add(long.Parse($"{option}{term}"));
        }
    }

    return newOptions.ToArray();
}

enum Copy
{
    NONE,
    STAR_ONE,
    STAR_TWO
};