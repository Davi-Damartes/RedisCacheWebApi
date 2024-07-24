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
                                .OrderBy(x  => x.Id)
                                .AsNoTracking().ToListAsync();
        }

        public async Task<TimeFutebol> ObterTime(int id)
        {
            return await _context.TimeFutebols
                                .Include(x => x.Jogadores)
                                .AsNoTracking()
                                .OrderBy(x => x.Id)
                                .FirstOrDefaultAsync(x => x.Id == id) ?? null!;

        }


        public async Task<bool> AddTime(TimeFutebol timeFutebol)
        {
            var timeExiste = await ObterTime(timeFutebol.Id);
            if (timeExiste != null)
            {
                return false;
            }

            await _context.AddAsync(timeFutebol);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<TimeFutebol> AtualizarTimeFut(TimeFutebol timeFutebol)
        {
            if(timeFutebol != null)
            {
                _context.TimeFutebols.Update(timeFutebol);
                await _context.SaveChangesAsync();
                return timeFutebol;
            }
            return default!;

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
