using Microsoft.EntityFrameworkCore;
using WebApiCaching.Data;
using WebApiCaching.Models;

namespace WebApiCaching.Repository
{
    public class TimeFutebolRepository : ITimeFutebolRepository
    {
        private readonly AppDbContext _context;

        public TimeFutebolRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TimeFutebol>> ObterTimes( )
        {
            return await _context.TimeFutebols
                                .Include(x => x.Jogadores)
                                .AsNoTracking().ToListAsync();
        }

        public async Task<TimeFutebol> ObterTime(int id)
        {
            return await _context.TimeFutebols
                                .Include(x => x.Jogadores)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.TimeFutebolId == id) ?? null!;

        }


        public async Task<bool> AddTime(TimeFutebol timeFutebol)
        {
            var timeExiste = await ObterTime(timeFutebol.TimeFutebolId);
            if (timeExiste != null)
            {
                return false;
            }

            await _context.AddAsync(timeFutebol);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExcluirTime(int id)
        {
            var timeExistente = await ObterTime(id);
            if (timeExistente != null)
            {
                _context.Remove(timeExistente);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
