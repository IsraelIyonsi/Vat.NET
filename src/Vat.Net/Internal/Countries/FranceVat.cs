using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>
/// France: numero de TVA, a 2-character key followed by the 9-digit SIREN company
/// identifier. All-numeric keys are checked with a mod-97 formula over the SIREN; keys
/// containing a letter use a related mod-97 formula over the key itself. The SIREN is also
/// independently Luhn-checked, unless it is the reserved all-zero Monaco prefix.
/// </summary>
internal static partial class FranceVat
{
    internal const string CountryCode = "FR";
    private const int KeyLength = 2;
    private const int SirenLength = 9;
    private const string MonacoSirenPrefix = "000";
    private const int MonacoSirenPrefixLength = 3;

    // Digits 0-9 followed by letters A-Z excluding I and O, matching the reference alphabet.
    private const string KeyAlphabet = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
    private const int Modulus97 = 97;
    private const int NumericKeySirenSuffix = 12;
    private const int DigitKeyMultiplier = 24;
    private const int DigitKeyOffset = 10;
    private const int LetterKeyMultiplier = 34;
    private const int LetterKeyOffset = 100;
    private const int AlphanumericKeyModulus = 11;

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number)
    {
        if (!FormatPattern().IsMatch(number))
        {
            return false;
        }

        var key = number[..KeyLength];
        var siren = number[KeyLength..];
        return KeyAlphabet.Contains(key[0]) && KeyAlphabet.Contains(key[1]) && siren.All(char.IsAsciiDigit);
    }

    internal static bool? IsChecksumValid(string number)
    {
        var key = number[..KeyLength];
        var siren = number[KeyLength..];

        if (siren[..MonacoSirenPrefixLength] != MonacoSirenPrefix && !LuhnChecksum.IsValid(siren))
        {
            return false;
        }

        return key.All(char.IsAsciiDigit)
            ? IsNumericKeyValid(key, siren)
            : IsAlphanumericKeyValid(key, siren);
    }

    private static bool IsNumericKeyValid(string key, string siren) =>
        int.Parse(key) == long.Parse(siren + NumericKeySirenSuffix.ToString()) % Modulus97;

    private static bool IsAlphanumericKeyValid(string key, string siren)
    {
        var firstIndex = KeyAlphabet.IndexOf(key[0]);
        var secondIndex = KeyAlphabet.IndexOf(key[1]);
        var check = char.IsAsciiDigit(key[0])
            ? firstIndex * DigitKeyMultiplier + secondIndex - DigitKeyOffset
            : firstIndex * LetterKeyMultiplier + secondIndex - LetterKeyOffset;

        var sirenValue = int.Parse(siren);
        var left = FloorMath.Mod(sirenValue + 1 + FloorMath.FloorDiv(check, AlphanumericKeyModulus), AlphanumericKeyModulus);
        return left == FloorMath.Mod(check, AlphanumericKeyModulus);
    }

    [GeneratedRegex("^[0-9A-Z]{11}$")]
    private static partial Regex FormatPattern();
}
