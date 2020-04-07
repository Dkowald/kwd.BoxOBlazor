using System;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace kwd.BoxOBlazor.Util
{
    public static class FloatExtensions
    {
        /// <summary>lhs equal to rhs withing given precision.
        /// </summary>
        public static bool Like(this float lhs, float rhs, int decimals = 5)
            => Math.Round(lhs - rhs, decimals) == 0;
    }
}