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
    int occurance = 0;

    for (int line = 0; line < input.Length; line++)
    {
        for (int c = 0; c < input[line].Length; c++)
        {
            if (input[line][c] == 'X')
            {
                occurance += CheckDirectionsStar1(input, line, c);
            }
        }
    }

    return occurance;
}

static int Star2(string[] input)
{
    int occurance = 0;

    for (int line = 0; line < input.Length; line++)
    {
        for (int c = 0; c < input[line].Length; c++)
        {
            if (input[line][c] == 'A')
            {
                occurance += CheckDirectionsStar2(input, line, c);
            }
        }
    }

    return occurance;
}

// UTIL //

static int CheckDirectionsStar1(string[] field, int line, int c)
{
    string word = "MAS";
    int len = word.Length;
    int occurance = 0;

    (short row, short col)[] vectors =
    {
        (0, 1),   // right
        (0, -1),  // left
        (-1, 0),  // up
        (1, 0),   // down
        (-1, 1),  // right-up
        (1, 1),   // right-down
        (-1, -1), // left-up
        (1, -1)  // left-down
    };

    foreach (var v in vectors)
    {
        if (line + v.row * len >= 0 && line + v.row * len < field.Length &&
              c + v.col * len >= 0 &&   c + v.col * len < field[line].Length)
        {
            bool correct = true;

            for (int i = 1; i <= len; i++)
            {
                if (field[line + v.row * i][c + v.col * i] != word[i - 1])
                {
                    correct = false;
                }
            }

            if (correct) occurance++;
        }
    }

    return occurance;
}

static int CheckDirectionsStar2(string[] field, int line, int c)
{
    int occurance = 0;

    if (line - 1 >= 0 && line + 1 < field.Length &&
        c - 1 >= 0 && c + 1 < field[line].Length)
    {
        string s1 = $"{field[line - 1][c - 1]}{field[line + 1][c + 1]}";
        string s2 = $"{field[line - 1][c + 1]}{field[line + 1][c - 1]}";

        if ((s1 == "SM" || s1 == "MS") && (s2 == "SM" || s2 == "MS"))
        {
            occurance++;
        }
    }

    return occurance;
}

enum Copy
{
    NONE,
    STAR_ONE,
    STAR_TWO
};

enum Direction
{
    LEFT_UP,
    LEFT,
    LEFT_DOWN,
    DOWN,
    RIGHT_DOWN,
    RIGHT,
    RIGHT_UP,
    UP
};