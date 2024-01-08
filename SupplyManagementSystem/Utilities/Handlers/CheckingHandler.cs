using SupplyManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace SupplyManagementSystem.Utilities.Handlers
{
    public class CheckingHandler
    {
        public static bool IsVendorJoinedProject(ICollection<ProjectTender> projectTenders, Guid vendorGuid)
        {
            foreach (var tender in projectTenders)
            {
                if (tender.VendorGuid == vendorGuid)
                {
                    return true;
                }
            }

            return false;
        }
    }
}