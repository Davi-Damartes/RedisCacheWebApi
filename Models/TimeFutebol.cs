namespace WebApiCaching.Models
{
    public class TimeFutebol
    {
        public int TimeFutebolId { get; set; }
        public string Nome { get; set; } = string.Empty; 
        public string Descricao { get; set; } = string.Empty; 
        public string Classificacao { get; set; } = string.Empty;  

        public List<Jogador>? Jogadores { get; set; } = new List<Jogador>();
    }


}
