using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using EXE_IT.Common.Data;

namespace EXE_IT.Common.Entity
{
    [Serializable]
    public class EntityBase
    {
        /// <summary>
        /// The name of the ID parameter in the SqlCommand.Parameters collection
        /// </summary>
        protected const string idParamName = "id";

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityBase()
        {
            LastUpdateDt = DateTime.MinValue;
            LastUpdatedBy = String.Empty;
            Id = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reader">Data reader containing a row holding data on this generic table</param>
        protected EntityBase(IDataReader reader)
        {
            LastUpdatedBy = String.Empty;

            Id = SqlDataReaderUtil.GetInt32(reader, reader.GetOrdinal("ID"), 0);

            try
            {
                if (!Convert.IsDBNull(reader.GetOrdinal("LastUpdatedBY")))
                    LastUpdatedBy = SqlDataReaderUtil.GetString(reader, reader.GetOrdinal("LastUpdatedBY"));        
            }
            catch {}

            try
            {
                if (!Convert.IsDBNull(reader.GetOrdinal("LastUpdateDT")))
                    LastUpdateDt = SqlDataReaderUtil.GetDateTime(reader, reader.GetOrdinal("LastUpdateDT"), DateTime.MaxValue);
            }
            catch {}


        }

        #endregion

        #region Properties

        public int Id { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdateDt { get; set; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Add all common parameters to the SQL command ready for an update command
        /// </summary>
        /// <param name="command">Command to add parameters to</param>
        /// <param name="updateSp">Stored procedure to call if updating rather than creating</param>
        protected void AddCommonSqlUpdateParameters(SqlCommand command, string updateSp)
        {
            SqlParameter param = command.Parameters.Add(idParamName, SqlDbType.Int);
            param.Value = Id;
            param = command.Parameters.Add("LastUpdatedBY", SqlDbType.VarChar, 50);
            param.Value = LastUpdatedBy;

            // define output param if creating
            if (Id == 0)
            {
                command.Parameters[idParamName].Direction = ParameterDirection.Output;
            }
            else
            {
                // we're actually doing an update not a create
                command.CommandText = updateSp;
            }
        }

        /// <summary>
        /// Update this entity on the database
        /// </summary>
        /// <param name="command">Populated Command object</param>
        protected void UpdateEntity(SqlCommand command)
        {
            Exception error = null;

            // start transaction
            command.Transaction = command.Connection.BeginTransaction();

            // execute
            try
            {
                command.ExecuteNonQuery();

                // commit change
                command.Transaction.Commit();

                // capture ID of created entity
                if (Id == 0)
                    Id = (int)command.Parameters[idParamName].Value;
            }
            catch (SqlException ex)
            {
                // abort transaction
                command.Transaction.Rollback();

                // remember
                error = ex;
            }
            finally
            {
                // close connection
                command.Connection.Close();
            }

            // throw exception if an error occurred
            if (error != null)
                throw error;
        }

        /// <summary>
        /// Update this entity on the database but do NOT end the transaction or CLOSE the connection
        /// </summary>
        /// <param name="command">Populated Command object</param>
        protected void UpdateInsideTransaction(SqlCommand command)
        {
            Exception error = null;

            // execute
            try
            {
                command.ExecuteNonQuery();

                // capture ID of created entity
                if (Id == 0)
                    Id = (int)command.Parameters[idParamName].Value;
            }
            catch (SqlException ex)
            {
                // remember
                error = ex;
            }
            finally { }

            // throw exception if an error occurred
            if (error != null)
                throw error;
        }

        /// <summary>
        /// Update this entity on the database and CLOSE the connection - leave the transaction alone
        /// as this will be dealt with in the business logic using scope
        /// </summary>
        /// <param name="command">Populated Command object</param>
        protected void UpdateAndCloseConnection(SqlCommand command)
        {
            Exception error = null;

            // execute
            try
            {
                command.ExecuteNonQuery();

                // capture ID of created entity
                if (Id == 0)
                    Id = (int)command.Parameters[idParamName].Value;
            }
            catch (SqlException ex)
            {
                // remember
                error = ex;
            }
            finally
            {
                // close connection
                command.Connection.Close();
            }

            // throw exception if an error occurred
            if (error != null)
                throw error;
        }

        #endregion

        #region Protected Static methods for Database Access

        /// <summary>
        /// Get an open connection to the database
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"];
            var con = new SqlConnection(connectionString.ConnectionString);
            con.Open();
            return con;
        }

        /// <summary>
        /// Load an entity by its ID
        /// </summary>
        /// <param name="storedProc">Stored Proc to use to load entity</param>
        /// <param name="id">ID to load</param>
        /// <returns>DataReader positioned at first row</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the given ID is not a valid ID for the entity type</exception>
        protected static SqlDataReader LoadById(string storedProc, int id)
        {
            SqlCommand command = GetSpCommand(storedProc);
            SqlDataReader reader = null;

            // define parameters
            SqlParameter param = command.Parameters.Add("ID", SqlDbType.Int);
            param.Value = id;

            // execute
            try
            {
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    // advance to row 1
                    reader.Read();
                }
                else
                {
                    // close connection
                    command.Connection.Close();

                    // no rows : invalid ID
                    throw new ArgumentOutOfRangeException("ID", id, "No entity exists for this ID");
                }
            }
            catch (SqlException)
            {
                // close connection
                command.Connection.Close();

                // should do something here
                throw;
            }

            // return reader
            return reader;
        }

        #endregion

        #region Internal Static methods for Database Access

        /// <summary>
        /// Get a command object ready to execute a stored procudure
        /// </summary>
        /// <remarks>
        /// An open connection is already associated with the command
        /// </remarks>
        /// <param name="storedProcName">Name of stored proc to be executed</param>
        /// <returns>Sql Command</returns>
        public static SqlCommand GetSpCommand(string storedProcName)
        {
            var command = new SqlCommand(storedProcName, GetConnection());
            command.CommandType = CommandType.StoredProcedure;

            return command;
        }

        #endregion
    }
}
