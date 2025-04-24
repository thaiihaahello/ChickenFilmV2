using ChickenFilmV2.Models;

namespace ChickenFilmV2.Services.Interfaces
{
    public interface ITheaterService
    {
        Task<IEnumerable<Theater>> GetAllTheatersAsync();
        Task<Theater> GetTheaterByIdAsync(int theaterId);
        Task AddTheaterAsync(Theater theater);
        Task UpdateTheaterAsync(Theater theater);
        Task DeleteTheaterAsync(int theaterId);
    }

}
