namespace App.Helpers;

public static class RandomHelper
{
    public static string RandomString(int length)
    {
        const string chars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }
}