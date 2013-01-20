using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;

namespace EXE_IT.Common.Data.SqlClient
{
    public interface ISqlCommand : IDbCommand
    {
        #region Methods

        /// <summary>
        /// Executes the XML reader.
        /// </summary>
        /// <returns>An XML Reader object</returns>
        XmlReader ExecuteXmlReader();

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <returns>SqlDataReader</returns>
        new SqlDataReader ExecuteReader();

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns></returns>
        new SqlDataReader ExecuteReader(CommandBehavior behavior);

        /// <summary>
        /// Executes the query and returns a db reader. Use this instead of <see cref="ISqlCommand.ExecuteReader(CommandBehavior)"/>
        /// to allow expectations to be set (return a <see cref="DataTableReader"/> as the result in the unit test)
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns></returns>
        DbDataReader ExecuteDbReader(CommandBehavior behavior);

        /// <summary>
        /// Executes the query and returns a db reader. Use this instead of <see cref="ISqlCommand.ExecuteReader()"/>
        /// to allow expectations to be set (return a <see cref="DataTableReader"/> as the result in the unit test)
        /// </summary>
        /// <returns></returns>
        DbDataReader ExecuteDbReader();

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <returns></returns>
        new SqlParameter CreateParameter();

        #endregion Execute Methods

        #region Properties

        /// <summary>
        /// Gets the native command.
        /// </summary>
        /// <value>The native command.</value>
        System.Data.SqlClient.SqlCommand MSSqlCommand
        {
            get;
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        new SqlParameterCollection Parameters { get; }

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        /// <value>The connection.</value>
        new SqlConnection Connection { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="T:System.Data.Common.DbTransaction"/> within which this <see cref="T:System.Data.Common.DbCommand"/> object executes.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The transaction within which a Command object of a .NET Framework data provider executes. The default value is a null reference (Nothing in Visual Basic).
        /// </returns>
        new SqlTransaction Transaction { get; set; }

        #endregion Properties
    }
}
