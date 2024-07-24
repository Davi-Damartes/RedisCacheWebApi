namespace WebApiCaching.Models
{
        public class Jogador
        {
            public int Id { get; set; }  
            public string Nome { get; set; } = string.Empty;  
            public int NumeroCamisa { get; set; } 
            public int TimeFutebolId { get; set; }
            public TimeFutebol? Time { get; set; }
        }

}
