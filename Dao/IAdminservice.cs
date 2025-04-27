using CarConnectEntityLibrary;

namespace CarConnectDaoLibrary
{
    public interface IAdminService
    {
        Admin GetAdminById(int adminId);
        Admin GetAdminByUsername(string username);
        void RegisterAdmin(Admin admin);
        void UpdateAdmin(Admin admin);
        void DeleteAdmin(int adminId);
    }
}
