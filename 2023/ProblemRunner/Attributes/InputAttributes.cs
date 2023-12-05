namespace Problems.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public abstract class BaseFileAttribute(string filename) : Attribute
{
    public string Filename { get; } = filename;

    public int Expected { get; init; }
}

[AttributeUsage(AttributeTargets.Method)]
public class SampleFileAttribute : BaseFileAttribute
{
    public SampleFileAttribute(string filename, int expected)
        : base(filename)
    {
        Expected = expected;
    }
};

[AttributeUsage(AttributeTargets.Method)]
public class PuzzleFileAttribute(string filename) : BaseFileAttribute(filename);
