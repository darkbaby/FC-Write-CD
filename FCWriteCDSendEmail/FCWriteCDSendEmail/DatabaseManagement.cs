using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCWriteCDSendEmail
{
    class DatabaseManagement
    {
        private SqlConnection connection;

        public DatabaseManagement(string dataSource, string databaseName, string username, string password, int timeout)
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
            connection = new SqlConnection(connectionStr.ToString());
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

        public DataTable GetAllComputerMapStore(int year, int month)
        {
            OpenConnection();
            string query = "SELECT ComputerName,t1.StoreCode,t2.StoreNameT FROM FC_MAP_COMPUTER_WORKING_STORE t1 LEFT JOIN FC_STORE t2 ON t1.StoreCode = t2.StoreCode WHERE Year = {0} AND Month = {1}";
            SqlCommand command = new SqlCommand(string.Format(query, year, month), connection);
            command.CommandType = CommandType.Text;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            CloseConnection();

            return dataTable;
        }

        public Dictionary<string, DataTable> AdjustComputerMapStore(DataTable dataTable, int year, int month)
        {
            Dictionary<string, DataTable> dicToReturn = new Dictionary<string, DataTable>();

            OpenConnection();
            string query = "SELECT DISTINCT t2.Sequence, t1.ComputerName FROM FC_MAP_COMPUTER_WORKING_STORE t1 LEFT JOIN FC_COMPUTER t2 ON t1.ComputerName = t2.ComputerName WHERE Year = {0} AND Month = {1} ORDER BY Sequence ASC";
            SqlCommand command = new SqlCommand(string.Format(query, year, month), connection);
            command.CommandType = CommandType.Text;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable computerTable = new DataTable();
            dataAdapter.Fill(computerTable);
            CloseConnection();

            foreach (DataRow rowDistince in computerTable.Rows)
            {
                DataTable tempDataTable = new DataTable();
                tempDataTable.Columns.Add("StoreCode", typeof(string));
                tempDataTable.Columns.Add("StoreName", typeof(string));
                string currentComputerName = rowDistince[1].ToString();
                foreach (DataRow row in dataTable.Rows)
                {
                    string computerNameForCompare = row[0].ToString();
                    if (currentComputerName.Equals(computerNameForCompare))
                    {
                        DataRow rowForAdd = tempDataTable.NewRow();
                        rowForAdd[0] = row[1].ToString();
                        rowForAdd[1] = row[2].ToString();
                        tempDataTable.Rows.Add(rowForAdd);
                    }
                }
                dicToReturn.Add(currentComputerName, tempDataTable);
            }

            return dicToReturn;
        }

        public void SendEmail(string fileName)
        {

            //string dataSource = @"192.168.10.185";
            //string databaseName = "HRMSDB";
            //string username = "sa";
            //string password = "?admin009$$";
            //int timeout = 20;

            //StringBuilder connectionStr = new StringBuilder();
            //connectionStr.Append("Data Source=" + dataSource + ";");
            //connectionStr.Append("Initial Catalog=" + databaseName + ";");
            //if (username != "")
            //{
            //    connectionStr.Append("User id=" + username + ";");
            //    if (password != "")
            //    {
            //        connectionStr.Append("Password=" + password + ";");
            //    }
            //}
            //else
            //{
            //    connectionStr.Append("Integrated Security=true;");
            //}

            //connectionStr.Append("Connection Timeout=" + timeout.ToString() + ";");
            //SqlConnection connection2 = new SqlConnection(connectionStr.ToString());

            //connection2.Open();

            OpenConnection();

            string query = @"EXEC msdb.dbo.sp_send_dbmail @profile_name = '{0}', @recipients = '{1}', @body = '{2}', @subject = '{3}', @file_attachments='{4}'";
            SqlCommand command = new SqlCommand(string.Format(query, "FC_WRITE_CD_PROFILE", "chotirote.k.esynergy@gmail.com", "FYI", "FC_WRITE_CD", fileName), connection);
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();

            CloseConnection();

            //connection2.Close();
        }
    }
}
