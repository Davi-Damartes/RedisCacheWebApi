using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCaching.Data;
using WebApiCaching.Dtos;
using WebApiCaching.MappingEntity;
using WebApiCaching.Models;
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
                                 AppDbContext context,
                                 ICacheService cacheService)
        {
            _jogadorRepository = jogadorRepository;
            _futebolRepository = futebolRepository;
            _cacheService = cacheService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogadorDto>>> ObterJogadores()
        {

            var cacheData = _cacheService.GetData<IEnumerable<JogadorDto>>("jogadores");

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

            var jogadoresDto = jogadores.Select(j => new JogadorDto
            {
                Id = j.Id,
                Nome = j.Nome,
                NumeroCamisa = j.NumeroCamisa,
                TimeFutebol = j.TimeFutebol == null ? null : new TimeFutebolDto
                {
                    Nome = j.TimeFutebol.Nome,
                    Descricao = j.TimeFutebol.Descricao,
                    Classificacao = j.TimeFutebol.Classificacao
                }
            }).ToList();

            var expiryTime = DateTimeOffset.Now.AddSeconds(15);
            _cacheService.SetData("jogadores", jogadoresDto, expiryTime);
            
            return Ok(jogadoresDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<JogadorDto>> ObterJogador(int id)
        {
            var cacheData = _cacheService.GetData<JogadorDto>($"Jogador{id}");

            if (cacheData != null)
            {
                Console.WriteLine("Cache");
                return Ok(cacheData);
            }

            var jogador = await _jogadorRepository.ObterJogador(id);
            var time = await _futebolRepository.ObterTime(jogador.TimeFutebolId);

            if (jogador == null || time == null)
            {
                return NotFound("Erro ao buscar Jogador!");
            }


            var jogadorDto = jogador.ConverterJogadorParaJogadorDto(time);

            var expiryTime = DateTimeOffset.Now.AddSeconds(15);
            _cacheService.SetData($"Jogador{id}", jogadorDto, expiryTime);

            return Ok(jogadorDto);
        }


        [HttpPost]
        public async Task<IActionResult> AdicionarJogador(JogadorDto jogadorDto)
        {
            if (jogadorDto == null)
            {
                return BadRequest();
            }

            var jogador = jogadorDto.ConverterJogadorETimeDtoParaJogador();

            var sucesso = await _jogadorRepository.AddJogador(jogador);
            var expiryTime = DateTimeOffset.Now.AddSeconds(15);

            _cacheService.SetData($"Jogador{jogadorDto.Id}", jogadorDto, expiryTime);

            return sucesso ?
                Ok("Jogador Adicionado com Sucesso!") :
                BadRequest("Erro ao adicionar Jogador!");
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> ExcluirJogador(int id)
        {
            var result = await _jogadorRepository.ExcluirJogador(id);

            if (result == false)
                return NotFound();

            _cacheService.RemoveData($"Jogador{id}");

            var jogadores = await _jogadorRepository.ObterJogadores();

            var jogadoresDto = jogadores.Select(j => new JogadorDto
            {
                Id = j.Id,
                Nome = j.Nome,
                NumeroCamisa = j.NumeroCamisa,
                TimeFutebol = j.TimeFutebol == null ? null : new TimeFutebolDto
                {
                    Nome = j.TimeFutebol.Nome,
                    Descricao = j.TimeFutebol.Descricao,
                    Classificacao = j.TimeFutebol.Classificacao
                }
            }).ToList();

            var expiryTime = DateTimeOffset.Now.AddSeconds(15);
            _cacheService.SetData("jogadores", jogadoresDto, expiryTime);

            return Ok("Jogador Excluido com Sucesso!");
        }
    }
}
