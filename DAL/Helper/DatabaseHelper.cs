using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL.Helper
{
    /// <summary>
    /// Helper kết nối và thực thi Stored Procedures trên SQL Server
    /// </summary>
    public class DatabaseHelper : IDatabaseHelper
    {
        public string StrConnection { get; set; }
        public SqlConnection sqlConnection { get; set; }
        public SqlTransaction sqlTransaction { get; set; }

        public DatabaseHelper(IConfiguration configuration)
        {
            StrConnection = configuration["Data Source=KHIEMSPEED\\SQLEXPRESS01;Initial Catalog=KhoBaiContainer;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"]!;
        }

        public string OpenConnection()
        {
            try
            {
                if (sqlConnection == null)
                    sqlConnection = new SqlConnection(StrConnection);
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();
                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string OpenConnectionAndBeginTransaction()
        {
            try
            {
                if (sqlConnection == null)
                    sqlConnection = new SqlConnection(StrConnection);
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                return "";
            }
            catch (Exception ex)
            {
                sqlConnection?.Dispose();
                sqlTransaction?.Dispose();
                return ex.Message;
            }
        }

        public string CommitTransaction()
        {
            try
            {
                sqlTransaction?.Commit();
                sqlConnection?.Close();
                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string RollbackTransaction()
        {
            try
            {
                sqlTransaction?.Rollback();
                sqlConnection?.Close();
                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }

        /// <summary>Thực thi SP trả về DataTable</summary>
        public DataTable ExecuteSProcedureReturnDataTable(out string msgError, string sprocedureName, params object[] paramObjects)
        {
            DataTable tb = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = sprocedureName };
                var connection = new SqlConnection(StrConnection);
                cmd.Connection = connection;
                BindParameters(cmd, paramObjects);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(tb);
                cmd.Dispose(); ad.Dispose(); connection.Dispose();
                msgError = "";
            }
            catch (Exception ex) { tb = null!; msgError = ex.ToString(); }
            return tb;
        }

        /// <summary>Thực thi SP trả về DataSet (nhiều bảng)</summary>
        public DataSet ExecuteSProcedureReturnDataset(out string msgError, string sprocedureName, params object[] paramObjects)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = sprocedureName };
                var connection = new SqlConnection(StrConnection);
                cmd.Connection = connection;
                BindParameters(cmd, paramObjects);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
                cmd.Dispose(); ad.Dispose(); connection.Dispose();
                msgError = "";
            }
            catch (Exception ex) { ds = null!; msgError = ex.ToString(); }
            return ds;
        }

        /// <summary>Thực thi SP trong Transaction, trả về scalar (chuỗi lỗi nếu có)</summary>
        public object ExecuteScalarSProcedureWithTransaction(out string msgError, string sprocedureName, params object[] paramObjects)
        {
            object result = null!;
            try
            {
                string errOpen = OpenConnectionAndBeginTransaction();
                if (!string.IsNullOrEmpty(errOpen)) { msgError = errOpen; return null!; }

                SqlCommand cmd = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = sprocedureName };
                cmd.Connection = sqlConnection;
                cmd.Transaction = sqlTransaction;
                BindParameters(cmd, paramObjects);
                result = cmd.ExecuteScalar()!;
                CommitTransaction();
                msgError = "";
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                msgError = ex.ToString();
            }
            return result;
        }

        /// <summary>Thực thi SP với connection có sẵn (không transaction)</summary>
        public string ExecuteSProcedure(SqlConnection conn, string sprocedureName, params object[] paramObjects)
        {
            try
            {
                SqlCommand cmd = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = sprocedureName };
                if (conn.State != ConnectionState.Open) return "CONNECTION_NOT_OPENING";
                cmd.Connection = conn;
                BindParameters(cmd, paramObjects);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return "";
            }
            catch (Exception ex) { return ex.ToString(); }
        }

        private void BindParameters(SqlCommand cmd, object[] paramObjects)
        {
            int count = paramObjects.Length / 2;
            int j = 0;
            for (int i = 0; i < count; i++)
            {
                string name = Convert.ToString(paramObjects[j++])!;
                object val = paramObjects[j++];
                if (name.ToLower().Contains("json"))
                    cmd.Parameters.Add(new SqlParameter { ParameterName = name, Value = val ?? DBNull.Value, SqlDbType = SqlDbType.NVarChar });
                else
                    cmd.Parameters.Add(new SqlParameter(name, val ?? DBNull.Value));
            }
        }
    }
}
