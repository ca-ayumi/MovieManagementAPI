using System;

namespace MovieManagementAPI.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int Active { get; set; }
        public int GenreId { get; set; } // Chave estrangeira como GenreId
    }
}