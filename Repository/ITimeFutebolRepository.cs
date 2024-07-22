using System.Collections.Generic;
using WebApiCaching.Models;

namespace WebApiCaching.Repository
{
    public interface ITimeFutebolRepository
    {
        Task<TimeFutebol> ObterTime(int id);
        Task<IEnumerable<TimeFutebol>> ObterTimes();
        Task<bool> AddTime(TimeFutebol timeFutebol);
        Task<bool> ExcluirTime(int id);
    }
}
