using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using CarConnectEntityLibrary;
using CarConnectUtilLibrary;
using CarConnectExceptionLibrary;

namespace CarConnectDaoLibrary
{
    public class ReservationService : IReservationService
    {
        private readonly string connectionString;

        public ReservationService()
        {
            connectionString = DBPropertyUtil.GetConnectionString();
        }

        public void CreateReservation(Reservation reservation)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                // Optional: Check for reservation conflicts here

                string query = @"INSERT INTO Reservation (CustomerID, VehicleID, StartDate, EndDate, TotalCost, Status)
                                 VALUES (@CustomerID, @VehicleID, @StartDate, @EndDate, @TotalCost, @Status)";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", reservation.CustomerID);
                cmd.Parameters.AddWithValue("@VehicleID", reservation.VehicleID);
                cmd.Parameters.AddWithValue("@StartDate", reservation.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", reservation.EndDate);
                cmd.Parameters.AddWithValue("@TotalCost", reservation.TotalCost);
                cmd.Parameters.AddWithValue("@Status", reservation.Status);

                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new ReservationException("Error creating reservation", ex);
            }
        }

        public void UpdateReservation(Reservation reservation)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = @"UPDATE Reservation 
                                 SET StartDate=@StartDate, EndDate=@EndDate, TotalCost=@TotalCost, Status=@Status 
                                 WHERE ReservationID=@ReservationID";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StartDate", reservation.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", reservation.EndDate);
                cmd.Parameters.AddWithValue("@TotalCost", reservation.TotalCost);
                cmd.Parameters.AddWithValue("@Status", reservation.Status);
                cmd.Parameters.AddWithValue("@ReservationID", reservation.ReservationID);

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    throw new ReservationException("Reservation not found to update.");
            }
            catch (SqlException ex)
            {
                throw new ReservationException("Error updating reservation", ex);
            }
        }

        public void CancelReservation(int reservationId)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = "DELETE FROM Reservation WHERE ReservationID = @ReservationID";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ReservationID", reservationId);

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    throw new ReservationException("Reservation not found to cancel.");
            }
            catch (SqlException ex)
            {
                throw new ReservationException("Error canceling reservation", ex);
            }
        }

        public Reservation GetReservationById(int reservationId)
        {
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = "SELECT * FROM Reservation WHERE ReservationID = @ReservationID";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ReservationID", reservationId);
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Reservation
                    {
                        ReservationID = (int)reader["ReservationID"],
                        CustomerID = (int)reader["CustomerID"],
                        VehicleID = (int)reader["VehicleID"],
                        StartDate = Convert.ToDateTime(reader["StartDate"]),
                        EndDate = Convert.ToDateTime(reader["EndDate"]),
                        TotalCost = Convert.ToDecimal(reader["TotalCost"]),
                        Status = reader["Status"].ToString()
                    };
                }

                throw new ReservationException("Reservation not found.");
            }
            catch (SqlException ex)
            {
                throw new ReservationException("Error fetching reservation", ex);
            }
        }

        public List<Reservation> GetReservationsByCustomerId(int customerId)
        {
            List<Reservation> reservations = new List<Reservation>();
            try
            {
                using SqlConnection conn = DBConnUtil.GetConnection(connectionString);
                conn.Open();

                string query = "SELECT * FROM Reservation WHERE CustomerID = @CustomerID";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    reservations.Add(new Reservation
                    {
                        ReservationID = (int)reader["ReservationID"],
                        CustomerID = (int)reader["CustomerID"],
                        VehicleID = (int)reader["VehicleID"],
                        StartDate = Convert.ToDateTime(reader["StartDate"]),
                        EndDate = Convert.ToDateTime(reader["EndDate"]),
                        TotalCost = Convert.ToDecimal(reader["TotalCost"]),
                        Status = reader["Status"].ToString()
                    });
                }

                return reservations;
            }
            catch (SqlException ex)
            {
                throw new ReservationException("Error fetching reservations by customer", ex);
            }
        }
    }
}
