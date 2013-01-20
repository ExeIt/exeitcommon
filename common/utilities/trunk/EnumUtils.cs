using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXE_IT.Common.Utilities
{
    /// <summary>
    /// Utility functions for Enums
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class EnumUtil<T>
    {
        /// <summary>
        /// Parses the specified value into an Enum.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static T Parse(string value, bool ignoreCase)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// Gets a list of all the values in an enum.
        /// </summary>
        /// <returns></returns>
        public static IList<T> GetValues()
        {
            IList<T> list = new List<T>();

            foreach (object value in Enum.GetValues(typeof(T)))
                list.Add((T)value);

            return list;
        }
    }
}
