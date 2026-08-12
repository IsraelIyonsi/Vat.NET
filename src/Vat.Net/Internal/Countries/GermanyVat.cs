using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>Germany: Ust-IdNr, 9 digits validated with the ISO 7064 Mod 11, 10 algorithm.</summary>
internal static partial class GermanyVat
{
    internal const string CountryCode = "DE";

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number) => Iso7064Mod1110.IsValid(number);

    [GeneratedRegex("^[1-9]\\d{8}$")]
    private static partial Regex FormatPattern();
}
