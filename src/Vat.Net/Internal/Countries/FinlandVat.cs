using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>Finland: ALV nro (Arvonlisaveronumero), 8 digits with a weighted mod-11 checksum.</summary>
internal static partial class FinlandVat
{
    internal const string CountryCode = "FI";
    private const int Modulus = 11;

    private static readonly int[] Weights = [7, 9, 10, 5, 8, 4, 2, 1];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number) =>
        WeightedSum.Compute(number, Weights) % Modulus == 0;

    [GeneratedRegex("^\\d{8}$")]
    private static partial Regex FormatPattern();
}
