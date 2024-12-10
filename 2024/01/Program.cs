// Check/Read file inputs //
if (!File.Exists("../../../input.txt"))
{
    Console.Write("Please ensure that the correct input file named \"input.txt\" is in the same directory as Program.cs!");
    Console.ReadKey(true);
    return;
}

var input = File.ReadAllLines("../../../input.txt");

// Main logic //
int[] left = new int[input.Length];
int[] right = new int[input.Length];

for (var i = 0; i < input.Length; i++)
{
    var split = input[i].Split("   ");
    left[i] = int.Parse(split[0]);
    right[i] = int.Parse(split[1]);
}

var sortedLeft = left.OrderBy(x => x);
var sortedRight = right.OrderBy(x => x);

var sum = 0;

for (var i = 0; i < input.Length; i++)
{
    sum += Math.Abs(sortedRight.ElementAt(i) - sortedLeft.ElementAt(i));
}

// Print result //

Console.WriteLine(sum);
