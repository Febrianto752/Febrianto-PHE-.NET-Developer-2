using SupplyManagementSystem.Utilities.Enums;

namespace SupplyManagementSystem.Utilities.Handlers
{
    public static class GetNameHandler
    {
        public static string GetVendorStatusName(VendorStatusEnum vendorStatus)
        {
            if (vendorStatus == VendorStatusEnum.Pending) return "Meminta bergabung";
            if (vendorStatus == VendorStatusEnum.Accepted) return "Bergabung";
            if (vendorStatus == VendorStatusEnum.ApproveByAdmin) return "Disetujui Admin";
            else return "Ditolak";
        }
    }
}