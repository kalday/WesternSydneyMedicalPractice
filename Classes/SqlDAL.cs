using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// additional references
using System.Data;              // contains the 'disconnected' objects (classes)
using System.Data.SqlClient;    // contains the 'connected' objects (classes) specific to the SQL Server
using System.Configuration;     // contains the classes to read items from app.config

namespace WesternSydneyMedicalPractice.Classes
{
    class SqlDataAccessLayer
    {

        #region Field Variables (private)

        private string _connString;
        private string _connectionStringName;

        #endregion Field Variables (private)

        #region Constructors

        // add a constructor to read app.config file, assigning a connection string to field variable (_connString)
        public SqlDataAccessLayer(string cnnStrName)
        {
            // used to read app.config file ConfigurationManager.ConnectionStrings["cnnStrWSMP"].ConnectionString;
            this._connString = ConfigurationManager.ConnectionStrings[cnnStrName].ConnectionString;
        }

        #endregion Constructors

        #region Public Properties
        public string ConnectionStringName
        {
            get 
            { 
                return _connectionStringName;  
            }
            set 
            { 
                // 'value' is a system variable, exchanging the property's value
                _connectionStringName = value;  
                _connString = ConfigurationManager.ConnectionStrings[value].ConnectionString;
            }
        }
        #endregion Public Properties

        #region Public Methods

        #region METHOD: ExecuteStoredProc(SPName)
        /// <summary>
        /// Executes a Stored Procedure (without parameters) and returns a DataTable result set. Use this for queries which return 'mutliple rows/columns'
        /// </summary>
        /// <param name="SPName">String: The name of the Stored Procedure to be executed.</param>
        /// <returns>DataTable: DataTable containing the results of the Stored Procedure execution.</returns>
        public DataTable ExecuteStoredProc(string SPName)
        {
            // create connection object (objects prefixed with 'Sql...' are connected (SQL Server Specific) objects
            SqlConnection conn = new SqlConnection(_connString);

            // create command object to specify name of the stored proc or sql to execute on the database
            SqlCommand cmd = new SqlCommand(SPName, conn);

            // specity command type is a Stored Procedure - default is 'Text' (i.e. string of SQL)
            cmd.CommandType = CommandType.StoredProcedure;

            /*
             * If this is going to fail, it will fail when we try to access (connect) to the database or when we try to execute the command (i.e. the sql)
             */
            try
            {
                // try open connection to database 
                cmd.Connection.Open();

                // define SqlDataReader 'fire-hose' (Forward-Only-Read-Only) cursor 
                // manipulating data returned into SqlDataReader requires a permanent 'container' (e.g. DataTable or DataSet)
                //      DataSet -> in-memory representation of multiple tables and their relationships 
                
                // 'execute' command on the Database 
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // create DataTable object for a more permanent storage of ersults of the command execution 
                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);

                return dataTable;

            }
            catch (SqlException SqlEx)
            {
                // specific exception (i.e. error with the database)
                throw new Exception(SqlEx.Message);
            }
            catch (Exception ex)
            {
                // most generic exception, catches ALL exceptions 
                throw new Exception(ex.Message);
            }
            finally
            {
                // executes regardless of exceptions thrown
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }
        #endregion ExecuteStoredProc(SPName)

        #region METHOD: ExecuteStoredProc(SPName, parameters) - OVERLOAD

