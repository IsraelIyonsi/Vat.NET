using Vat;

namespace Vat.Net.Tests;

/// <summary>
/// Northern Ireland "XI" numbers share the United Kingdom "GB" rule byte for byte (same
/// format, same restarted-checksum allowance, same government department / health authority
/// forms). These tests reuse the exact GB fixture vectors re-prefixed to XI and prove, per
/// vector, that XI and GB reach the identical validity verdict.
/// </summary>
public class NorthernIrelandTests
{
    private const string GbCountryCode = "GB";
    private const string XiCountryCode = "XI";
    private const int PrefixLength = 2;

    private static string ReprefixToXi(string gbRawInput) => XiCountryCode + gbRawInput[PrefixLength..];

    public static IEnumerable<object[]> ValidGbVectors() =>
        VatFixtures.ValidNumbers()
            .Concat(VatFixtures.AdditionalValidNumbers())
            .Where(row => (string)row[1] == GbCountryCode);

    public static IEnumerable<object[]> InvalidChecksumGbVectors() =>
        VatFixtures.InvalidChecksumNumbers()
            .Concat(VatFixtures.AdditionalInvalidChecksumNumbers())
            .Where(row => (string)row[1] == GbCountryCode);

    [Theory]
    [MemberData(nameof(ValidGbVectors))]
    public void Validate_accepts_valid_GB_vector_reprefixed_to_XI(
        string gbRawInput, string _, string expectedNormalizedNumber, bool expectedChecksumVerified)
    {
        var result = VatNumberValidator.Validate(ReprefixToXi(gbRawInput));

        Assert.True(result.IsValid);
        Assert.Equal(XiCountryCode, result.CountryCode);
        Assert.Equal(expectedNormalizedNumber, result.NormalizedNumber);
        Assert.Equal(expectedChecksumVerified, result.IsChecksumVerified);
        Assert.Equal(VatValidationFailure.None, result.Failure);
    }

    [Theory]
    [MemberData(nameof(InvalidChecksumGbVectors))]
    public void Validate_rejects_invalid_checksum_GB_vector_reprefixed_to_XI(string gbRawInput, string _)
    {
        var result = VatNumberValidator.Validate(ReprefixToXi(gbRawInput));

        Assert.False(result.IsValid);
        Assert.Equal(XiCountryCode, result.CountryCode);
        Assert.Equal(VatValidationFailure.InvalidChecksum, result.Failure);
        Assert.False(result.IsChecksumVerified);
    }

    [Theory]
    [MemberData(nameof(ValidGbVectors))]
    [MemberData(nameof(InvalidChecksumGbVectors))]
    public void XI_and_GB_reach_the_same_verdict_on_the_same_number(string gbRawInput, params object[] _)
    {
        var digits = gbRawInput[PrefixLength..];

        var gbResult = VatNumberValidator.Validate(GbCountryCode, digits);
        var xiResult = VatNumberValidator.Validate(XiCountryCode, digits);

        Assert.Equal(gbResult.IsValid, xiResult.IsValid);
        Assert.Equal(gbResult.NormalizedNumber, xiResult.NormalizedNumber);
        Assert.Equal(gbResult.IsChecksumVerified, xiResult.IsChecksumVerified);
        Assert.Equal(gbResult.Failure, xiResult.Failure);
        Assert.Equal(GbCountryCode, gbResult.CountryCode);
        Assert.Equal(XiCountryCode, xiResult.CountryCode);
    }

    [Theory]
    [InlineData("XI 980 7806 84", "980780684")]
    [InlineData("XI980780684000", "980780684000")]
    [InlineData("XIGD100", "GD100")]
    [InlineData("XIHA501", "HA501")]
    [InlineData("XIGD888810003", "GD888810003")]
    public void Validate_accepts_XI_format_variants_that_GB_supports(string xiRawInput, string expectedNormalizedNumber)
    {
        var gbEquivalent = GbCountryCode + xiRawInput[PrefixLength..];

        var xiResult = VatNumberValidator.Validate(xiRawInput);
        var gbResult = VatNumberValidator.Validate(gbEquivalent);

        Assert.True(xiResult.IsValid);
        Assert.Equal(XiCountryCode, xiResult.CountryCode);
        Assert.Equal(expectedNormalizedNumber, xiResult.NormalizedNumber);
        Assert.Equal(gbResult.IsValid, xiResult.IsValid);
        Assert.Equal(gbResult.IsChecksumVerified, xiResult.IsChecksumVerified);
    }

    [Fact]
    public void Validate_two_argument_strips_redundant_XI_prefix()
    {
        var result = VatNumberValidator.Validate(XiCountryCode, "XI980780684");

        Assert.True(result.IsValid);
        Assert.Equal(XiCountryCode, result.CountryCode);
        Assert.Equal("980780684", result.NormalizedNumber);
    }
}
