using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyNewProject.Models;
using Newtonsoft.Json;

namespace MyNewProject.Services
{
    public class MovieDbServices : IMovieDbServices
    {
        public HttpClient client = new HttpClient();

        public async Task<List<Movie>> GetPopularMovies()
        {
            MoviesResponse popularMoviesResponse = new MoviesResponse();
            string url = "https://api.themoviedb.org/3/movie/popular?api_key=580bda8a5ab6a951e45ea4385de5bdd8";
            popularMoviesResponse = await client.GetFromJsonAsync<MoviesResponse>(url);
            return popularMoviesResponse.results.OrderByDescending(m => m.vote_average).ToList();
        }

        public async Task<List<Movie>> GetNowPlayingMovies() {
            MoviesResponse nowPlayingMovies = new MoviesResponse();
            string url = "https://api.themoviedb.org/3/movie/now_playing?api_key=580bda8a5ab6a951e45ea4385de5bdd8";
            nowPlayingMovies = await client.GetFromJsonAsync<MoviesResponse>(url);
            return nowPlayingMovies.results.OrderByDescending(m => m.release_date).ToList();
        }

        public async Task<List<Genre>> GetGenres()
        {
            GenreResponse genreResponse = new GenreResponse();
            genreResponse = await client.GetFromJsonAsync<GenreResponse>("https://api.themoviedb.org/3/genre/movie/list?api_key=580bda8a5ab6a951e45ea4385de5bdd8");
            return genreResponse.genres;
        }

        public async Task<List<Movie>> GetAllMovies() {
            var popularMovies = await this.GetPopularMovies();
            var nowPlayingMovies = await this.GetNowPlayingMovies();
            return popularMovies.Union(nowPlayingMovies).GroupBy(m => m.original_title).Select(g => g.First()).ToList();
        }

        public async Task<MovieDetailsClass> GetMovieDetails(int id)
        {
            string requestUri = "https://api.themoviedb.org/3/movie/" + id + "?api_key=580bda8a5ab6a951e45ea4385de5bdd8&append_to_response=credits,videos,images";
            var httpContent = await client.GetAsync(requestUri);
            string jsonContent = httpContent.Content.ReadAsStringAsync().Result;
            MovieDetailsClass movieDetails = JsonConvert.DeserializeObject<MovieDetailsClass>(jsonContent);
            return movieDetails;
        }

        public async Task<List<string>> GetGenresOfMovie(int id)
        {
            var genres = await GetGenres();
            var movie = await GetMovieDetails(id);
            int genresCount = movie.genres.Count;
            List<string> genresList = new List<string>();
            for (int i = 0; i < genresCount; i++)
            {
                if (i < genresCount - 1) {
                    genresList.Add(movie.genres[i].name + ", ");
                }
                else {
                    genresList.Add(movie.genres[i].name);
                }
            }
            return genresList;
        }


    }
}