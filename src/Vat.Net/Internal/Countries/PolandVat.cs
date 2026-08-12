using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>Poland: NIP (Numer Identyfikacji Podatkowej), 10 digits with a weighted mod-11 checksum.</summary>
internal static partial class PolandVat
{
    internal const string CountryCode = "PL";
    private const int Modulus = 11;

    private static readonly int[] Weights = [6, 5, 7, 2, 3, 4, 5, 6, 7, -1];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number) =>
        FloorMath.Mod(WeightedSum.Compute(number, Weights), Modulus) == 0;

    [GeneratedRegex("^\\d{10}$")]
    private static partial Regex FormatPattern();
}
