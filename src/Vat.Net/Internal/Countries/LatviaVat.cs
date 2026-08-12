using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>
/// Latvia: PVN, 11 digits. Legal entities (first digit greater than 3) use a weighted
/// mod-11 checksum.
/// </summary>
/// <remarks>
/// Numbers belonging to natural persons (first digit 3 or less) mirror the holder's
/// national personal code, whose check digit formula is not independently confirmed by any
/// primary source this package could verify, so those numbers are accepted on format alone.
/// </remarks>
internal static partial class LatviaVat
{
    internal const string CountryCode = "LV";
    private const char LegalEntityThreshold = '3';
    private const int Modulus = 11;
    private const int ExpectedRemainder = 3;

    private static readonly int[] Weights = [9, 1, 4, 8, 3, 10, 2, 5, 7, 6, 1];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number)
    {
        if (number[0] <= LegalEntityThreshold)
        {
            return null;
        }

        return WeightedSum.Compute(number, Weights) % Modulus == ExpectedRemainder;
    }

    [GeneratedRegex("^\\d{11}$")]
    private static partial Regex FormatPattern();
}
