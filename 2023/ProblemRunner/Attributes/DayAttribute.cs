namespace Problems.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DayAttribute(int day) : Attribute
{
    public int Day { get; } = day;
}
