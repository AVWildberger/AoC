using TextCopy; //Using NuGet Package TextCopy (https://github.com/CopyText/TextCopy)

// CONFIG //

const Copy COPY = Copy.STAR_ONE;
const string FILE = "../../../input.txt";

// CHECK & READ FILE INPUTS //

if (!File.Exists(FILE))
{
    Console.Write($"Please ensure that the correct input file named \"{FILE.Split('/').Last()}\" is in the same directory as Program.cs!");
    Console.ReadKey(true);
    return;
}

var input = File.ReadAllText(FILE);

while (input.Contains('\n'))
{
    input = input.Remove(input.IndexOf('\n'));
}

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

static long Star1(string input)
{
    List<char> chain = new();

    var currentIndex = 0;
    for (int i = 0; i < input.Length; i++)
    {
        var count = input[i] - '0';

        for (int j = 0; j < count; j++)
        {
            if (i % 2 == 0)
            {
                chain.Add((char) (currentIndex + '0'));
            }
            else
            {
                chain.Add('.');
            }
        }

        if (i % 2 == 0) { currentIndex++; }
    }
    Console.WriteLine();

    for (int i = 0; i < chain.Count; i++)
    {
        if (chain[i] == '.')
        {
            while (chain[^1] == '.')
            {
                chain.RemoveAt(chain.Count - 1);
            }

            if (i < chain.Count - 1)
            {
                chain[i] = chain[^1];
                chain.RemoveAt(chain.Count - 1);
            }
        }
    }

    long sum = 0;
    for (int i = 0; i < chain.Count; i++)
    {
        sum += Convert.ToInt32(chain[i] - '0') * i;
    }

    return sum;
}

static int Star2(string input)
{
    return 0;
}

// UTIL //

enum Copy
{
    NONE,
    STAR_ONE,
    STAR_TWO
};