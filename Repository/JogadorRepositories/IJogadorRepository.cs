﻿using WebApiCaching.Models;

namespace WebApiCaching.Repository.JogadorRepositories
{
    public interface IJogadorRepository
    {
        Task<IEnumerable<Jogador>> ObterJogadores();

        Task<Jogador> ObterJogador(int Id);

        Task<bool> AddJogador(Jogador Jogador);

        Task<Jogador> TransferirJogador(int Id, int TimeId);

        Task<bool> ExcluirJogador(Jogador jogador);

    }
}
