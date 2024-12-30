namespace Library.Domain.Utilities;

public static class Converter
{
    public static long ToUnixTimestampSeconds(DateTime dateTime)
    {
        return new DateTimeOffset(dateTime.ToUniversalTime()).ToUnixTimeSeconds();
    }
}