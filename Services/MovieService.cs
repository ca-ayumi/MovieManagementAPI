//A MovieService é uma classe que implementa a interface IMovieService. Ela conterá a lógica de negócios relacionada à entidade Movie
//Todos os métodos são assíncronos, utilizando await para operações de banco de dados.
using System;
using MovieManagementAPI.Data;
using MovieManagementAPI.DTOs;
using MovieManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MovieManagementAPI.Services
{

    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            // Injeção de dependência do contexto do banco de dados
            _context = context;
        }

        // Operação para listar os filmes cadastrados
        public async Task<IEnumerable<MovieDTO>> GetAllMoviesAsync()
        {
            try
            {
                // Consulta assíncrona ao banco de dados para obter todos os filmes e mapeá-los para MovieDTO
                return await _context.Movies
                    .Select(movie => new MovieDTO
                    {
                        Id = movie.Id,
                        Name = movie.Name,
                        GenreId = movie.GenreId,
                        CreationDate = movie.CreationDate,
                        Active = movie.Active
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Tratamento de exceção - Log e relançamento de uma exceção personalizada
                throw new ServiceException("Erro ao buscar filmes", ex);
            }
        }

        // Operação para cadastrar um novo filme
        public async Task<MovieDTO> AddMovieAsync(MovieDTO movieDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
            // Verifique se o gênero com o ID fornecido existe no banco de dados
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == movieDto.GenreId);

                // Crie um novo filme com o ID do gênero
                var movie = new Movie
                {
                    Name = movieDto.Name,
                    CreationDate = DateTime.Now,
                    Active = movieDto.Active,
                    GenreId = movieDto.GenreId // Atribua o ID do gênero
                };

                // Adicionando e salvando o filme no banco de dados
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();

                // Confirmação da transação se tudo estiver correto
                await transaction.CommitAsync();

                movieDto.Id = movie.Id; // Atualizando o DTO com o ID do filme recém-criado
                return movieDto;
            }
            catch (Exception ex)
            {
                // Rollback da transação em caso de exceção
                await transaction.RollbackAsync();
                throw new ServiceException("Erro ao adicionar filme", ex);
            }
        }

        // Operação para editar um filme
        public async Task<MovieDTO> UpdateMovieAsync(int id, MovieDTO movieDto)
        {
            // Inicia uma nova transação
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Busca o filme pelo ID fornecido
                var movie = await _context.Movies.FindAsync(id);

                // Se não encontrar o filme, faz rollback e retorna null
                if (movie == null)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                // Atualiza as propriedades do filme com os dados do DTO
                movie.Name = movieDto.Name;
                movie.GenreId = movieDto.GenreId;
                movie.CreationDate = movieDto.CreationDate;
                movie.Active = movieDto.Active;

                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();
                // Confirma a transação
                await transaction.CommitAsync();

                // Retorna o DTO atualizado
                movieDto.Id = movie.Id;
                return movieDto;
            }
            catch (Exception ex)
            {
                // Rollback da transação em caso de exceção
                await transaction.RollbackAsync();
                throw new ServiceException("Erro ao atualizar filme", ex);
            }
        }

        // Operação para remover um filme individualmente
        public async Task<bool> DeleteMovieAsync(int id)
        {
            // Inicia uma nova transação
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Busca o filme pelo ID fornecido
                var movie = await _context.Movies.FindAsync(id);

                // Se o filme não for encontrado, faz rollback e retorna false
                if (movie == null)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                // Remove o filme do banco de dados
                _context.Movies.Remove(movie);
                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();
                // Confirma a transação
                await transaction.CommitAsync();

                // Retorna true indicando sucesso na operação
                return true;
            }
            catch (Exception ex)
            {
                // Em caso de exceção, faz rollback e lança ServiceException
                await transaction.RollbackAsync();
                throw new ServiceException("Erro ao deletar filme", ex);
            }
        }

        // Operação para remover vários filmes de uma só vez
        public async Task DeleteMultipleMoviesAsync(List<int> ids)
        {
            // Inicia uma nova transação
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Busca todos os filmes que correspondem aos IDs fornecidos
                var movies = await _context.Movies.Where(m => ids.Contains(m.Id)).ToListAsync();

                // Remove os filmes encontrados do banco de dados
                _context.Movies.RemoveRange(movies);
                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();
                // Confirma a transação
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Em caso de exceção, faz rollback e lança ServiceException
                await transaction.RollbackAsync();
                throw new ServiceException("Erro ao deletar múltiplos filmes", ex);
            }
        }

        // Classe para exceções personalizadas na camada de serviço
        public class ServiceException : Exception
        {
            public ServiceException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }
    }
}