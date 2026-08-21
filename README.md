# Vat.NET

Validate European Union and United Kingdom VAT registration numbers offline: per-country format and check-digit verification without calling VIES. Zero external dependencies.

The [VIES web service](https://ec.europa.eu/taxation_customs/vies/) is the only way to confirm a VAT number is actually registered, and Vat.NET does not replace it. But VIES is a network call: it goes down, it rate-limits, it is unreachable from a background job with no outbound internet, and it cannot run inside a unit test. Most of the time what you actually need before you ever call VIES, or instead of calling it for a cheap first pass, is to know whether a number is even well-formed: right length, right character set, and where the country defines one, a correct check digit. Vat.NET answers that offline, synchronously, with no dependency on a third-party service being up.

There is no actively maintained, dependency-free .NET package that does this today across the full EU membership plus the UK with per-country checksums. Vat.NET is that package.

## Install

```
dotnet add package Vat.Net
```

## Usage

Infer the country from the number's own prefix:

```csharp
using Vat;

VatNumberValidator.IsValid("DE136695976");        // true
VatNumberValidator.IsValid("DE 136.695.976");      // true, spaces and dots are ignored
VatNumberValidator.IsValid("DE136695978");         // false, bad check digit
```

Validate against a country you already know, and inspect why a number failed:

```csharp
using Vat;

var result = VatNumberValidator.Validate("FR", "23334175221");

result.IsValid;             // true
result.CountryCode;         // "FR"
result.NormalizedNumber;    // "23334175221"
result.IsChecksumVerified;  // true, France's key algorithm ran and passed
```

A failed validation reports which stage rejected it:

```csharp
using Vat;

var result = VatNumberValidator.Validate("IE6433435E");

result.IsValid;    // false
result.Failure;    // VatValidationFailure.InvalidChecksum
```

`Failure` is one of `EmptyInput`, `UnknownCountry`, `InvalidFormat` or `InvalidChecksum` (`None` on success).

## Northern Ireland (XI)

Since the post-Brexit Northern Ireland Protocol, Northern Irish businesses trading goods with the EU use VAT numbers prefixed `XI` instead of `GB`. An `XI` number has the identical structure and check digit algorithm as a UK number, so Vat.NET validates `XI` with the exact same rule as `GB` (one shared implementation, so the two can never drift). A valid UK number stays valid when its prefix is swapped to `XI`; the result's `CountryCode` is reported as `XI`.

```csharp
using Vat;

VatNumberValidator.IsValid("XI980780684");   // true, same rule as GB980780684
VatNumberValidator.Validate("XI980780684").CountryCode;   // "XI"
```

`XI` is the WTO/VAT prefix for Northern Ireland; it is not an ISO 3166-1 alpha-2 country code, and is surfaced as the country code exactly as Greece's VAT prefix `EL` is.

## Normalization

Input is normalized by stripping spaces, dots, and (for the single-argument overload) the number's own leading two-letter country prefix, before format and checksum rules run. `VatNumberValidator.Validate("DE", "DE 136.695.976")` and `VatNumberValidator.Validate("DE136695976")` both resolve to the same normalized number. Separators other than spaces and dots (hyphens, slashes) are not stripped; if your source data uses them, remove them before calling in.

## Checksum-verified vs format-only

Twenty-five of the 28 supported countries have their check digit or checksum algorithm implemented and verified. For three countries, one specific sub-format is deliberately left format-only rather than guessed:

| Country | Coverage |
|---|---|
| Austria, Belgium, Croatia, Cyprus, Denmark, Estonia, Finland, France, Germany, Greece, Hungary, Ireland, Italy, Lithuania, Luxembourg, Malta, Netherlands, Poland, Portugal, Romania, Slovakia, Slovenia, Spain, Sweden, United Kingdom, Northern Ireland | Checksum-verified |
| Bulgaria | Checksum-verified for the 9-digit legal-entity form. The 10-digit form (physical persons, foreigners and others) is accepted on format alone: it can validly derive from either a general weighted formula or the holder's national identity number, and this package does not implement national identity number validation. |
| Czechia | Checksum-verified for the 8-digit legal-entity form and the 9-digit special form starting with "6". Other 9 and 10-digit numbers encode a birth number (rodné číslo) whose validity depends on parsing a real calendar date, which this package does not implement, so they are accepted on format alone. |
| Latvia | Checksum-verified for legal entities (first digit greater than 3). Natural-person numbers mirror a personal code whose check digit formula has no independently confirmed primary source, so they are accepted on format alone rather than guessed. |

`VatValidationResult.IsChecksumVerified` tells you, per call, whether the specific number you passed in was actually checksum-checked or only format-checked, so you never have to guess which path a given result took.

Every implemented checksum (ISO 7064 Mod 11-10 for Germany and Croatia, the UK's 97-minus modulus including its restarted-number allowance, France's mod-97 key over the SIREN, Italy's Luhn check, and the rest) is a direct, line-by-line port of the corresponding algorithm in [python-stdnum](https://github.com/arthurdejong/python-stdnum), an actively maintained, widely used open-source reference implementation, cross-checked by hand against its embedded published test numbers during development. The full fixture table, including every valid number and its corrupted-checksum counterpart, is in [`tests/Vat.Net.Tests/VatFixtures.cs`](tests/Vat.Net.Tests/VatFixtures.cs).

## Dependencies and AOT

Zero runtime NuGet dependencies. Built entirely on the .NET base class library: `System.Text.RegularExpressions` (source-generated, compile-time regex, no reflection) and `System.Numerics.BigInteger` for the one checksum (the Dutch alternate numbering scheme) that needs arbitrary-precision arithmetic. No reflection, no dynamic code generation, no I/O. This makes Vat.NET compatible with Native AOT publishing and trimming out of the box.

## License

MIT. See [LICENSE](LICENSE).
