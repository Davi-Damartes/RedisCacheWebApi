using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCaching.Data;
using WebApiCaching.Models;


namespace WebApiCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeFutebolController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TimeFutebolController(AppDbContext context)
        {
            _context = context;
        }

      
        [HttpGet]
        public async Task<ActionResult<List<TimeFutebol>>> ObterTimes( )
        {
            var times = await _context.TimeFutebols.ToListAsync();

            if( times == null )
                return NotFound();

            return Ok(times);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TimeFutebol>> ObterTime(int id)
        {
            var time = await _context.TimeFutebols.FirstOrDefaultAsync(x => x.TimeFutebolId == id);

            if (time == null)
                return NotFound();

            return Ok(time);
        }


        [HttpPost]
        public async Task<IActionResult> AdicionarTimeFutebol([FromBody] TimeFutebol timeFutebol)
        {
            if (timeFutebol == null)
            {
                return BadRequest();
            }

            await _context.TimeFutebols.AddAsync(timeFutebol);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirTimeFutebol(int id)
        {
            var time = await _context.TimeFutebols.FirstOrDefaultAsync(x => x.TimeFutebolId == id);
            if (time == null)
                return NotFound();

            await _context.TimeFutebols.Where(x => x.TimeFutebolId == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();

            return Ok("Produto deletado com Sucesso!");
        }
    }
}
