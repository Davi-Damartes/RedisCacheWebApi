using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCaching.Data;
using WebApiCaching.Dtos;
using WebApiCaching.Models;
using WebApiCaching.Repository;
using WebApiCaching.Service;


namespace WebApiCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogadorController : ControllerBase
    {
        private readonly IJogadorRepository _jogadorRepository;
        private readonly AppDbContext _context;
        private readonly ICacheService _cacheService;

        public JogadorController(IJogadorRepository jogadorRepository,
                                 AppDbContext context,
                                 ICacheService cacheService)
        {
            _jogadorRepository = jogadorRepository;
            _context = context;
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

            var expiryTime = DateTimeOffset.Now.AddSeconds(60);
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

            if (jogador == null)
            {
                return NotFound("Jogador não encontrado!");
            }

            var jogadorDto = new JogadorDto
            {
                Id = jogador.Id,
                Nome = jogador.Nome,
                NumeroCamisa = jogador.NumeroCamisa,
                TimeFutebol = jogador.TimeFutebol == null ? null : new TimeFutebolDto
                {
                    Nome = jogador.TimeFutebol.Nome,
                    Descricao = jogador.TimeFutebol.Descricao,
                    Classificacao = jogador.TimeFutebol.Classificacao
                }
            };

            var expiryTime = DateTimeOffset.Now.AddMinutes(60);
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

            var timeFutebol = await _context.TimeFutebols
                            .FirstOrDefaultAsync(x => x.TimeFutebolId == jogadorDto.TimeFutebolId);

            if (timeFutebol == null)
            {
                return NotFound("Time de futebol não encontrado.");
            }

            var jogador = new Jogador
            {
                Id = jogadorDto.Id,
                Nome = jogadorDto.Nome,
                NumeroCamisa = jogadorDto.NumeroCamisa,
                TimeFutebolId = jogadorDto.TimeFutebolId,
                TimeFutebol = timeFutebol
            };

            var sucesso = await _jogadorRepository.AddJogador(jogador);
            var expiryTime = DateTimeOffset.Now.AddSeconds(60);

            _cacheService.SetData<JogadorDto>($"Jogador{jogadorDto.Id}", jogadorDto, expiryTime);

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
            return Ok("Jogador Excluido com Sucesso!");
        }
    }
}
