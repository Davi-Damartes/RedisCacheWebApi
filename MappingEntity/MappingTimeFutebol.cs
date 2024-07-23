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
                TimeFutebolId = timeFutebolDto.TimeFutebolId,
                Nome = timeFutebolDto.Nome,
                Descricao = timeFutebolDto.Descricao,
                Classificacao = timeFutebolDto.Classificacao,
                Jogadores = timeFutebolDto.Jogadores?.Select(jogado => new Jogador
                {
                    Id = jogado.Id,
                    Nome = jogado.Nome,
                    NumeroCamisa = jogado.NumeroCamisa,
                    //TimeFutebolId = jogado.TimeFutebolId,

                }).ToList()
            };
        }
        public static TimeFutebolDto ConverterTimeFutParaTimeFutDto(this TimeFutebol timeFutebol)
        {
            return new TimeFutebolDto
            {
                TimeFutebolId = timeFutebol.TimeFutebolId,
                Nome = timeFutebol.Nome,
                Descricao = timeFutebol.Descricao,
                Classificacao = timeFutebol.Classificacao,
                Jogadores = timeFutebol.Jogadores?.Select(jogado => new JogadorDto
                {
                    Id = jogado.Id,
                    Nome = jogado.Nome,
                    NumeroCamisa = jogado.NumeroCamisa,
                    //TimeFutebolId = jogado.TimeFutebolId,

                }).ToList()
            };
        } 
        
        public static IEnumerable<TimeFutebolDto> ConverterTimesFutParaTimesFutDto(this IEnumerable<TimeFutebol> timesFutebol)
        {            
            return (from time in timesFutebol
                    select new TimeFutebolDto
                    {
                        TimeFutebolId = time.TimeFutebolId,
                        Nome = time.Nome,
                        Descricao = time.Descricao,
                        Classificacao = time.Classificacao,
                        Jogadores = time.Jogadores?.Select(jogador => new JogadorDto
                        {
                            Id = jogador.Id,
                            Nome = jogador.Nome,
                            NumeroCamisa = jogador.NumeroCamisa,
                            //TimeFutebolId = jogador.TimeFutebolId,

                        }).ToList()
                    });
        }
    }
}
