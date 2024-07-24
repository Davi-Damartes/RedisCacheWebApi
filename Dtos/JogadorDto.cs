namespace WebApiCaching.Dtos
{
    public class JogadorDto
    {
        public int Id { get; set; }
        public string? NomeJogador { get; set; }
        public int NumeroCamisa { get; set; }
        public string? NomeTime { get; set;}
        public int TimeFutebolId { get; set; }


    }
    public class JogadorAddicionarDto
    {
        public int Id { get; set; }
        public string? NomeJogador { get; set; }
        public int NumeroCamisa { get; set; }
        public int TimeFutebolId { get; set; }
    }
}
