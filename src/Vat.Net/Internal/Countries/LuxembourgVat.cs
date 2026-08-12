using System.Text.RegularExpressions;

namespace Vat.Internal.Countries;

/// <summary>Luxembourg: n TVA, 8 digits where the last two are the first six modulo 89.</summary>
internal static partial class LuxembourgVat
{
    internal const string CountryCode = "LU";
    private const int BodyLength = 6;
    private const int Modulus = 89;

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number)
    {
        var body = int.Parse(number[..BodyLength]);
        var checkDigits = int.Parse(number[BodyLength..]);
        return body % Modulus == checkDigits;
    }

    [GeneratedRegex("^\\d{8}$")]
    private static partial Regex FormatPattern();
}
