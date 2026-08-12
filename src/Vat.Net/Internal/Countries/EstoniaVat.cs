using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>Estonia: KMKR (Kaibemaksukohuslase), 9 digits with a weighted mod-10 checksum.</summary>
internal static partial class EstoniaVat
{
    internal const string CountryCode = "EE";
    private const int Modulus = 10;

    private static readonly int[] Weights = [3, 7, 1, 3, 7, 1, 3, 7, 1];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number) =>
        WeightedSum.Compute(number, Weights) % Modulus == 0;

    [GeneratedRegex("^\\d{9}$")]
    private static partial Regex FormatPattern();
}
