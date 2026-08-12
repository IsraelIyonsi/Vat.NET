using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>Denmark: CVR (Momsregistreringsnummer), 8 digits with a weighted mod-11 checksum.</summary>
internal static partial class DenmarkVat
{
    internal const string CountryCode = "DK";
    private const int Modulus = 11;

    private static readonly int[] Weights = [2, 7, 6, 5, 4, 3, 2, 1];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number) =>
        WeightedSum.Compute(number, Weights) % Modulus == 0;

    [GeneratedRegex("^[1-9]\\d{7}$")]
    private static partial Regex FormatPattern();
}
