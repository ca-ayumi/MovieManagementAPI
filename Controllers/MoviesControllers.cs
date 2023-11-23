using Microsoft.AspNetCore.Mvc;
using MovieManagementAPI.Services;
using MovieManagementAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MovieManagementAPI.Controllers
{

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: api/movies
        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

        // POST: api/movies
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] MovieDTO movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdMovie = await _movieService.AddMovieAsync(movieDto);

            return CreatedAtAction(nameof(CreateMovie), new { id = createdMovie.Id }, createdMovie);
        }

        // PUT: api/movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieDTO movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedMovie = await _movieService.UpdateMovieAsync(id, movieDto);

            if (updatedMovie == null)
            {
                return NotFound();
            }

            return Ok(updatedMovie);
        }

        // DELETE: api/movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var result = await _movieService.DeleteMovieAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/movies
        [HttpDelete]
        public async Task<IActionResult> DeleteMultipleMovies([FromBody] List<int> ids)
        {
            await _movieService.DeleteMultipleMoviesAsync(ids);
            return NoContent();
        }
    }
}