using TextCopy;

// CONFIG //

const Copy COPY = Copy.NONE;

// CHECK & READ FILE INPUTS //

if (!File.Exists("../../../input.txt"))
{
    Console.Write("Please ensure that the correct input file named \"input.txt\" is in the same directory as Program.cs!");
    Console.ReadKey(true);
    return;
}

var input = File.ReadAllLines("../../../input.txt");

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



static int Star1(string[] input)
{
    var sum = 0;
    SplitAndConvertInput(input, out int [] left, out int[] right);

    var sortedLeft = left.OrderBy(x => x);
    var sortedRight = right.OrderBy(x => x);

    for (var i = 0; i < input.Length; i++)
    {
        sum += Math.Abs(sortedRight.ElementAt(i) - sortedLeft.ElementAt(i));
    }

    return sum;
}

static int Star2(string[] input)
{
    var similarityScore = 0;

    SplitAndConvertInput(input, out int[] left, out int[] right);

    foreach (int item in left)
    {
        int count = 0;

        foreach (var r in right)
        {
            if (r == item) count++;
        }

        similarityScore += count * item;
    }

    return similarityScore;
}

static void SplitAndConvertInput(string[] input, out int[] left, out int[] right)
{
    left = new int[input.Length];
    right = new int[input.Length];

    for (var i = 0; i < input.Length; i++)
    {
        var split = input[i].Split("   ");
        left[i] = int.Parse(split[0]);
        right[i] = int.Parse(split[1]);
    }
}

enum Copy
{
    NONE,
    STAR_ONE,
    STAR_TWO
}