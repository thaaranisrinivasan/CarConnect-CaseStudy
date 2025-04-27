using System.Collections.Generic;
using CarConnectEntityLibrary;

namespace CarConnectDaoLibrary
{
    public interface ICustomerService
    {
        Customer GetCustomerById(int customerId);
        Customer GetCustomerByUsername(string username);
        void RegisterCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int customerId);
    }
}
