﻿using System.ComponentModel.DataAnnotations;

namespace Omaha_market.Models
{
    public class favoriteModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdOfProduct { get; set; }

        [Required]
        public int IdOfCustomer { get; set; }
    }
}
