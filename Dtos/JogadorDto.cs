namespace WebApiCaching.Dtos
{
    public class JogadorDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int NumeroCamisa { get; set; }

        public int TimeFutebolId { get; set; }
        public TimeFutebolDto? TimeFutebol { get; set; }
    }
}
