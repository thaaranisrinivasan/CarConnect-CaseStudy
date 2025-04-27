using System;
using System.Collections.Generic;
using CarConnectEntityLibrary;

namespace CarConnectDaoLibrary
{
    public static class ReportGenerator
    {
        public static void GenerateReservationReport(List<Reservation> reservations)
        {
            Console.WriteLine("\n===== Reservation Report =====");
            foreach (var r in reservations)
            {
                Console.WriteLine($"ID: {r.ReservationID} | Customer: {r.CustomerID} | Vehicle: {r.VehicleID}");
                Console.WriteLine($"Dates: {r.StartDate:dd-MM-yyyy} to {r.EndDate:dd-MM-yyyy} | Cost: ₹{r.TotalCost} | Status: {r.Status}\n");
            }
        }

        public static void GenerateVehicleUtilizationReport(List<Vehicle> vehicles)
        {
            Console.WriteLine("\n===== Vehicle Utilization Report =====");
            foreach (var v in vehicles)
            {
                string status = v.Availability ? "Available" : "Reserved";
                Console.WriteLine($"ID: {v.VehicleID} | {v.Make} {v.Model} ({v.Year}) | Rate: ₹{v.DailyRate} | {status}");
            }
        }
    }
}
