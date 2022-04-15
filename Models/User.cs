using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersTable.Models
{
    // Add profile data for application users by adding properties to the UsersTableUser class
    public class User : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(40)")]
        public string Name { get; set; }

        [PersonalData]
        public DateTime RegistrationDate { get; set; }

        [PersonalData]
        public DateTime LastLogin { get; set; }

        [PersonalData]
        public Status Status { get; set; }

    }

    public enum Status
    {
        Active,
        Blocked,
    }
}
