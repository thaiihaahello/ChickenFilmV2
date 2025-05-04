
using ChickenFilmV2.Models;
using ChickenFilmV2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TheaterService : ITheaterService
{
    private readonly MovieDbContext _context;

    public TheaterService(MovieDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Theater>> GetAllTheatersAsync()
    {
        return await _context.Theaters.ToListAsync();
    }

    public async Task<Theater> GetTheaterByIdAsync(int theaterId)
    {
        return await _context.Theaters.FirstOrDefaultAsync(t => t.TheaterId == theaterId);
    }

    public async Task AddTheaterAsync(Theater theater)
    {
        _context.Theaters.Add(theater);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTheaterAsync(Theater theater)
    {
        _context.Theaters.Update(theater);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTheaterAsync(int theaterId)
    {
        var theater = await GetTheaterByIdAsync(theaterId);
        if (theater != null)
        {
            _context.Theaters.Remove(theater);
            await _context.SaveChangesAsync();
        }
    }
}
