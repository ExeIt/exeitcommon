using System;
using System.Data.SqlClient;

namespace EXE_IT.Common.Data
{
    public static class SqlUtil
    {
        /// <summary>
        /// Enforces that any parameters with a value of null are passed to SQL as <see cref="DBNull.Value"/>
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public static void EnforceNullParameters(SqlParameterCollection parameters)
        {
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter.Value == null)
                    parameter.Value = DBNull.Value;
            }
        }
    }
}
