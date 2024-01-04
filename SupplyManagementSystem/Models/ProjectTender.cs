using SupplyManagementSystem.Utilities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyManagementSystem.Models
{
    [Table("project_tenders")]
    public class ProjectTender : BaseEntity
    {
        [Required, Column("vendor_guid")]
        public Guid VendorGuid { get; set; }

        [ForeignKey("VendorGuid")]
        public Vendor Vendor { get; set; }


        [Required, Column("project_guid")]
        public Guid ProjectGuid { get; set; }

        [ForeignKey("ProjectGuid")]
        public Project Project { get; set; }


        [Required, Column("status")]
        public TenderStatusEnum Status { get; set; }
    }
}