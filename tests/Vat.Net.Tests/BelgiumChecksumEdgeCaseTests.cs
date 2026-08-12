using Vat;

namespace Vat.Net.Tests;

/// <summary>
/// Pins the exact behavior of <see cref="Vat.Internal.Countries.BelgiumVat.IsChecksumValid"/>
/// for a body that is itself divisible by 97, where the modular check digit rule accepts two
/// different two-digit representations of the same residue. Both numbers below were verified
/// against python-stdnum's <c>stdnum.be.vat.validate</c> reference implementation, which accepts
/// both - this package intentionally matches that behavior rather than adding a stricter rule
/// the reference does not enforce.
/// </summary>
public class BelgiumChecksumEdgeCaseTests
{
    [Fact]
    public void Validate_accepts_the_officially_issued_97_check_digits_for_a_body_divisible_by_97()
    {
        // Body "0010000" + "7" (0010000 7 -> 0010000797) has an 8-digit body "00100007" that is
        // divisible by 97; the check digits HMRC-equivalent Belgian issuing rule assigns are
        // "97" (97 - 0 % 97 = 97), never "00". Real, issuable form.
        var result = VatNumberValidator.Validate("BE0010000797");

        Assert.True(result.IsValid);
        Assert.True(result.IsChecksumVerified);
        Assert.Equal("0010000797", result.NormalizedNumber);
    }

    [Fact]
    public void Validate_also_accepts_the_never_issued_00_check_digits_for_the_same_body_matching_python_stdnum()
    {
        // Same body "00100007", but with check digits "00" instead of the real "97". No
        // Belgian authority would ever issue this exact number, but (body + check) % 97 == 0
        // holds for it too, and python-stdnum's checksum() accepts it - confirmed directly
        // against the reference implementation. This package matches that on purpose.
        var result = VatNumberValidator.Validate("BE0010000700");

        Assert.True(result.IsValid);
        Assert.True(result.IsChecksumVerified);
        Assert.Equal("0010000700", result.NormalizedNumber);
    }
}
