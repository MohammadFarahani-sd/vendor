namespace OFood.Shop.Common.StringTools;

public static class RandomStringHelper
{
    private static readonly Random Random = new();

    public static string GetRandomString(int length)
    {
        const string chars = RandomStringHelperConstants.AlphaCharacters;
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }

    public static string GetRandomNumericString(int length)
    {
        const string chars = RandomStringHelperConstants.NumericCharacters;
        return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
    }

    public static string GetRandomAlphaNumericString(int length)
    {
        const string chars = RandomStringHelperConstants.NumericCharacters;
        return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
    }

    public static string GetRandomAlphaString(int length)
    {
        const string chars = RandomStringHelperConstants.AlphaCharacters;
        return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}