using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EXE_IT.Common.Data.SqlClient
{
    public class SqlCommandExecutor
    {
        private static readonly List<ExpectedResult> _expectedResults = new List<ExpectedResult>();
        private static int _call;
        private static ISqlCommand _mockedCommand;

        #region ExpectedResult

        /// <summary>
        /// Defines the expected results
        /// </summary>
        private class ExpectedResult
        {
            private readonly string _commandText;
            private readonly List<SqlParameter> _parameters;
            private readonly object _result;

            /// <summary>
            /// Initializes a new instance of the <see cref="ExpectedResult"/> class.
            /// </summary>
            /// <param name="commandText">The command text.</param>
            /// <param name="parameters">The parameters.</param>
            /// <param name="result">The result.</param>
            public ExpectedResult(string commandText, List<SqlParameter> parameters, object result)
            {
                _commandText = commandText;
                _parameters = parameters;
                _result = result;
            }

            /// <summary>
            /// Gets the command text.
            /// </summary>
            /// <value>The command text.</value>
            public string CommandText
            {
                get { return _commandText; }
            }

            /// <summary>
            /// Gets the parameters.
            /// </summary>
            /// <value>The parameters.</value>
            public List<SqlParameter> Parameters
            {
                get { return _parameters; }
            }

            /// <summary>
            /// Gets the result.
            /// </summary>
            /// <value>The result.</value>
            public object Result
            {
                get { return _result; }
            }
        }

        #endregion

        #region Mock objects

        /// <summary>
        /// Gets or sets the mocked command.
        /// </summary>
        /// <value>The mocked command.</value>
        public static ISqlCommand MockedCommand
        {
            get { return _mockedCommand; }
            set { _mockedCommand = value; }
        }

        #endregion Mock objects

        #region CreateCommand Methods

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <returns>ISqlCommand</returns>
        public static ISqlCommand CreateCommand()
        {
            if (MockedCommand == null)
                return new SqlCommand();
            else
                return MockedCommand;
        }

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>ISqlCommand</returns>
        public static ISqlCommand CreateCommand(string commandText)
        {
            if (MockedCommand == null)
            {
                return new SqlCommand(commandText);
            }
            else
            {
                return MockedCommand;
            }
        }

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="connection">The connection.</param>
        /// <returns>ISqlCommand</returns>
        public static ISqlCommand CreateCommand(string commandText, SqlConnection connection)
        {
            if (MockedCommand == null)
            {
                return new SqlCommand(commandText, connection);
            }
            else
            {
                return MockedCommand;
            }
        }

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>ISqlCommand</returns>
        public static ISqlCommand CreateCommand(string commandText, SqlConnection connection, SqlTransaction transaction)
        {
            if (MockedCommand == null)
            {
                return new SqlCommand(commandText, connection, transaction);
            }
            else
            {
                return MockedCommand;
            }
        }

        #endregion CreateCommand Methods

        #region Expected Results

        /// <summary>
        /// Clears all the expected results.
        /// </summary>
        public static void ClearExpectations()
        {
            lock (_expectedResults)
            {
                _expectedResults.Clear();
                _call = 0;
                MockedCommand = null;
            }
        }

        /// <summary>
        /// Adds an expected result.
        /// </summary>
        /// <param name="commandText">The command text. If null then no check is made</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="result">The result.</param>
        public static void AddExpectation(string commandText, List<SqlParameter> parameters, object result)
        {
            lock (_expectedResults)
            {
                _expectedResults.Add(new ExpectedResult(commandText, parameters, result));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has expectations.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has expectations; otherwise, <c>false</c>.
        /// </value>
        public static bool HasExpectations
        {
            get
            {
                lock (_expectedResults)
                {
                    return _expectedResults.Count > 0;
                }
            }
        }

        /// <summary>
        /// Compares the command to an expectation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>the expected result</returns>
        internal static object CompareCommandToExpectation(ISqlCommand command)
        {
            lock (_expectedResults)
            {
                if (_call < _expectedResults.Count)
                {
                    ExpectedResult result = _expectedResults[_call];
                    _call++;

                    // check command text matches if we've been given one to compare against.
                    if (!String.IsNullOrEmpty(result.CommandText))
                    {
                        //Assert.AreEqual(result.CommandText, command.CommandText, "Command Text is not that expected");
                    }

                    // check all params match
                    if (result.Parameters == null && command.Parameters.Count > 0)
                    {
                        //Assert.Fail("Number of Parameters does not match");
                    }
                    else if (result.Parameters != null)
                    {
                        //Assert.AreEqual(result.Parameters.Count, command.Parameters.Count, "Number of Parameters does not match");
                        foreach (SqlParameter parameter in result.Parameters)
                        {
                            //Assert.IsTrue(command.Parameters.Contains(parameter.ParameterName),
                            //              string.Format("No parameter with name {0} found", parameter.ParameterName));
                            //Assert.AreEqual(parameter.Value, command.Parameters[parameter.ParameterName].Value,
                            //                string.Format("Parameter {0} value mismatch", parameter.ParameterName));
                            //Assert.AreEqual(parameter.Direction, command.Parameters[parameter.ParameterName].Direction,
                            //                string.Format("Parameter {0} direction mismatch", parameter.ParameterName));
                            //Assert.AreEqual(parameter.DbType, command.Parameters[parameter.ParameterName].DbType,
                            //                string.Format("Parameter {0} DbType mismatch", parameter.ParameterName));
                        }
                    }

                    // to have got here means params are OK, so return result
                    return result.Result;
                }
                else
                {
                    throw new InvalidOperationException(string.Format("This is the {0} call and there are only {1} expected results defined",
                                                                 _call, _expectedResults.Count));
                }
            }
        }

        #endregion Execute Methods
    }
}
