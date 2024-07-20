namespace WebApiCaching.Models
{
    public class TimeFutebol
    {
        public int TimeFutebolId { get; set; }  // Identificador único do Time
        public string Nome { get; set; } = string.Empty;  // Nome do Time
        public string Descricao { get; set; } = string.Empty;  // Descrição do Time
        public string Classificacao { get; set; } = string.Empty;  // Classificação do Time

        // Relação de um-para-muitos com Jogador
        public List<Jogador>? Jogadores { get; set; } = new List<Jogador>();
    }


}
