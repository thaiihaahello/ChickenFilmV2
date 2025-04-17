using ChickenFilmV2.Models;

namespace ChickenFilmV2.Contacts
{
    public interface IMoviesServices 
    {
        public List<Movie> GetAllMovies();
        public Movie GetMovieById(int id);
        public void AddMovie(Movie movie);
        public void UpdateMovie(Movie movie);
        public void DeleteMovie(int id);
    }
}
