﻿namespace WebApiCaching.Dtos
{
    public class TimeFutebolDto
    {
        public int TimeFutebolId { get; set; }
        public string Nome { get; set; } = string.Empty; 
        public string Classificacao { get; set;} = string.Empty;
        public List<JogadorDto>? Jogadores { get; set; } = new List<JogadorDto>();
    }
    public class TimeFutebolEditDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Classificacao { get; set; } = string.Empty;
    }

    public class TimeFutebolAddDto
    {
        public int TimeFutebolId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Classificacao { get; set;} = string.Empty;
    }
}
