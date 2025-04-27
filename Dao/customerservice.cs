using System;
using Microsoft.Data.SqlClient;
using CarConnectEntityLibrary;
using CarConnectUtilLibrary;
using CarConnectExceptionLibrary;

namespace CarConnectDaoLibrary
{
    public class CustomerService : ICustomerService
    {
        private readonly string connectionString;

        public CustomerService()
        {
            connectionString = DBPropertyUtil.GetConnectionString();
        }

        public void RegisterCustomer(Customer customer)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = @"INSERT INTO Customer 
                    (FirstName, LastName, Email, PhoneNumber, Address, Username, Password, RegistrationDate)
                    VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Address, @Username, @Password, @RegistrationDate)";

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@Username", customer.Username);
                cmd.Parameters.AddWithValue("@Password", customer.Password);
                cmd.Parameters.AddWithValue("@RegistrationDate", customer.RegistrationDate);

                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Failed to register customer.", ex);
            }
        }

        public Customer GetCustomerById(int customerId)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = "SELECT * FROM Customer WHERE CustomerID = @CustomerID";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerId);

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Customer
                    {
                        CustomerID = (int)reader["CustomerID"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"])
                    };
                }
                throw new CustomerNotFoundException("Customer not found.");
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Failed to retrieve customer.", ex);
            }
        }

        public Customer GetCustomerByUsername(string username)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = "SELECT * FROM Customer WHERE Username = @Username";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Customer
                    {
                        CustomerID = (int)reader["CustomerID"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"])
                    };
                }
                throw new CustomerNotFoundException("Customer not found.");
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Failed to retrieve customer by username.", ex);
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = @"UPDATE Customer SET 
                    FirstName = @FirstName, LastName = @LastName, Email = @Email,
                    PhoneNumber = @PhoneNumber, Address = @Address, 
                    Username = @Username, Password = @Password
                    WHERE CustomerID = @CustomerID";

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@Username", customer.Username);
                cmd.Parameters.AddWithValue("@Password", customer.Password);
                cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    throw new CustomerNotFoundException("Customer not found for update.");
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Failed to update customer.", ex);
            }
        }

        public void DeleteCustomer(int customerId)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = "DELETE FROM Customer WHERE CustomerID = @CustomerID";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerId);

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    throw new CustomerNotFoundException("Customer not found to delete.");
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Failed to delete customer.", ex);
            }
        }
    }
}
