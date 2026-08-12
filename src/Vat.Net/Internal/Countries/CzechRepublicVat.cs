using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>
/// Czechia: DIC, 8, 9 or 10 digits. The 8-digit legal entity form and the 9-digit special
/// form (starting with "6") each have a dedicated weighted mod-11 checksum.
/// </summary>
/// <remarks>
/// The remaining 9 and 10-digit forms encode the holder's birth number (rodne cislo),
/// whose validity depends on parsing a real calendar date. This package does not implement
/// birth-number validation, so those numbers are accepted on format alone.
/// </remarks>
internal static partial class CzechRepublicVat
{
    internal const string CountryCode = "CZ";
    private const int LegalEntityLength = 8;
    private const int SpecialLength = 9;
    private const char SpecialCaseLeadingDigit = '6';
    private const char LegalEntityForbiddenLeadingDigit = '9';
    private const int Modulus11 = 11;
    private const int Modulus10 = 10;
    private const int LegalEntityCheckBase = 8;
    private const int SpecialCaseCheckBase = 8;
    private const int SpecialCaseOffset = 10;

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number)
    {
        if (!FormatPattern().IsMatch(number))
        {
            return false;
        }

        return number.Length != LegalEntityLength || number[0] != LegalEntityForbiddenLeadingDigit;
    }

    internal static bool? IsChecksumValid(string number)
    {
        if (number.Length == LegalEntityLength)
        {
            return IsLegalEntityChecksumValid(number);
        }

        if (number.Length == SpecialLength && number[0] == SpecialCaseLeadingDigit)
        {
            return IsSpecialCaseChecksumValid(number);
        }

        return null;
    }

    private static bool IsLegalEntityChecksumValid(string number)
    {
        var checkDigit = number[^1] - '0';
        var sum = 0;
        for (var i = 0; i < LegalEntityLength - 1; i++)
        {
            sum += (LegalEntityCheckBase - i) * (number[i] - '0');
        }

        var remainder = (Modulus11 - sum % Modulus11) % Modulus11;
        var expected = (remainder == 0 ? 1 : remainder) % Modulus10;
        return expected == checkDigit;
    }

    private static bool IsSpecialCaseChecksumValid(string number)
    {
        var checkDigit = number[^1] - '0';
        var middle = number[1..^1];
        var sum = 0;
        for (var i = 0; i < middle.Length; i++)
        {
            sum += (SpecialCaseCheckBase - i) * (middle[i] - '0');
        }

        var remainder = sum % Modulus11;
        var expected = FloorMath.Mod(SpecialCaseCheckBase - FloorMath.Mod(SpecialCaseOffset - remainder, Modulus11), Modulus10);
        return expected == checkDigit;
    }

    [GeneratedRegex("^(\\d{8}|\\d{9}|\\d{10})$")]
    private static partial Regex FormatPattern();
}
