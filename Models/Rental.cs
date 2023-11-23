using System.Collections.Generic;
using System;

namespace MovieManagementAPI.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public List<Movie> Movies { get; set; } // non-nullable
        public string CustomerCPF { get; set; } // Máx 14 caracteres, non-nullable
        public DateTime RentalDate { get; set; }
        
    }
}