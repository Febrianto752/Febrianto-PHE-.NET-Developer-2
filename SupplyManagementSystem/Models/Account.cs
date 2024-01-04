using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyManagementSystem.Models
{
    [Table("accounts")]
    public class Account : BaseEntity
    {
        [EmailAddress, Required, Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("name"), Required]
        public string Name { get; set; }

        [Column("no_telp"), Required]
        public string NoTelp { get; set; }

        [Column("role"), Required] // Admin, Manager, Vendor
        public string Role { get; set; }
    }
}