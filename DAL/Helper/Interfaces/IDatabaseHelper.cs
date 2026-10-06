using System.Data;

namespace DAL.Helper
{
    public interface IDatabaseHelper
    {
        DataTable ExecuteSProcedureReturnDataTable(out string msgError, string sprocedureName, params object[] paramObjects);
        DataSet ExecuteSProcedureReturnDataset(out string msgError, string sprocedureName, params object[] paramObjects);
        object ExecuteScalarSProcedureWithTransaction(out string msgError, string sprocedureName, params object[] paramObjects);
        string ExecuteSProcedure(Microsoft.Data.SqlClient.SqlConnection conn, string sprocedureName, params object[] paramObjects);
    }
}
