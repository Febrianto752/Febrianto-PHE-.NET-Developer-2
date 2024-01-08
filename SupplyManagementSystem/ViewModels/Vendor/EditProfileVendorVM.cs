using SupplyManagementSystem.Utilities.Enums;
using System;

namespace SupplyManagementSystem.ViewModels.Vendor
{
    public class EditProfileVendorVM
    {
        public Guid VendorGuid { get; set; }

        public string VendorName { get; set; }
        public string BusinessField { get; set; }

        public string TypeCompany { get; set; }


        public VendorStatusEnum Status { get; set; }
    }
}