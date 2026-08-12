namespace Vat;

/// <summary>The outcome of validating a VAT number: whether it is valid, and why not if it is not.</summary>
public sealed class VatValidationResult
{
    private VatValidationResult(
        bool isValid,
        string? countryCode,
        string? normalizedNumber,
        bool isChecksumVerified,
        VatValidationFailure failure)
    {
        IsValid = isValid;
        CountryCode = countryCode;
        NormalizedNumber = normalizedNumber;
        IsChecksumVerified = isChecksumVerified;
        Failure = failure;
    }

    /// <summary>Whether the VAT number is valid.</summary>
    public bool IsValid { get; }

    /// <summary>
    /// The two-letter country code the number was validated against (using Greece's VAT
    /// prefix "EL" rather than its ISO code "GR"), or null when no country could be
    /// determined.
    /// </summary>
    public string? CountryCode { get; }

    /// <summary>
    /// The number with its country prefix, spaces and dots removed, or null when it could
    /// not be normalized at all (for example, empty input).
    /// </summary>
    public string? NormalizedNumber { get; }

    /// <summary>
    /// True when <see cref="IsValid"/> is true because the number satisfied its country's
    /// check digit or checksum rule. False when the country has no such rule for this
    /// number (a format-only pass) or when validation failed.
    /// </summary>
    public bool IsChecksumVerified { get; }

    /// <summary>The reason validation failed, or <see cref="VatValidationFailure.None"/> when it succeeded.</summary>
    public VatValidationFailure Failure { get; }

    internal static VatValidationResult Success(string countryCode, string normalizedNumber, bool isChecksumVerified) =>
        new(true, countryCode, normalizedNumber, isChecksumVerified, VatValidationFailure.None);

    internal static VatValidationResult Fail(VatValidationFailure failure, string? countryCode = null, string? normalizedNumber = null) =>
        new(false, countryCode, normalizedNumber, false, failure);
}
