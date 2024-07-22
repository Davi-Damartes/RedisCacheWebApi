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
    public class TimeFutebolController : ControllerBase
    {
        private readonly ITimeFutebolRepository _futebolRepository;
        private readonly ICacheService _cacheService;
        public TimeFutebolController(ITimeFutebolRepository futebolRepository,
                                     ICacheService cacheService)
        {
            _futebolRepository = futebolRepository;
            _cacheService = cacheService;
        }


        [HttpGet("TimesFutebol")]
        public async Task<ActionResult<IEnumerable<TimeFutebolDto>>> ObterTimes( )
        {
            var cache = _cacheService.GetData<IEnumerable<TimeFutebolDto>>("Times");

            if(cache != null)
            {
                Console.WriteLine("Cache");
                return Ok(cache);
            }

            var timeFutebolList = await _futebolRepository.ObterTimes();

            var timeFutebolDtos = timeFutebolList.Select(t => new TimeFutebolDto
            {
                Nome = t.Nome,
                Descricao = t.Descricao,
                Classificacao = t.Classificacao
            }).ToList();

            var expiryTime = DateTimeOffset.Now.AddSeconds(10);
            _cacheService.SetData("Times", timeFutebolDtos, expiryTime);

            return Ok(timeFutebolDtos);
        }

        [HttpGet("TimeFutebol/{id}")]
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
            var timeDto = new TimeFutebolDto
            {
                Nome = time.Nome,
                Classificacao = time.Classificacao,
                Descricao = time.Descricao,
            };

            var expiryTime = DateTimeOffset.Now.AddSeconds(10);
            _cacheService.SetData($"Time{id}", timeDto, expiryTime);


            return Ok(timeDto);
        } 
        
        //[HttpPut("{id}")]
        //public async Task<ActionResult<TimeFutebol>> AtualizarTime(int id, TimeFutebol novoTime)
        //{
        //    var time = await _futebolRepository.ObterTime(id);

        //    if (time == null)
        //        return NotFound();

        //    time.Nome = novoTime.Nome;
        //    time.Descricao = novoTime.Descricao;
        //    time.Classificacao = novoTime.Classificacao;
        //    time.Jogadores = novoTime.Jogadores;


        //     _context.TimeFutebols.Update(time);
        //    await _context.SaveChangesAsync();
        //    return Ok(time);
        //}


        //[HttpPost]
        //public async Task<IActionResult> AdicionarTimeFutebol([FromBody] TimeFutebol timeFutebol)
        //{
        //    if (timeFutebol == null)
        //    {
        //        return BadRequest();
        //    }

        //    await _context.TimeFutebols.AddAsync(timeFutebol);
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}


        //[HttpDelete("{id}")]
        //public async Task<IActionResult> ExcluirTimeFutebol(int id)
        //{
        //    var time = await _context.TimeFutebols.FirstOrDefaultAsync(x => x.TimeFutebolId == id);
        //    if (time == null)
        //        return NotFound();

        //    await _context.TimeFutebols.Where(x => x.TimeFutebolId == id).ExecuteDeleteAsync();
        //    await _context.SaveChangesAsync();

        //    return Ok("Produto deletado com Sucesso!");
        //}
    }
}
