namespace Vat;

/// <summary>The reason a VAT number failed validation, or <see cref="None"/> when it passed.</summary>
public enum VatValidationFailure
{
    /// <summary>Validation succeeded; there is no failure.</summary>
    None = 0,

    /// <summary>The input was null, empty, or contained only whitespace.</summary>
    EmptyInput,

    /// <summary>No supported country could be determined from the input.</summary>
    UnknownCountry,

    /// <summary>The number does not match the shape (length and character rules) required by its country.</summary>
    InvalidFormat,

    /// <summary>The number has a valid shape but fails its country's check digit or checksum rule.</summary>
    InvalidChecksum,
}
