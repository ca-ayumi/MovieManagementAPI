using System;
using Xunit;
using MovieManagementAPI.Models;
using MovieManagementAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieManagementAPI.Data;
using MovieManagementAPI.DTOs;

namespace MovieManagementAPITests
{
    public class MovieServiceTests
    {
        // Teste para Listar Filmes
        [Fact]
        public async Task GetAllMoviesAsync_Should_ReturnAllMovies()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Adicione alguns filmes de teste ao banco de dados de teste
                context.Movies.Add(new Movie { Id = 1, Name = "Movie 1", GenreId = 1 });
                context.Movies.Add(new Movie { Id = 2, Name = "Movie 2", GenreId = 2 });
                context.SaveChanges();

                var movieService = new MovieService(context);

                // Act
                var movies = await movieService.GetAllMoviesAsync();

                // Assert
                Assert.NotNull(movies);
                Assert.Equal(2, movies.Count());
            }
        }
        // Teste para Adicionar um Filme
        [Fact]
        public async Task AddMovieAsync_Should_AddMovieToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var movieService = new MovieService(context);

                // Act
                var movieDto = new MovieDTO
                {
                    Name = "Test Movie",
                    GenreId = 1, // Substitua pelo ID do gênero correto
                    CreationDate = DateTime.Now,
                    Active = 1
                };
                var addedMovie = await movieService.AddMovieAsync(movieDto);

                // Assert
                Assert.NotNull(addedMovie);
                Assert.NotEqual(0, addedMovie.Id);

                // Verifique se o filme foi realmente adicionado ao banco de dados.
                var movieFromDb = context.Movies.FirstOrDefault(m => m.Id == addedMovie.Id);
                Assert.NotNull(movieFromDb);
                Assert.Equal(movieDto.Name, movieFromDb.Name);
            }
        }

        //  Teste para Atualizar um Filme

        [Fact]
        public async Task UpdateMovieAsync_Should_UpdateMovieInDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Adicione um filme de teste ao banco de dados de teste
                context.Movies.Add(new Movie { Id = 1, Name = "Test Movie", GenreId = 1 });
                context.SaveChanges();

                var movieService = new MovieService(context);

                // Act
                var movieDto = new MovieDTO
                {
                    Name = "Updated Movie",
                    GenreId = 2, // Substitua pelo ID do novo gênero correto
                    CreationDate = DateTime.Now,
                    Active = 1
                };
                var updatedMovie = await movieService.UpdateMovieAsync(1, movieDto);

                // Assert
                Assert.NotNull(updatedMovie);
                Assert.Equal(1, updatedMovie.Id);

                // Verifique se o filme foi realmente atualizado no banco de dados.
                var movieFromDb = context.Movies.FirstOrDefault(m => m.Id == 1);
                Assert.NotNull(movieFromDb);
                Assert.Equal(movieDto.Name, movieFromDb.Name);

            }
        }

        //Teste para Excluir um Filme
        [Fact]
        public async Task DeleteMovieAsync_Should_DeleteMovieFromDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Adicione um filme de teste ao banco de dados de teste
                context.Movies.Add(new Movie { Id = 1, Name = "Test Movie", GenreId = 1 });
                context.SaveChanges();

                var movieService = new MovieService(context);

                // Act
                var result = await movieService.DeleteMovieAsync(1);

                // Assert
                Assert.True(result);

                // Verifique se o filme foi realmente excluído do banco de dados.
                var movieFromDb = context.Movies.FirstOrDefault(m => m.Id == 1);
                Assert.Null(movieFromDb);
            }
        }

    }
}
