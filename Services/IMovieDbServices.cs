using MyNewProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyNewProject.Services
{
    public interface IMovieDbServices
    {
        Task<List<Movie>> GetPopularMovies();
        Task<List<Movie>> GetNowPlayingMovies();
        Task<List<Genre>> GetGenres();
        Task<List<Movie>> GetAllMovies();
        Task<List<string>> GetGenresOfMovie(int id);
        Task<MovieDetailsClass> GetMovieDetails(int id);
    }
}