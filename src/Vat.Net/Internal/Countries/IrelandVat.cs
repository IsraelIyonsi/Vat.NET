using System.Text.RegularExpressions;

namespace Vat.Internal.Countries;

/// <summary>
/// Ireland: VAT registration number. The current format is 7 digits followed by one or two
/// letters; the legacy format is a digit, a letter (or "+"/"*"), 5 digits and a letter. Both
/// share the same mod-23 check letter algorithm once reordered into 7 digits plus a letter.
/// </summary>
internal static partial class IrelandVat
{
    internal const string CountryCode = "IE";
    private const int BodyDigitLength = 7;
    private const int Modulus = 23;
    private const int SecondLetterWeight = 9;
    private const char NoSecondLetterPlaceholder = 'W';

    private const string CheckLetterAlphabet = "WABCDEFGHIJKLMNOPQRSTUV";
    private const string LegacySecondCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ+*";

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) =>
        NewFormatPattern().IsMatch(number) || LegacyFormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number)
    {
        if (IsDigits(number[..BodyDigitLength]))
        {
            var secondLetter = number.Length > BodyDigitLength + 1 ? number[BodyDigitLength + 1] : NoSecondLetterPlaceholder;
            var expected = CalculateCheckLetter(number[..BodyDigitLength], secondLetter);
            return expected == number[BodyDigitLength];
        }

        // Legacy format: digit, letter/symbol, 5 digits, check letter.
        var reordered = (number[2..7] + number[0]).PadLeft(BodyDigitLength, '0');
        var expectedLegacy = CalculateCheckLetter(reordered, NoSecondLetterPlaceholder);
        return expectedLegacy == number[7];
    }

    private static char CalculateCheckLetter(string sevenDigits, char secondLetter)
    {
        var sum = 0;
        for (var i = 0; i < BodyDigitLength; i++)
        {
            sum += (BodyDigitLength + 1 - i) * (sevenDigits[i] - '0');
        }

        sum += SecondLetterWeight * CheckLetterAlphabet.IndexOf(secondLetter);
        return CheckLetterAlphabet[sum % Modulus];
    }

    private static bool IsDigits(string value) => value.All(char.IsAsciiDigit);

    [GeneratedRegex("^\\d{7}[A-Z][A-Z]?$")]
    private static partial Regex NewFormatPattern();

    [GeneratedRegex("^\\d[A-Z+*]\\d{5}[A-Z]$")]
    private static partial Regex LegacyFormatPattern();
}
