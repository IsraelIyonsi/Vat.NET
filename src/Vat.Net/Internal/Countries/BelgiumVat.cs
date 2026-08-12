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

    internal static bool? IsChecksumValid(string number)
    {
        var body = int.Parse(number[..^CheckDigitsLength]);
        var check = int.Parse(number[^CheckDigitsLength..]);
        return (body + check) % Modulus == 0;
    }

    [GeneratedRegex("^[01]\\d{9}$")]
    private static partial Regex FormatPattern();
}
