var max = 616942;
var min = 145852;

var totalCombinations =
    Enumerable.Range(min, max - min)
    .Where(pwd =>
    {
        var pairs = 0; // The total number of length 2 pairs found
        var inMatch = 0; // Increment each time we find a matching letter
        var prev = pwd.ToString()[0];
        foreach (char c in pwd.ToString().Skip(1))
        {
            // Ensure least to greatest ordering
            if (Char.GetNumericValue(prev) > Char.GetNumericValue(c))
            {
                return false;
            }
            if (prev == c) // Match found 
            {
                inMatch++; // Increment the length of the current match 
            }
            else
            {
                // Out of the match, make sure it was only 2 letters
                if (inMatch == 1)
                {
                    pairs++;
                }
                inMatch = 0; // Reset match length counter
            }
            prev = c;
        }
        // We have to check inMatch again because it could be the last digit
        return inMatch == 1 || pairs > 0;
    }).Count();

Console.WriteLine(totalCombinations);