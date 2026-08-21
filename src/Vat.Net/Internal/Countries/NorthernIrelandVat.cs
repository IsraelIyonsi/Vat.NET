namespace Vat.Internal.Countries;

/// <summary>
/// Northern Ireland: since the post-Brexit Northern Ireland Protocol, businesses trading
/// goods with the EU use VAT numbers prefixed "XI" instead of "GB". An XI number has the
/// identical structure and check digit algorithm as a United Kingdom number; only the
/// country prefix differs, and EU VIES treats "XI" as a valid member-state-style prefix.
/// Every rule is delegated to <see cref="UnitedKingdomVat"/> so the two share one
/// implementation and can never drift apart. "XI" is not an ISO 3166-1 alpha-2 country
/// code but is the WTO/VAT prefix for Northern Ireland, and is surfaced as the result's
/// country code exactly as Greece's VAT prefix "EL" is.
/// </summary>
internal static class NorthernIrelandVat
{
    internal const string CountryCode = "XI";

    internal static string Canonicalize(string number) => UnitedKingdomVat.Canonicalize(number);

    internal static bool IsFormatValid(string number) => UnitedKingdomVat.IsFormatValid(number);

    internal static bool? IsChecksumValid(string number) => UnitedKingdomVat.IsChecksumValid(number);
}
