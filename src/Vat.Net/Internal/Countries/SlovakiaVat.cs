using System.Text.RegularExpressions;

namespace Vat.Internal.Countries;

/// <summary>
/// Slovakia: IC DPH, 10 digits. The whole number, read as an integer, must be divisible by 11.
/// </summary>
internal static partial class SlovakiaVat
{
    internal const string CountryCode = "SK";
    private const int Modulus = 11;
    private static readonly char[] ValidThirdDigits = ['2', '3', '4', '7', '8', '9'];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) =>
        FormatPattern().IsMatch(number) && ValidThirdDigits.Contains(number[2]);

    internal static bool? IsChecksumValid(string number) => long.Parse(number) % Modulus == 0;

    [GeneratedRegex("^[1-9]\\d{9}$")]
    private static partial Regex FormatPattern();
}
