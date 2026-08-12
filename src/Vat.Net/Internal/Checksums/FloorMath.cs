namespace Vat.Internal.Checksums;

/// <summary>
/// Floor-based modulo and division, matching the semantics used by the reference French
/// TVA key algorithm when its intermediate value is negative. C#'s built-in <c>%</c> and
/// <c>/</c> truncate toward zero instead of flooring, so they cannot be used directly.
/// </summary>
internal static class FloorMath
{
    /// <summary>Returns <paramref name="dividend"/> modulo <paramref name="divisor"/>, floored toward negative infinity.</summary>
    internal static int Mod(int dividend, int divisor) => ((dividend % divisor) + divisor) % divisor;

    /// <summary>Returns <paramref name="dividend"/> divided by <paramref name="divisor"/>, floored toward negative infinity.</summary>
    internal static int FloorDiv(int dividend, int divisor) => (dividend - Mod(dividend, divisor)) / divisor;
}
