﻿using System.Runtime.InteropServices;
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

long star1 = Star1(input);
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

static long Star1(string[] input)
{
    int[][] field = CreateField(input);
    (int row, int col)[] trailheadCoordsArr = GetTrailheadCoords(field);

    (int row, int col)[] vectors =
    {
        (-1, 0),    // UP
        (0, 1),     // RIGHT
        (1, 0),     // DOWN
        (0, -1)     // LEFT
    };

    int possibilities = 0;

    foreach (var coords in trailheadCoordsArr)
    {
        List<(int, int)> knownCoords = new();
        possibilities += FindDirection(field, coords, ref knownCoords);
    }

    return possibilities;
}

static int Star2(string[] input)
{
    return 0;
}

// UTIL //

static int FindDirection(int[][] field, (int row, int col) coords, ref List<(int, int)> knownCoords, int number = 1)
{
    int foundSolutions = 0;

    (int row, int col)[] vectors =
    {
        (-1, 0),    // UP
        (0, 1),     // RIGHT
        (1, 0),     // DOWN
        (0, -1)     // LEFT
    };

    foreach (var vector in vectors)
    {
        (int row, int col) newCoords = (coords.row + vector.row, coords.col + vector.col);

        if (newCoords.row >= 0 &&
            newCoords.row < field.Length &&
            newCoords.col >= 0 &&
            newCoords.col < field[0].Length &&
            field[newCoords.row][newCoords.col] == number &&
            !knownCoords.Contains(newCoords))
        {
            if (number == 9)
            {
                foundSolutions++;
                knownCoords.Add(newCoords);
            }
            else
            {
                foundSolutions += FindDirection(field, newCoords, ref knownCoords, number+1);
            }
        }
    }

    return foundSolutions;
}

static int[][] CreateField(string[] input)
{
    return input.Select(
        x => x.ToCharArray()
        .Select(
            y => Convert.ToInt32(y - '0'))
        .ToArray())
        .ToArray();
}

static (int, int)[] GetTrailheadCoords(int[][] field)
{
    List<(int, int)> coords = new();

    for (int row = 0; row < field.Length; row++)
    {
        for (int col = 0; col < field[0].Length; col++)
        {
            if (field[row][col] == 0)
            {
                coords.Add((row, col));
            }
        }
    }

    return coords.ToArray();
}

enum Copy
{
    NONE,
    STAR_ONE,
    STAR_TWO
};