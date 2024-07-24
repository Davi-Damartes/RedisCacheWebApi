using Microsoft.AspNetCore.Mvc;
using WebApiCaching.MappingEntity;
using WebApiCaching.Dtos;
using WebApiCaching.Repository.JogadorRepositories;
using WebApiCaching.Repository.TimeFutRepositories;
using WebApiCaching.Service;


namespace WebApiCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeFutebolController : ControllerBase
    {
        private readonly ITimeFutebolRepository _futebolRepository;
        private readonly IJogadorRepository _jogadorRepository;
        private readonly ICacheService _cacheService;
        public TimeFutebolController(ITimeFutebolRepository futebolRepository,
                                     IJogadorRepository jogadorRepository,
                                     ICacheService cacheService)
        {
            _futebolRepository = futebolRepository;
            _jogadorRepository = jogadorRepository;
            _cacheService = cacheService;
        }


        [HttpGet("BuscarTimes")]
        public async Task<ActionResult<IEnumerable<TimeFutebolDto>>> ObterTimes( )
        {
            var cache = _cacheService.GetData<IEnumerable<TimeFutebolDto>>("Times");

            if(cache != null)
            {
                Console.WriteLine("Cache");
                return Ok(cache);
            }

            var timesFutebol = await _futebolRepository.ObterTimes();

            var timeFutebolDtos = timesFutebol.ConverterTimesFutParaTimesFutDto();

            var expiryTime = DateTimeOffset.Now.AddMinutes(10);
            _cacheService.SetData("Times", timeFutebolDtos, expiryTime);

            return Ok(timeFutebolDtos);
        }

        [HttpGet("BuscarTimeId/{id}")]
        public async Task<ActionResult<TimeFutebolDto>> ObterTime(int id)
        {
            var cache = _cacheService.GetData<TimeFutebolDto>($"Time{id}");

            if (cache != null)
            {
                Console.WriteLine("Cache");
                return Ok(cache);
            }

            var time = await _futebolRepository.ObterTime(id);

            if(time == null)
            {
                return NotFound("Time não encontrado!");
            }

            var timeDto = time.ConverterTimeFutParaTimeFutDto();

            var expiryTime = DateTimeOffset.Now.AddMinutes(1);
            _cacheService.SetData($"Time{id}", timeDto, expiryTime);


            return Ok(timeDto);
        }

        [HttpPut("EditarTime/{id}")]
        public async Task<IActionResult> AtualizarTime(int id, TimeFutebolEditDto editarTime)
        {
            var time = await _futebolRepository.ObterTime(id);
            if (time == null)
                return NotFound("Time não encontrado!");

            var timeEditado = editarTime.ConverterTimeFutEditParaTimeFut(time);

            var timeAtualizado = await _futebolRepository.AtualizarTimeFut(timeEditado);
            var timeCache = timeAtualizado.ConverterTimeFutParaTimeFutDto();
            var expiryTime = DateTimeOffset.Now.AddMinutes(1);


            _cacheService.SetData($"Time{timeAtualizado.Id}", timeCache, expiryTime);

            return Ok("Time Adicionado com Sucesso!");
        }


        [HttpPost("AdicionarTime")]
        public async Task<IActionResult> AdicionarTimeFutebol(TimeFutebolDto timeFutebolDto)
        {   
            var timeExiste = await _futebolRepository.ObterTime(timeFutebolDto.TimeFutebolId);

            if(timeExiste != null)
            {
                BadRequest("Time já existe!");
            }

            var timeFutebol = timeFutebolDto.ConverterTimeFutDtoParaTimeFut();

            var resultado = await _futebolRepository.AddTime(timeFutebol);


            var expiryTime = DateTimeOffset.Now.AddMinutes(1);
            _cacheService.SetData($"Time{timeFutebolDto.TimeFutebolId}", timeFutebolDto, expiryTime);

            return resultado ? Ok("Time Adicionado com Sucesso!") :
                  BadRequest("Erro ao Adicionar o Time!");
        } 
        
        [HttpPost("AdicionarJogadorAoTime")]
        public async Task<IActionResult> AdicionarJogadorAoTime(int IdTime, int IdJogador)
        {   
            var jogadorExiste = await _jogadorRepository.ObterJogador(IdJogador);
            var time = await _futebolRepository.ObterTime(IdTime);
           
            if(jogadorExiste == null || time == null)
            {
                BadRequest("Jogador não existe!");
            }

            var resultado = await _futebolRepository.AddJogadorTime(time, jogadorExiste);

            return resultado ? Ok("Jogador Adicionado ao Time com Sucesso!") :
                  BadRequest("Erro ao Adicionar Jogador!");
        }


        [HttpDelete("ExcluirTimeId/{id}")]
        public async Task<IActionResult> ExcluirTimeFutebol(int id)
        {
            var time = await _futebolRepository.ObterTime(id);
            if (time == null)
                return NotFound();

            var resultado = await _futebolRepository.ExcluirTime(id);
            if(resultado == true)
            {
                var jogadoresAtt = await _jogadorRepository.ObterJogadores();
                var jogadoresAttDto = jogadoresAtt.ConverterJogadoresParaJogadoresDto();
                var timesAtualizados = await  _futebolRepository.ObterTimes();

                _cacheService.RemoveData($"Time{time.Id}");

                var expiryTime = DateTimeOffset.Now.AddMinutes(1);
                foreach(var jogador in time.Jogadores)
                {
                    _cacheService.RemoveData($"Jogador{jogador.Id}");
                }

                _cacheService.SetData($"Jogadores", jogadoresAttDto, expiryTime);
                _cacheService.SetData("Times", timesAtualizados.ConverterTimesFutParaTimesFutDto(), expiryTime);
            }


            return resultado ? Ok("Time Deletado com Sucesso!"): 
                  BadRequest("Erro ao Excluir o Time!");
        }
    }
}
