namespace Vat.Internal.Checksums;

/// <summary>
/// The ISO 7064 Mod 11, 10 check digit algorithm, used by Germany's
/// Umsatzsteuer-Identifikationsnummer and Croatia's OIB (embedded in the Croatian VAT number).
/// </summary>
internal static class Iso7064Mod1110
{
    private const int InitialCheck = 5;
    private const int Multiplier = 2;
    private const int Modulus11 = 11;
    private const int Modulus10 = 10;
    private const int ValidCheck = 1;

    /// <summary>
    /// Determines whether <paramref name="digits"/>, including its own trailing check
    /// digit, satisfies the ISO 7064 Mod 11, 10 recurrence.
    /// </summary>
    internal static bool IsValid(string digits)
    {
        var check = InitialCheck;
        foreach (var digitChar in digits)
        {
            var digit = digitChar - '0';
            var carry = check == 0 ? Modulus10 : check;
            check = (carry * Multiplier % Modulus11 + digit) % Modulus10;
        }

        return check == ValidCheck;
    }
}
