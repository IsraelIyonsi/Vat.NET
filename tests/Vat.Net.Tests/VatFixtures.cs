namespace Vat.Net.Tests;

/// <summary>
/// Canonical published test VAT numbers for every supported country, sourced from the
/// doctest fixtures embedded in python-stdnum (https://github.com/arthurdejong/python-stdnum),
/// an actively maintained, widely used open-source reference implementation. Each valid and
/// invalid pair is reproduced here as-is except where noted, and is cross-checked against an
/// independent hand computation of the underlying algorithm during development.
/// </summary>
internal static class VatFixtures
{
    /// <summary>
    /// One number per country that is valid, with the confidence level (checksum-verified
    /// or format-only) this package assigns to it.
    /// </summary>
    public static IEnumerable<object[]> ValidNumbers()
    {
        yield return ["AT U13585627", "AT", "U13585627", true];
        yield return ["BE 0428759497", "BE", "0428759497", true];
        yield return ["BG175074752", "BG", "175074752", true];
        yield return ["CY 10259033P", "CY", "10259033P", true];
        yield return ["CZ25123891", "CZ", "25123891", true];
        yield return ["CZ640903926", "CZ", "640903926", true];
        yield return ["DE 136695976", "DE", "136695976", true];
        yield return ["DK 13585628", "DK", "13585628", true];
        yield return ["EE 100594102", "EE", "100594102", true];
        yield return ["EL 094259216", "EL", "094259216", true];
        yield return ["ES54362315K", "ES", "54362315K", true];
        yield return ["ESX2482300W", "ES", "X2482300W", true];
        yield return ["ESJ99216582", "ES", "J99216582", true];
        yield return ["FI 20774740", "FI", "20774740", true];
        yield return ["FR23334175221", "FR", "23334175221", true];
        yield return ["FRK7399859412", "FR", "K7399859412", true];
        yield return ["GB 980 7806 84", "GB", "980780684", true];
        yield return ["HU 12892312", "HU", "12892312", true];
        yield return ["IE 6433435F", "IE", "6433435F", true];
        yield return ["IE 6433435OA", "IE", "6433435OA", true];
        yield return ["IE8D79739I", "IE", "8D79739I", true];
        yield return ["IT 00743110157", "IT", "00743110157", true];
        yield return ["HR 33392005961", "HR", "33392005961", true];
        yield return ["LT119511515", "LT", "119511515", true];
        yield return ["LT 100001919017", "LT", "100001919017", true];
        yield return ["LU 150 274 42", "LU", "15027442", true];
        yield return ["LV 4000 3521 600", "LV", "40003521600", true];
        yield return ["MT 1167 9112", "MT", "11679112", true];
        yield return ["NL004495445B01", "NL", "004495445B01", true];
        yield return ["NL002455799B11", "NL", "002455799B11", true];
        yield return ["PL 8567346215", "PL", "8567346215", true];
        yield return ["PT 501 964 843", "PT", "501964843", true];
        yield return ["RO 185 472 90", "RO", "18547290", true];
        yield return ["SE 123456789701", "SE", "123456789701", true];
        yield return ["SI 5022 3054", "SI", "50223054", true];
        yield return ["SK 202 274 96 19", "SK", "2022749619", true];
    }

    /// <summary>
    /// One number per country that fails its checksum, paired with its base checksum-verified
    /// country. Most are the corresponding valid number above with its check digit corrupted;
    /// a source-independent negative is used where the reference doctest provided one.
    /// </summary>
    public static IEnumerable<object[]> InvalidChecksumNumbers()
    {
        yield return ["AT U13585626", "AT"];
        yield return ["BE 0431150351", "BE"];
        yield return ["BG175074751", "BG"];
        yield return ["CY 10259033Z", "CY"];
        yield return ["CZ25123890", "CZ"];
        yield return ["DE 136695978", "DE"];
        yield return ["DK 13585627", "DK"];
        yield return ["EE 100594103", "EE"];
        yield return ["EL 094259217", "EL"];
        yield return ["ES54362315Z", "ES"];
        yield return ["ESX2482300A", "ES"];
        yield return ["ESJ99216583", "ES"];
        yield return ["FI 20774741", "FI"];
        yield return ["FR84323140391", "FR"];
        yield return ["GB 802311781", "GB"];
        yield return ["HU 12892313", "HU"];
        yield return ["IE 6433435E", "IE"];
        yield return ["IT 00743110158", "IT"];
        yield return ["HR 33392005962", "HR"];
        yield return ["LT 100001919018", "LT"];
        yield return ["LU 150 274 43", "LU"];
        yield return ["LV 4000 3521 601", "LV"];
        yield return ["MT 1167 9113", "MT"];
        yield return ["NL123456789B90", "NL"];
        yield return ["PL 8567346216", "PL"];
        yield return ["PT 501 964 842", "PT"];
        yield return ["RO 185 472 91", "RO"];
        yield return ["SE 123456789101", "SE"];
        yield return ["SI 50223055", "SI"];
        yield return ["SK 202 274 96 18", "SK"];
    }

    /// <summary>
    /// Numbers that are valid EU28 format-only, sub-cases where this package deliberately does
    /// not implement a checksum (see README for the reasoning per country).
    /// </summary>
    public static IEnumerable<object[]> FormatOnlyNumbers()
    {
        yield return ["BG1234567890", "BG"];
        yield return ["CZ7103192745", "CZ"];
        yield return ["LV32867300679", "LV"];
    }
}
