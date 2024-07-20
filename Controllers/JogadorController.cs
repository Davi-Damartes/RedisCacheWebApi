using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCaching.Data;
using WebApiCaching.Models;


namespace WebApiCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogadorController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public JogadorController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        [HttpGet]
        public async Task<ActionResult<Jogador>> ObterJogadores( )
        {
            var result = await _appDbContext.Jogadores.ToListAsync();

            if ( result == null)
                return NotFound();

            return Ok(result);
        }

  
        [HttpGet("{id}")]
        public async Task<ActionResult<Jogador>> ObterJogador (int id)
        {
            var result = await _appDbContext.Jogadores.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

    
        [HttpPost]
        public async Task<IActionResult> AdicionarJogador([FromBody] Jogador jogador)
        {
            if(jogador == null)
            {
                return BadRequest();
            }

            await _appDbContext.Jogadores.AddAsync(jogador);
            await _appDbContext.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> ExcluirJogador(int id)
        {
            var result = await _appDbContext.Jogadores.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                return NotFound();

            await _appDbContext.Jogadores.Where(x => x.Id == id).ExecuteDeleteAsync();
            await _appDbContext.SaveChangesAsync();
            return Ok(result);
        }
    }
}
