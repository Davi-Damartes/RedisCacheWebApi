using WebApiCaching.Dtos;
using WebApiCaching.Models;

namespace WebApiCaching.MappingEntity
{
    public static class MappingTimeFutebol
    {
        public static TimeFutebol ConverterTimeFutDtoParaTimeFut(this TimeFutebolDto timeFutebolDto)
        {
            return new TimeFutebol
            {
                Id = timeFutebolDto.TimeFutebolId,
                Nome = timeFutebolDto.Nome,
                Classificacao = timeFutebolDto.Classificacao,
                Jogadores = timeFutebolDto.Jogadores?.Select(jogador => new Jogador
                {
                    Id = jogador.Id,
                    Nome = jogador.NomeJogador,
                    NumeroCamisa = jogador.NumeroCamisa

                }).ToList()
            };
        }
        public static TimeFutebolDto ConverterTimeFutParaTimeFutDto(this TimeFutebol timeFutebol)
        {
            return new TimeFutebolDto
            {
                TimeFutebolId = timeFutebol.Id,
                Nome = timeFutebol.Nome,
                Classificacao = timeFutebol.Classificacao,
                Jogadores = timeFutebol.Jogadores?.Select(jogador => new JogadorDto
                {
                    Id = jogador.Id,
                    NomeJogador = jogador.Nome,
                    NumeroCamisa = jogador.NumeroCamisa,
                    NomeTime = jogador.Time.Nome,
                    TimeFutebolId = jogador.TimeFutebolId,

                }).ToList()
            };
        } 
        
        public static TimeFutebol ConverterTimeFutEditParaTimeFut(this TimeFutebolEditDto novoTime, TimeFutebol time)
        {
            return new TimeFutebol
            {
                Id = time.Id,
                Nome = novoTime.Nome,
                Classificacao = novoTime.Classificacao,
                Jogadores = time.Jogadores.Select(jogador => new Jogador
                {
                    Id = jogador.Id,
                    Nome = jogador.Nome,
                    NumeroCamisa = jogador.NumeroCamisa,
                    TimeFutebolId = jogador.TimeFutebolId,
                    
                }).ToList()
            };
        } 
        
        public static IEnumerable<TimeFutebolDto> ConverterTimesFutParaTimesFutDto(this IEnumerable<TimeFutebol> timesFutebol)
        {            
            return (from time in timesFutebol
                    select new TimeFutebolDto
                    {
                        TimeFutebolId = time.Id,
                        Nome = time.Nome,
                        Classificacao = time.Classificacao,
                        Jogadores = time.Jogadores?.Select(jogador => new JogadorDto
                        {
                            Id = jogador.Id,
                            NomeJogador = jogador.Nome,
                            NumeroCamisa = jogador.NumeroCamisa,
                            NomeTime = jogador.Time.Nome,
                            TimeFutebolId = jogador.TimeFutebolId,

                        }).ToList()
                    });
        }
    }
}
