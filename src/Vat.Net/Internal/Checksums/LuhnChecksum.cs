namespace Vat.Internal.Checksums;

/// <summary>
/// The Luhn algorithm, used by Italy's Partita IVA, France's SIREN (embedded in the
/// French VAT number) and Sweden's Organisationsnummer (embedded in the Swedish VAT number).
/// </summary>
internal static class LuhnChecksum
{
    private const int Radix = 10;
    private const int DoubledDigitOffset = 9;
    private const int DoubledDigitThreshold = 9;
    private const int ValidRemainder = 0;

    /// <summary>
    /// Computes the Luhn sum modulo 10 over <paramref name="digits"/>, doubling every
    /// second digit starting from the rightmost. A valid number has a result of zero.
    /// </summary>
    internal static int Compute(string digits)
    {
        var sum = 0;
        var doubleNext = false;
        for (var i = digits.Length - 1; i >= 0; i--)
        {
            var digit = digits[i] - '0';
            if (doubleNext)
            {
                digit *= 2;
                if (digit > DoubledDigitThreshold)
                {
                    digit -= DoubledDigitOffset;
                }
            }

            sum += digit;
            doubleNext = !doubleNext;
        }

        return sum % Radix;
    }

    /// <summary>Determines whether <paramref name="digits"/> passes the Luhn check.</summary>
    internal static bool IsValid(string digits) => Compute(digits) == ValidRemainder;
}
