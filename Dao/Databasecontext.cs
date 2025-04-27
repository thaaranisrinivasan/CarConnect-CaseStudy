namespace CarConnectDaoLibrary
{
    public static class DatabaseContext
    {
        public static IAdminService AdminService => new AdminService();
        public static ICustomerService CustomerService => new CustomerService();
        public static IVehicleService VehicleService => new VehicleService();
        public static IReservationService ReservationService => new ReservationService();

        public static AuthenticationService AuthService => new AuthenticationService(AdminService, CustomerService);
    }
}
