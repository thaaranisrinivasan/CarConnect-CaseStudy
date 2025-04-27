using Microsoft.Data.SqlClient;
using CarConnectExceptionLibrary;

namespace CarConnectUtilLibrary
{
    public static class DBConnUtil
    {
        public static SqlConnection GetConnection(string connectionString)
        {
            try
            {
                return new SqlConnection(connectionString);
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Failed to create SQL connection.", ex);
            }
        }
    }
}
