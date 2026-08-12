using System.Text.RegularExpressions;

namespace Vat.Internal.Countries;

/// <summary>
/// Lithuania: PVM moketojo kodas, 9 digits for legal entities or 12 digits for temporarily
/// registered taxpayers, both with a two-step weighted mod-11 checksum.
/// </summary>
internal static partial class LithuaniaVat
{
    internal const string CountryCode = "LT";
    private const int LegalEntityLength = 9;
    private const int TemporaryLength = 12;
    private const int LegalEntityMarkerIndex = 7;
    private const int TemporaryMarkerIndex = 10;
    private const char RequiredMarker = '1';
    private const int PrimaryWeightSpan = 9;
    private const int SecondaryWeightOffset = 2;
    private const int Modulus11 = 11;
    private const int Modulus10 = 10;
    private const int RemainderNeedsFallback = 10;

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number)
    {
        if (!FormatPattern().IsMatch(number))
        {
            return false;
        }

        return number.Length switch
        {
            LegalEntityLength => number[LegalEntityMarkerIndex] == RequiredMarker,
            TemporaryLength => number[TemporaryMarkerIndex] == RequiredMarker,
            _ => false,
        };
    }

    internal static bool? IsChecksumValid(string number)
    {
        var body = number[..^1];
        var checkDigit = number[^1] - '0';

        var primaryRemainder = WeightedSum(body, offset: 0) % Modulus11;
        var finalValue = primaryRemainder == RemainderNeedsFallback
            ? WeightedSum(body, offset: SecondaryWeightOffset) % Modulus11 % Modulus10
            : primaryRemainder % Modulus10;

        return finalValue == checkDigit;
    }

    private static int WeightedSum(string digits, int offset)
    {
        var sum = 0;
        for (var i = 0; i < digits.Length; i++)
        {
            var weight = 1 + (i + offset) % PrimaryWeightSpan;
            sum += weight * (digits[i] - '0');
        }

        return sum;
    }

    [GeneratedRegex("^(\\d{9}|\\d{12})$")]
    private static partial Regex FormatPattern();
}
