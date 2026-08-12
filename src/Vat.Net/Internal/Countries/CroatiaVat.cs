using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>Croatia: OIB, 11 digits validated with the ISO 7064 Mod 11, 10 algorithm.</summary>
internal static partial class CroatiaVat
{
    internal const string CountryCode = "HR";

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number) => Iso7064Mod1110.IsValid(number);

    [GeneratedRegex("^\\d{11}$")]
    private static partial Regex FormatPattern();
}
