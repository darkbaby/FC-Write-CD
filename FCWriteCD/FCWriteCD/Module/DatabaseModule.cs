using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FCWriteCD.Module
{
    class DatabaseModule
    {
        private SqlConnection connection;

        private string connectionString;

        public DatabaseModule()
            : this("192.168.10.192", "FC_WRITE_CD", "sa", "P@ssw0rd", 30)
        {

        }

        public DatabaseModule(string dataSource, string databaseName, string username, string password, int timeout)
        {
            StringBuilder connectionStr = new StringBuilder();
            connectionStr.Append("Data Source=" + dataSource + ";");
            connectionStr.Append("Initial Catalog=" + databaseName + ";");
            if (username != "")
            {
                connectionStr.Append("User id=" + username + ";");
                if (password != "")
                {
                    connectionStr.Append("Password=" + password + ";");
                }
            }
            else
            {
                connectionStr.Append("Integrated Security=true;");
            }

            connectionStr.Append("Connection Timeout=" + timeout.ToString() + ";");
            this.connection = new SqlConnection(connectionStr.ToString());
            connectionString = connectionStr.ToString();
        }

        public bool CheckConnection()
        {
            try 
            {
                OpenConnection();
                CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private void OpenConnection()
        {
            if (this.connection.State == ConnectionState.Closed)
            {
                this.connection.Open();
            }
        }

        private void CloseConnection()
        {
            if (this.connection.State != ConnectionState.Closed)
            {
                this.connection.Close();
            }
        }

        public DataTable ExecuteQuery(string query)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.Text;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }


        }

        public void ExecuteQueryNonReturn(string query)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't execute sql query");
            }
            finally
            {
                CloseConnection();
            }

        }

        public DataTable ExecuteStoreProcedure(string procedureName, List<SqlParameter> sqlParameterList)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter para in sqlParameterList)
                {
                    command.Parameters.Add(para);
                }
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }

        }

        public void ExecuteStoreProcedureNonReturn(string procedureName, List<SqlParameter> sqlParameterList)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter para in sqlParameterList)
                {
                    command.Parameters.Add(para);
                }
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }

        }

        public void ExecuteStoreProcedureNonReturn(string procedureName)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }

        }

        public string CreateXMLParameter(string node, List<string> lAttr)
        {
            string root = "Para";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<{0}>", root);
            foreach (string str in lAttr)
            {
                sb.AppendFormat("<{0}>{1}</{0}>", node, str);
            }
            sb.AppendFormat("</{0}>", root);

            return sb.ToString();
        }
    }

    public enum Log_Type
    {
        Process, Database, Disc, DiscExtra, Directory
    }
}
