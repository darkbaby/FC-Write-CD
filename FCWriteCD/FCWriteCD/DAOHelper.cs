using FCWriteCD.Module;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FCWriteCD
{
    public class DAOHelper
    {
        private static DAOHelper instance;

        public static DAOHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DAOHelper();
                }
                return instance;
            }
        }

        private string targetPathMapDrive;
        private string usernameMapDrive;
        private string passwordMapDrive;
        private string emptyDriveToMap;

        private DatabaseModule databaseModule;

        private DAOHelper()
        {
            IniModule iniModule = new IniModule(AppDomain.CurrentDomain.BaseDirectory + @"\setting.ini");
            SettingBean settingBean = new SettingBean();
            settingBean.dataSource = iniModule.IniReadValue("Database", "DataSource");
            settingBean.databaseName = iniModule.IniReadValue("Database", "DatabaseName");
            settingBean.username = iniModule.IniReadValue("Database", "Username");
            settingBean.password = iniModule.IniReadValue("Database", "Password");
            settingBean.databaseTimeout = Int32.Parse(iniModule.IniReadValue("Database", "DatabaseTimeout"));

            databaseModule = new DatabaseModule(settingBean.dataSource, settingBean.databaseName, settingBean.username,
                settingBean.password, settingBean.databaseTimeout);
        }

        public bool CheckConnection()
        {
            try
            {
                string query = "SELECT Value FROM FC_SETTING WHERE Name = '{0}'";
                targetPathMapDrive = databaseModule.ExecuteQuery(string.Format(query, "targetPathMapDrive")).Rows[0][0].ToString();
                usernameMapDrive = databaseModule.ExecuteQuery(string.Format(query, "usernameMapDrive")).Rows[0][0].ToString();
                passwordMapDrive = databaseModule.ExecuteQuery(string.Format(query, "passwordMapDrive")).Rows[0][0].ToString();
                emptyDriveToMap = databaseModule.ExecuteQuery(string.Format(query, "emptyDriveToMap")).Rows[0][0].ToString();
                return databaseModule.CheckConnection();                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetStoreName(string storeCode)
        {
            string query = "SELECT StoreNameT FROM FC_STORE WHERE StoreCode = '{0}'";
            DataTable dtTable = databaseModule.ExecuteQuery(string.Format(query, storeCode));
            if (dtTable.Rows.Count > 0)
            {
                return dtTable.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        public string GetSettingValue(string name)
        {
            switch (name)
            {
                case "targetPathMapDrive":
                    return targetPathMapDrive;
                case "usernameMapDrive":
                    return usernameMapDrive;
                case "passwordMapDrive":
                    return passwordMapDrive;
                case "emptyDriveToMap":
                    return emptyDriveToMap;
                default:
                    return "";
            }
        }

        public List<DataRow> GetStoreList()
        {
            string query = "SELECT StoreCode,StoreNameT FROM FC_STORE WHERE ExpDate >= getDate() - 150 ORDER BY StoreCode ASC";
            DataTable dtTable = databaseModule.ExecuteQuery(query);
            return dtTable.AsEnumerable().ToList();
        }

        public List<DataRow> GetComputerList()
        {
            string query = "SELECT * FROM FC_COMPUTER ORDER BY Sequence ASC";
            DataTable dtTable = databaseModule.ExecuteQuery(query);
            return dtTable.AsEnumerable().ToList();
        }

        public bool GetAccessForWorking(string computerName)
        {
            string query = "SELECT * FROM FC_Computer WHERE ComputerName = '{0}'";
            DataTable dtTable = databaseModule.ExecuteQuery(string.Format(query, computerName));
            if (dtTable.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetDeviceID(string computerName)
        {
            string query = "SELECT DeviceID FROM FC_Computer WHERE ComputerName = '{0}'";
            DataTable dtTable = databaseModule.ExecuteQuery(string.Format(query, computerName));
            if (dtTable.Rows.Count > 0)
            {
                if (dtTable.Rows[0][0] != DBNull.Value)
                {
                    return dtTable.Rows[0][0].ToString();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public List<DataRow> GetMapComputerList(int year, int month)
        {
            string query = "SELECT ComputerName FROM FC_MAP_COMPUTER_WORKING_STORE WHERE Year = {0} AND Month = {1} GROUP BY ComputerName";
            DataTable dtTable = databaseModule.ExecuteQuery(string.Format(query, year, month));
            return dtTable.AsEnumerable().ToList();
        }

        public List<DataRow> GetMapComputerWorkingStore(int year, int month)
        {
            string query = "SELECT * FROM FC_MAP_COMPUTER_WORKING_STORE WHERE Year = {0} AND Month = {1}";
            DataTable dtTable = databaseModule.ExecuteQuery(string.Format(query, year, month));
            return dtTable.AsEnumerable().ToList();
        }

        public List<DataRow> GetMapComputerWorkingStore(int year, int month, string computerName)
        {
            string query = "SELECT * FROM FC_MAP_COMPUTER_WORKING_STORE WHERE Year = {0} AND Month = {1} AND ComputerName = '{2}'";
            DataTable dtTable = databaseModule.ExecuteQuery(string.Format(query, year
                , month, computerName));
            return dtTable.AsEnumerable().ToList();
        }

        public void ClearMapComputerWorkingStore(int year, int month)
        {
            string query = "DELETE FROM FC_MAP_COMPUTER_WORKING_STORE WHERE Year = {0} AND Month = {1}";
            databaseModule.ExecuteQueryNonReturn(string.Format(query, year, month));
        }

        public DataTable GetWroteStoreList(int year, int month, string computerName)
        {
            if ("All".Equals(computerName))
            {
                string query = "SELECT StoreCode,ComputerName,FinishTime FROM FC_WROTE_DATA WHERE Year={0} AND Month={1}";
                DataTable dtTable = databaseModule.ExecuteQuery(string.Format(query, year, month));
                return dtTable;
            }
            else
            {
                string query = "SELECT StoreCode,ComputerName,FinishTime FROM FC_WROTE_DATA WHERE Year={0} AND Month={1} AND ComputerName='{2}'";
                DataTable dtTable = databaseModule.ExecuteQuery(string.Format(query, year, month, computerName));
                return dtTable;
            }
        }

        public void InsertWroteStoreData(int year, int month, string storeCode, string computerName,
            DateTime startDT, DateTime endDT)
        {
            string query = "INSERT INTO FC_WROTE_DATA VALUES({0},{1},'{2}','{3}','{4}','{5}')";
            databaseModule.ExecuteQueryNonReturn(string.Format(query, year, month, storeCode, computerName,
                startDT.ToString("yyyy-MM-dd HH:mm:ss.fff"), endDT.ToString("yyyy-MM-dd HH:mm:ss.fff")));
        }

        public List<DataRow> Execute_SP_CustomSelectStoreName(List<string> toCreateXML)
        {
            string xmlParameter = databaseModule.CreateXMLParameter("StoreCode", toCreateXML);
            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            sqlParameterList.Add(new SqlParameter("@StoreCode", xmlParameter));

            DataTable dtTable = databaseModule.ExecuteStoreProcedure("SP_CustomSelectStoreName", sqlParameterList);
            return dtTable.AsEnumerable().ToList();
        }

        public void Execute_SP_MapComputerWorkingStore(List<string> toCreateXML, string computerName,
            int year, int month)
        {
            string xmlParameter = databaseModule.CreateXMLParameter("StoreCode", toCreateXML);

            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            sqlParameterList.Add(new SqlParameter("@Year", year));
            sqlParameterList.Add(new SqlParameter("@Month", month));
            sqlParameterList.Add(new SqlParameter("@ComputerName", computerName));
            sqlParameterList.Add(new SqlParameter("@StoreCode", xmlParameter));

            databaseModule.ExecuteStoreProcedureNonReturn("SP_MapComputerWorkingStore", sqlParameterList);
        }

        public void InsertLog(string computerName, Log_Type type, string description, int lineCode
            , int errorFlag, string errorMessage = null)
        {
            string query = "INSERT INTO FC_LOG VALUES('{0}','{1}','{2}','{3}','{4}',{5},'{6}')";
            databaseModule.ExecuteQueryNonReturn(string.Format(query, computerName, type.ToString(),
                description, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), lineCode.ToString("0000"),
                errorFlag, errorMessage));
        }

        public void UpdateDeviceID(string computerName, string deviceID)
        {
            string query = "UPDATE FC_COMPUTER SET DeviceID = '{0}' WHERE ComputerName = '{1}'";
            databaseModule.ExecuteQueryNonReturn(string.Format(query, deviceID, computerName));
        }
    }
}
