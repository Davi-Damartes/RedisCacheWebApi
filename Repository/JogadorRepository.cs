using Microsoft.EntityFrameworkCore;
using WebApiCaching.Data;
using WebApiCaching.Models;

namespace WebApiCaching.Repository
{
    public class JogadorRepository : IJogadorRepository
    {
        private readonly AppDbContext _context;

        public JogadorRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Jogador> ObterJogador(int Id)
        {
            return await _context.Jogadores.Include(x => x.TimeFutebol)
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync(x => x.Id == Id) ?? null!;
           
        }

        public async Task<IEnumerable<Jogador>> ObterJogadores( )
        {
            return await _context.Jogadores.Include(x => x.TimeFutebol)
                                           .AsNoTracking()
                                           .ToListAsync();
        }


        public async Task<bool> AddJogador(Jogador Jogador)
        {
            var jogadorExistente = await ObterJogador(Jogador.Id);
            if (jogadorExistente != null)
            {
                return false;
            }

            await _context.Jogadores.AddAsync(Jogador);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExcluirJogador(int Id)
        {
            var jogadorExistente = await ObterJogador(Id);
            if (jogadorExistente != null)
            {
                _context.Remove(jogadorExistente);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public Task<Jogador> TransferirJogador(int Id, int TimeId)
        {
            throw new NotImplementedException();
        }
    }
}
