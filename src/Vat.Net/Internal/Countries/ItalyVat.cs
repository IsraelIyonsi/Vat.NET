using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>
/// Italy: Partita IVA, 11 digits: a 7-digit company identifier, a 3-digit province code and
/// a Luhn check digit.
/// </summary>
internal static partial class ItalyVat
{
    internal const string CountryCode = "IT";
    private const int CompanyIdLength = 7;
    private const int ProvinceCodeLength = 3;
    private static readonly string[] SpecialProvinceCodes = ["120", "121", "888", "999"];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number)
    {
        if (!FormatPattern().IsMatch(number) || number[..CompanyIdLength] == "0000000")
        {
            return false;
        }

        var provinceCode = number.Substring(CompanyIdLength, ProvinceCodeLength);
        return (string.CompareOrdinal(provinceCode, "001") >= 0 && string.CompareOrdinal(provinceCode, "100") <= 0)
            || SpecialProvinceCodes.Contains(provinceCode);
    }

    internal static bool? IsChecksumValid(string number) => LuhnChecksum.IsValid(number);

    [GeneratedRegex("^\\d{11}$")]
    private static partial Regex FormatPattern();
}
