using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>
/// Austria: UID, format "U" followed by 8 digits. The last digit is a Luhn-derived check digit.
/// </summary>
internal static partial class AustriaVat
{
    internal const string CountryCode = "AT";
    private const int DigitsLength = 8;
    private const int CheckDigitModulus = 10;
    private const int CheckDigitBase = 6;

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number)
    {
        var digits = number[1..];
        var body = digits[..(DigitsLength - 1)];
        var checkDigit = digits[DigitsLength - 1] - '0';
        var expected = FloorMath.Mod(CheckDigitBase - LuhnChecksum.Compute(body), CheckDigitModulus);
        return expected == checkDigit;
    }

    [GeneratedRegex("^U\\d{8}$")]
    private static partial Regex FormatPattern();
}
