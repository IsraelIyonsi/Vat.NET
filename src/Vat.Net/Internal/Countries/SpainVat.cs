using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>
/// Spain: NIF, a 9-character number covering three underlying schemes: the DNI (Documento
/// Nacional de Identidad) for Spanish residents, the NIE (Numero de Identificacion de
/// Extranjero) for foreign residents, and the CIF (Codigo de Identificacion Fiscal) for
/// legal entities. All three resolve to a mod-23 or Luhn-derived check character.
/// </summary>
internal static class SpainVat
{
    internal const string CountryCode = "ES";
    private const int Length = 9;
    private const int Modulus23 = 23;
    private const int Modulus10 = 10;
    private const char LuhnPlaceholderDigit = '0';
    private const string DniCheckLetters = "TRWAGMYFPDXBNJZSQVHLCKE";
    private const string NieLeadingLetters = "XYZ";
    private const string LegacyNifLetters = "KLM";
    private const string CifOrganizationLetters = "ABCDEFGHJNPQRSUVW";
    private const string CifDigitToLetter = "JABCDEFGHI";

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number)
    {
        if (number.Length != Length)
        {
            return false;
        }

        if (char.IsAsciiDigit(number[0]))
        {
            return IsAllDigits(number[..8]) && char.IsAsciiLetterUpper(number[8]);
        }

        if (NieLeadingLetters.Contains(number[0]) || LegacyNifLetters.Contains(number[0]))
        {
            return IsAllDigits(number[1..8]) && char.IsAsciiLetterUpper(number[8]);
        }

        if (CifOrganizationLetters.Contains(number[0]))
        {
            return IsAllDigits(number[1..8]) && char.IsAsciiLetterOrDigit(number[8]);
        }

        return false;
    }

    internal static bool? IsChecksumValid(string number)
    {
        if (char.IsAsciiDigit(number[0]))
        {
            return DniCheckLetter(number[..8]) == number[8];
        }

        if (NieLeadingLetters.Contains(number[0]))
        {
            var replaced = NieLeadingLetters.IndexOf(number[0]).ToString() + number[1..8];
            return DniCheckLetter(replaced) == number[8];
        }

        if (LegacyNifLetters.Contains(number[0]))
        {
            return DniCheckLetter(number[1..8]) == number[8];
        }

        // CIF: the trailing character may legitimately be either the Luhn-derived digit
        // or its letter equivalent, depending on the organisation type.
        var digitCandidate = LuhnCheckDigit(number[1..8]);
        var letterCandidate = CifDigitToLetter[digitCandidate];
        return number[8] == (char)('0' + digitCandidate) || number[8] == letterCandidate;
    }

    private static char DniCheckLetter(string eightDigits) => DniCheckLetters[int.Parse(eightDigits) % Modulus23];

    private static int LuhnCheckDigit(string sevenDigits)
    {
        var sum = LuhnChecksum.Compute(sevenDigits + LuhnPlaceholderDigit);
        return (Modulus10 - sum) % Modulus10;
    }

    private static bool IsAllDigits(string value) => value.All(char.IsAsciiDigit);
}
