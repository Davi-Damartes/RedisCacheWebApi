using Microsoft.EntityFrameworkCore;
using WebApiCaching.Data;
using WebApiCaching.Models;

namespace WebApiCaching.Repository.JogadorRepositories
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
            return await _context.Jogadores.Include(x => x.Time)
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync(x => x.Id == Id) ?? null!;

        }

        public async Task<IEnumerable<Jogador>> ObterJogadores()
        {
            return await _context.Jogadores.Include(x => x.Time)
                                           .AsNoTracking()
                                           .OrderBy(x => x.Id)  
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

        public async Task<bool> ExcluirJogador(Jogador jogador)
        {
            _context.Remove(jogador);
            await _context.SaveChangesAsync();
            return true;
            
        }

        public Task<Jogador> TransferirJogador(int Id, int TimeId)
        {
            throw new NotImplementedException();
        }
    }
}
