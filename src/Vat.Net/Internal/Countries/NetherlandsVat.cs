using System.Text.RegularExpressions;
using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>
/// Netherlands: btw-identificatienummer, a 9-digit RSIN/BSN, the literal letter "B", and a
/// 2-digit sequence number. Valid when the 9-digit body passes the BSN "elfproef" check, or
/// (for the alternate numbering introduced in 2020 for sole traders) when the full number
/// passes the ISO 7064 Mod 97, 10 check.
/// </summary>
internal static partial class NetherlandsVat
{
    internal const string CountryCode = "NL";
    private const int BodyLength = 9;
    private const int SequenceStart = 10;
    private const int Modulus11 = 11;

    private static readonly int[] BsnWeights = [9, 8, 7, 6, 5, 4, 3, 2];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number)
    {
        if (!FormatPattern().IsMatch(number))
        {
            return false;
        }

        return int.Parse(number[..BodyLength]) > 0 && int.Parse(number[SequenceStart..]) > 0;
    }

    internal static bool? IsChecksumValid(string number)
    {
        var body = number[..BodyLength];
        var elevenProofOk = (WeightedSum.Compute(body, BsnWeights) - (body[^1] - '0')) % Modulus11 == 0;
        return elevenProofOk || Iso7064Mod9710.IsValid(CountryCode + number);
    }

    [GeneratedRegex("^\\d{9}B\\d{2}$")]
    private static partial Regex FormatPattern();
}
