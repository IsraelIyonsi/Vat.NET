using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>Hungary: ANUM (Kozossegi adoszam), 8 digits with a weighted mod-10 checksum.</summary>
internal static partial class HungaryVat
{
    internal const string CountryCode = "HU";
    private const int Modulus = 10;

    private static readonly int[] Weights = [9, 7, 3, 1, 9, 7, 3, 1];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number) =>
        WeightedSum.Compute(number, Weights) % Modulus == 0;

    [GeneratedRegex("^\\d{8}$")]
    private static partial Regex FormatPattern();
}
