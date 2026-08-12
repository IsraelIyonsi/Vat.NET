using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>Malta: VAT number, 8 digits with a weighted mod-37 checksum.</summary>
internal static partial class MaltaVat
{
    internal const string CountryCode = "MT";
    private const int Modulus = 37;

    private static readonly int[] Weights = [3, 4, 6, 7, 8, 9, 10, 1];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number) =>
        WeightedSum.Compute(number, Weights) % Modulus == 0;

    [GeneratedRegex("^[1-9]\\d{7}$")]
    private static partial Regex FormatPattern();
}
