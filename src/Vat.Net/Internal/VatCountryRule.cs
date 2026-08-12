namespace Vat.Internal;

/// <summary>
/// The validation rule set for a single country: how to reshape the number into its
/// canonical digit layout, how to check its format, and how to check its checksum.
/// </summary>
/// <param name="CountryCode">The two-letter VAT prefix the rule is registered under (for example "DE").</param>
/// <param name="Canonicalize">
/// Reshapes a normalized number (prefix, spaces and dots already stripped) into the
/// exact layout the format and checksum checks expect, for example zero-padding a
/// shortened Belgian or Greek number back to full length.
/// </param>
/// <param name="IsFormatValid">Determines whether a canonicalized number has a structurally valid shape.</param>
/// <param name="IsChecksumValid">
/// Determines whether a format-valid, canonicalized number satisfies the country's check
/// digit rule. Returns null when no checksum rule applies to that particular number, in
/// which case the number is accepted on format alone.
/// </param>
internal sealed record VatCountryRule(
    string CountryCode,
    Func<string, string> Canonicalize,
    Func<string, bool> IsFormatValid,
    Func<string, bool?> IsChecksumValid);
