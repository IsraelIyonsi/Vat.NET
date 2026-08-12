using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>Slovenia: ID za DDV (Davcna stevilka), 8 digits with a weighted mod-11 checksum.</summary>
internal static partial class SloveniaVat
{
    internal const string CountryCode = "SI";
    private const int Modulus = 11;
    private const int UnrepresentableRemainder = 11;
    private const int WrappedCheckDigit = 10;
    private const int WrappedCheckDigitValue = 0;
    private const int NeverMatches = -1;

    private static readonly int[] Weights = [8, 7, 6, 5, 4, 3, 2];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number)
    {
        var checkDigit = number[^1] - '0';
        var sum = WeightedSum.Compute(number, Weights);
        var computed = Modulus - sum % Modulus;
        var expected = computed switch
        {
            WrappedCheckDigit => WrappedCheckDigitValue,
            UnrepresentableRemainder => NeverMatches,
            _ => computed,
        };
        return expected == checkDigit;
    }

    [GeneratedRegex("^[1-9]\\d{7}$")]
    private static partial Regex FormatPattern();
}
