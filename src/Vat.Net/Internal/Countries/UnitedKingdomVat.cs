using Vat.Internal.Checksums;

namespace Vat.Internal.Countries;

/// <summary>
/// United Kingdom: VAT registration number. Supports the standard 9-digit form, the
/// 12-digit form with a 3-digit branch suffix, the 5-character government department (GD)
/// and health authority (HA) forms, and the extended 11-character GD8888/HA8888 form.
/// </summary>
internal static class UnitedKingdomVat
{
    internal const string CountryCode = "GB";
    private const int DepartmentLength = 5;
    private const int ExtendedDepartmentLength = 11;
    private const int StandardLength = 9;
    private const int BranchLength = 12;
    private const string GovernmentDepartmentPrefix = "GD";
    private const string HealthAuthorityPrefix = "HA";
    private const string ExtendedDepartmentMarker = "8888";
    private const int DepartmentThreshold = 500;
    private const int RestartedNumberThreshold = 100;
    private const int Modulus97 = 97;
    private const int LegacyValidRemainder = 0;
    private const int ExpandedValidRemainderA = 42;
    private const int ExpandedValidRemainderB = 55;

    private static readonly int[] Weights = [8, 7, 6, 5, 4, 3, 2, 10, 1];

    internal static string Canonicalize(string number) => number;

    internal static bool IsFormatValid(string number) => number.Length switch
    {
        DepartmentLength => IsDepartmentNumberValid(number[..2], number[2..]),
        ExtendedDepartmentLength => IsExtendedDepartmentFormatValid(number),
        StandardLength or BranchLength => number.All(char.IsAsciiDigit),
        _ => false,
    };

    internal static bool? IsChecksumValid(string number) => number.Length switch
    {
        DepartmentLength => null,
        ExtendedDepartmentLength => int.Parse(number[6..9]) % Modulus97 == int.Parse(number[9..11]),
        StandardLength or BranchLength => IsWeightedChecksumValid(number[..StandardLength]),
        _ => null,
    };

    private static bool IsDepartmentNumberValid(string prefix, string digits)
    {
        if (!digits.All(char.IsAsciiDigit))
        {
            return false;
        }

        var value = int.Parse(digits);
        return (prefix == GovernmentDepartmentPrefix && value < DepartmentThreshold) ||
            (prefix == HealthAuthorityPrefix && value >= DepartmentThreshold);
    }

    private static bool IsExtendedDepartmentFormatValid(string number)
    {
        var marker = number[..6];
        if (marker != GovernmentDepartmentPrefix + ExtendedDepartmentMarker &&
            marker != HealthAuthorityPrefix + ExtendedDepartmentMarker)
        {
            return false;
        }

        var digits = number[6..];
        if (!digits.All(char.IsAsciiDigit))
        {
            return false;
        }

        var value = int.Parse(number[6..9]);
        var isGovernmentDepartment = marker.StartsWith(GovernmentDepartmentPrefix, StringComparison.Ordinal);
        return (isGovernmentDepartment && value < DepartmentThreshold) ||
            (!isGovernmentDepartment && value >= DepartmentThreshold);
    }

    private static bool IsWeightedChecksumValid(string standardNumber)
    {
        var remainder = WeightedSum.Compute(standardNumber, Weights) % Modulus97;
        var allowedRemainders = int.Parse(standardNumber[..3]) >= RestartedNumberThreshold
            ? new[] { LegacyValidRemainder, ExpandedValidRemainderA, ExpandedValidRemainderB }
            : new[] { LegacyValidRemainder };
        return allowedRemainders.Contains(remainder);
    }
}
