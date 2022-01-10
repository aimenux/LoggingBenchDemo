namespace App.Helpers;

public static class RandomHelper
{
    private static readonly Random Random = new(Guid.NewGuid().GetHashCode());

    public static string RandomString(int length)
    {
        const string chars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}