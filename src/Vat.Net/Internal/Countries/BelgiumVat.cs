using System.Text.RegularExpressions;

namespace Vat.Internal.Countries;

/// <summary>
/// Belgium: BTW/TVA/ondernemingsnummer, 10 digits (older registrations were issued with 9
/// digits and are zero-padded back to 10). The last two digits are a mod-97 check.
/// </summary>
internal static partial class BelgiumVat
{
    internal const string CountryCode = "BE";
    private const int FullLength = 10;
    private const int ShortLength = 9;
    private const int CheckDigitsLength = 2;
    private const int Modulus = 97;

    internal static string Canonicalize(string number) =>
        number.Length == ShortLength ? "0" + number : number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    /// <remarks>
    /// The issuing rule for a freshly-registered number sets the check digits to
    /// <c>97 - (body % 97)</c>, which is always in the range 1-97 and therefore never "00" -
    /// a body divisible by 97 is issued with check digits "97", not "00". This method instead
    /// tests the weaker, mathematically equivalent invariant <c>(body + check) % 97 == 0</c>,
    /// which both "97" and "00" satisfy for such a body. That is intentional and matches
    /// python-stdnum's <c>stdnum.be.vat.checksum</c> exactly (verified against the reference
    /// implementation): no real Belgian number ends "00" with a body divisible by 97, so this
    /// never accepts a genuine number it should reject, but a corrupted number ending "00" in
    /// that narrow case would pass. See BelgiumChecksumEdgeCaseTests in the test project for the
    /// pinned reference behavior.
    /// </remarks>
    internal static bool? IsChecksumValid(string number)
    {
        var body = int.Parse(number[..^CheckDigitsLength]);
        var check = int.Parse(number[^CheckDigitsLength..]);
        return (body + check) % Modulus == 0;
    }

    [GeneratedRegex("^[01]\\d{9}$")]
    private static partial Regex FormatPattern();
}
