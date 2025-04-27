using CarConnectEntityLibrary;
using CarConnectExceptionLibrary;

namespace CarConnectDaoLibrary
{
    public class AuthenticationService
    {
        private readonly IAdminService adminService;
        private readonly ICustomerService customerService;

        public AuthenticationService(IAdminService adminService, ICustomerService customerService)
        {
            this.adminService = adminService;
            this.customerService = customerService;
        }

        public Admin AuthenticateAdmin(string username, string password)
        {
            Admin admin = adminService.GetAdminByUsername(username);
            if (admin == null || !admin.Authenticate(password))
                throw new AuthenticationException("Invalid admin credentials.");
            return admin;
        }

        public Customer AuthenticateCustomer(string username, string password)
        {
            Customer customer = customerService.GetCustomerByUsername(username);
            if (customer == null || !customer.Authenticate(password))
                throw new AuthenticationException("Invalid customer credentials.");
            return customer;
        }
    }
}
