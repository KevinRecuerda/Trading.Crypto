﻿using System;

namespace Trading.Crypto.Tools
{
    public static class ComparableExtensions
    {
        public static bool IsInRange<T>(this T actual, T lower, T upper) where T : IComparable<T>
        {
            return actual.CompareTo(lower) >= 0 && actual.CompareTo(upper) <= 0;
        }
    }
}
