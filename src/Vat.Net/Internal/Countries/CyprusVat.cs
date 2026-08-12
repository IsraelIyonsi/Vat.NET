using System.Text.RegularExpressions;

namespace Vat.Internal.Countries;

/// <summary>Cyprus: VAT number, 8 digits followed by a letter derived from an alphabet-mapped checksum.</summary>
internal static partial class CyprusVat
{
    internal const string CountryCode = "CY";
    private const int DigitsLength = 8;
    private const int Modulus = 26;
    private const string ExcludedLeadingDigits = "12";
    private const string CheckLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private static readonly IReadOnlyDictionary<char, int> OddPositionValues = new Dictionary<char, int>
    {
        ['0'] = 1,
        ['1'] = 0,
        ['2'] = 5,
        ['3'] = 7,
        ['4'] = 9,
        ['5'] = 13,
        ['6'] = 15,
        ['7'] = 17,
        ['8'] = 19,
        ['9'] = 21,
    };

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) =>
        FormatPattern().IsMatch(number) && !number.StartsWith(ExcludedLeadingDigits, StringComparison.Ordinal);

    internal static bool? IsChecksumValid(string number)
    {
        var digits = number[..DigitsLength];
        var checkLetter = number[DigitsLength];

        var sum = 0;
        for (var i = 0; i < digits.Length; i++)
        {
            sum += i % 2 == 0 ? OddPositionValues[digits[i]] : digits[i] - '0';
        }

        return CheckLetters[sum % Modulus] == checkLetter;
    }

    [GeneratedRegex("^\\d{8}[A-Z]$")]
    private static partial Regex FormatPattern();
}
