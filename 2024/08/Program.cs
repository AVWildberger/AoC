using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
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
    char[][] field = input.Select(x => x.ToCharArray()).ToArray();
    Dictionary<char, List<(int row, int col)>> symbolCoords = GetAllAntennaCoords(field);

    HashSet<(int row, int col)> locations = new();

    foreach (var list in symbolCoords.Values)
    {
        foreach (var e1 in list)
        {
            foreach (var e2 in list)
            {
                if (e1 != e2)
                {
                    (int row, int col) difference = (e2.row - e1.row, e2.col - e1.col);
                    (int row, int col) newCoords = (e2.row + difference.row, e2.col + difference.col);

                    if (newCoords.row >= 0 && newCoords.row < field.Length && newCoords.col >= 0 && newCoords.col < field[0].Length && !locations.Contains((newCoords.row, newCoords.col)))
                    {
                        locations.Add((newCoords.row, newCoords.col));
                    }
                }
            }
        }
    }

    return locations.Count;
}

static int Star2(string[] input)
{
    char[][] field = input.Select(x => x.ToCharArray()).ToArray();
    Dictionary<char, List<(int row, int col)>> symbolCoords = GetAllAntennaCoords(field);

    HashSet<(int row, int col)> locations = new();

    foreach (var list in symbolCoords.Values)
    {
        foreach (var e1 in list)
        {
            foreach (var e2 in list)
            {
                if (e1 != e2)
                {
                    locations.Add(e1);

                    (int row, int col) difference = (e2.row - e1.row, e2.col - e1.col);
                    (int row, int col) newCoords = (e2.row + difference.row, e2.col + difference.col);

                    while (newCoords.row >= 0 && newCoords.row < field.Length && newCoords.col >= 0 && newCoords.col < field[0].Length)
                    {
                        if (!locations.Contains(newCoords))
                        {
                            locations.Add(newCoords);
                        }

                        newCoords = (newCoords.row + difference.row, newCoords.col + difference.col);
                    }
                }
            }
        }
    }

    return locations.Count;
}

// UTIL //

static Dictionary<char, List<(int row, int col)>> GetAllAntennaCoords(char[][] field)
{
    Dictionary<char, List<(int row, int col)>> symbolCoords = new();
    
    for (int row = 0; row < field.Length; row++)
    {
        for (int col = 0; col < field[row].Length; col++)
        {
            if (field[row][col] != '.' && !symbolCoords.ContainsKey(field[row][col]))
            {
                symbolCoords[field[row][col]] = new();
            }

            if (field[row][col] != '.')
            {
                symbolCoords[field[row][col]].Add((row, col));
            }
        }
    }

    return symbolCoords;
}

enum Copy
{
    NONE,
    STAR_ONE,
    STAR_TWO
};