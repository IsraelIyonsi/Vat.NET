using Vat;

namespace Vat.Net.Tests;

public class NormalizationTests
{
    [Theory]
    [InlineData("DE136695976")]
    [InlineData("DE 136695976")]
    [InlineData("DE 136.695.976")]
    [InlineData("de136695976")]
    [InlineData(" DE136695976 ")]
    [InlineData("D E 1 3 6 6 9 5 9 7 6")]
    public void Validate_normalizes_spaces_dots_case_and_prefix_consistently(string variant)
    {
        var result = VatNumberValidator.Validate(variant);

        Assert.True(result.IsValid);
        Assert.Equal("DE", result.CountryCode);
        Assert.Equal("136695976", result.NormalizedNumber);
    }

    [Fact]
    public void Validate_with_explicit_country_code_strips_redundant_prefix()
    {
        var withPrefix = VatNumberValidator.Validate("DE", "DE136695976");
        var withoutPrefix = VatNumberValidator.Validate("DE", "136695976");

        Assert.True(withPrefix.IsValid);
        Assert.Equal(withoutPrefix.NormalizedNumber, withPrefix.NormalizedNumber);
    }

    [Fact]
    public void Validate_treats_lower_case_country_code_the_same_as_upper_case()
    {
        var lower = VatNumberValidator.Validate("de", "136695976");
        var upper = VatNumberValidator.Validate("DE", "136695976");

        Assert.Equal(upper.IsValid, lower.IsValid);
        Assert.Equal(upper.CountryCode, lower.CountryCode);
    }

    [Fact]
    public void Validate_accepts_greece_iso_alias_gr_as_the_vat_prefix_el()
    {
        var viaAlias = VatNumberValidator.Validate("GR", "094259216");
        var viaVatPrefix = VatNumberValidator.Validate("EL", "094259216");

        Assert.True(viaAlias.IsValid);
        Assert.True(viaVatPrefix.IsValid);
        Assert.Equal("EL", viaAlias.CountryCode);
        Assert.Equal("EL", viaVatPrefix.CountryCode);
    }

    [Fact]
    public void Validate_infers_greece_vat_prefix_el_from_a_full_vat_number()
    {
        var result = VatNumberValidator.Validate("EL 094259216");

        Assert.True(result.IsValid);
        Assert.Equal("EL", result.CountryCode);
    }
}
