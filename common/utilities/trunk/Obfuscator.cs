using System;
using System.Text;

namespace EXE_IT.Common.Utilities
{
    public static class Obfuscator
    {
        public static string Obfuscate(string text)
        {
            var sb = new StringBuilder("_");

            if (text != null)
            {
                foreach (char c in text)
                {
                    string ac = ((int)c).ToString("X").ToLower();
                    sb.Append(ac);
                }
            }

            return sb.ToString();
        }

        public static string UnObfuscate(string text)
        {
            if (text == null) throw new ArgumentNullException("text");

            if (text == "_")
                return null;

            if ((!text.StartsWith("_") || ((text.Length % 2) == 0)))
            {
                throw new FormatException("The obfuscated text was not in the correct format.");
            }
            else
            {
                try
                {
                    var sb = new StringBuilder();

                    for (int i = 1; i < text.Length; i = i + 2)
                    {
                        string acs = text.Substring(i, 2);
                        int ac = Convert.ToInt32("0x" + acs, 16);
                        sb.Append((char)ac);
                    }

                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    throw new FormatException(String.Format(
                        "Error unobfuscating '{0}'. If value is from query string, user may be attempting hack.", text), ex);
                }
            }

        }
    }
}
