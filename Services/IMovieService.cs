using MovieManagementAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieManagementAPI.Services
{

    public interface IMovieService
    {
        Task<IEnumerable<MovieDTO>> GetAllMoviesAsync();
        Task<MovieDTO> AddMovieAsync(MovieDTO movieDto);
        Task<MovieDTO> UpdateMovieAsync(int id, MovieDTO movieDto);
        Task<bool> DeleteMovieAsync(int id);
        Task DeleteMultipleMoviesAsync(List<int> ids);
    }
}