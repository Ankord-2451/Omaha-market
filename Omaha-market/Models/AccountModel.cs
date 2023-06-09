﻿using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Omaha_market.Models
{
    public enum role
    { 
        Customer,

        Admin     
    }

    public class AccountModel
    {
        [Key]
        [AutoIncrement]
        public int ID { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string Name { set; get; }

        [Required]
        public role Role { get; set; }

        
        public string PhoneNumber { get; set; }

        
        public string Email { get; set; }

        
        public string Address { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [StringLength(150,MinimumLength = 8)]
        public string Password { get; set; }
    }
}
