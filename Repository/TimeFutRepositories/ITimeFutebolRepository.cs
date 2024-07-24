using System.Collections.Generic;
using WebApiCaching.Models;

namespace WebApiCaching.Repository.TimeFutRepositories
{
    public interface ITimeFutebolRepository
    {
        Task<TimeFutebol> ObterTime(int id);
        Task<IEnumerable<TimeFutebol>> ObterTimes();
        Task<bool> AddTime(TimeFutebol timeFutebol);

        Task<TimeFutebol> AtualizarTimeFut(TimeFutebol timeFutebol);
        Task<bool> AddJogadorTime(TimeFutebol time,Jogador jogador);
        Task<bool> ExcluirTime(int id);
    }
}