        public DataTable ExecuteStoredProc(string SPName, SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(_connString);

            SqlCommand cmd = new SqlCommand(SPName, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            // call the FillParameters method passing to it the command object and the array of parameters
            FillParameters(cmd, parameters);

            try
            {
                cmd.Connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);

                return dataTable;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        #endregion METHOD: ExecuteStoredProc(SPName, parameters) - OVERLOAD

        #region METHOD: ExecuteSql(sql)
        /// <summary>
        /// Executes a string of SQL code and returns a DataTable result set.
        /// </summary>
        /// <param name="sql">String: The SQL command to be executed.</param>
        /// <returns>DataTable: Returns a DataTable of the result set.</returns>
        public DataTable ExecuteSql(string sql)
        {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.Connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);

                return dataTable;
            }
            catch (Exception)
            {
                throw;
            } 
            finally
            {
                if(cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }
        #endregion METHOD: ExecuteSql(sql)

        #region METHOD: ExecuteSql(sql, parameters) - OVERLOAD
        /// <summary>
        /// Executes a string of SQL code with parameters and returns a DataTable result set.
        /// </summary>
        /// <param name="sql">String: The SQL command to be executed.</param>
        /// <param name="parameters">SqlParameter[]: An array of SqlParameters required by the SQL query.</param>
        /// <returns>DataTable: Returns a DataTable of the result set.</returns>
        public DataTable ExecuteSql(string sql, SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.CommandType = CommandType.Text; // default (redundant)

            FillParameters(cmd, parameters);

            try
            {
                cmd.Connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);

                return dataTable;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }
        #endregion METHOD: ExecuteSql(sql, parameters) - OVERLOAD

        #region METHOD: ExecuteNonQuerySql(sql)
        /// <summary>
        /// Executes a non-query (INSERTs, UPDATEs, and DELETEs) using an 'embedded' SQL query string.
        /// </summary>
        /// <param name="sql">String: The SQL command to be executed.</param>
        /// <returns>int: Returns an integer indicating the number of rows affected.</returns>
        public int ExecuteNonQuerySql(string sql)
        {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.Connection.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }
        #endregion METHOD: ExecuteNonQuerySql(sql)

        #region METHOD: ExecuteNonQuerySql(sql, parameters) - OVERLOAD
        /// <summary>
        /// Executes a non-query (INSERTs, UPDATEs, and DELETEs) using an 'embedded' SQL query string and SqlParameters.
        /// </summary>
        /// <param name="sql">String: The SQL command to be executed.</param>
        /// <param name="parameters">SqlParameter[]: An array of SqlParameters required for the query.</param>
        /// <returns>int: Returns an integer indicating the number of rows affected.</returns>
        public int ExecuteNonQuerySql(string sql, SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.CommandType = CommandType.Text;

            FillParameters(cmd, parameters);

            try
            {
                cmd.Connection.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }
        #endregion METHOD: ExecuteNonQuerySql(sql, parameters) - OVERLOAD

        #region METHOD: ExecuteNonQuerySP(SPName, parameters)
        /// <summary>
        /// Executes a non-query stored procedure with Parameters for INSERTs, UPDATEs, or DELETEs.
        /// </summary>
        /// <param name="SPName">string: The name of the stored procedure to be executed.</param>
        /// <param name="parameters">SqlParameter[]: An array of SqlParameters required by the Stored Procedure.</param>
        /// <returns>int: Indicating the number of rows affected by the query.</returns>
        public int ExecuteNonQuerySP(string SPName, SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(SPName, conn);

            cmd.CommandType = CommandType.StoredProcedure;
                        
            FillParameters(cmd, parameters);

            try
            {
                cmd.Connection.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }

        #endregion METHOD: ExecuteNonQuerySP(SPName, parameters)

        #region METHOD: ExecuteScalarSql(sql)
        /// <summary>
        /// Executes a string of SQL code and returns the value at the first row/column intersection of the result set, ignoring the rest.
        /// </summary>
        /// <param name="sql">String: The SQL command to be executed.</param>
        /// <returns>object: Returns a single value as type object.</returns>
        public object ExecuteScalarSql(string sql)
        {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.Connection.Open();

                object returnValue = cmd.ExecuteScalar();

                return returnValue;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }

        #endregion METHOD: ExecuteScalarSql(sql)

        #region METHOD: ExecuteScalarSql(sql, parameters) - OVERLOAD
        /// <summary>
        /// Executes a string of SQL code with parameters and returns the value at the first row/column intersection of the result set, ignoring the rest.
        /// </summary>
        /// <param name="sql">String: The SQL command to be executed.</param>
        /// <param name="parameters">SqlParameter[]: An array of SqlParameters required for the query.</param>
        /// <returns>object: Returns a single value as type object.</returns>
        public object ExecuteScalarSql(string sql, SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.CommandType = CommandType.Text;

            FillParameters(cmd, parameters);

            try
            {
                cmd.Connection.Open();

                object returnValue = cmd.ExecuteScalar();

                return returnValue;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }
        #endregion METHOD: ExecuteScalarSql(sql, parameters) - OVERLOAD

        #region METHOD: ExecuteScalarSP(SPName)
        /// <summary>
        /// Executes a stored procedure and returns the value at the first row/column intersection of the result set, ignoring the rest.
        /// </summary>
        /// <param name="SPName">String: the name of the Stored Procedure to be executed.</param>
        /// <returns>object: Returns a single value as type object.</returns>
        public object ExecuteScalarSP(string SPName)
        {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(SPName, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Connection.Open();

                object returnValue = cmd.ExecuteScalar();

                return returnValue;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }
        #endregion METHOD: ExecuteScalarSP(SPName)

        #region METHOD: ExecuteScalarSP(SPName, parameters) - OVERLOAD
        /// <summary>
        /// Executes a stored procedure with parameters and returns the value at the first row/column intersection of the result set, ignoring the rest.
        /// </summary>
        /// <param name="SPName">String: the name of the Stored Procedure to be executed.</param>
        /// <param name="parameters">SqlParameter[]: An array of SqlParameters required by the Stored Procedure.</param>
        /// <returns>object: Returns a single value as type object.</returns>
        public object ExecuteScalarSP(string SPName, SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(SPName, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            FillParameters(cmd, parameters);

            try
            {
                cmd.Connection.Open();

                object returnValue = cmd.ExecuteScalar();

                return returnValue;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

        }
        #endregion METHOD: ExecuteScalarSP(SPName, parameters) - OVERLOAD

        #region METHOD: FillParameters(cmd, parameters)
        /// <summary>
        /// Fills the SqlCommand object with the supplied array of SqlParameters 
        /// </summary>
        /// <param name="cmd">SqlCommand: The SqlCommand object to which parameters are to be added.</param>
        /// <param name="parameters">SqlParameter[]: Array of SqlParameter objects.</param>
        private void FillParameters(SqlCommand cmd, SqlParameter[] parameters)
        {
            // Loop through array of parameters passed into method, add to command object's parameters collection
            foreach (SqlParameter parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }
        }
        #endregion METHOD: FillParameters(cmd, parameters)
        
        #endregion Public Methods
    }
}