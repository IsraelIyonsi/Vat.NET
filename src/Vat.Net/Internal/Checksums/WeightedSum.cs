namespace Vat.Internal.Checksums;

/// <summary>
/// Computes a weighted digit sum, the building block behind most European mod-N check
/// digit schemes (Denmark, Estonia, Finland, Hungary, Malta, Poland, Portugal and others).
/// </summary>
internal static class WeightedSum
{
    /// <summary>
    /// Multiplies each digit in <paramref name="digits"/> by the weight at the same
    /// position and returns the sum. Only as many digits as there are weights are used.
    /// </summary>
    internal static int Compute(string digits, IReadOnlyList<int> weights)
    {
        var sum = 0;
        var count = Math.Min(digits.Length, weights.Count);
        for (var i = 0; i < count; i++)
        {
            sum += weights[i] * (digits[i] - '0');
        }

        return sum;
    }
}
