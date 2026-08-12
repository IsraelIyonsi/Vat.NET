using Vat;

namespace Vat.Net.Tests;

public class ChecksumFixtureTests
{
    [Theory]
    [MemberData(nameof(VatFixtures.ValidNumbers), MemberType = typeof(VatFixtures))]
    public void Validate_accepts_canonical_valid_number_with_checksum_verified(
        string rawInput, string expectedCountryCode, string expectedNormalizedNumber, bool expectedChecksumVerified)
    {
        var result = VatNumberValidator.Validate(rawInput);

        Assert.True(result.IsValid);
        Assert.True(VatNumberValidator.IsValid(rawInput));
        Assert.Equal(expectedCountryCode, result.CountryCode);
        Assert.Equal(expectedNormalizedNumber, result.NormalizedNumber);
        Assert.Equal(expectedChecksumVerified, result.IsChecksumVerified);
        Assert.Equal(VatValidationFailure.None, result.Failure);
    }

    [Theory]
    [MemberData(nameof(VatFixtures.InvalidChecksumNumbers), MemberType = typeof(VatFixtures))]
    public void Validate_rejects_corrupted_checksum(string rawInput, string expectedCountryCode)
    {
        var result = VatNumberValidator.Validate(rawInput);

        Assert.False(result.IsValid);
        Assert.False(VatNumberValidator.IsValid(rawInput));
        Assert.Equal(expectedCountryCode, result.CountryCode);
        Assert.Equal(VatValidationFailure.InvalidChecksum, result.Failure);
        Assert.False(result.IsChecksumVerified);
    }

    [Theory]
    [MemberData(nameof(VatFixtures.FormatOnlyNumbers), MemberType = typeof(VatFixtures))]
    public void Validate_accepts_format_only_number_without_verifying_checksum(string rawInput, string expectedCountryCode)
    {
        var result = VatNumberValidator.Validate(rawInput);

        Assert.True(result.IsValid);
        Assert.Equal(expectedCountryCode, result.CountryCode);
        Assert.False(result.IsChecksumVerified);
        Assert.Equal(VatValidationFailure.None, result.Failure);
    }

    [Theory]
    [MemberData(nameof(VatFixtures.AdditionalValidNumbers), MemberType = typeof(VatFixtures))]
    public void Validate_accepts_additional_valid_number_covering_a_specific_sub_format(
        string rawInput, string expectedCountryCode, string expectedNormalizedNumber, bool expectedChecksumVerified)
    {
        var result = VatNumberValidator.Validate(rawInput);

        Assert.True(result.IsValid);
        Assert.True(VatNumberValidator.IsValid(rawInput));
        Assert.Equal(expectedCountryCode, result.CountryCode);
        Assert.Equal(expectedNormalizedNumber, result.NormalizedNumber);
        Assert.Equal(expectedChecksumVerified, result.IsChecksumVerified);
        Assert.Equal(VatValidationFailure.None, result.Failure);
    }

    [Theory]
    [MemberData(nameof(VatFixtures.AdditionalInvalidChecksumNumbers), MemberType = typeof(VatFixtures))]
    public void Validate_rejects_additional_corrupted_checksum_covering_a_specific_sub_format(
        string rawInput, string expectedCountryCode)
    {
        var result = VatNumberValidator.Validate(rawInput);

        Assert.False(result.IsValid);
        Assert.False(VatNumberValidator.IsValid(rawInput));
        Assert.Equal(expectedCountryCode, result.CountryCode);
        Assert.Equal(VatValidationFailure.InvalidChecksum, result.Failure);
        Assert.False(result.IsChecksumVerified);
    }

    [Theory]
    [MemberData(nameof(VatFixtures.ValidNumbers), MemberType = typeof(VatFixtures))]
    public void Validate_with_explicit_country_code_agrees_with_prefix_inference(
        string rawInput, string expectedCountryCode, string expectedNormalizedNumber, bool expectedChecksumVerified)
    {
        var prefixed = VatNumberValidator.Validate(rawInput);
        var explicitResult = VatNumberValidator.Validate(expectedCountryCode, expectedNormalizedNumber);

        Assert.Equal(prefixed.IsValid, explicitResult.IsValid);
        Assert.Equal(prefixed.CountryCode, explicitResult.CountryCode);
        Assert.Equal(prefixed.NormalizedNumber, explicitResult.NormalizedNumber);
        Assert.Equal(prefixed.IsChecksumVerified, explicitResult.IsChecksumVerified);
        Assert.Equal(expectedChecksumVerified, explicitResult.IsChecksumVerified);
    }
}
