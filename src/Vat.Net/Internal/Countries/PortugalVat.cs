using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>Portugal: NIF (Numero de identificacao fiscal), 9 digits with a weighted mod-11 checksum.</summary>
internal static partial class PortugalVat
{
    internal const string CountryCode = "PT";
    private const int Modulus11 = 11;
    private const int Modulus10 = 10;

    private static readonly int[] Weights = [9, 8, 7, 6, 5, 4, 3, 2];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number)
    {
        var checkDigit = number[^1] - '0';
        var sum = WeightedSum.Compute(number, Weights);
        var expected = (Modulus11 - sum % Modulus11) % Modulus10;
        return expected == checkDigit;
    }

    [GeneratedRegex("^[1-9]\\d{8}$")]
    private static partial Regex FormatPattern();
}
