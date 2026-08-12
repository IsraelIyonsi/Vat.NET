using Vat.Internal;

namespace Vat;

/// <summary>
/// Validates European Union and United Kingdom VAT registration numbers entirely offline,
/// by country-specific format rules and, where the country defines one, its check digit or
/// checksum algorithm. No network call is made; nothing is checked against VIES.
/// </summary>
public static class VatNumberValidator
{
    /// <summary>
    /// Determines whether <paramref name="vatNumber"/> is a valid VAT number, inferring the
    /// country from its leading two-letter prefix.
    /// </summary>
    public static bool IsValid(string? vatNumber) => Validate(vatNumber).IsValid;

    /// <summary>
    /// Determines whether <paramref name="number"/> is a valid VAT number for
    /// <paramref name="countryCode"/>.
    /// </summary>
    public static bool IsValid(string? countryCode, string? number) => Validate(countryCode, number).IsValid;

    /// <summary>
    /// Validates <paramref name="vatNumber"/>, inferring the country from its leading
    /// two-letter prefix, and returns the full validation detail.
    /// </summary>
    /// <remarks>
    /// The input is normalized by stripping spaces, dots and the leading country prefix
    /// before format and checksum rules are applied.
    /// </remarks>
    public static VatValidationResult Validate(string? vatNumber)
    {
        if (string.IsNullOrWhiteSpace(vatNumber))
        {
            return VatValidationResult.Fail(VatValidationFailure.EmptyInput);
        }

        var cleaned = VatNumberNormalizer.StripSpacesAndDots(vatNumber);
        var (prefix, number) = VatNumberNormalizer.SplitLeadingPrefix(cleaned);
        if (prefix is null)
        {
            return VatValidationResult.Fail(VatValidationFailure.UnknownCountry);
        }

        return ValidateNormalized(prefix, number);
    }

    /// <summary>
    /// Validates <paramref name="number"/> against the rules for <paramref name="countryCode"/>
    /// and returns the full validation detail.
    /// </summary>
    /// <remarks>
    /// The number is normalized by stripping spaces, dots and a redundant leading country
    /// prefix (if present) before format and checksum rules are applied.
    /// </remarks>
    public static VatValidationResult Validate(string? countryCode, string? number)
    {
        if (string.IsNullOrWhiteSpace(countryCode) || string.IsNullOrWhiteSpace(number))
        {
            return VatValidationResult.Fail(VatValidationFailure.EmptyInput);
        }

        var normalizedCountryCode = VatNumberNormalizer.StripSpacesAndDots(countryCode);
        var cleanedNumber = VatNumberNormalizer.StripSpacesAndDots(number);
        cleanedNumber = VatNumberNormalizer.RemoveMatchingPrefix(cleanedNumber, normalizedCountryCode);

        return ValidateNormalized(normalizedCountryCode, cleanedNumber);
    }

    private static VatValidationResult ValidateNormalized(string countryCode, string number)
    {
        var rule = VatCountryRegistry.Find(countryCode);
        if (rule is null)
        {
            return VatValidationResult.Fail(VatValidationFailure.UnknownCountry, countryCode);
        }

        var canonicalNumber = rule.Canonicalize(number);
        if (!rule.IsFormatValid(canonicalNumber))
        {
            return VatValidationResult.Fail(VatValidationFailure.InvalidFormat, rule.CountryCode, canonicalNumber);
        }

        var checksumOutcome = rule.IsChecksumValid(canonicalNumber);
        if (checksumOutcome == false)
        {
            return VatValidationResult.Fail(VatValidationFailure.InvalidChecksum, rule.CountryCode, canonicalNumber);
        }

        return VatValidationResult.Success(rule.CountryCode, canonicalNumber, checksumOutcome == true);
    }
}
