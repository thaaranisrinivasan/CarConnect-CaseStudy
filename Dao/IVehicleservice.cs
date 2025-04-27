using System.Collections.Generic;
using CarConnectEntityLibrary;

namespace CarConnectDaoLibrary
{
    public interface IVehicleService
    {
        Vehicle GetVehicleById(int vehicleId);
        List<Vehicle> GetAvailableVehicles();
        void AddVehicle(Vehicle vehicle);
        void UpdateVehicle(Vehicle vehicle);
        void RemoveVehicle(int vehicleId);
    }
}
