using System;
using System.Collections.Generic;

namespace MovieManagementAPI.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } // Máx 100 caracteres, non-nullable
        public DateTime CreationDate { get; set; }
        public int Active { get; set; }

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();

    }
}