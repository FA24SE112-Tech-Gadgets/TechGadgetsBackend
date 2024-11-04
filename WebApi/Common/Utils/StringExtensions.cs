namespace WebApi.Common.Utils;

public static class StringExtensions
{
    public static (int number, string text) ExtractNumberAndString(this string input)
    {
        // Find the index where the numeric part ends
        int index = 0;
        while (index < input.Length && char.IsDigit(input[index]))
        {
            index++;
        }

        // Extract the number and the string
        string numberPart = input.Substring(0, index);
        string stringPart = input.Substring(index);

        // Convert the number part to an integer
        int number = int.Parse(numberPart);

        return (number, stringPart);
    }
}