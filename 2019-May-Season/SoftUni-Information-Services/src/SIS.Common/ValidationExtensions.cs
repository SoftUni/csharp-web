namespace SIS.Common
{
    using System;

    public static class ValidationExtensions
    {
        public static void ThrowIfNull(this object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ThrowIfNullOrEmpty(this string text, string name)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException($"{name} cannot be null or empty", name);
            }
        }
    }
}
