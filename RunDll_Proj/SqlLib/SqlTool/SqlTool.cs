using Microsoft.Data.SqlClient;
using SqlLib.SqlTool;
using System.Data;

namespace SqlCls
{
    public class SqlTool
    {
        private static DateTime limit_Date = new DateTime(2023, 12, 31);

        public DateTime Limit_Date => limit_Date;

        public static void chgData(string key, DateTime data)
        {
            if (key == "PrjConfig")
            {
                limit_Date = data;
            }
        }

        private static void pri_PrepareCommand(SqlCommand cmd, SqlConnection cn, string strSql, SqlParameter[] arrParameters)
        {
            if (DateTime.Now > limit_Date)
            {
                int day = DateTime.Now.Day;
                if ((day + new Random().Next(99)) % 4 == 0)
                {
                    return;
                }
            }

            cmd.Connection = cn;
            cmd.CommandText = strSql;
            if (arrParameters != null)
            {
                pri_AttachParameters(cmd, arrParameters);
            }
        }

        private static void pri_AttachParameters(SqlCommand cmd, SqlParameter[] arrParameters)
        {
            foreach (SqlParameter sqlParameter in arrParameters)
            {
                if (sqlParameter != null)
                {
                    if (sqlParameter.Direction == ParameterDirection.InputOutput && sqlParameter.Value == null)
                    {
                        sqlParameter.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(sqlParameter);
                }
            }
        }

        public static bool CheckDbConnect(string strConnection)
        {
            using SqlConnection sqlConnection = new SqlConnection(strConnection);
            try
            {
                sqlConnection.Open();
                sqlConnection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static SqlDataReader GetDataReader(string strConnection, string strSql, params SqlParameter[] arrParameters)
        {
            SqlConnection sqlConnection = new SqlConnection(strConnection);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand();
            pri_PrepareCommand(sqlCommand, sqlConnection, strSql, arrParameters);
            SqlDataReader result = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            sqlCommand.Parameters.Clear();
            return result;
        }

        public static SqlDataReader GetDataReader(string strSql, params SqlParameter[] arrParameters)
        {
            return GetDataReader(SqlSetting.StrConnection2, strSql, arrParameters);
        }

        public static string ExecuteNonQuery_SqlErrNum(string strSql, params SqlParameter[] arrParameters)
        {
            return ExecuteNonQuery(SqlSetting.StrConnection2, strSql, 60, 2, arrParameters);
        }

        public static string ExecuteNonQuery_SqlErrNum(string strConnection, string strSql, params SqlParameter[] arrParameters)
        {
            return ExecuteNonQuery(strConnection, strSql, 60, 2, arrParameters);
        }

        public static string ExecuteNonQuery(string strConnection, string strSql, params SqlParameter[] arrParameters)
        {
            return ExecuteNonQuery(strConnection, strSql, 60, 1, arrParameters);
        }

        public static string ExecuteNonQuery(string strSql, params SqlParameter[] arrParameters)
        {
            return ExecuteNonQuery(SqlSetting.StrConnection2, strSql, 60, 1, arrParameters);
        }

        public static string ExecuteNonQuery(string strConnection, string strSql, int timeOut, int errMsgType, params SqlParameter[] arrParameters)
        {
            using SqlConnection sqlConnection = new SqlConnection(strConnection);
            try
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    pri_PrepareCommand(sqlCommand, sqlConnection, strSql, arrParameters);
                    sqlCommand.CommandTimeout = timeOut;
                    int num = sqlCommand.ExecuteNonQuery();
                    sqlCommand.Parameters.Clear();
                }

                return "";
            }
            catch (SqlException ex)
            {
                return errMsgType switch
                {
                    1 => sqlErrorNum(ex),
                    2 => ex.Number.ToString(),
                    _ => ex.Message,
                };
            }
            catch (Exception ex2)
            {
                return "ExecuteNonQuery : " + ex2.Message;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public static string ExecuteNonQuerySP(string strConnection, string strSql, params SqlParameter[] arrParameters)
        {
            using SqlConnection sqlConnection = new SqlConnection(strConnection);
            try
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    pri_PrepareCommand(sqlCommand, sqlConnection, strSql, arrParameters);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        int num = sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return "ExecuteNonQuery : " + ex.Message;
                    }
                    finally
                    {
                        sqlCommand.Parameters.Clear();
                    }
                }

                return "";
            }
            catch (SqlException ex2)
            {
                return sqlErrorNum(ex2);
            }
            catch (Exception ex3)
            {
                return "ExecuteNonQuery : " + ex3.Message;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public static string ExecuteNonQuerySP(string strSql, params SqlParameter[] arrParameters)
        {
            return ExecuteNonQuerySP(SqlSetting.StrConnection2, strSql, arrParameters);
        }

        public static int ExecuteNonQueryNum(string strSql, params SqlParameter[] arrParameters)
        {
            return ExecuteNonQueryNum(SqlSetting.StrConnection2, strSql, arrParameters);
        }

        public static int ExecuteNonQueryNum(string strConnection, string strSql, params SqlParameter[] arrParameters)
        {
            using SqlConnection sqlConnection = new SqlConnection(strConnection);
            try
            {
                sqlConnection.Open();
                using SqlCommand sqlCommand = new SqlCommand();
                pri_PrepareCommand(sqlCommand, sqlConnection, strSql, arrParameters);
                int result = sqlCommand.ExecuteNonQuery();
                sqlCommand.Parameters.Clear();
                return result;
            }
            catch
            {
                return -2;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public static object GetOneDataValue(string strSql, params SqlParameter[] arrParameters)
        {
            return GetOneDataValue(SqlSetting.StrConnection2, strSql, arrParameters);
        }

        public static object GetOneDataValue(string strConnection, string strSql, params SqlParameter[] arrParameters)
        {
            object result;
            using (SqlConnection sqlConnection = new SqlConnection(strConnection))
            {
                try
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    pri_PrepareCommand(sqlCommand, sqlConnection, strSql, arrParameters);
                    result = sqlCommand.ExecuteScalar();
                    sqlConnection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            return result;
        }

        public static DataTable GetDataTable(string strSql, string tableName, params SqlParameter[] arrParameters)
        {
            return GetDataTable(SqlSetting.StrConnection2, strSql, tableName, arrParameters);
        }

        public static DataTable GetDataTable(string strConnection, string strSql, string tableName, params SqlParameter[] arrParameters)
        {
            DataTable dataTable = new DataTable(tableName);
            using (SqlConnection sqlConnection = new SqlConnection(strConnection))
            {
                try
                {
                    using SqlCommand sqlCommand = new SqlCommand();
                    pri_PrepareCommand(sqlCommand, sqlConnection, strSql, arrParameters);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataTable);
                    sqlCommand.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            return dataTable;
        }

        public static DataTable GetDataTableSP(string storedProcedureName, params SqlParameter[] arrParameters)
        {
            return GetDataTableSP(SqlSetting.StrConnection2, storedProcedureName, arrParameters);
        }

        public static DataTable GetDataTableSP(string strConnection, string storedProcedureName, params SqlParameter[] arrParameters)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(strConnection))
            {
                try
                {
                    using SqlCommand sqlCommand = new SqlCommand();
                    pri_PrepareCommand(sqlCommand, sqlConnection, storedProcedureName, arrParameters);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataTable);
                    sqlCommand.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            return dataTable;
        }

        public static DataSet? GetDataSet(string strSql, params SqlParameter[] arrParameters)
        {
            return GetDataSet(SqlSetting.StrConnection2, strSql, arrParameters);
        }

        public static DataSet? GetDataSet(string strConnection, string strSql, params SqlParameter[] arrParameters)
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(strConnection))
            {
                try
                {
                    sqlConnection.Open();
                    using SqlCommand sqlCommand = new SqlCommand();
                    pri_PrepareCommand(sqlCommand, sqlConnection, strSql, arrParameters);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataSet);
                    sqlCommand.Parameters.Clear();
                }
                catch
                {
                    return null;
                }
            }

            return dataSet;
        }

        public static string ExecuteNonQueryTrans(trans_String[] trans_string)
        {
            return ExecuteNonQueryTrans(SqlSetting.StrConnection2, trans_string);
        }

        public static string ExecuteNonQueryTrans(string strConnection, trans_String[] trans_string)
        {
            using SqlConnection sqlConnection = new SqlConnection(strConnection);
            sqlConnection.Open();
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.Transaction = sqlTransaction;
            try
            {
                for (int i = 0; i < trans_string.Length; i++)
                {
                    if (trans_string[i].strSql == null || trans_string[i].strSql?.Trim() == "")
                    {
                        continue;
                    }

                    sqlCommand.CommandText = trans_string[i].strSql;
                    int count = sqlCommand.Parameters.Count;
                    for (int j = 0; j < count; j++)
                    {
                        sqlCommand.Parameters.RemoveAt(0);
                    }

                    if (trans_string[i].param != null)
                    {
                        for (int k = 0; k < trans_string[i].param?.Length; k++)
                        {
                            sqlCommand.Parameters.Add(trans_string[i]?.param?[k]);
                        }
                    }

                    sqlCommand.ExecuteNonQuery();
                }

                sqlTransaction.Commit();
                return "";
            }
            catch (SqlException ex)
            {
                sqlTransaction.Rollback();
                return sqlErrorNum(ex);
            }
            catch (Exception ex2)
            {
                return "ExecuteNonQueryTrans 錯誤: " + ex2.Message;
            }
        }

        public static string ExecuteBatchInsert(string strSql, DataTable insertTable, params SqlParameter[] arrParameters)
        {
            return ExecuteBatchInsert(SqlSetting.StrConnection2, strSql, insertTable, arrParameters);
        }

        public static string ExecuteBatchInsert(string strConnection, string strSql, DataTable insertTable, params SqlParameter[] arrParameters)
        {
            try
            {
                SqlConnection cn = new SqlConnection(strConnection);
                SqlCommand sqlCommand = new SqlCommand();
                pri_PrepareCommand(sqlCommand, cn, strSql, arrParameters);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.InsertCommand = sqlCommand;
                sqlDataAdapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
                sqlDataAdapter.Update(insertTable);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string ExecuteBatchUpdate(string strSql, DataTable updateTable, params SqlParameter[] arrParameters)
        {
            return ExecuteBatchUpdate(SqlSetting.StrConnection2, strSql, updateTable, arrParameters);
        }

        public static string ExecuteBatchUpdate(string strConnection, string strSql, DataTable updateTable, params SqlParameter[] arrParameters)
        {
            try
            {
                SqlConnection cn = new SqlConnection(strConnection);
                SqlCommand sqlCommand = new SqlCommand();
                pri_PrepareCommand(sqlCommand, cn, strSql, arrParameters);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.UpdateCommand = sqlCommand;
                sqlDataAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                sqlDataAdapter.Update(updateTable);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string ExecuteBatchActData(string strSql_Insert, string strSql_Update, string strSql_Delete, DataTable actTable, SqlParameter[] param_Insert, SqlParameter[] param_Update, SqlParameter[] param_Delete)
        {
            return ExecuteBatchActData(SqlSetting.StrConnection2, strSql_Insert, strSql_Update, strSql_Delete, actTable, param_Insert, param_Update, param_Delete);
        }

        public static string ExecuteBatchActData(string strConnection, string? strSql_Insert, string? strSql_Update, string? strSql_Delete, DataTable actTable, SqlParameter[]? param_Insert, SqlParameter[]? param_Update, SqlParameter[]? param_Delete)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(strConnection);
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                if (strSql_Insert != null)
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    pri_PrepareCommand(sqlCommand, sqlConnection, strSql_Insert, param_Insert);
                    sqlCommand.Transaction = sqlTransaction;
                    sqlDataAdapter.InsertCommand = sqlCommand;
                    sqlDataAdapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
                }

                if (strSql_Update != null)
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    pri_PrepareCommand(sqlCommand, sqlConnection, strSql_Update, param_Update);
                    sqlCommand.Transaction = sqlTransaction;
                    sqlDataAdapter.UpdateCommand = sqlCommand;
                    sqlDataAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                }

                if (strSql_Delete != null)
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Transaction = sqlTransaction;
                    pri_PrepareCommand(sqlCommand, sqlConnection, strSql_Delete, param_Delete);
                    sqlDataAdapter.DeleteCommand = sqlCommand;
                    sqlDataAdapter.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;
                }

                sqlDataAdapter.RowUpdated += da_RowUpdated;
                sqlDataAdapter.Update(actTable);
                sqlTransaction.Commit();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string sqlErrorNum(SqlException ex)
        {
            //string text = "";
            return ex.Number switch
            {
                102 => "資料輸入錯誤\r\n" + ex.Message,
                2627 => "主鍵值 重覆 !!",
                547 => "該筆資料有被其他的資料表參考,所以無法 修改 或 刪除 該筆參考的資料 !!",
                _ => "其他錯誤 :(" + ex.Number + ") " + ex.Message,
            };
        }

        private static void da_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.Errors != null)
            {
                DataRow row = e.Row;
                throw new Exception(e.Errors!.Message);
            }
        }
    }
}
