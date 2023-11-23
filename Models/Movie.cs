using System;
using System.Collections.Generic;

namespace MovieManagementAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } // Nome do filme (máximo de 200 caracteres, non-nullable)
        public DateTime CreationDate { get; set; } // Data de criação (DateTime)
        public int Active { get; set; } // Ativo (1 ou 0)
        public int GenreId { get; set; } // Chave estrangeira para o gênero do filme
        public Genre Genre { get; set; } // Gênero do filme (relacionamento com a classe Genre)
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    }
}