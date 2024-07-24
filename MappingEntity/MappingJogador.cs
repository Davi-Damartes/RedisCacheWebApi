using WebApiCaching.Dtos;
using WebApiCaching.Models;

namespace WebApiCaching.MappingEntity
{
    public static class MappingJogador
    {
        public static Jogador ConverterJogadorETimeDtoParaJogador(this JogadorDto jogadoraddDto, TimeFutebol timeFutebol)
        {
            return new Jogador
            {
                Id = jogadoraddDto.Id,
                Nome = jogadoraddDto.NomeJogador,
                TimeFutebolId = jogadoraddDto.TimeFutebolId,
                NumeroCamisa = jogadoraddDto.NumeroCamisa,
                Time = new TimeFutebol
                {
                    Id = timeFutebol.Id,
                    Nome = timeFutebol.Nome,
                    Classificacao = timeFutebol.Classificacao,
                    Jogadores = null
                }
            };
        }
        
         public static Jogador ConverterJogadorAddDtoParaJogador(this JogadorAddicionarDto jogadorDto)
        {
            return new Jogador
            {
                Id = jogadorDto.Id,
                Nome = jogadorDto.NomeJogador,
                NumeroCamisa = jogadorDto.NumeroCamisa,
                TimeFutebolId = jogadorDto.TimeFutebolId,
                Time = null
            };
        }
        

        public static JogadorDto ConverterJogadorParaJogadorDto(this Jogador jogador, TimeFutebol time)
        {
            return new JogadorDto
            {
                Id = jogador.Id,
                NomeJogador = jogador.Nome,
                NumeroCamisa = jogador.NumeroCamisa,
                TimeFutebolId = time.Id,
                NomeTime = time.Nome

            };
        }
        public static JogadorDto ConverterJogadorParaJogadorDto(this Jogador jogador)
        {
            return new JogadorDto
            {
                Id = jogador.Id,
                NomeJogador = jogador.Nome,
                NumeroCamisa = jogador.NumeroCamisa,
                TimeFutebolId = jogador.Time.Id,
                NomeTime = jogador.Time.Nome
            };
        }


        public static IEnumerable<JogadorDto> ConverterJogadoresParaJogadoresDto(this IEnumerable<Jogador> jogadores)
        {
            return (from jogador in jogadores
                    select new JogadorDto
                    {
                        Id = jogador.Id,
                        NomeJogador = jogador.Nome,
                        NumeroCamisa = jogador.NumeroCamisa,
                        TimeFutebolId = jogador.Time.Id,
                        NomeTime = jogador.Time.Nome

                    });                
        }
    }
}
