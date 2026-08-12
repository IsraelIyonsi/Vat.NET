using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>
/// Sweden: Momsregistreringsnummer, the 10-digit Organisationsnummer (Luhn-checked)
/// followed by the literal branch suffix "01".
/// </summary>
internal static partial class SwedenVat
{
    internal const string CountryCode = "SE";
    private const int OrgNumberLength = 10;

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => FormatPattern().IsMatch(number);

    internal static bool? IsChecksumValid(string number) => LuhnChecksum.IsValid(number[..OrgNumberLength]);

    [GeneratedRegex("^\\d{10}01$")]
    private static partial Regex FormatPattern();
}
