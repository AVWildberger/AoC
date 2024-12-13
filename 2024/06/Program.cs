using System.Data;
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
    (int row, int col) coords = GetStartPos(input);
    bool[,] visits = InitVisits(input);
    (int row, int col) direction = (-1, 0);
    bool needTurn;

    visits[coords.row, coords.col] = true;

    bool outOfBounds;
    do
    {
        needTurn = false;
        outOfBounds = CheckIfOutOfBounds(input, (coords.row + direction.row, coords.col + direction.row));

        if (!outOfBounds)
        {
            needTurn = input[coords.row + direction.row][coords.col + direction.col] == '#';

            if (!needTurn)
            {
                coords = (coords.row + direction.row, coords.col + direction.col);
            }
            else
            {
                direction = Turn(direction);
            }

            Console.WriteLine(coords);
            visits[coords.row, coords.col] = true;
        }
    } while (!outOfBounds);

    return CountVisits(visits);
}

static int Star2(string[] input)
{
    return 0;
}

// UTIL //

static (int row, int col) Turn((int row, int col) coords)
{
    if (coords.row == -1)
    {
        coords.row = 0;
        coords.col = 1;
    }
    else if (coords.col == 1)
    {
        coords.row = 1;
        coords.col = 0;
    }
    else if (coords.row == 1)
    {
        coords.row = 0;
        coords.col = -1;
    }
    else if (coords.col == -1)
    {
        coords.row = -1;
        coords.col = 0;
    }

    return coords;
}

static bool CheckIfOutOfBounds(string[] input, (int row, int col) newCoords)
{
    return !(newCoords.row >= 0 &&
           newCoords.col >= 0 &&
           newCoords.row < input.Length &&
           newCoords.col < input[newCoords.row].Length);
}

static bool[,] InitVisits(string[] input)
{
    bool[,] visits = new bool[input.Length, input[0].Length];

    for (int i = 0; i < input.Length; i++)
    {
        for (int k = 0; k < input[0].Length; k++)
        {
            visits[i, k] = false;
        }
    }

    return visits;
}

static int CountVisits(bool[,] visits)
{
    int sum = 0;

    foreach (bool element in visits)
    {
        if (element) sum++;
    }

    return sum;
}

static (int row, int col) GetStartPos(string[] input)
{
    (int row, int col) coordinates;

    coordinates.row = Array.FindIndex(input.Select(i => i.IndexOf('^')).ToArray(), line => line != -1);
    coordinates.col = input[coordinates.row].IndexOf('^');

    return coordinates;
}

enum Copy
{
    NONE,
    STAR_ONE,
    STAR_TWO
};