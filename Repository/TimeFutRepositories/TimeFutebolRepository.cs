using Microsoft.EntityFrameworkCore;
using WebApiCaching.Data;
using WebApiCaching.Models;

namespace WebApiCaching.Repository.TimeFutRepositories
{
    public class TimeFutebolRepository : ITimeFutebolRepository
    {
        private readonly AppDbContext _context;

        public TimeFutebolRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TimeFutebol>> ObterTimes()
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


        public async Task<bool> AtualizarTimeFut(TimeFutebol timeFutebol)
        {
            if(timeFutebol != null)
            {
                _context.TimeFutebols.Update(timeFutebol);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<bool> AddJogadorTime(TimeFutebol time, Jogador jogador)
        {
            if (!time.Jogadores.Contains(jogador))
            {
                time.Jogadores.Add(jogador);
                _context.TimeFutebols.Update(time);
                await _context.SaveChangesAsync();
                return true;          
            }
            return false;

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
