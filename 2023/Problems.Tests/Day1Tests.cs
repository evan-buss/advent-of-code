namespace Problems.Tests;

public class Day1Tests
{
    public static IEnumerable<object[]> ConvertsLineWithTextNumbersData()
    {
        yield return new object[] { "oneeight", "18" };
        yield return new object[] { "one7one", "11" };
        yield return new object[] { "bhfhszrhzgrhsfd2threeseventwosevenoneseven", "27" };
        yield return new object[] { "one", "1" };
    }

    [Theory]
    [MemberData(nameof(ConvertsLineWithTextNumbersData))]
    public void ConvertsLineWithTextNumbers(string line, string expected)
    {
        Assert.Equivalent(expected, Day1.ConvertLine(line));
    }
}
