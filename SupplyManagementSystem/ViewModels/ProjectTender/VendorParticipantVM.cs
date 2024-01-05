using System;

namespace SupplyManagementSystem.ViewModels.ProjectTender
{
    public class VendorParticipantVM
    {
        public Guid ProjectTenderGuid { get; set; }

        public string VendorName { get; set; }

        public string BusinessField { get; set; }

        public string TypeCompany { get; set; }
    }
}