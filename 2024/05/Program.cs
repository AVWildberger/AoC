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

static int Star1(string[] input)
{
    int sum = 0;

    ParseInput(input, out (int first, int second)[] rules, out string[] linesStr);

    foreach (var lineStr in linesStr)
    {
        var line = lineStr.Split(',').Select(int.Parse).ToArray();

        bool correct = true;

        foreach (var rule in rules)
        {
            int firstAppearedIdx = -1;
            int secondAppearedIdx = -1;
            int i = 0;

            foreach (var value in line)
            {
                if (rule.first == value)
                {
                    firstAppearedIdx = i;
                }
                else if (rule.second == value)
                {
                    secondAppearedIdx = i;
                }

                if (firstAppearedIdx != -1 && secondAppearedIdx != -1 && secondAppearedIdx < firstAppearedIdx)
                {
                    correct = false;
                    break;
                }

                i++;
            }
        }

        if (correct)
        {
            sum += line[(int) Math.Floor(line.Length / 2.0)];
        }
    }

    return sum;
}

static int Star2(string[] input)
{
    int sum = 0;

    ParseInput(input, out (int first, int second)[] rules, out string[] linesStr);

    foreach (var lineStr in linesStr)
    {
        var line = lineStr.Split(',').Select(int.Parse).ToArray();

        bool correct = true;

        for (int r = 0; r < rules.Length; r++)
        {
            var rule = rules[r];

            int firstAppearedIdx = -1;
            int secondAppearedIdx = -1;
            int i = 0;

            foreach (var value in line)
            {
                if (rule.first == value)
                {
                    firstAppearedIdx = i;
                }
                else if (rule.second == value)
                {
                    secondAppearedIdx = i;
                }

                if (firstAppearedIdx != -1 && secondAppearedIdx != -1 && secondAppearedIdx < firstAppearedIdx)
                {
                    correct = false;
                    line = Switch(line, firstAppearedIdx, secondAppearedIdx);
                    firstAppearedIdx = -1;
                    secondAppearedIdx = -1;
                    r = 0;
                    break;
                }

                i++;
            }
        }

        if (!correct)
        {
            sum += line[(int)Math.Floor(line.Length / 2.0)];
        }
    }

    return sum;
}

// UTIL //

static int[] Switch(int[] arr, int a, int b)
{
    var tmp = arr[a];
    arr[a] = arr[b];
    arr[b] = tmp;

    return arr;
}

static void ParseInput(string[] input, out (int, int)[] rules, out string[] lines)
{
    List<(int, int)> rulesList = new();

    int i = 0;
    while (input[i] != "")
    {
        var arr = input[i].Split('|').Select(int.Parse);
        rulesList.Add((arr.ElementAt(0), arr.ElementAt(1)));
        i++;
    }

    rules = rulesList.ToArray();
    lines = input.Skip(rulesList.Count + 1).ToArray();
}

enum Copy
{
    NONE,
    STAR_ONE,
    STAR_TWO
};