# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.2.0] - 2026-08-21

### Added

- Northern Ireland "XI" VAT numbers. Since the post-Brexit Northern Ireland Protocol, Northern Irish businesses trading goods with the EU use the "XI" prefix instead of "GB". An XI number has the identical structure and check digit algorithm as a United Kingdom number, so "XI" reuses the exact United Kingdom rule (same format, restarted-checksum allowance, and government department / health authority forms) and reports "XI" as the result country code. The two share a single implementation and cannot drift apart.

## [0.1.0] - 2026-08-12

### Added

- `VatNumberValidator` static API: `IsValid(string)`, `IsValid(string countryCode, string number)`, `Validate(string)`, `Validate(string countryCode, string number)`.
- `VatValidationResult` with `IsValid`, `CountryCode`, `NormalizedNumber`, `IsChecksumVerified` and `Failure`.
- `VatValidationFailure` enum: `EmptyInput`, `UnknownCountry`, `InvalidFormat`, `InvalidChecksum`.
- Offline format and checksum validation for all 27 EU member states plus the United Kingdom.
- Checksum algorithms implemented and verified for 25 countries, including ISO 7064 Mod 11-10 (Germany, Croatia), ISO 7064 Mod 97-10 (the Dutch alternate numbering scheme), Luhn (Italy, Sweden, embedded in France's SIREN), the UK's 97-minus modulus with its restarted-number allowance, France's mod-97 key over the SIREN, and per-country weighted mod-10/11/23/26/37/89 formulas for the remaining countries.
- Format-only validation, clearly documented per country and per sub-format, for the three sub-formats (Bulgaria's 10-digit form, Czechia's birth-number-linked forms, Latvia's natural-person form) whose checksum this package does not implement, either because it depends on national identity number validation outside VAT's scope or because no primary source could confirm the formula.
- Normalization of spaces, dots and the leading country prefix, and acceptance of Greece's ISO code "GR" as an alias for its VAT prefix "EL".
- A fixture table of canonical published test VAT numbers, one valid and one corrupted-checksum number per country, sourced from and cross-checked against python-stdnum.
- Zero runtime dependencies; built on `System.Text.RegularExpressions` (source-generated) and `System.Numerics.BigInteger`.
