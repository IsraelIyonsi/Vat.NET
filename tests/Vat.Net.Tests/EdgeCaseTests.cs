using Vat;

namespace Vat.Net.Tests;

public class EdgeCaseTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_single_argument_reports_empty_input(string? input)
    {
        var result = VatNumberValidator.Validate(input);

        Assert.False(result.IsValid);
        Assert.Equal(VatValidationFailure.EmptyInput, result.Failure);
        Assert.Null(result.CountryCode);
        Assert.Null(result.NormalizedNumber);
    }

    [Theory]
    [InlineData(null, "136695976")]
    [InlineData("DE", null)]
    [InlineData("", "136695976")]
    [InlineData("DE", "")]
    [InlineData(" ", " ")]
    public void Validate_two_argument_reports_empty_input(string? countryCode, string? number)
    {
        var result = VatNumberValidator.Validate(countryCode, number);

        Assert.False(result.IsValid);
        Assert.Equal(VatValidationFailure.EmptyInput, result.Failure);
    }

    [Theory]
    [InlineData("135")]
    [InlineData("1234567890123")]
    [InlineData("!!invalid!!")]
    public void Validate_single_argument_reports_unknown_country_when_no_letter_prefix_present(string input)
    {
        var result = VatNumberValidator.Validate(input);

        Assert.False(result.IsValid);
        Assert.Equal(VatValidationFailure.UnknownCountry, result.Failure);
        Assert.Null(result.CountryCode);
    }

    [Theory]
    [InlineData("ZZ123456789")]
    [InlineData("XX000000000")]
    public void Validate_single_argument_reports_unknown_country_for_unsupported_prefix(string input)
    {
        var result = VatNumberValidator.Validate(input);

        Assert.False(result.IsValid);
        Assert.Equal(VatValidationFailure.UnknownCountry, result.Failure);
    }

    [Fact]
    public void Validate_two_argument_reports_unknown_country_for_unsupported_code()
    {
        var result = VatNumberValidator.Validate("US", "123456789");

        Assert.False(result.IsValid);
        Assert.Equal(VatValidationFailure.UnknownCountry, result.Failure);
        Assert.Equal("US", result.CountryCode);
    }

    [Theory]
    [InlineData("DE12345678")]
    [InlineData("DE1234567890")]
    [InlineData("DEABCDEFGHI")]
    [InlineData("DE012345678")]
    public void Validate_rejects_structurally_malformed_numbers_as_invalid_format(string input)
    {
        var result = VatNumberValidator.Validate(input);

        Assert.False(result.IsValid);
        Assert.Equal(VatValidationFailure.InvalidFormat, result.Failure);
        Assert.False(result.IsChecksumVerified);
    }

    [Fact]
    public void IsValid_two_argument_matches_Validate_two_argument()
    {
        Assert.Equal(
            VatNumberValidator.Validate("DE", "136695976").IsValid,
            VatNumberValidator.IsValid("DE", "136695976"));

        Assert.Equal(
            VatNumberValidator.Validate("DE", "136695978").IsValid,
            VatNumberValidator.IsValid("DE", "136695978"));
    }

    [Fact]
    public void Failed_result_never_reports_checksum_as_verified()
    {
        var invalidFormat = VatNumberValidator.Validate("DE12345678");
        var invalidChecksum = VatNumberValidator.Validate("DE136695978");
        var unknownCountry = VatNumberValidator.Validate("ZZ123456789");

        Assert.False(invalidFormat.IsChecksumVerified);
        Assert.False(invalidChecksum.IsChecksumVerified);
        Assert.False(unknownCountry.IsChecksumVerified);
    }
}
