using System.Text.RegularExpressions;

namespace Vat.Internal.Countries;

/// <summary>
/// Greece: FPA / AFM, 9 digits (older 8-digit numbers are zero-padded back to 9). Uses the
/// VAT prefix "EL"; the ISO 3166 country code "GR" is accepted as an alias.
/// </summary>
internal static partial class GreeceVat
{
    internal const string CountryCode = "EL";
    internal const string CountryCodeAlias = "GR";
    private const int FullLength = 9;
    private const int ShortLength = 8;
    private const int Doubling = 2;
    private const int Modulus11 = 11;
    private const int Modulus10 = 10;

    internal static string Canonicalize(string number) =>
        number.Length == ShortLength ? "0" + number : number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number)
    {
        var checkDigit = number[^1] - '0';
        var checksum = 0;
        for (var i = 0; i < FullLength - 1; i++)
        {
            checksum = checksum * Doubling + (number[i] - '0');
        }

        var expected = checksum * Doubling % Modulus11 % Modulus10;
        return expected == checkDigit;
    }

    [GeneratedRegex("^\\d{9}$")]
    private static partial Regex FormatPattern();
}
