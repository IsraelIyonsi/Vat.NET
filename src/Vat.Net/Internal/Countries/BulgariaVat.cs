using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>
/// Bulgaria: VAT number (Identification Number, Danak varhu dobavenata stoynost), 9 digits
/// for legal entities or 10 digits for physical persons, foreigners and others.
/// </summary>
/// <remarks>
/// Only the 9-digit legal entity checksum is verified. The 10-digit variant can validly be
/// derived either from a general weighted formula or from the holder's national identity
/// number (EGN or the foreigner equivalent, PNF), and this package does not implement
/// national identity number validation, so 10-digit Bulgarian numbers are accepted on
/// format alone.
/// </remarks>
internal static partial class BulgariaVat
{
    internal const string CountryCode = "BG";
    private const int OtherLength = 10;
    private const int Modulus11 = 11;
    private const int Modulus10 = 10;
    private const int RemainderNeedsFallback = 10;

    private static readonly int[] PrimaryWeights = [1, 2, 3, 4, 5, 6, 7, 8];
    private static readonly int[] FallbackWeights = [3, 4, 5, 6, 7, 8, 9, 10];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number)
    {
        if (number.Length == OtherLength)
        {
            return null;
        }

        var body = number[..^1];
        var checkDigit = number[^1] - '0';
        var remainder = WeightedSum.Compute(body, PrimaryWeights) % Modulus11;
        if (remainder == RemainderNeedsFallback)
        {
            remainder = WeightedSum.Compute(body, FallbackWeights) % Modulus11;
        }

        return remainder % Modulus10 == checkDigit;
    }

    [GeneratedRegex("^\\d{9,10}$")]
    private static partial Regex FormatPattern();
}
