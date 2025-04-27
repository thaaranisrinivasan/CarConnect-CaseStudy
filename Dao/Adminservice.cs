using System;
using Microsoft.Data.SqlClient;
using CarConnectEntityLibrary;
using CarConnectUtilLibrary;
using CarConnectExceptionLibrary;

namespace CarConnectDaoLibrary
{
    public class AdminService : IAdminService
    {
        private readonly string connectionString;

        public AdminService()
        {
            connectionString = DBPropertyUtil.GetConnectionString();
        }

        public void RegisterAdmin(Admin admin)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = @"INSERT INTO Admin (FirstName, LastName, Email, PhoneNumber, Username, Password, Role, JoinDate)
                                 VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Username, @Password, @Role, @JoinDate)";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", admin.FirstName);
                cmd.Parameters.AddWithValue("@LastName", admin.LastName);
                cmd.Parameters.AddWithValue("@Email", admin.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", admin.PhoneNumber);
                cmd.Parameters.AddWithValue("@Username", admin.Username);
                cmd.Parameters.AddWithValue("@Password", admin.Password);
                cmd.Parameters.AddWithValue("@Role", admin.Role);
                cmd.Parameters.AddWithValue("@JoinDate", admin.JoinDate);

                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Error registering admin", ex);
            }
        }

        public Admin GetAdminById(int adminId)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = "SELECT * FROM Admin WHERE AdminID = @AdminID";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AdminID", adminId);
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Admin
                    {
                        AdminID = (int)reader["AdminID"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = reader["Role"].ToString(),
                        JoinDate = Convert.ToDateTime(reader["JoinDate"])
                    };
                }

                throw new AdminNotFoundException("Admin not found.");
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Error retrieving admin", ex);
            }
        }

        public Admin GetAdminByUsername(string username)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = "SELECT * FROM Admin WHERE Username = @Username";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Admin
                    {
                        AdminID = (int)reader["AdminID"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = reader["Role"].ToString(),
                        JoinDate = Convert.ToDateTime(reader["JoinDate"])
                    };
                }

                throw new AdminNotFoundException("Admin not found.");
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Error retrieving admin", ex);
            }
        }

        public void UpdateAdmin(Admin admin)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = @"UPDATE Admin SET FirstName=@FirstName, LastName=@LastName, Email=@Email, 
                                 PhoneNumber=@PhoneNumber, Username=@Username, Password=@Password, Role=@Role 
                                 WHERE AdminID=@AdminID";

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", admin.FirstName);
                cmd.Parameters.AddWithValue("@LastName", admin.LastName);
                cmd.Parameters.AddWithValue("@Email", admin.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", admin.PhoneNumber);
                cmd.Parameters.AddWithValue("@Username", admin.Username);
                cmd.Parameters.AddWithValue("@Password", admin.Password);
                cmd.Parameters.AddWithValue("@Role", admin.Role);
                cmd.Parameters.AddWithValue("@AdminID", admin.AdminID);

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    throw new AdminNotFoundException("Admin not found for update.");
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Error updating admin", ex);
            }
        }

        public void DeleteAdmin(int adminId)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = "DELETE FROM Admin WHERE AdminID = @AdminID";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AdminID", adminId);

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    throw new AdminNotFoundException("Admin not found to delete.");
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Error deleting admin", ex);
            }
        }
    }
}
