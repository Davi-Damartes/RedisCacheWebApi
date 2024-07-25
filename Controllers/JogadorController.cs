using Microsoft.AspNetCore.Mvc;
using WebApiCaching.Dtos;
using WebApiCaching.MappingEntity;
using WebApiCaching.Repository.JogadorRepositories;
using WebApiCaching.Repository.TimeFutRepositories;
using WebApiCaching.Service;


namespace WebApiCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogadorController : ControllerBase
    {
        private readonly IJogadorRepository _jogadorRepository;
        private readonly ITimeFutebolRepository _futebolRepository;
        private readonly ICacheService _cacheService;

        public JogadorController(IJogadorRepository jogadorRepository,
                                 ITimeFutebolRepository futebolRepository,
                                 ICacheService cacheService)
        {
            _jogadorRepository = jogadorRepository;
            _futebolRepository = futebolRepository;
            _cacheService = cacheService;
        }


        [HttpGet("BuscarJogadores")]
        public async Task<ActionResult<IEnumerable<JogadorDto>>> ObterJogadores()
        {

            var cacheData = _cacheService.GetData<IEnumerable<JogadorDto>>("Jogadores");

            if (cacheData != null)
            {
                Console.WriteLine("Cache");
                return Ok(cacheData);
            }

            var jogadores = await _jogadorRepository.ObterJogadores();

            if (jogadores == null)
            {
                return NotFound();
            }

            var jogadoresDto = jogadores.ConverterJogadoresParaJogadoresDto();

            var expiryTime = DateTimeOffset.Now.AddSeconds(10);
            _cacheService.SetData("Jogadores", jogadoresDto, expiryTime);
            
            return Ok(jogadoresDto);
        }


        [HttpGet("BuscarJogadorId/{id}")]
        public async Task<ActionResult<JogadorDto>> ObterJogador(int id)
        {
            var cacheData = _cacheService.GetData<JogadorDto>($"Jogador{id}");

            if (cacheData != null)
            {
                Console.WriteLine("Cache");
                return Ok(cacheData);
            }

            var jogador = await _jogadorRepository.ObterJogador(id);

            if (jogador == null)
            {
                return NotFound("Jogador não encontrado!");
            }


            var jogadorDto = jogador.ConverterJogadorParaJogadorDto();

            var expiryTime = DateTimeOffset.Now.AddSeconds(10);
            _cacheService.SetData($"Jogador{id}", jogadorDto, expiryTime);

            return Ok(jogadorDto);
        }


        [HttpPost("AdicionarJogador")]
        public async Task<IActionResult> AdicionarJogador(JogadorAddicionarDto jogadorAddicionarDto)
        {
            var time = await _futebolRepository.ObterTime(jogadorAddicionarDto.TimeFutebolId);

            if (time == null)
                return NotFound("Time não encontrado");

            var jogador = jogadorAddicionarDto.ConverterJogadorAddDtoParaJogador();

            var sucesso = await _jogadorRepository.AddJogador(jogador);

            if(sucesso == true)
            {

                var jogadores = await _jogadorRepository.ObterJogadores();
                var jogadoresDto = jogadores.ConverterJogadoresParaJogadoresDto();

                var expiryTime = DateTimeOffset.Now.AddSeconds(10);
                _cacheService.SetData($"Jogador{jogadorAddicionarDto.Id}", jogadorAddicionarDto, expiryTime);
                _cacheService.SetData("Jogadores", jogadoresDto, expiryTime);
            }

            return sucesso ?
                Ok("Jogador Adicionado com Sucesso!") :
                BadRequest("Erro ao adicionar Jogador!");
        }


        [HttpDelete("DeletarJogadorId/{id}")]
        public async Task<ActionResult> ExcluirJogador(int id)
        {

            var jogador = await _jogadorRepository.ObterJogador(id);
            if(jogador == null)
            {
                return NotFound("Jogador não encontrado!");
            }

            var result = await _jogadorRepository.ExcluirJogador(jogador);

            if (result == true)
            {
                _cacheService.RemoveData($"Jogador{id}");

                var jogadores = await _jogadorRepository.ObterJogadores();

                var jogadoresDto = jogadores.ConverterJogadoresParaJogadoresDto();

                var expiryTime = DateTimeOffset.Now.AddSeconds(10);
                _cacheService.SetData("Jogadores", jogadoresDto, expiryTime);
            }

            return result ? Ok("Jogador Excluído com Sucesso!") :
                             BadRequest("Erro ao Excluir Jogador!");
            
        }
    }
}
