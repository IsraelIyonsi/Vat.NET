using System.Numerics;
using System.Text;

namespace Vat.Internal.Checksums;

/// <summary>
/// The ISO 7064 Mod 97, 10 check digit algorithm, used by the alternate (2020-onwards)
/// Dutch btw-identificatienummer format for sole traders.
/// </summary>
internal static class Iso7064Mod9710
{
    private const int Modulus = 97;
    private const int ValidCheck = 1;
    private const int DigitBase = 10;

    /// <summary>
    /// Determines whether the base-36 digits of <paramref name="alphanumeric"/>, read as
    /// a single decimal number, are congruent to 1 modulo 97.
    /// </summary>
    internal static bool IsValid(string alphanumeric)
    {
        var expanded = new StringBuilder();
        foreach (var character in alphanumeric)
        {
            expanded.Append(ToBase36Value(character));
        }

        var checksum = BigInteger.Parse(expanded.ToString()) % Modulus;
        return checksum == ValidCheck;
    }

    private static int ToBase36Value(char character) => character switch
    {
        >= '0' and <= '9' => character - '0',
        >= 'A' and <= 'Z' => character - 'A' + DigitBase,
        _ => throw new ArgumentOutOfRangeException(nameof(character), character, "Expected an upper-case alphanumeric character."),
    };
}
