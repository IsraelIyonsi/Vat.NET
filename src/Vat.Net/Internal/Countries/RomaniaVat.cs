using System.Text.RegularExpressions;

namespace Vat.Internal.Countries;

/// <summary>
/// Romania: CUI/CIF, 2 to 10 digits with a weighted mod-11 checksum computed over the
/// number zero-padded to 9 digits.
/// </summary>
internal static partial class RomaniaVat
{
    internal const string CountryCode = "RO";
    private const int PaddedLength = 9;
    private const int SumMultiplier = 10;
    private const int Modulus11 = 11;
    private const int Modulus10 = 10;

    private static readonly int[] Weights = [7, 5, 3, 2, 1, 7, 5, 3, 2];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number)
    {
        var checkDigit = number[^1] - '0';
        var padded = number[..^1].PadLeft(PaddedLength, '0');

        var sum = 0;
        for (var i = 0; i < Weights.Length; i++)
        {
            sum += Weights[i] * (padded[i] - '0');
        }

        var expected = sum * SumMultiplier % Modulus11 % Modulus10;
        return expected == checkDigit;
    }

    [GeneratedRegex("^[1-9]\\d{1,9}$")]
    private static partial Regex FormatPattern();
}
