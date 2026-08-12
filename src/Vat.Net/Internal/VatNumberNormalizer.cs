using System.Text;

namespace Vat.Internal;

/// <summary>
/// Strips the formatting noise (spaces, dots, an optional leading country prefix) that
/// real-world VAT numbers are copied around with, leaving the bare alphanumeric payload
/// each country's validator operates on.
/// </summary>
internal static class VatNumberNormalizer
{
    private const int PrefixLength = 2;
    private const char DotSeparator = '.';

    /// <summary>Removes whitespace and dot separators and upper-cases the result.</summary>
    internal static string StripSpacesAndDots(string input)
    {
        var builder = new StringBuilder(input.Length);
        foreach (var character in input)
        {
            if (character == DotSeparator || char.IsWhiteSpace(character))
            {
                continue;
            }

            builder.Append(char.ToUpperInvariant(character));
        }

        return builder.ToString();
    }

    /// <summary>
    /// Splits a cleaned string into its two-letter country prefix and the remaining
    /// number, when the string starts with two letters. Returns null for the prefix
    /// otherwise, with the full input returned unchanged as the number.
    /// </summary>
    internal static (string? Prefix, string Number) SplitLeadingPrefix(string cleaned)
    {
        if (cleaned.Length >= PrefixLength &&
            char.IsAsciiLetterUpper(cleaned[0]) &&
            char.IsAsciiLetterUpper(cleaned[1]))
        {
            return (cleaned[..PrefixLength], cleaned[PrefixLength..]);
        }

        return (null, cleaned);
    }

    /// <summary>Removes a redundant leading country prefix from <paramref name="cleaned"/> when it matches <paramref name="countryCode"/>.</summary>
    internal static string RemoveMatchingPrefix(string cleaned, string countryCode)
    {
        return cleaned.StartsWith(countryCode, StringComparison.Ordinal)
            ? cleaned[countryCode.Length..]
            : cleaned;
    }
}
