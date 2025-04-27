using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using CarConnectEntityLibrary;
using CarConnectUtilLibrary;
using CarConnectExceptionLibrary;

namespace CarConnectDaoLibrary
{
    public class VehicleService : IVehicleService
    {
        private readonly string connectionString;

        public VehicleService()
        {
            connectionString = DBPropertyUtil.GetConnectionString();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = @"INSERT INTO Vehicle (Model, Make, Year, Color, RegistrationNumber, Availability, DailyRate) 
                                 VALUES (@Model, @Make, @Year, @Color, @RegistrationNumber, @Availability, @DailyRate)";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Model", vehicle.Model);
                cmd.Parameters.AddWithValue("@Make", vehicle.Make);
                cmd.Parameters.AddWithValue("@Year", vehicle.Year);
                cmd.Parameters.AddWithValue("@Color", vehicle.Color);
                cmd.Parameters.AddWithValue("@RegistrationNumber", vehicle.RegistrationNumber);
                cmd.Parameters.AddWithValue("@Availability", vehicle.Availability);
                cmd.Parameters.AddWithValue("@DailyRate", vehicle.DailyRate);

                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Error adding vehicle", ex);
            }
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = @"UPDATE Vehicle SET Model=@Model, Make=@Make, Year=@Year, Color=@Color,
                                 RegistrationNumber=@RegistrationNumber, Availability=@Availability, DailyRate=@DailyRate 
                                 WHERE VehicleID=@VehicleID";

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Model", vehicle.Model);
                cmd.Parameters.AddWithValue("@Make", vehicle.Make);
                cmd.Parameters.AddWithValue("@Year", vehicle.Year);
                cmd.Parameters.AddWithValue("@Color", vehicle.Color);
                cmd.Parameters.AddWithValue("@RegistrationNumber", vehicle.RegistrationNumber);
                cmd.Parameters.AddWithValue("@Availability", vehicle.Availability);
                cmd.Parameters.AddWithValue("@DailyRate", vehicle.DailyRate);
                cmd.Parameters.AddWithValue("@VehicleID", vehicle.VehicleID);

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    throw new VehicleNotFoundException("Vehicle not found for update.");
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Error updating vehicle", ex);
            }
        }

        public void RemoveVehicle(int vehicleId)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = "DELETE FROM Vehicle WHERE VehicleID = @VehicleID";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@VehicleID", vehicleId);

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    throw new VehicleNotFoundException("Vehicle not found for deletion.");
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Error deleting vehicle", ex);
            }
        }

        public Vehicle GetVehicleById(int vehicleId)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = "SELECT * FROM Vehicle WHERE VehicleID = @VehicleID";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@VehicleID", vehicleId);
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Vehicle
                    {
                        VehicleID = (int)reader["VehicleID"],
                        Model = reader["Model"].ToString(),
                        Make = reader["Make"].ToString(),
                        Year = Convert.ToInt32(reader["Year"]),
                        Color = reader["Color"].ToString(),
                        RegistrationNumber = reader["RegistrationNumber"].ToString(),
                        Availability = Convert.ToBoolean(reader["Availability"]),
                        DailyRate = Convert.ToDecimal(reader["DailyRate"])
                    };
                }

                throw new VehicleNotFoundException("Vehicle not found.");
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Error retrieving vehicle", ex);
            }
        }

        public List<Vehicle> GetAvailableVehicles()
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = "SELECT * FROM Vehicle WHERE Availability = 1";
                using SqlCommand cmd = new SqlCommand(query, conn);
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    vehicles.Add(new Vehicle
                    {
                        VehicleID = (int)reader["VehicleID"],
                        Model = reader["Model"].ToString(),
                        Make = reader["Make"].ToString(),
                        Year = Convert.ToInt32(reader["Year"]),
                        Color = reader["Color"].ToString(),
                        RegistrationNumber = reader["RegistrationNumber"].ToString(),
                        Availability = Convert.ToBoolean(reader["Availability"]),
                        DailyRate = Convert.ToDecimal(reader["DailyRate"])
                    });
                }

                return vehicles;
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Error retrieving available vehicles", ex);
            }
        }
    }
}
