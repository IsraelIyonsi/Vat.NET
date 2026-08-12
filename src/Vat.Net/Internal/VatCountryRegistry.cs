using Vat.Internal.Countries;

namespace Vat.Internal;

/// <summary>
/// The full set of supported country rules, keyed by VAT prefix. Built once and reused for
/// every validation call.
/// </summary>
internal static class VatCountryRegistry
{
    private static readonly IReadOnlyDictionary<string, VatCountryRule> Rules = BuildRules();

    /// <summary>Looks up the rule for <paramref name="countryCode"/>, resolving known aliases first.</summary>
    internal static VatCountryRule? Find(string countryCode)
    {
        var resolved = countryCode == GreeceVat.CountryCodeAlias ? GreeceVat.CountryCode : countryCode;
        return Rules.GetValueOrDefault(resolved);
    }

    private static IReadOnlyDictionary<string, VatCountryRule> BuildRules()
    {
        VatCountryRule[] rules =
        [
            new(AustriaVat.CountryCode, AustriaVat.Canonicalize, AustriaVat.IsFormatValid, AustriaVat.IsChecksumValid),
            new(BelgiumVat.CountryCode, BelgiumVat.Canonicalize, BelgiumVat.IsFormatValid, BelgiumVat.IsChecksumValid),
            new(BulgariaVat.CountryCode, BulgariaVat.Canonicalize, BulgariaVat.IsFormatValid, BulgariaVat.IsChecksumValid),
            new(CroatiaVat.CountryCode, CroatiaVat.Canonicalize, CroatiaVat.IsFormatValid, CroatiaVat.IsChecksumValid),
            new(CyprusVat.CountryCode, CyprusVat.Canonicalize, CyprusVat.IsFormatValid, CyprusVat.IsChecksumValid),
            new(CzechRepublicVat.CountryCode, CzechRepublicVat.Canonicalize, CzechRepublicVat.IsFormatValid, CzechRepublicVat.IsChecksumValid),
            new(GermanyVat.CountryCode, GermanyVat.Canonicalize, GermanyVat.IsFormatValid, GermanyVat.IsChecksumValid),
            new(DenmarkVat.CountryCode, DenmarkVat.Canonicalize, DenmarkVat.IsFormatValid, DenmarkVat.IsChecksumValid),
            new(EstoniaVat.CountryCode, EstoniaVat.Canonicalize, EstoniaVat.IsFormatValid, EstoniaVat.IsChecksumValid),
            new(GreeceVat.CountryCode, GreeceVat.Canonicalize, GreeceVat.IsFormatValid, GreeceVat.IsChecksumValid),
            new(SpainVat.CountryCode, SpainVat.Canonicalize, SpainVat.IsFormatValid, SpainVat.IsChecksumValid),
            new(FinlandVat.CountryCode, FinlandVat.Canonicalize, FinlandVat.IsFormatValid, FinlandVat.IsChecksumValid),
            new(FranceVat.CountryCode, FranceVat.Canonicalize, FranceVat.IsFormatValid, FranceVat.IsChecksumValid),
            new(UnitedKingdomVat.CountryCode, UnitedKingdomVat.Canonicalize, UnitedKingdomVat.IsFormatValid, UnitedKingdomVat.IsChecksumValid),
            new(HungaryVat.CountryCode, HungaryVat.Canonicalize, HungaryVat.IsFormatValid, HungaryVat.IsChecksumValid),
            new(IrelandVat.CountryCode, IrelandVat.Canonicalize, IrelandVat.IsFormatValid, IrelandVat.IsChecksumValid),
            new(ItalyVat.CountryCode, ItalyVat.Canonicalize, ItalyVat.IsFormatValid, ItalyVat.IsChecksumValid),
            new(LithuaniaVat.CountryCode, LithuaniaVat.Canonicalize, LithuaniaVat.IsFormatValid, LithuaniaVat.IsChecksumValid),
            new(LuxembourgVat.CountryCode, LuxembourgVat.Canonicalize, LuxembourgVat.IsFormatValid, LuxembourgVat.IsChecksumValid),
            new(LatviaVat.CountryCode, LatviaVat.Canonicalize, LatviaVat.IsFormatValid, LatviaVat.IsChecksumValid),
            new(MaltaVat.CountryCode, MaltaVat.Canonicalize, MaltaVat.IsFormatValid, MaltaVat.IsChecksumValid),
            new(NetherlandsVat.CountryCode, NetherlandsVat.Canonicalize, NetherlandsVat.IsFormatValid, NetherlandsVat.IsChecksumValid),
            new(PolandVat.CountryCode, PolandVat.Canonicalize, PolandVat.IsFormatValid, PolandVat.IsChecksumValid),
            new(PortugalVat.CountryCode, PortugalVat.Canonicalize, PortugalVat.IsFormatValid, PortugalVat.IsChecksumValid),
            new(RomaniaVat.CountryCode, RomaniaVat.Canonicalize, RomaniaVat.IsFormatValid, RomaniaVat.IsChecksumValid),
            new(SwedenVat.CountryCode, SwedenVat.Canonicalize, SwedenVat.IsFormatValid, SwedenVat.IsChecksumValid),
            new(SloveniaVat.CountryCode, SloveniaVat.Canonicalize, SloveniaVat.IsFormatValid, SloveniaVat.IsChecksumValid),
            new(SlovakiaVat.CountryCode, SlovakiaVat.Canonicalize, SlovakiaVat.IsFormatValid, SlovakiaVat.IsChecksumValid),
        ];

        return rules.ToDictionary(rule => rule.CountryCode, rule => rule);
    }
}
