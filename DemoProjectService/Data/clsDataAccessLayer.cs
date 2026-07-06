using MicroserviesWebApplication.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;



namespace DataAccess
{
    /*
    public struct SqlParamData
    {
        public string pName, pValue;
        public SqlDbType pDataType;
        public ParameterDirection pDirection;
        public SqlParamData(string pName, SqlDbType pDataType, string pValue, ParameterDirection pDirection)
        {
            this.pName = pName;
            this.pDataType = pDataType;
            this.pValue = pValue;
            this.pDirection = pDirection;
        }
    }

    public struct MySqlParamData
    {
        public string pName, pValue;
        public MySqlDbType pDataType;
        public ParameterDirection pDirection;
        public MySqlParamData(string pName, MySqlDbType pDataType, string pValue, ParameterDirection pDirection)
        {
            this.pName = pName;
            this.pDataType = pDataType;
            this.pValue = pValue;
            this.pDirection = pDirection;
        }
    }
    */
    public class clsDataAccessLayer : Object
    {
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        #region Variables Declaration

        #region SQL
        protected SqlCommand mobjSqlCommand;
        protected SqlConnection mobjSqlConection;
        protected SqlDataAdapter mobjSqlAdapter;
        protected SqlDataReader mobjSqlDataReader;
        protected SqlException mobjException;
        #endregion

        #region mySQL
        protected MySqlCommand mobjMySqlCommand;
        protected MySqlConnection mobjMySqlConection;
        protected MySqlDataAdapter mobjMySqlAdapter;
        protected MySqlDataReader mobjMySqlDataReader;
        protected MySqlException mobjMyException;
        #endregion

        protected string msConnectionString;
        protected string msConnectionType;
        protected DataTable mdtTable;
        protected DataSet mdsDataSet;
        protected string msQuery;

        public enum enumConnectionType
        {
            MSSQL = '1',
            MYSQL = '2'
        };

        #endregion

        /// SUMMARY : GET DATABASE CONNECTION AND TYPE
        /// PARAMETER : NONE
        /// RETURN : NONE
        #region DataBase Settings
        private void LoadDataBaseSettings()
        {
            if (ConfigurationSettings.AppSettings["ConnectionType"] == null)
            {
                msConnectionType = "mssql2k";
            }
            if (ConfigurationSettings.AppSettings["ConnectionString"] == null)
            {
                var configuation = GetConfiguration();
                msConnectionString = (configuation.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value).ToString();
            }
            msConnectionType = msConnectionType.Trim().ToLower();
            if (msConnectionType.Equals("mssql2k"))
                msConnectionType = enumConnectionType.MSSQL.ToString();
            else if (msConnectionType.Equals("mssql"))
                msConnectionType = enumConnectionType.MSSQL.ToString();
            else if (msConnectionType.Equals("mysql"))
                msConnectionType = enumConnectionType.MYSQL.ToString();
        }
        #endregion

        /// SUMMARY : CONSTRUCTOR INITIALIZE PARAMETERS
        /// PARAMETER : NONE
        /// RETURN : NONE
        #region Constructor/Destructor

        public clsDataAccessLayer()
        {

            LoadDataBaseSettings();

            if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
            {
                mobjSqlCommand = new SqlCommand();
                mobjSqlConection = new SqlConnection(msConnectionString);
                mobjSqlCommand.Connection = mobjSqlConection;
                msConnectionString = string.Empty;
                msQuery = string.Empty;
                mobjSqlAdapter = new SqlDataAdapter();
                mdsDataSet = new DataSet();
            }
            else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
            {
                mobjMySqlCommand = new MySqlCommand();
                mobjMySqlConection = new MySqlConnection(msConnectionString);
                mobjMySqlCommand.Connection = mobjMySqlConection;
                msConnectionString = string.Empty;
                msQuery = string.Empty;
                mobjMySqlAdapter = new MySqlDataAdapter();
                mdsDataSet = new DataSet();
            }
        }
        #endregion

        #region open / close DataBase
        /// SUMMARY : OPEN DATABASE
        /// PARAMETER : NONE
        /// RETURN : NONE
        public bool OpenDataBase()
        {
            try
            {
                if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
                {
                    if (mobjSqlConection.State == ConnectionState.Closed)
                        mobjSqlConection.Open();
                    return true;
                }
                else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
                {
                    if (mobjMySqlConection.State == ConnectionState.Closed)
                        mobjMySqlConection.Open();
                    return true;
                }
                return false;
            }
            catch (System.Exception ex)
            {
                Console.Write(ex.Message);
                throw;
                return false;
                //throw (new System.Exception);
            }
        }

