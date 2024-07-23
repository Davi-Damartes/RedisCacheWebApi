using WebApiCaching.Dtos;
using WebApiCaching.Models;

namespace WebApiCaching.MappingEntity
{
    public static class MappingJogador
    {
        public static Jogador ConverterJogadorDtoParaJogador(this JogadorDto jogadorDto)
        {
            return new Jogador
            {
                Id = jogadorDto.Id,
                Nome = jogadorDto.Nome,
                NumeroCamisa = jogadorDto.NumeroCamisa,
                //TimeFutebolId = jogadorDto.TimeFutebolId,
                TimeFutebol = new TimeFutebol
                {
                    TimeFutebolId = jogadorDto.TimeFutebol.TimeFutebolId,
                    Nome = jogadorDto.TimeFutebol.Nome,
                    Descricao = jogadorDto.TimeFutebol.Descricao,
                    Classificacao = jogadorDto.TimeFutebol.Classificacao,
                    Jogadores = null
                }
            };
        }
         public static Jogador ConverterJogadorETimeDtoParaJogador(this JogadorDto jogadorDto)
        {
            return new Jogador
            {
                Id = jogadorDto.Id,
                Nome = jogadorDto.Nome,
                NumeroCamisa = jogadorDto.NumeroCamisa,
                //TimeFutebolId = jogadorDto.TimeFutebolId,
                TimeFutebol = null
            };
        }
        
        public static JogadorDto ConverterJogadorParaJogadorDto(this Jogador jogador, TimeFutebol time)
        {
            return new JogadorDto
            {
                Id = jogador.Id,
                Nome = jogador.Nome,
                NumeroCamisa = jogador.NumeroCamisa,
                TimeFutebol = new TimeFutebolDto
                {
                    TimeFutebolId = time.TimeFutebolId,
                    Nome = time.Nome,
                    Descricao = time.Descricao,
                    Classificacao = time.Classificacao
                }
            };
        }

        public static IEnumerable<JogadorDto> ConverterJogadoresParaJogadoresDto(this IEnumerable<Jogador> jogadores)
        {
            return (from jogador in jogadores
                    select new JogadorDto
                    {
                        Id = jogador.Id,
                        Nome = jogador.Nome,
                        NumeroCamisa = jogador.NumeroCamisa,
                        //TimeFutebolId = jogador.TimeFutebol.TimeFutebolId,
                        TimeFutebol = new TimeFutebolDto
                        {
                            TimeFutebolId = jogador.TimeFutebol.TimeFutebolId,
                            Nome = jogador.TimeFutebol.Nome,
                            Descricao = jogador.TimeFutebol.Descricao,
                            Classificacao = jogador.TimeFutebol.Classificacao,
                            Jogadores = null,
                        }
                    });
                    
        }
    }
}
