using System;
using System.Data;
using System.IO;

namespace EXE_IT.Common.Data
{
    /// <summary>
    /// Utility methods for reading from a SqlDataReader
    /// </summary>
    public static class SqlDataReaderUtil
    {
        #region IDataReader versions

        public static byte[] GetBytes(IDataReader reader, int ordinal)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buff = new byte[8192];
                long offset = 0L;
                long n = 0L;
                do
                {
                    n = reader.GetBytes(ordinal, offset, buff, 0, buff.Length);
                    ms.Write(buff, 0, (int)n);
                    offset += n;
                } while (n >= buff.Length);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Gets a string from a <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public static string GetString(IDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetString(ordinal);
        }

        /// <summary>
        /// Gets a guid from a <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public static Guid? GetGuid(IDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetGuid(ordinal);
        }

        /// <summary>
        /// Gets a int32 from a <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="ordinal">The ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int GetInt32(IDataReader reader, int ordinal, int defaultValue)
        {
            if (reader.IsDBNull(ordinal))
                return defaultValue;
            return reader.GetInt32(ordinal);
        }

        public static int? GetInt32Nullable(IDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetInt32(ordinal);
        }

        public static double GetDouble(IDataReader reader, int ordinal, int defaultValue)
        {
            if (reader.IsDBNull(ordinal))
                return defaultValue;
            return reader.GetDouble(ordinal);
        }

        public static double? GetDoubleNullable(IDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetDouble(ordinal);
        }

        public static decimal GetDecimal(IDataReader reader, int ordinal, decimal defaultValue)
        {
            if (reader.IsDBNull(ordinal))
                return defaultValue;
            return reader.GetDecimal(ordinal);
        }

        public static decimal? GetDecimalNullable(IDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetDecimal(ordinal);
        }

        public static DateTime GetDateTime(IDataReader reader, int ordinal, DateTime defaultValue)
        {
            if (reader.IsDBNull(ordinal))
                return defaultValue;
            return reader.GetDateTime(ordinal);
        }

        public static DateTime? GetDateTimeNullable(IDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetDateTime(ordinal);
        }

        public static Guid GetGuid(IDataReader reader, int ordinal, Guid defaultValue)
        {
            if (reader.IsDBNull(ordinal))
                return defaultValue;
            return reader.GetGuid(ordinal);
        }

        public static bool GetBoolean(IDataReader reader, int ordinal, bool defaultValue)
        {
            if (reader.IsDBNull(ordinal))
                return defaultValue;
            return reader.GetBoolean(ordinal);
        }

        #endregion

        #region IDataRecord versions

        public static string GetString(IDataRecord reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetString(ordinal);
        }

        public static int GetInt32(IDataRecord reader, int ordinal, int defaultValue)
        {
            if (reader.IsDBNull(ordinal))
                return defaultValue;
            return reader.GetInt32(ordinal);
        }

        public static int? GetInt32Nullable(IDataRecord reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetInt32(ordinal);
        }

        public static double GetDouble(IDataRecord reader, int ordinal, int defaultValue)
        {
            if (reader.IsDBNull(ordinal))
                return defaultValue;
            return reader.GetDouble(ordinal);
        }

        public static double? GetDoubleNullable(IDataRecord reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetDouble(ordinal);
        }

        public static decimal GetDecimal(IDataRecord reader, int ordinal, decimal defaultValue)
        {
            if (reader.IsDBNull(ordinal))
                return defaultValue;
            return reader.GetDecimal(ordinal);
        }

        public static decimal? GetDecimalNullable(IDataRecord reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetDecimal(ordinal);
        }

        public static DateTime GetDateTime(IDataRecord reader, int ordinal, DateTime defaultValue)
        {
            if (reader.IsDBNull(ordinal))
                return defaultValue;
            return reader.GetDateTime(ordinal);
        }

        public static DateTime? GetDateTimeNullable(IDataRecord reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetDateTime(ordinal);
        }

        public static Guid GetGuid(IDataRecord reader, int ordinal, Guid defaultValue)
        {
            if (reader.IsDBNull(ordinal))
                return defaultValue;
            return reader.GetGuid(ordinal);
        }

        public static bool GetBoolean(IDataRecord reader, int ordinal, bool defaultValue)
        {
            if (reader.IsDBNull(ordinal))
                return defaultValue;
            return reader.GetBoolean(ordinal);
        }

        #endregion

    }
}
