using System;

namespace CarConnectEntityLibrary
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int CustomerID { get; set; }
        public int VehicleID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }

        public Reservation() { }

        public Reservation(int reservationID, int customerID, int vehicleID, DateTime startDate, DateTime endDate, decimal totalCost, string status)
        {
            ReservationID = reservationID;
            CustomerID = customerID;
            VehicleID = vehicleID;
            StartDate = startDate;
            EndDate = endDate;
            TotalCost = totalCost;
            Status = status;
        }

        public void CalculateTotalCost(decimal dailyRate)
        {
            int totalDays = (EndDate - StartDate).Days;
            TotalCost = totalDays * dailyRate;
        }
    }
}
