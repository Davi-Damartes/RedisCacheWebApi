using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCaching.Data;
using WebApiCaching.Dtos;
using WebApiCaching.Models;


namespace WebApiCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogadorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JogadorController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }


        [HttpGet]
        public async Task<ActionResult<Jogador>> ObterJogadores( )
        {
            var jogadores = await _context
                               .Jogadores
                               .Include(a => a.TimeFutebol)
                               .ToListAsync();

            if (jogadores == null)
                return NotFound();

            var jogadoresDto = jogadores.Select(j => new JogadorDto
            {
                Id = j.Id,
                Nome = j.Nome,
                NumeroCamisa = j.NumeroCamisa,
                TimeFutebol = j.TimeFutebol == null ? null! : new TimeFutebolDto
                {
                    Nome = j.TimeFutebol.Nome,
                    Descricao = j.TimeFutebol.Descricao,
                    Classificacao = j.TimeFutebol.Classificacao
                }}
            ).ToList();


            return Ok(jogadoresDto);
        }

  
        [HttpGet("{id}")]
        public async Task<ActionResult<JogadorDto>> ObterJogador (int id)
        {
            var jogador = await _context
                               .Jogadores
                               .Include(a => a.TimeFutebol)
                               .FirstOrDefaultAsync(x => x.Id == id);

            if(jogador == null || jogador.TimeFutebol == null)
            {
                return NotFound("Jogador não encontrado!");
            }

            var jogadorDTO = new JogadorDto
            {
                Id = jogador.Id,
                Nome = jogador.Nome,
                NumeroCamisa = jogador.NumeroCamisa,
                TimeFutebol = new TimeFutebolDto
                {
                    Nome = jogador.TimeFutebol.Nome,
                    Descricao = jogador.TimeFutebol.Descricao,
                    Classificacao = jogador.TimeFutebol.Classificacao
                }
            };


            return Ok(jogadorDTO);
        }

    
        [HttpPost]
        public async Task<IActionResult> AdicionarJogador(JogadorDto jogadorDto)
        {
            if(jogadorDto == null)
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


            await _context.Jogadores.AddAsync(jogador);
            await _context.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> ExcluirJogador(int id)
        {
            var result = await _context.Jogadores.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                return NotFound();

            await _context.Jogadores.Where(x => x.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return Ok(result);
        }
    }
}
