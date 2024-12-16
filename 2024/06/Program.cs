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
    CheckField(input, out int visits);

    return visits;
}

static int Star2(string[] input)
{
    int options = 0;

    for (int row = 0; row < input.Length; row++)
    {
        for (int col = 0; col < input[0].Length; col++)
        {
            if (input[row][col] == '.')
            {
                string[] newField = (string[])input.Clone();

                string first = input[row].Substring(0, col);
                char second = '#';
                string third = input[row].Substring(col + 1, input[row].Length - first.Length - 1);
                newField[row] = first + second + third;

                Console.WriteLine("Alt: " + input[row]);
                Console.WriteLine("Neu: " + newField[row]);

                bool isEnding = CheckField(newField, out _);
                if (!isEnding)
                {
                    options++;
                }
            }
        }
    }

    return options;
}

// UTIL //

static bool CheckField(string[] input, out int visitNumber)
{
    (int row, int col) coords = GetStartPos(input);
    bool[,] visits = InitVisits(input);
    (int row, int col) direction = (-1, 0);
    List<((int row, int col) coords, (int row, int col) direction)> visitedCorners = new();
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
                visitedCorners.Add((coords, direction));
                direction = Turn(direction);

                if (CheckIfExisting(visitedCorners, coords, direction))
                {
                    visitNumber = CountVisits(visits);
                    return false;
                }
            }

            visits[coords.row, coords.col] = true;
        }
    } while (!outOfBounds);

    visitNumber = CountVisits(visits);

    return true;
}

static bool CheckIfExisting(List<((int row, int col) coords, (int row, int col) direction)> visitedCorners, (int row, int col) coords, (int row, int col) direction)
{
    foreach (var corner in visitedCorners)
    {
        if (corner.direction == direction && corner.coords == coords) return true;
    }

    return false;
}

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