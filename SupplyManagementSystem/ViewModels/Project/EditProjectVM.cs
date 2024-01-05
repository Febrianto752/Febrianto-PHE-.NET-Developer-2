using System;
using System.ComponentModel.DataAnnotations;

namespace SupplyManagementSystem.ViewModels.Project
{
    public class EditProjectVM
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}