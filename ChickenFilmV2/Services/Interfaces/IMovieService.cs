using ChickenFilmV2.Models;

namespace ChickenFilmV2.Services.Interfaces;
public interface IMovieService
{
    Task<IEnumerable<Movie>> GetAllMoviesAsync();
    Task<Movie> GetMovieByIdAsync(int movieId);
    Task AddMovieAsync(Movie movie);
    Task UpdateMovieAsync(Movie movie);
    Task DeleteMovieAsync(int movieId);
}
