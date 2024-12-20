﻿using TextCopy; //Using NuGet Package TextCopy (https://github.com/CopyText/TextCopy)

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
    int safeReports = 0;

    foreach (var line in input)
    {
        var arr = line.Split(' ').Select(x => int.Parse(x)).ToArray();

        if (CheckLine(arr)) safeReports++;
    }

    return safeReports;
}

static int Star2(string[] input)
{
    int safeReports = 0;

    foreach (var line in input)
    {
        var arr = line.Split(' ').Select(x => int.Parse(x)).ToArray();

        //check without removing
        bool isSafe = CheckLine(arr);

        if (!isSafe)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                //remove element
                var list = arr.ToList();
                list.RemoveAt(i);
                var newArr = list.ToArray();

                if (CheckLine(newArr)) isSafe = true;
            }
        }

        if (isSafe) safeReports++;
    }

    return safeReports;
}

// UTIL //

static bool CheckLine(int[] arr)
{   
    bool ruleViolated = false;
    bool ascending = true;

    for (int i = 0; i < arr.Length - 1; i++)
    {
        if (!ruleViolated)
        {
            int diff = arr[i + 1] - arr[i];

            if (i == 0 && diff < 0)
            {
                ascending = false;
            }

            if (ascending)
            {
                if (diff < 1 || diff > 3)
                {
                    ruleViolated = true;
                }
            }
            else
            {
                if (diff < -3 || diff > -1)
                {
                    ruleViolated = true;
                }
            }
        }
    }

    return !ruleViolated;
}

enum Copy
{
    NONE,
    STAR_ONE,
    STAR_TWO
}