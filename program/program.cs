using System;
using System.Collections.Generic;
using CarConnectDaoLibrary;
using CarConnectEntityLibrary;
using CarConnectExceptionLibrary;

namespace CarConnectApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Welcome to CarConnect ===");

            while (true)
            {
                Console.WriteLine("\nLogin as:");
                Console.WriteLine("1. Admin");
                Console.WriteLine("2. Customer");
                Console.WriteLine("3. Exit");

                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AdminLogin();
                        break;
                    case "2":
                        CustomerLogin();
                        break;
                    case "3":
                        Console.WriteLine("Thank you for using CarConnect!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void AdminLogin()
        {
            try
            {
                Console.Write("\nEnter Admin Username: ");
                string username = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                Admin admin = DatabaseContext.AuthService.AuthenticateAdmin(username, password);
                Console.WriteLine($"Welcome Admin {admin.FirstName}!");

                AdminMenu();
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
            }
        }

        static void AdminMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Admin Menu ---");
                Console.WriteLine("1. Add Vehicle");
                Console.WriteLine("2. View Available Vehicles");
                Console.WriteLine("3. Update Vehicle");
                Console.WriteLine("4. Delete Vehicle");
                Console.WriteLine("5. Generate Reports");
                Console.WriteLine("6. Logout");

                Console.Write("Enter choice: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddVehicle();
                        break;
                    case "2":
                        ViewAvailableVehicles();
                        break;
                    case "3":
                        UpdateVehicle();
                        break;
                    case "4":
                        DeleteVehicle();
                        break;
                    case "5":
                        GenerateReports();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void CustomerLogin()
        {
            try
            {
                Console.Write("\nEnter Customer Username: ");
                string username = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                Customer customer = DatabaseContext.AuthService.AuthenticateCustomer(username, password);
                Console.WriteLine($"Welcome {customer.FirstName}!");

                CustomerMenu(customer.CustomerID);
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
            }
        }

        static void CustomerMenu(int customerId)
        {
            while (true)
            {
                Console.WriteLine("\n--- Customer Menu ---");
                Console.WriteLine("1. View Available Vehicles");
                Console.WriteLine("2. Create Reservation");
                Console.WriteLine("3. View My Reservations");
                Console.WriteLine("4. Logout");

                Console.Write("Enter choice: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ViewAvailableVehicles();
                        break;
                    case "2":
                        CreateReservation(customerId);
                        break;
                    case "3":
                        ViewReservations(customerId);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void AddVehicle()
        {
            try
            {
                Console.Write("Model: ");
                string model = Console.ReadLine();

                Console.Write("Make: ");
                string make = Console.ReadLine();

                Console.Write("Year: ");
                int year = int.Parse(Console.ReadLine());

                Console.Write("Color: ");
                string color = Console.ReadLine();

                Console.Write("Registration Number: ");
                string reg = Console.ReadLine();

                Console.Write("Available (true/false): ");
                bool avail = bool.Parse(Console.ReadLine());

                Console.Write("Daily Rate: ");
                decimal rate = decimal.Parse(Console.ReadLine());

                var vehicle = new Vehicle(0, model, make, year, color, reg, avail, rate);
                DatabaseContext.VehicleService.AddVehicle(vehicle);
                Console.WriteLine("Vehicle added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add vehicle: {ex.Message}");
            }
        }

        static void ViewAvailableVehicles()
        {
            try
            {
                List<Vehicle> vehicles = DatabaseContext.VehicleService.GetAvailableVehicles();
                if (vehicles.Count == 0)
                {
                    Console.WriteLine("No available vehicles.");
                    return;
                }

                Console.WriteLine($"\n{"ID",-5} {"Make",-12} {"Model",-12} {"Year",-6} {"Color",-10} {"Reg#",-15} {"Rate",-10} {"Available",-10}");
                Console.WriteLine(new string('-', 85));

                foreach (var v in vehicles)
                {
                    Console.WriteLine($"{v.VehicleID,-5} {v.Make,-12} {v.Model,-12} {v.Year,-6} {v.Color,-10} {v.RegistrationNumber,-15} {v.DailyRate,-10} {(v.Availability ? "Yes" : "No"),-10}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void UpdateVehicle()
        {
            try
            {
                Console.Write("Enter Vehicle ID to update: ");
                int id = int.Parse(Console.ReadLine());

                Vehicle vehicle = DatabaseContext.VehicleService.GetVehicleById(id);

                Console.Write("New Model: ");
                vehicle.Model = Console.ReadLine();

                Console.Write("New Make: ");
                vehicle.Make = Console.ReadLine();

                Console.Write("New Year: ");
                vehicle.Year = int.Parse(Console.ReadLine());

                Console.Write("New Color: ");
                vehicle.Color = Console.ReadLine();

                Console.Write("New Registration Number: ");
                vehicle.RegistrationNumber = Console.ReadLine();

                Console.Write("Available (true/false): ");
                vehicle.Availability = bool.Parse(Console.ReadLine());

                Console.Write("New Daily Rate: ");
                vehicle.DailyRate = decimal.Parse(Console.ReadLine());

                DatabaseContext.VehicleService.UpdateVehicle(vehicle);
                Console.WriteLine("Vehicle updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update vehicle: {ex.Message}");
            }
        }

        static void DeleteVehicle()
        {
            try
            {
                Console.Write("Enter Vehicle ID to delete: ");
                int id = int.Parse(Console.ReadLine());

                DatabaseContext.VehicleService.RemoveVehicle(id);
                Console.WriteLine("Vehicle deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete vehicle: {ex.Message}");
            }
        }

        static void CreateReservation(int customerId)
        {
            try
            {
                ViewAvailableVehicles();

                Console.Write("\nEnter Vehicle ID to reserve: ");
                int vehicleId = int.Parse(Console.ReadLine());

                Console.Write("Start Date (yyyy-mm-dd): ");
                DateTime start = DateTime.Parse(Console.ReadLine());

                Console.Write("End Date (yyyy-mm-dd): ");
                DateTime end = DateTime.Parse(Console.ReadLine());

                var vehicle = DatabaseContext.VehicleService.GetVehicleById(vehicleId);
                var reservation = new Reservation
                {
                    CustomerID = customerId,
                    VehicleID = vehicleId,
                    StartDate = start,
                    EndDate = end,
                    Status = "Confirmed"
                };
                reservation.CalculateTotalCost(vehicle.DailyRate);

                DatabaseContext.ReservationService.CreateReservation(reservation);
                Console.WriteLine("Reservation successful. Total Cost: Rs." + reservation.TotalCost);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reservation failed: {ex.Message}");
            }
        }

        static void ViewReservations(int customerId)
        {
            try
            {
                var reservations = DatabaseContext.ReservationService.GetReservationsByCustomerId(customerId);
                if (reservations.Count == 0)
                {
                    Console.WriteLine("You have no reservations.");
                    return;
                }

                Console.WriteLine($"\n{"ID",-5} {"CustomerID",-12} {"VehicleID",-12} {"Start Date",-15} {"End Date",-15} {"Cost",-10} {"Status",-10}");
                Console.WriteLine(new string('-', 85));

                foreach (var r in reservations)
                {
                    Console.WriteLine($"{r.ReservationID,-5} {r.CustomerID,-12} {r.VehicleID,-12} {r.StartDate:yyyy-MM-dd,-15} {r.EndDate:yyyy-MM-dd,-15} {r.TotalCost,-10} {r.Status,-10}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching reservations: {ex.Message}");
            }
        }

        static void GenerateReports()
        {
            try
            {
                Console.WriteLine("\n1. Reservation Report");
                Console.WriteLine("2. Vehicle Utilization Report");

                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter Customer ID (or 0 for all): ");
                        int cid = int.Parse(Console.ReadLine());

                        var reservations = cid == 0
                            ? DatabaseContext.ReservationService.GetReservationsByCustomerId(0) // Use a modified method if needed
                            : DatabaseContext.ReservationService.GetReservationsByCustomerId(cid);

                        Console.WriteLine($"\n{"ID",-5} {"CustomerID",-12} {"VehicleID",-12} {"Start Date",-15} {"End Date",-15} {"Cost",-10} {"Status",-10}");
                        Console.WriteLine(new string('-', 85));
                        foreach (var r in reservations)
                        {
                            Console.WriteLine($"{r.ReservationID,-5} {r.CustomerID,-12} {r.VehicleID,-12} {r.StartDate:yyyy-MM-dd,-15} {r.EndDate:yyyy-MM-dd,-15} {r.TotalCost,-10} {r.Status,-10}");
                        }
                        break;

                    case "2":
                        var vehicles = DatabaseContext.VehicleService.GetAvailableVehicles(); // Only available
                        Console.WriteLine($"\n{"ID",-5} {"Make",-12} {"Model",-12} {"Year",-6} {"Color",-10} {"Reg#",-15} {"Rate",-10} {"Status",-10}");
                        Console.WriteLine(new string('-', 85));
                        foreach (var v in vehicles)
                        {
                            string status = v.Availability ? "Available" : "Booked";
                            Console.WriteLine($"{v.VehicleID,-5} {v.Make,-12} {v.Model,-12} {v.Year,-6} {v.Color,-10} {v.RegistrationNumber,-15} {v.DailyRate,-10} {status,-10}");
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid report type.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating report: {ex.Message}");
            }
        }
    }
}