        /// SUMMARY : CLOSE DATABASE
        /// PARAMETER : NONE
        /// RETURN : NONE
        public bool CloseDataBase()
        {
            try
            {
                if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
                {
                    if (mobjSqlConection != null)
                    {
                        if (mobjSqlConection.State == ConnectionState.Open)
                        {
                            mobjSqlConection.Close();
                            mobjSqlConection.Dispose();
                        }
                    }
                    return true;
                }
                else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
                {
                    if (mobjMySqlConection != null)
                    {
                        if (mobjMySqlConection.State == ConnectionState.Open)
                        {
                            mobjMySqlConection.Close();
                            mobjMySqlConection.Dispose();
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (System.Exception ex)
            {
                Console.Write(ex.Message);
                throw;
                return false;
                //throw (new System.Exception);
            }
        }
        #endregion

        #region Data Fetching/Manuplation Methods
        /// SUMMARY : EXECUTE QUERY AND RETURN DATATABLE
        /// PARAMETER : QUERY STRING
        /// RETURN : DATATABLE
        public DataTable ExecuteDataTable(string rQry)
        {
            msQuery = rQry;
            mdsDataSet = new DataSet();
            mdtTable = new DataTable();
            if (msQuery != "" || msQuery != string.Empty || msQuery != null)
            {
                try
                {
                    OpenDataBase();
                    if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
                    {
                        mobjSqlCommand.CommandType = CommandType.Text;
                        mobjSqlCommand.CommandText = msQuery;
                        mobjSqlCommand.Connection = mobjSqlConection;
                        mobjSqlCommand.CommandTimeout = 10000;
                        mobjSqlAdapter.SelectCommand = mobjSqlCommand;
                        mobjSqlAdapter.Fill(mdsDataSet, mobjSqlCommand.CommandText);
                    }
                    else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
                    {
                        mobjMySqlCommand.CommandType = CommandType.Text;
                        mobjMySqlCommand.CommandText = msQuery;
                        mobjMySqlCommand.Connection = mobjMySqlConection;
                        mobjMySqlCommand.CommandTimeout = 10000;
                        mobjMySqlAdapter.SelectCommand = mobjMySqlCommand;
                        mobjMySqlAdapter.Fill(mdsDataSet, mobjMySqlCommand.CommandText);
                    }
                    if (mdsDataSet != null && mdsDataSet.Tables.Count > 0)
                        mdtTable = mdsDataSet.Tables[0];
                    return mdtTable;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return null;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return null;
            }
        }

        /// SUMMARY : EXECUTE QUERY AND RETURN DATATABLE
        /// PARAMETER : SQL COMMAND
        /// RETURN : DATATABLE <summary>
        /// SUMMARY : EXECUTE QUERY AND RETURN DATATABLE
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        /// 
        public DataTable ExecuteDataTable(SqlCommand cmd)
        {
            msQuery = cmd.CommandText;
            mdsDataSet = new DataSet();
            mdtTable = new DataTable();
            if (msQuery != "" || msQuery != string.Empty || msQuery != null)
            {
                try
                {
                    OpenDataBase();
                    cmd.Connection = mobjSqlConection;
                    mobjSqlAdapter.SelectCommand = cmd;
                    mobjSqlAdapter.Fill(mdsDataSet, cmd.CommandText);
                    if (mdsDataSet != null && mdsDataSet.Tables.Count > 0)
                        mdtTable = mdsDataSet.Tables[0];
                    return mdtTable;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return null;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return null;
            }
        }
        public async Task<DataTable> ExecuteDataTable(SqlCommand cmd,string s)
        {
            msQuery = cmd.CommandText;
            mdsDataSet = new DataSet();
            mdtTable = new DataTable();
            if (msQuery != "" || msQuery != string.Empty || msQuery != null)
            {
                try
                {
                    OpenDataBase();
                    cmd.Connection = mobjSqlConection;
                    mobjSqlAdapter.SelectCommand = cmd;
                    mobjSqlAdapter.Fill(mdsDataSet, cmd.CommandText);
                    if (mdsDataSet != null && mdsDataSet.Tables.Count > 0)
                        mdtTable = mdsDataSet.Tables[0];
                    return mdtTable;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return null;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return null;
            }
        }

        /// SUMMARY : EXECUTE QUERY AND RETURN DATATABLE
        /// PARAMETER : SQL COMMAND
        /// RETURN : DATATABLE
        public DataTable ExecuteDataTable(SqlCommand cmd, int iPageSize, int iPageNum, out long iTotalRec)
        {
            iTotalRec = 0;
            msQuery = cmd.CommandText;
            mdsDataSet = new DataSet();
            mdtTable = new DataTable();
            if (msQuery != "" || msQuery != string.Empty || msQuery != null)
            {
                try
                {
                    OpenDataBase();
                    cmd.Connection = mobjSqlConection;
                    mobjSqlAdapter.SelectCommand = cmd;
                    mobjSqlAdapter.Fill(mdsDataSet, (iPageNum - 1) * iPageSize, iPageSize, "tbl");
                    if (mdsDataSet != null && mdsDataSet.Tables.Count > 0)
                        mdtTable = mdsDataSet.Tables[0];
                    return mdtTable;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return null;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return null;
            }
        }

        /// SUMMARY : EXECUTE QUERY AND RETURN DATATABLE
        /// PARAMETER : MYSQL COMMAND
        /// RETURN : DATATABLE
        public  DataTable ExecuteDataTable(MySqlCommand cmd)
        {
            msQuery = cmd.CommandText;
            mdsDataSet = new DataSet();
            mdtTable = new DataTable();
            if (msQuery != "" || msQuery != string.Empty || msQuery != null)
            {

                try
                {
                    OpenDataBase();
                    cmd.Connection = mobjMySqlConection;
                    mobjMySqlAdapter.SelectCommand = cmd;
                    mobjMySqlAdapter.Fill(mdsDataSet, cmd.CommandText);
                    if (mdsDataSet != null && mdsDataSet.Tables.Count > 0)
                        mdtTable = mdsDataSet.Tables[0];
                    return mdtTable;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return null;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return null;
            }
        }

        /// SUMMARY : EXECUTE QUERY AND RETURN DATATABLE
        /// PARAMETER : QUERY STRING, COMMAND PARAMS LIST
        /// RETURN : DATATABLE
        public DataTable ExecuteDataTable(string strQuery, Dictionary<string, object> dicObjParams)
        {
            mdtTable = new DataTable();
            if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
            {
                mobjSqlCommand = new SqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjSqlCommand.Parameters.AddWithValue(pair.Key, pair.Value);
                }
                mdtTable = this.ExecuteDataTable(mobjSqlCommand);
            }
            else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
            {
                mobjMySqlCommand = new MySqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjMySqlCommand.Parameters.AddWithValue(pair.Key, pair.Value);
                }
                mdtTable = this.ExecuteDataTable(mobjMySqlCommand);
            }
            return mdtTable;
        }

        ///// SUMMARY : EXECUTE QUERY AND RETURN DATATABLE
        ///// PARAMETER : MYSQL COMMAND
        ///// RETURN : DATATABLE
        //public DataTable ExecuteDataTablePaging(SqlCommand cmd, int iPageSize, int iPageNum)
        //{

        //    //SqlConnection conn = new SqlConnection(connectionString);
        //    //SqlCommand comm = new SqlCommand("select * from mytable", conn);
        //    //comm.Connection.Open();
        //    //SqlDataReader r =
        //    //    comm.ExecuteReader(CommandBehavior.CloseConnection);
        //    //while (r.Read())
        //    //{
        //    //    Console.WriteLine(r.GetString(0));
        //    //}
        //    //r.Close();
        //    //conn.Close();


        //    msQuery = cmd.CommandText;
        //    mdsDataSet = new DataSet();
        //    mdtTable = new DataTable();
        //    if (msQuery != "" || msQuery != string.Empty || msQuery != null)
        //    {
        //        try
        //        {
        //            OpenDataBase();
        //            cmd.Connection = mobjSqlConection;
        //            SqlDataReader objDR = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //            mdtTable = objDR.GetSchemaTable();
        //            System.Console.Write ( objDR.RecordsAffected);
        //            //while (objDR.Read())
        //            //{
        //            //    mdtTable.ImportRow(objDR
        //            //    Response.Write(reader["field"]); // or reader[0] - int or string lookup
        //            //}
        //            objDR.Close();


        //            mobjSqlAdapter.SelectCommand = cmd;
        //            mobjSqlAdapter.Fill(mdsDataSet, cmd.CommandText);
        //            if (mdsDataSet != null && mdsDataSet.Tables.Count > 0)
        //                mdtTable = mdsDataSet.Tables[0];
        //            return mdtTable;
        //        }
        //        catch (System.Exception ex)
        //        {
        //            Console.Write(ex.Message);
        //            throw;
        //            return null;
        //        }
        //        finally
        //        {
        //            CloseDataBase();
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// SUMMARY : PROCEDURE
        /// PARAMETER : QUERY STRING, COMMAND PARAMS LIST
        /// RETURN : DATATABLE
        public void ExecuteStoredProcedure(string strQuery, SqlParamData[] objParams)
        {
            mobjSqlCommand = new SqlCommand(strQuery);
            mobjSqlCommand.CommandType = CommandType.StoredProcedure;
            mobjSqlCommand.CommandTimeout = 5000;
            foreach (SqlParamData objParam in objParams)
            {
                if (objParam.Direction == ParameterDirection.Output)
                    mobjSqlCommand.Parameters.Add(objParam.Name, objParam.DataType, objParam.Length).Direction = objParam.Direction;
                else
                    mobjSqlCommand.Parameters.Add(objParam.Name, objParam.DataType).Value = objParam.Value;
            }
            this.ExecuteNonQuery(mobjSqlCommand);
            for (int i = 0; i < objParams.Length; i++)
            {
                if (objParams[i].Direction == ParameterDirection.Output)
                    objParams[i].Value = mobjSqlCommand.Parameters[objParams[i].Name].Value;

            }
        }
        public async Task<long> ExecuteStoredProcedureLong(string strQuery, SqlParamData[] objParams)
        {
            long lrt = 0;
            mobjSqlCommand = new SqlCommand(strQuery);
            mobjSqlCommand.CommandType = CommandType.StoredProcedure;
            mobjSqlCommand.CommandTimeout = 5000;
            foreach (SqlParamData objParam in objParams)
            {
                if (objParam.Direction == ParameterDirection.Output)
                    mobjSqlCommand.Parameters.Add(objParam.Name, objParam.DataType, objParam.Length).Direction = objParam.Direction;
                else
                    mobjSqlCommand.Parameters.Add(objParam.Name, objParam.DataType).Value = objParam.Value;
            }
            lrt = await this.ExecuteNonQuery(mobjSqlCommand);
            for (int i = 0; i < objParams.Length; i++)
            {
                if (objParams[i].Direction == ParameterDirection.Output)
                    objParams[i].Value = mobjSqlCommand.Parameters[objParams[i].Name].Value;

            }
            return lrt;
        }

        public  long ExecuteStoredProcedureLongSync(string strQuery, SqlParamData[] objParams)
        {
            long lrt = 0;
            mobjSqlCommand = new SqlCommand(strQuery);
            mobjSqlCommand.CommandType = CommandType.StoredProcedure;
            mobjSqlCommand.CommandTimeout = 5000;
            foreach (SqlParamData objParam in objParams)
            {
                if (objParam.Direction == ParameterDirection.Output)
                    mobjSqlCommand.Parameters.Add(objParam.Name, objParam.DataType, objParam.Length).Direction = objParam.Direction;
                else
                    mobjSqlCommand.Parameters.Add(objParam.Name, objParam.DataType).Value = objParam.Value;
            }
            lrt =  this.ExecuteNonQuerySync(mobjSqlCommand);
            for (int i = 0; i < objParams.Length; i++)
            {
                if (objParams[i].Direction == ParameterDirection.Output)
                    objParams[i].Value = mobjSqlCommand.Parameters[objParams[i].Name].Value;

            }
            return lrt;
        }

        public DataTable ExecuteStoredProcedureAndReturnDataTable(string strQuery, SqlParamData[] objParams)
        {
            DataTable dt = null;
            mobjSqlCommand = new SqlCommand(strQuery);
            mobjSqlCommand.CommandType = CommandType.StoredProcedure;
            foreach (SqlParamData objParam in objParams)
            {
                if (objParam.Direction == ParameterDirection.Output)
                    mobjSqlCommand.Parameters.Add(objParam.Name, objParam.DataType, objParam.Length).Direction = objParam.Direction;
                else
                    mobjSqlCommand.Parameters.Add(objParam.Name, objParam.DataType).Value = objParam.Value;
            }

            //this.ExecuteNonQuery(mobjSqlCommand);
            dt = this.ExecuteDataTable(mobjSqlCommand);
            return dt;
        }

        public async Task<DataTable> ExecuteStoredProcedureReturnDataTableAsync(string storedProcedureName, SqlParamData[] parameters)
        {
            //using (SqlConnection connection = new SqlConnection(msConnectionString))
            //{
                DataTable dt = null;
                mobjSqlCommand = new SqlCommand(storedProcedureName);
                mobjSqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (SqlParamData objParam in parameters)
                {
                    if (objParam.Direction == ParameterDirection.Output)
                        mobjSqlCommand.Parameters.Add(objParam.Name, objParam.DataType, objParam.Length).Direction = objParam.Direction;
                    else
                        mobjSqlCommand.Parameters.Add(objParam.Name, objParam.DataType).Value = objParam.Value;
                }
                OpenDataBase();
                mobjSqlCommand.Connection = mobjSqlConection;
                using (SqlDataAdapter adapter = new SqlDataAdapter(mobjSqlCommand))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable); // Fill DataTable with results
                        return dataTable;
                    }
             //   }
           // }
        }
    
    public async Task<string> ExecuteStoredProcedureAndReturnJsonAsync(string strQuery, SqlParamData[] objParams)
        {
            DataTable dt = null;
            mobjSqlCommand = new SqlCommand(strQuery);
            mobjSqlCommand.CommandType = CommandType.StoredProcedure;
            foreach (SqlParamData objParam in objParams)
            {
                if (objParam.Direction == ParameterDirection.Output)
                    mobjSqlCommand.Parameters.Add(objParam.Name, objParam.DataType, objParam.Length).Direction = objParam.Direction;
                else
                    mobjSqlCommand.Parameters.Add(objParam.Name, objParam.DataType).Value = objParam.Value;
            }
            OpenDataBase();
            mobjSqlCommand.Connection = mobjSqlConection;
            var jsonResult = await mobjSqlCommand.ExecuteScalarAsync();
            //int length = jsonResult.ToString().Length;
            return jsonResult?.ToString();

        }

        public void ExecuteStoredProcedure(string strQuery, MySqlParamData[] objParams)
        {
            mobjMySqlCommand = new MySqlCommand(strQuery);
            mobjMySqlCommand.CommandType = CommandType.StoredProcedure;
            foreach (MySqlParamData objParam in objParams)
            {
                if (objParam.Direction == ParameterDirection.Output)
                    mobjMySqlCommand.Parameters.Add(objParam.Name, objParam.DataType, objParam.Length).Direction = objParam.Direction;
                else
                    mobjMySqlCommand.Parameters.Add(objParam.Name, objParam.DataType).Value = objParam.Value;
            }
            this.ExecuteNonQuery(mobjMySqlCommand);
            for (int i = 0; i < objParams.Length; i++)
            {
                if (objParams[i].Direction == ParameterDirection.Output)
                    objParams[i].Value = mobjMySqlCommand.Parameters[objParams[i].Name].Value;

            }
        }

        ///// SUMMARY : EXECUTE QUERY AND RETURN DATATABLE
        ///// PARAMETER : QUERY STRING, COMMAND PARAMS LIST
        ///// RETURN : DATATABLE
        //public DataTable ExecuteDataTablePaging(string strQuery, Dictionary<string, object> dicObjParams, int iPageSize, int iPageNum, out long iTotalRec)
        //{
        //    mdtTable = new DataTable();
        //    iTotalRec = 0;
        //    if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
        //    {
        //        mobjSqlCommand = new SqlCommand(strQuery);
        //        foreach (KeyValuePair<string, object> pair in dicObjParams)
        //        {
        //            mobjSqlCommand.Parameters.AddWithValue(pair.Key, pair.Value);
        //        }
        //        mdtTable = this.ExecuteDataTable(mobjSqlCommand, iPageSize,iPageNum, out iTotalRec);
        //    }
        //    else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
        //    {
        //        mobjMySqlCommand = new MySqlCommand(strQuery);
        //        foreach (KeyValuePair<string, object> pair in dicObjParams)
        //        {
        //            mobjMySqlCommand.Parameters.AddWithValue(pair.Key, pair.Value);
        //        }
        //        mdtTable = this.ExecuteDataTable(mobjMySqlCommand);
        //    }
        //    return mdtTable;
        //}

        ///// SUMMARY : EXECUTE QUERY AND RETURN DATATABLE
        ///// PARAMETER : QUERY STRING, COMMAND PARAMS LIST
        ///// RETURN : DATATABLE
        ////public DataTable ExecuteDataTablePaging(string strProc, string strColumnIN, string strColumnOut, string strTable, string strWhere, string strOrderByIn, string strOrderByOut, int iPageSize, int iPageNum, out long iTotalRec)
        //public DataTable ExecuteDataTablePaging(string strProc, string strID, string strColumns, string strTable, string strWhere, string strOrderBy, int iPageSize, int iPageNum, out long iTotalRec)
        //{
        //    iTotalRec = 0;
        //    mdtTable = new DataTable();
        //    if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
        //    {
        //        mobjSqlCommand = new SqlCommand(strProc);
        //        mobjSqlCommand.CommandType = CommandType.StoredProcedure;
        //        //mobjSqlCommand.Parameters.Add("@currentPage", SqlDbType.Int).Value = iPageNum;
        //        //mobjSqlCommand.Parameters.Add("@pageSize", SqlDbType.Int).Value =iPageSize;
        //        //mobjSqlCommand.Parameters.Add("@SQLCOLUMNSOUT", SqlDbType.VarChar).Value =strColumnOut;
        //        //mobjSqlCommand.Parameters.Add("@SQLCOLUMNSIN", SqlDbType.VarChar).Value =strColumnIN;
        //        //mobjSqlCommand.Parameters.Add("@SQLTABLES", SqlDbType.VarChar).Value =strTable;
        //        //mobjSqlCommand.Parameters.Add("@SQLWHERE", SqlDbType.VarChar).Value =strWhere;
        //        //mobjSqlCommand.Parameters.Add("@SQLORDERBYOUT", SqlDbType.VarChar).Value =strOrderByOut;
        //        //mobjSqlCommand.Parameters.Add("@SQLORDERBYIN", SqlDbType.VarChar).Value =strOrderByIn;
        //        //mobjSqlCommand.Parameters.Add("@totalRec", SqlDbType.BigInt).Direction = ParameterDirection.Output;
        //        mobjSqlCommand.Parameters.Add("@currentPage", SqlDbType.Int).Value = iPageNum;
        //        mobjSqlCommand.Parameters.Add("@pageSize", SqlDbType.Int).Value = iPageSize;
        //        mobjSqlCommand.Parameters.Add("@QRYID", SqlDbType.VarChar).Value = strID;
        //        mobjSqlCommand.Parameters.Add("@SQLCOLUMNS", SqlDbType.VarChar).Value = strColumns;
        //        mobjSqlCommand.Parameters.Add("@SQLTABLES", SqlDbType.VarChar).Value = strTable;
        //        mobjSqlCommand.Parameters.Add("@SQLWHERE", SqlDbType.VarChar).Value = strWhere;
        //        mobjSqlCommand.Parameters.Add("@SQLORDERBY", SqlDbType.VarChar).Value = strOrderBy;
        //        mobjSqlCommand.Parameters.Add("@totalRec", SqlDbType.BigInt).Direction = ParameterDirection.Output;

        //        //CREATE PROCEDURE UserPaging
        //        //(
        //        //  @currentPage int = 1, @pageSize int =10,  @SQLCOLUMNSOUT varchar(1000), @SQLCOLUMNSIN varchar(1500), 
        //        //  @SQLTABLES varchar(1000), @SQLWHERE varchar(1000), @SQLORDERBYOUT varchar(500), @SQLORDERBYIN varchar(500), @totalRec int output
        //        //)

        //        //SqlCommand Cmd = new SqlCommand("dbo.UserPaging", cn);
        //        //Cmd.CommandType = CommandType.StoredProcedure;
        //        //SqlDataReader dr;
        //        //Cmd.Parameters.Add("@PageSize", SqlDbType.Int, 4).Value = pager1.PageSize;
        //        //Cmd.Parameters.Add("@CurrentPage", SqlDbType.Int, 4).Value = pager1.CurrentIndex;
        //        //Cmd.Parameters.Add("@ItemCount", SqlDbType.Int).Direction = ParameterDirection.Output;
        //        //cn.Open();
        //        //dr = Cmd.ExecuteReader();
        //        //rptProducts.DataSource = dr;
        //        //rptProducts.DataBind();
        //        //dr.Close();
        //        //cn.Close();
        //        //Int32 _totalRecords = Convert.ToInt32(Cmd.Parameters["@ItemCount"].Value);
        //        //pager1.ItemCount = _totalRecords;

        //        mdtTable = this.ExecuteDataTable(mobjSqlCommand);
        //        iTotalRec = Convert.ToInt64(mobjSqlCommand.Parameters["@totalRec"].Value);
        //    }
        //    else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
        //    {
        //        mobjMySqlCommand = new MySqlCommand(strProc);
        //        mobjMySqlCommand.CommandType = CommandType.StoredProcedure;
        //        //mobjMySqlCommand.Parameters.Add("@currentPage",MySqlDbType.Int32).Value = iPageNum;
        //        //mobjMySqlCommand.Parameters.Add("@pageSize", MySqlDbType.Int32).Value = iPageSize;
        //        //mobjMySqlCommand.Parameters.Add("@SQLCOLUMNSOUT", MySqlDbType.VarChar).Value = strColumnOut;
        //        //mobjMySqlCommand.Parameters.Add("@SQLCOLUMNSIN", MySqlDbType.VarChar).Value = strColumnIN;
        //        //mobjMySqlCommand.Parameters.Add("@SQLTABLES", MySqlDbType.VarChar).Value = strTable;
        //        //mobjMySqlCommand.Parameters.Add("@SQLWHERE", MySqlDbType.VarChar).Value = strWhere;
        //        //mobjMySqlCommand.Parameters.Add("@SQLORDERBYOUT", MySqlDbType.VarChar).Value = strOrderByOut;
        //        //mobjMySqlCommand.Parameters.Add("@SQLORDERBYIN", MySqlDbType.VarChar).Value = strOrderByIn;
        //        //mobjMySqlCommand.Parameters.Add("@totalRec", MySqlDbType.Int32).Direction = ParameterDirection.Output;
        //        mobjMySqlCommand.Parameters.Add("@currentPage", MySqlDbType.Int32).Value = iPageNum;
        //        mobjMySqlCommand.Parameters.Add("@pageSize", MySqlDbType.Int32).Value = iPageSize;
        //        mobjMySqlCommand.Parameters.Add("@QRYID", MySqlDbType.VarChar).Value = strID;
        //        mobjMySqlCommand.Parameters.Add("@SQLCOLUMNS", MySqlDbType.VarChar).Value = strColumns;
        //        mobjMySqlCommand.Parameters.Add("@SQLTABLES", MySqlDbType.VarChar).Value = strTable;
        //        mobjMySqlCommand.Parameters.Add("@SQLWHERE", MySqlDbType.VarChar).Value = strWhere;
        //        mobjMySqlCommand.Parameters.Add("@SQLORDERBY", MySqlDbType.VarChar).Value = strOrderBy;
        //        mobjMySqlCommand.Parameters.Add("@totalRec", MySqlDbType.Int32).Direction = ParameterDirection.Output;
        //        mdtTable = this.ExecuteDataTable(mobjMySqlCommand);
        //        iTotalRec = Convert.ToInt32(mobjMySqlCommand.Parameters["@totalRec"].Value);
        //        Console.WriteLine(iTotalRec);
        //    }
        //    return mdtTable;
        //}


        /// SUMMARY : EXECUTE QUERY AND RETURN DATATABLE
        /// PARAMETER : QUERY STRING
        /// RETURN : DATASET
        public DataSet ExecuteDataSet(string rQry)
        {
            msQuery = rQry;
            mdsDataSet = new DataSet();
            mdtTable = new DataTable();
            if (msQuery != "" || msQuery != string.Empty || msQuery != null)
            {

                try
                {
                    OpenDataBase();
                    if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
                    {
                        mobjSqlCommand.CommandType = CommandType.Text;
                        mobjSqlCommand.CommandText = msQuery;
                        mobjSqlAdapter.SelectCommand = mobjSqlCommand;
                        mobjSqlAdapter.Fill(mdsDataSet, mobjSqlCommand.CommandText);
                    }
                    else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
                    {
                        mobjMySqlCommand.CommandType = CommandType.Text;
                        mobjMySqlCommand.CommandText = msQuery;
                        mobjMySqlAdapter.SelectCommand = mobjMySqlCommand;
                        mobjMySqlAdapter.Fill(mdsDataSet, mobjMySqlCommand.CommandText);
                    }
                    return mdsDataSet;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return null;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return null;
            }
        }

        /// SUMMARY : EXECUTE COMMAND AND RETURN DATASET
        /// PARAMETER : SQL COMMAND
        /// RETURN : DATASET
        public DataSet ExecuteDataSet(SqlCommand cmd)
        {
            msQuery = cmd.CommandText;
            mdsDataSet = new DataSet();
            //mdtTable = new DataTable();
            if (msQuery != "" || msQuery != string.Empty || msQuery != null)
            {
                try
                {
                    OpenDataBase();
                    cmd.Connection = mobjSqlConection;
                    mobjSqlAdapter.SelectCommand = cmd;
                    mobjSqlAdapter.Fill(mdsDataSet, cmd.CommandText);
                    mobjSqlAdapter.Dispose();
                    return mdsDataSet;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return null;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return null;
            }
        }

        /// SUMMARY : EXECUTE COMMAND AND RETURN DATASET
        /// PARAMETER : MYSQL COMMAND
        /// RETURN : DATASET
        public DataSet ExecuteDataSet(MySqlCommand cmd)
        {
            msQuery = cmd.CommandText;
            mdsDataSet = new DataSet();
            mdtTable = new DataTable();
            if (msQuery != "" || msQuery != string.Empty || msQuery != null)
            {
                try
                {
                    OpenDataBase();
                    cmd.Connection = mobjMySqlConection;
                    mobjMySqlAdapter.SelectCommand = cmd;
                    mobjMySqlAdapter.Fill(mdsDataSet, cmd.CommandText);
                    mobjSqlAdapter.Dispose();
                    return mdsDataSet;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return null;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return null;
            }
        }

        /// SUMMARY : EXECUTE TRANSACTIONAL QUERY AND RETURN dataset
        /// PARAMETER : QUERY, COMMAND PARAMS LIST
        /// RETURN : dataset
        public DataSet ExecuteDataSet(String strQuery, Dictionary<string, object> dicObjParams)
        {
            DataSet ds = null;
            if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
            {
                mobjSqlCommand = new SqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjSqlCommand.Parameters.AddWithValue(pair.Key, pair.Value);
                }
                ds = this.ExecuteDataSet(mobjSqlCommand);
            }
            else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
            {
                mobjMySqlCommand = new MySqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjMySqlCommand.Parameters.AddWithValue(pair.Key, pair.Value);
                }
                ds = this.ExecuteDataSet(mobjMySqlCommand);
            }
            return ds;
        }

        /// SUMMARY : EXECUTE TRANSACTIONAL QUERY AND RETURN NUMBER OF RECORDS EFFECTED
        /// PARAMETER : QUERY STRING
        /// RETURN : NUMBER OF RECORDS EFFECTED
        public long ExecuteNonQuery(string rQry)
        {
            long iRecordEffected = 0;
            msQuery = rQry;
            if (msQuery != "" || msQuery != string.Empty || msQuery != null)
            {
                try
                {
                    OpenDataBase();
                    if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
                    {
                        //MySqlTransaction mobjTransaction = cnn.BeginTransaction();
                        //mobjSqlCommand.Transaction = mobjTransaction;
                        //mobjTransaction.Commit();
                        //mobjTransaction.Rollback();
                        mobjSqlCommand.CommandType = CommandType.Text;
                        mobjSqlCommand.CommandText = msQuery;
                        iRecordEffected = mobjSqlCommand.ExecuteNonQuery();
                    }
                    else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
                    {
                        mobjMySqlCommand.CommandType = CommandType.Text;
                        mobjMySqlCommand.CommandText = msQuery;
                        iRecordEffected = mobjMySqlCommand.ExecuteNonQuery();
                    }
                    return iRecordEffected;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return 0;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return 0;
            }
        }

        /// SUMMARY : EXECUTE TRANSACTIONAL QUERY AND RETURN NUMBER OF RECORDS EFFECTED
        /// PARAMETER : COMMAND
        /// RETURN : NUMBER OF RECORDS EFFECTED
        public async Task<long> ExecuteNonQuery(SqlCommand cmd)
        {
            long iRecordEffected = 0;
            msQuery = cmd.CommandText;
            if (msQuery != "" || msQuery != string.Empty || msQuery != null)
            {
                try
                {
                    OpenDataBase();
                    cmd.Connection = mobjSqlConection;
                    iRecordEffected = await cmd.ExecuteNonQueryAsync();
                    return iRecordEffected;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return 0;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return 0;
            }
        }

        public long ExecuteNonQuerySync(SqlCommand cmd)
        {
            long iRecordEffected = 0;
            msQuery = cmd.CommandText;
            if (msQuery != "" || msQuery != string.Empty || msQuery != null)
            {
                try
                {
                    OpenDataBase();
                    cmd.Connection = mobjSqlConection;
                    iRecordEffected = cmd.ExecuteNonQuery();
                    return iRecordEffected;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return 0;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return 0;
            }
        }

        /// SUMMARY : EXECUTE TRANSACTIONAL QUERY AND RETURN NUMBER OF RECORDS EFFECTED
        /// PARAMETER : MYSQL COMMAND
        /// RETURN : NUMBER OF RECORDS EFFECTED
        public long ExecuteNonQuery(MySqlCommand cmd)
        {
            long iRecordEffected = 0;
            msQuery = cmd.CommandText;
            if (msQuery != "" || msQuery != string.Empty || msQuery != null)
            {
                try
                {
                    OpenDataBase();
                    cmd.Connection = mobjMySqlConection;
                    iRecordEffected = cmd.ExecuteNonQuery();
                    return iRecordEffected;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return 0;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return 0;
            }
        }

        /// SUMMARY : EXECUTE TRANSACTIONAL BATCH QUERY AND RETURN NUMBER OF RECORDS EFFECTED
        /// PARAMETER : QUERY STRING
        /// RETURN : NUMBER OF RECORDS EFFECTED
        public long ExecuteBatchNonQuery(string[] rQrys)
        {
            long iRecordEffected = 0;
            //msQuery = rQry;
            if (rQrys != null && rQrys.Length > 0)
            {
                try
                {
                    OpenDataBase();
                    for (int i = 0; i < rQrys.Length; i++)
                    {
                        if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
                        {
                            //MySqlTransaction mobjTransaction = cnn.BeginTransaction();
                            //mobjSqlCommand.Transaction = mobjTransaction;
                            //mobjTransaction.Commit();
                            //mobjTransaction.Rollback();
                            mobjSqlCommand.CommandType = CommandType.Text;
                            mobjSqlCommand.CommandText = rQrys[i];
                            iRecordEffected = mobjSqlCommand.ExecuteNonQuery();
                        }
                        else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
                        {
                            mobjMySqlCommand.CommandType = CommandType.Text;
                            mobjMySqlCommand.CommandText = rQrys[i];
                            iRecordEffected = iRecordEffected + mobjMySqlCommand.ExecuteNonQuery();
                        }
                    }
                    return iRecordEffected;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return iRecordEffected;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return iRecordEffected;
            }
        }

        /// SUMMARY : EXECUTE TRANSACTIONAL BATCH QUERY AND RETURN NUMBER OF RECORDS EFFECTED
        /// PARAMETER : COMMAND
        /// RETURN : NUMBER OF RECORDS EFFECTED
        public long ExecuteBatchNonQuery(SqlCommand[] cmd)
        {
            long iRecordEffected = 0;
            //msQuery = cmd.CommandText;
            if (cmd != null && cmd.Length > 0)
            {
                try
                {
                    OpenDataBase();
                    for (int i = 0; i < cmd.Length; i++)
                    {
                        if (cmd[i].CommandText.Length > 0)
                        {
                            cmd[i].Connection = mobjSqlConection;
                            iRecordEffected = iRecordEffected + cmd[i].ExecuteNonQuery();
                        }
                    }
                    return iRecordEffected;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return iRecordEffected;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return iRecordEffected;
            }
        }

        /// SUMMARY : EXECUTE TRANSACTIONAL BATCH QUERY AND RETURN NUMBER OF RECORDS EFFECTED
        /// PARAMETER : MYSQL COMMAND
        /// RETURN : NUMBER OF RECORDS EFFECTED
        public long ExecuteBatchNonQuery(MySqlCommand[] cmd)
        {
            long iRecordEffected = 0;
            //msQuery = cmd.CommandText;
            if (cmd != null && cmd.Length > 0)
            {
                try
                {
                    OpenDataBase();
                    for (int i = 0; i < cmd.Length; i++)
                    {
                        if (cmd[i].CommandText.Length > 0)
                        {
                            cmd[i].Connection = mobjMySqlConection;
                            iRecordEffected = iRecordEffected + cmd[i].ExecuteNonQuery();
                        }
                    }
                    return iRecordEffected;
                }
                catch (System.Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                    return 0;
                }
                finally
                {
                    //CloseDataBase();
                }
            }
            else
            {
                return 0;
            }
        }

        public long ExecuteBatchNonQuery(object[] objParam)
        {
            long iRecordEffected = 0;
            if (objParam.GetType() != null && objParam.Length > 0)
            {
                if (objParam[0].GetType().FullName.Equals("System.Data.SqlClient.SqlCommand"))
                {
                    SqlCommand[] objSqlCmd = new SqlCommand[objParam.Length];
                    Array.Copy(objParam, objSqlCmd, objParam.Length);
                    iRecordEffected = ExecuteBatchNonQuery(objSqlCmd);
                }
                else if (objParam[0].GetType().FullName.Equals("MySql.Data.MySqlClient.MySqlCommand"))
                {
                    MySqlCommand[] objMySqlCmd = new MySqlCommand[objParam.Length];
                    Array.Copy(objParam, objMySqlCmd, objParam.Length);
                    iRecordEffected = ExecuteBatchNonQuery(objMySqlCmd);
                }
                //if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
                //    iRecordEffected = ExecuteBatchNonQuery((SqlCommand[])objParam);
                //else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
                //    iRecordEffected = ExecuteBatchNonQuery((MySqlCommand[])objParam);
                //iRecordEffected = ExecuteBatchNonQuery();
            }
            return iRecordEffected;
        }

        /// SUMMARY : CONVERT QUERY TO COMAND OBJECT
        /// PARAMETER : QUERY, COMMAND PARAMS LIST
        /// RETURN : NUMBER OF RECORDS EFFECTED
        public object ToCommandObject(String strQuery, Dictionary<string, object> dicObjParams)
        {
            if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
            {
                mobjSqlCommand = new SqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjSqlCommand.Parameters.AddWithValue(pair.Key, pair.Value);
                }
                return mobjSqlCommand;
            }
            else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
            {
                mobjMySqlCommand = new MySqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjMySqlCommand.Parameters.AddWithValue(pair.Key, pair.Value);
                }
                return mobjMySqlCommand;
            }
            return null;
        }

        /// SUMMARY : EXECUTE TRANSACTIONAL QUERY AND RETURN NUMBER OF RECORDS EFFECTED
        /// PARAMETER : QUERY, COMMAND PARAMS LIST
        /// RETURN : NUMBER OF RECORDS EFFECTED
        public async Task<long> ExecuteNonQuery(String strQuery, Dictionary<string, object> dicObjParams)
        {
            long iRecordEffected = 0;
            if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
            {
                mobjSqlCommand = new SqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjSqlCommand.Parameters.AddWithValue(pair.Key, IsDBNull(pair.Value));
                }
                iRecordEffected = await this.ExecuteNonQuery(mobjSqlCommand);
            }
            else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
            {
                mobjMySqlCommand = new MySqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjMySqlCommand.Parameters.AddWithValue(pair.Key, IsDBNull(pair.Value));
                }
                iRecordEffected = this.ExecuteNonQuery(mobjMySqlCommand);
            }

            try
            {
                long lrt = 0;
                //LOG Entry
                lrt = await ExecuteLogQuery(strQuery, dicObjParams);


            }

            catch (Exception ex)
            {
                //
            }
            return iRecordEffected;

        }
        public  long ExecuteNonQuerySync(String strQuery, Dictionary<string, object> dicObjParams)
        {
            long iRecordEffected = 0;
            if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
            {
                mobjSqlCommand = new SqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjSqlCommand.Parameters.AddWithValue(pair.Key, IsDBNull(pair.Value));
                }
                iRecordEffected = this.ExecuteNonQuerySync(mobjSqlCommand);
            }
            else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
            {
                mobjMySqlCommand = new MySqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjMySqlCommand.Parameters.AddWithValue(pair.Key, IsDBNull(pair.Value));
                }
                iRecordEffected = this.ExecuteNonQuery(mobjMySqlCommand);
            }

            try
            {
                long lrt = 0;
                //LOG Entry
                lrt =  ExecuteLogQuerySync(strQuery, dicObjParams);


            }

            catch (Exception ex)
            {
                //
            }
            return iRecordEffected;

        }
        ///// SUMMARY : EXECUTE INSERT QUERY AND RETURN PRIMARY KEY
        ///// PARAMETER : QUERY STRING
        ///// RETURN : PRIMARY KEY
        //public long ExecuteNonQueryReturnID(string rQry)
        //{
        //    long iRecordEffected = 0;
        //    msQuery = rQry;
        //    if (msQuery != "" || msQuery != string.Empty || msQuery != null)
        //    {
        //        try
        //        {
        //            OpenDataBase();
        //            if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
        //            {
        //                mobjSqlCommand.CommandType = CommandType.Text;
        //                mobjSqlCommand.CommandText = msQuery;
        //                iRecordEffected = Convert.ToInt64(mobjSqlCommand.ExecuteScalar());
        //            }
        //            else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
        //            {
        //                mobjMySqlCommand.CommandType = CommandType.Text;
        //                mobjMySqlCommand.CommandText = msQuery;
        //                iRecordEffected = Convert.ToInt64(mobjMySqlCommand.ExecuteScalar());
        //            }
        //            return iRecordEffected;
        //        }
        //        catch (System.Exception ex)
        //        {
        //            Console.Write(ex.Message);
        //            throw;
        //            return 0;
        //        }
        //        finally
        //        {
        //            CloseDataBase();
        //        }
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        ///// SUMMARY : EXECUTE TRANSACTIONAL QUERY AND RETURN PRIMARY KEY
        ///// PARAMETER : COMMAND
        ///// RETURN : PRIMARY KEY
        //public long ExecuteNonQueryReturnID(SqlCommand cmd)
        //{
        //    long iRecordEffected = 0;
        //    msQuery = cmd.CommandText;
        //    if (msQuery != "" || msQuery != string.Empty || msQuery != null)
        //    {
        //        try
        //        {
        //            OpenDataBase();
        //            //mobjSqlCommand.CommandType = cmd.CommandType;
        //            //mobjSqlCommand.CommandText = cmd.CommandText;
        //            cmd.Connection = mobjSqlConection;
        //            iRecordEffected = Convert.ToInt64(cmd.ExecuteScalar());
        //            return iRecordEffected;
        //        }
        //        catch (System.Exception ex)
        //        {
        //            Console.Write(ex.Message);
        //            throw;
        //            return 0;
        //        }
        //        finally
        //        {
        //            CloseDataBase();
        //        }
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        ///// SUMMARY : EXECUTE TRANSACTIONAL QUERY AND RETURN PRIMARY KEY
        ///// PARAMETER : MYSQL COMMAND
        ///// RETURN : PRIMARY KEY
        //public long ExecuteNonQueryReturnID(MySqlCommand cmd)
        //{
        //    long iRecordEffected = 0;
        //    msQuery = cmd.CommandText;
        //    if (msQuery != "" || msQuery != string.Empty || msQuery != null)
        //    {
        //        try
        //        {
        //            OpenDataBase();
        //            cmd.Connection = mobjMySqlConection;
        //            iRecordEffected = Convert.ToInt64(cmd.ExecuteScalar());
        //            return iRecordEffected;
        //        }
        //        catch (System.Exception ex)
        //        {
        //            Console.Write(ex.Message);
        //            throw;
        //            return 0;
        //        }
        //        finally
        //        {
        //            CloseDataBase();
        //        }
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        /// SUMMARY : EXECUTE TRANSACTIONAL QUERY AND RETURN PRIMARY KEY
        /// PARAMETER : QUERY STRING, PARAMS
        /// RETURN : PRIMARY KEY
        public async Task<long> ExecuteNonQueryReturnID(String strQuery, Dictionary<string, object> dicObjParams, String strTable, String strColumn)
        {
            long iRecordEffected = 0;
            string strRecRndKey = "";
            if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
            {
                mobjSqlCommand = new SqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjSqlCommand.Parameters.AddWithValue(pair.Key, IsDBNull(pair.Value));
                    if (pair.Key.ToLower().Equals("@rec_rnd_key"))
                        strRecRndKey = pair.Value.ToString();
                }
                //iRecordEffected = this.ExecuteNonQueryReturnID(mobjSqlCommand);
                iRecordEffected = await this.ExecuteNonQuery(mobjSqlCommand);
            }
            else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
            {
                mobjMySqlCommand = new MySqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjMySqlCommand.Parameters.AddWithValue(pair.Key, IsDBNull(pair.Value));
                    if (pair.Key.ToLower().Equals("@rec_rnd_key"))
                        strRecRndKey = pair.Value.ToString();
                }
                //iRecordEffected = this.ExecuteNonQueryReturnID(mobjMySqlCommand);
                iRecordEffected = this.ExecuteNonQuery(mobjMySqlCommand);
            }
            if (iRecordEffected > 0)
            {
                string strQry = "SELECT MAX(" + strColumn + ") as PKID FROM " + strTable + " WHERE rec_rnd_key='" + strRecRndKey + "'";
                DataTable dtTmp = this.ExecuteDataTable(strQry);
                if (dtTmp.Rows.Count > 0 && dtTmp.Rows[0][0] != System.DBNull.Value)
                    iRecordEffected = Convert.ToInt64(dtTmp.Rows[0][0].ToString());
            }


            try
            {
                long lrt = 0;
                //LOG Entry
                lrt = await ExecuteLogQuery(strQuery, dicObjParams);


            }

            catch (Exception ex)
            {
                //
            }
            return iRecordEffected;
        }

        public async Task<long> ExecuteLogQuery(String strQuery, Dictionary<string, object> dicObjParams)
        {
            long lrt = 0;
            string LogQry = CreateLogQuery(strQuery, dicObjParams);
            string strLogQry = "insert into Log (Query,rec_status,rec_modified_by,rec_action,rec_rnd_key) "
               + " values(@Query,@rec_status,@rec_modified_by,@rec_action,@rec_rnd_key)";

            Dictionary<string, object> dicObjParams1 = new Dictionary<string, object>();

            dicObjParams1.Add("@Query", LogQry);
            dicObjParams1.Add("@rec_status", dicObjParams["@rec_status"]);
            dicObjParams1.Add("@rec_modified_by", dicObjParams["@rec_modified_by"]);
            dicObjParams1.Add("@rec_action", dicObjParams["@rec_action"]);
            dicObjParams1.Add("@rec_rnd_key", 0);

            lrt = await ExecuteNonLogQuery(strLogQry, dicObjParams1);

            return lrt;
        }
        public long ExecuteLogQuerySync(String strQuery, Dictionary<string, object> dicObjParams)
        {
            long lrt = 0;
            string LogQry = CreateLogQuery(strQuery, dicObjParams);
            string strLogQry = "insert into Log (Query,rec_status,rec_modified_by,rec_action,rec_rnd_key) "
               + " values(@Query,@rec_status,@rec_modified_by,@rec_action,@rec_rnd_key)";

            Dictionary<string, object> dicObjParams1 = new Dictionary<string, object>();

            dicObjParams1.Add("@Query", LogQry);
            dicObjParams1.Add("@rec_status", dicObjParams["@rec_status"]);
            dicObjParams1.Add("@rec_modified_by", dicObjParams["@rec_modified_by"]);
            dicObjParams1.Add("@rec_action", dicObjParams["@rec_action"]);
            dicObjParams1.Add("@rec_rnd_key", 0);

            lrt =  ExecuteNonLogQuerySync(strLogQry, dicObjParams1);

            return lrt;
        }
        public string CreateLogQuery(string Query, Dictionary<string, object> dicObjParams)
        {
            string LogQry = Query;
            foreach (var pair1 in dicObjParams)
            {
                Type type = dicObjParams[pair1.Key].GetType();

                if (type == typeof(string) || type == typeof(DateTime) || type == typeof(char))
                {
                    LogQry = LogQry.Replace(pair1.Key, "'" + pair1.Value.ToString() + "'");

                }
                else
                {
                    LogQry = LogQry.Replace(pair1.Key, pair1.Value.ToString());
                }

            }
            return LogQry;
        }
        public async Task<long> ExecuteNonLogQuery(String strQuery, Dictionary<string, object> dicObjParams)
        {
            long iRecordEffected = 0;
            if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
            {
                mobjSqlCommand = new SqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjSqlCommand.Parameters.AddWithValue(pair.Key, IsDBNull(pair.Value));
                }
                iRecordEffected = await this.ExecuteNonQuery(mobjSqlCommand);
            }
            else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
            {
                mobjMySqlCommand = new MySqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjMySqlCommand.Parameters.AddWithValue(pair.Key, IsDBNull(pair.Value));
                }
                iRecordEffected = this.ExecuteNonQuery(mobjMySqlCommand);
            }
            return iRecordEffected;
        }
        public long ExecuteNonLogQuerySync(String strQuery, Dictionary<string, object> dicObjParams)
        {
            long iRecordEffected = 0;
            if (msConnectionType.Equals(enumConnectionType.MSSQL.ToString()))
            {
                mobjSqlCommand = new SqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjSqlCommand.Parameters.AddWithValue(pair.Key, IsDBNull(pair.Value));
                }
                iRecordEffected = this.ExecuteNonQuerySync(mobjSqlCommand);
            }
            else if (msConnectionType.Equals(enumConnectionType.MYSQL.ToString()))
            {
                mobjMySqlCommand = new MySqlCommand(strQuery);
                foreach (KeyValuePair<string, object> pair in dicObjParams)
                {
                    mobjMySqlCommand.Parameters.AddWithValue(pair.Key, IsDBNull(pair.Value));
                }
                iRecordEffected = this.ExecuteNonQuery(mobjMySqlCommand);
            }
            return iRecordEffected;
        }
        public static object IsDBNull(Object objVar)
        {
            if (objVar.GetType() == typeof(String))
            {
                if (objVar.ToString().Equals(String.Empty) || objVar.ToString().Equals(null))
                    return DBNull.Value;
                else
                    return objVar;
            }
            else if (objVar.GetType() == typeof(Int32))
            {
                if (Convert.ToInt32(objVar) == Int32.MinValue)
                    return DBNull.Value;
                else
                    return objVar;
            }
            else if (objVar.GetType() == typeof(Int64))
            {
                if (Convert.ToInt64(objVar) == Int64.MinValue)
                    return DBNull.Value;
                else
                    return objVar;
            }
            else if (objVar.GetType() == typeof(long))
            {
                if (Convert.ToInt64(objVar) == long.MinValue)
                    return DBNull.Value;
                else
                    return objVar;
            }
            else if (objVar.GetType() == typeof(decimal))
            {
                if (Convert.ToDecimal(objVar) == decimal.MinValue)
                    return DBNull.Value;
                else
                    return objVar;
            }
            else if (objVar.GetType() == typeof(DateTime))
            {
                if (Convert.ToDateTime(objVar) == DateTime.MinValue)
                    return DBNull.Value;
                else
                    return objVar;
            }
            else if (objVar.GetType() == typeof(Boolean))
            {
                return objVar;
            }
            return DBNull.Value;
        }

        #endregion
    }
}
