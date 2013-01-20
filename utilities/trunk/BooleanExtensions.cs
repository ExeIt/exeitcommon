using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXE_IT.Common.Utilities
{
    public static class BooleanExtensions
    {
        /// <summary>
        /// Gets the java script value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public static string GetJavaScriptValue(this bool value)
        {
            return value ? "true" : "false";
        }
    }
}
