using WebApiCaching.Models;

namespace WebApiCaching.Models
{
    public class Jogador
    {
        public int Id { get; set; }  // Identificador único do Jogador
        public string Nome { get; set; } = string.Empty;  // Nome do Jogador
        public int NumeroCamisa { get; set; }  // Número da camisa do Jogador

        // Chave estrangeira para a tabela TimeFutebol
        public int TimeFutebolId { get; set; }
        // Navegação para TimeFutebol
        public TimeFutebol? TimeFutebol { get; set; }
    }

}
