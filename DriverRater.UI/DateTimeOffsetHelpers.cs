namespace DriverRater.UI;

public static class DateTimeOffsetHelpers
{
    public static string ToDisplay(this DateTimeOffset? dateTimeOffset, string format, string whenNull)
    {
        return dateTimeOffset.HasValue ? dateTimeOffset.Value.ToString(format) : whenNull;
    }
}