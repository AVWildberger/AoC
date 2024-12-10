using TextCopy; //Using NuGet Package TextCopy (https://github.com/CopyText/TextCopy)

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
    throw new NotImplementedException();
}

static int Star2(string[] input)
{
    throw new NotImplementedException();
}

enum Copy
{
    NONE,
    STAR_ONE,
    STAR_TWO
}