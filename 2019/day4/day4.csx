var max = 616942;
var min = 145852;

var totalCombinations =
    // Enumerable.Range(min, max)
    Enumerable.Range(min, max - min)
    .Where(pwd =>
    {
        var pairs = 0;
        var prev = pwd.ToString()[0];
        foreach (char c in pwd.ToString().Skip(1))
        {
            // Ensure least to greatest ordering
            if (prev > c)
            {
                return false;
            }
            // Count all matches found
            if (prev == c)
            {
                pairs++;
            }
            prev = c;
        }
        return pairs > 0;
    }).Count();

Console.WriteLine(totalCombinations);