using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using MSSqlClient = System.Data.SqlClient;

namespace EXE_IT.Common.Data.SqlClient
{
    public sealed partial class SqlCommand : DbCommand, ISqlCommand
    {
        #region ConnectionOpener

        /// <summary>
        /// Auto-opens a connection, and closes it again
        /// </summary>
        private class ConnectionOpener : IDisposable
        {
            private readonly bool _opened;
            private SqlConnection _connection;

            /// <summary>
            /// Initializes a new instance of the <see cref="ConnectionOpener"/> class.
            /// </summary>
            /// <param name="connection">The connection.</param>
            public ConnectionOpener(SqlConnection connection)
            {
                _connection = connection;

                if (_connection != null && _connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                    _opened = true;
                }
            }

            #region Implementation of IDisposable

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                // close connection if we opened it
                if (_opened && _connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                _connection = null;
            }

            #endregion
        }

        #endregion

        #region SQL Exception Error Constants

        internal const string CONNECTION = "Connection";
        internal const string COMMAND_TEXT = "CommandText";
        internal const string COMMAND_TIMEOUT = "CommandTimeout";
        internal const string PARAMETERS = "Parameters";

        #endregion SQL Exception Error Constants

        #region Private Members

        private readonly MSSqlClient.SqlCommand _command;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommand"/> class.
        /// </summary>
        public SqlCommand()
        {
            _command = new MSSqlClient.SqlCommand();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommand"/> class.
        /// </summary>
        /// <param name="cmdText">The CMD text.</param>
        public SqlCommand(string cmdText)
        {
            _command = new MSSqlClient.SqlCommand(cmdText);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommand"/> class.
        /// </summary>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="connection">The connection.</param>
        public SqlCommand(string cmdText, SqlConnection connection)
        {
            _command = new MSSqlClient.SqlCommand(cmdText, connection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommand"/> class.
        /// </summary>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        public SqlCommand(string cmdText, SqlConnection connection, SqlTransaction transaction)
        {
            _command = new MSSqlClient.SqlCommand(cmdText, connection, transaction);
        }

        #endregion Constructors

        #region ISqlCommand Implementation

        /// <summary>
        /// Attempts to cancels the execution of a <see cref="T:System.Data.Common.DbCommand"/>.
        /// </summary>
        public override void Cancel()
        {
            _command.Cancel();
        }

        /// <summary>
        /// Gets or sets the text command to run against the data source.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The text command to execute. The default value is an empty string ("").
        /// </returns>
        public override string CommandText
        {
            get
            {
                return _command.CommandText;
            }
            set
            {
                _command.CommandText = value;
            }
        }

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The time in seconds to wait for the command to execute.
        /// </returns>
        public override int CommandTimeout
        {
            get
            {
                return _command.CommandTimeout;
            }
            set
            {
                _command.CommandTimeout = value;
            }
        }

        /// <summary>
        /// Indicates or specifies how the <see cref="P:System.Data.Common.DbCommand.CommandText"/> property is interpreted.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Data.CommandType"/> values. The default is Text.
        /// </returns>
        public override CommandType CommandType
        {
            get
            {
                return _command.CommandType;
            }
            set
            {
                _command.CommandType = value;
            }
        }

        /// <summary>
        /// Creates a new instance of a <see cref="T:System.Data.Common.DbParameter"/> object.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Data.Common.DbParameter"/> object.
        /// </returns>
        protected override DbParameter CreateDbParameter()
        {
            return CreateParameter();
        }

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <returns></returns>
        public new SqlParameter CreateParameter()
        {
            return _command.CreateParameter();
        }

        /// <summary>
        /// Gets or sets the <see cref="T:System.Data.Common.DbConnection"/> used by this <see cref="T:System.Data.Common.DbCommand"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The connection to the data source.
        /// </returns>
        protected override DbConnection DbConnection
        {
            get
            {
                return Connection;
            }
            set
            {
                Connection = (SqlConnection)value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:System.Data.SqlClient.SqlConnection"/> used by this <see cref="T:System.Data.SqlClient.SqlCommand"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The connection to the data source.
        /// </returns>
        public new SqlConnection Connection
        {
            get
            {
                return _command.Connection;
            }
            set
            {
                _command.Connection = value;
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="T:System.Data.Common.DbParameter"/> objects.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The parameters of the SQL statement or stored procedure.
        /// </returns>
        protected override DbParameterCollection DbParameterCollection
        {
            get { return Parameters; }
        }

        /// <summary>
        /// Gets the collection of <see cref="T:System.Data.Common.DbParameter"/> objects.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The parameters of the SQL statement or stored procedure.
        /// </returns>
        public new SqlParameterCollection Parameters
        {
            get
            {
                return _command.Parameters;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="P:System.Data.Common.DbCommand.DbTransaction"/> within which this <see cref="T:System.Data.Common.DbCommand"/> object executes.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The transaction within which a Command object of a .NET Framework data provider executes. The default value is a null reference (Nothing in Visual Basic).
        /// </returns>
        protected override DbTransaction DbTransaction
        {
            get
            {
                return Transaction;
            }
            set
            {
                Transaction = (SqlTransaction)value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:System.Data.Common.DbTransaction"/> within which this <see cref="T:System.Data.Common.DbCommand"/> object executes.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The transaction within which a Command object of a .NET Framework data provider executes. The default value is a null reference (Nothing in Visual Basic).
        /// </returns>
        public new SqlTransaction Transaction
        {
            get
            {
                return _command.Transaction;
            }
            set
            {
                _command.Transaction = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the command object should be visible in a customized interface control.
        /// </summary>
        /// <value></value>
        /// <returns>true, if the command object should be visible in a control; otherwise false. The default is true.
        /// </returns>
        public override bool DesignTimeVisible
        {
            get
            {
                return _command.DesignTimeVisible;
            }
            set
            {
                _command.DesignTimeVisible = value;
            }
        }

        /// <summary>
        /// Executes the command text against the connection.
        /// </summary>
        /// <param name="behavior">An instance of <see cref="T:System.Data.CommandBehavior"/>.</param>
        /// <returns>
        /// A <see cref="T:System.Data.Common.DbDataReader"/>.
        /// </returns>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            try
            {
                if (SqlCommandExecutor.HasExpectations)
                {
                    return (DbDataReader)SqlCommandExecutor.CompareCommandToExpectation(this);
                }
                else
                {
                    using (new ConnectionOpener(Connection))
                    {
                        return _command.ExecuteReader(behavior);
                    }
                }
            }
            catch (SqlException ex)
            {
                ExtendSqlExceptionInfo(ex);
                Exception newEx;
                if (TranslateException(ex, out newEx))
                    throw newEx;
                else
                    throw;
            }
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns></returns>
        public new SqlDataReader ExecuteReader(CommandBehavior behavior)
        {
            return (SqlDataReader)base.ExecuteReader(behavior);
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <returns></returns>
        public new SqlDataReader ExecuteReader()
        {
            return (SqlDataReader)base.ExecuteReader();
        }

        /// <summary>
        /// Executes the query and returns a db reader. Use this instead of <see cref="ISqlCommand.ExecuteReader(CommandBehavior)"/>
        /// to allow expectations to be set (return a <see cref="DataTableReader"/> as the result in the unit test)
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns></returns>
        public DbDataReader ExecuteDbReader(CommandBehavior behavior)
        {
            return base.ExecuteReader(behavior);
        }

        /// <summary>
        /// Executes the query and returns a db reader. Use this instead of <see cref="ISqlCommand.ExecuteReader()"/>
        /// to allow expectations to be set (return a <see cref="DataTableReader"/> as the result in the unit test)
        /// </summary>
        /// <returns></returns>
        public DbDataReader ExecuteDbReader()
        {
            return base.ExecuteReader();
        }

        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        public override int ExecuteNonQuery()
        {
            try
            {
                if (SqlCommandExecutor.HasExpectations)
                {
                    return (int)SqlCommandExecutor.CompareCommandToExpectation(this);
                }
                else
                {
                    using (new ConnectionOpener(Connection))
                    {
                        return _command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                ExtendSqlExceptionInfo(ex);
                Exception newEx;
                if (TranslateException(ex, out newEx))
                    throw newEx;
                else
                    throw;
            }
        }

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <returns>
        /// The first column of the first row in the result set.
        /// </returns>
        public override object ExecuteScalar()
        {
            try
            {
                if (SqlCommandExecutor.HasExpectations)
                {
                    return SqlCommandExecutor.CompareCommandToExpectation(this);
                }
                else
                {
                    using (new ConnectionOpener(Connection))
                    {
                        return _command.ExecuteScalar();
                    }
                }
            }
            catch (SqlException ex)
            {
                ExtendSqlExceptionInfo(ex);
                Exception newEx;
                if (TranslateException(ex, out newEx))
                    throw newEx;
                else
                    throw;
            }
        }

        /// <summary>
        /// Executes the XML reader.
        /// </summary>
        /// <returns></returns>
        public XmlReader ExecuteXmlReader()
        {
            try
            {
                if (SqlCommandExecutor.HasExpectations)
                {
                    return (XmlReader)SqlCommandExecutor.CompareCommandToExpectation(this);
                }
                else
                {
                    using (new ConnectionOpener(Connection))
                    {
                        return _command.ExecuteXmlReader();
                    }
                }
            }
            catch (SqlException ex)
            {
                ExtendSqlExceptionInfo(ex);
                Exception newEx;
                if (TranslateException(ex, out newEx))
                    throw newEx;
                else
                    throw;
            }
        }

        /// <summary>
        /// Creates a prepared (or compiled) version of the command on the data source.
        /// </summary>
        public override void Prepare()
        {
            _command.Prepare();
        }

        /// <summary>
        /// Gets or sets how command results are applied to the <see cref="T:System.Data.DataRow"/> when used by the Update method of a <see cref="T:System.Data.Common.DbDataAdapter"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Data.UpdateRowSource"/> values. The default is Both unless the command is automatically generated. Then the default is None.
        /// </returns>
        public override UpdateRowSource UpdatedRowSource
        {
            get
            {
                return _command.UpdatedRowSource;
            }
            set
            {
                _command.UpdatedRowSource = value;
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _command.Dispose();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets or sets the <see cref="T:System.ComponentModel.ISite"/> of the <see cref="T:System.ComponentModel.Component"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.ComponentModel.ISite"/> associated with the <see cref="T:System.ComponentModel.Component"/>, or null if the <see cref="T:System.ComponentModel.Component"/> is not encapsulated in an <see cref="T:System.ComponentModel.IContainer"/>, the <see cref="T:System.ComponentModel.Component"/> does not have an <see cref="T:System.ComponentModel.ISite"/> associated with it, or the <see cref="T:System.ComponentModel.Component"/> is removed from its <see cref="T:System.ComponentModel.IContainer"/>.
        /// </returns>
        public override ISite Site
        {
            get
            {
                return _command.Site;
            }
            set
            {
                _command.Site = value;
            }
        }

        #endregion ISqlCommand Implementation

        #region Properties

        /// <summary>
        /// Gets the native command.
        /// </summary>
        /// <value>The native command.</value>
        public MSSqlClient.SqlCommand MSSqlCommand
        {
            get
            {
                return _command;
            }
        }

        #endregion Properties

        #region Exception Methods Helper

        /// <summary>
        /// Translates the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="newException">The new exception.</param>
        /// <returns></returns>
        internal static bool TranslateException(SqlException ex, out Exception newException)
        {
            newException = null;
            //foreach (SqlError error in ex.Errors)
            //{
            //    // +ve error numbers come from SQL server
            //    // select * from sys.messages where language_id=1033 order by message_id
            //    // -ve error numbers come from SqlClient
            //    switch (error.Number)
            //    {
            //        #region UnknownServerException

            //        case -1:
            //            newException = new UnknownServerException(error.Message, ex);
            //            return true;

            //        #endregion

            //        #region TimeoutException

            //        case -2:
            //        case 1222:
            //            // 1222 = Lock request time out period exceeded.
            //            newException = new TimeoutException(error.Message, ex);
            //            return true;

            //        #endregion

            //        #region OperationCanceledException

            //        case 1205:
            //        case 1206:
            //            // 1205 = Transaction (Process ID %d) was deadlocked on %.*ls resources with another process and has been chosen as the deadlock victim. Rerun the transaction.
            //            // 1206 = The Microsoft Distributed Transaction Coordinator (MS DTC) has cancelled the distributed transaction.
            //            newException = new OperationCanceledException(error.Message, ex);
            //            return true;

            //        #endregion
            //    }
            //}

            // didn't match any of the errors so stick with the exception            
            return false;
        }

        /// <summary>
        /// Extends the SQL exception info.
        /// </summary>
        /// <param name="dbException">The db exception.</param>
        private void ExtendSqlExceptionInfo(Exception dbException)
        {
            // append more information into this exception
            if (Connection != null)
                dbException.Data.Add(CONNECTION, Connection.ConnectionString);
            else
                dbException.Data.Add(CONNECTION, "Database connection does not exist");
            dbException.Data.Add(COMMAND_TEXT, CommandText);
            dbException.Data.Add(COMMAND_TIMEOUT, CommandTimeout);

            StringBuilder sb = new StringBuilder();
            if (Parameters.Count > 0)
            {
                sb.Append("Number of paramters: " + Parameters.Count + ":\r\n");
                for (int i = 0; i < Parameters.Count; i++)
                {
                    sb.Append("\tParameter #" + i + "\r\n");
                    sb.Append("\tName: " + Parameters[i].ParameterName + "\r\n");

                    // beware of long strings !
                    switch (Parameters[i].DbType)
                    {
                        case DbType.AnsiString:
                        case DbType.AnsiStringFixedLength:
                        case DbType.String:
                        case DbType.StringFixedLength:
                        case DbType.Xml:
                            if (Parameters[i].Value != null && Parameters[i].Value.ToString().Length > 50)
                            {
                                sb.Append("\tValue: " + Parameters[i].Value.ToString().Substring(0, 50) + "...\r\n");
                            }
                            else
                            {
                                sb.Append("\tValue: " + (Parameters[i].Value ?? string.Empty) + "\r\n");
                            }
                            break;
                        default:
                            sb.Append("\tValue: " + (Parameters[i].Value ?? string.Empty) + "\r\n");
                            break;
                    }

                    sb.Append("\tType: " + Parameters[i].DbType + "\r\n");
                    sb.Append("\tDirection: " + Parameters[i].Direction + "\r\n");
                    sb.Append("\r\n");
                }
            }

            dbException.Data.Add(PARAMETERS, sb.ToString());
        }

        #endregion Exception Methods Helper
    }
}
