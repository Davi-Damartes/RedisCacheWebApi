//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebApiCaching.Data;
//using WebApiCaching.Models;
//using WebApiCaching.Service;

//namespace WebApiCaching.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DriversController : ControllerBase
//    {
//        private readonly ICacheService _cacheService;
//        private readonly ILogger<DriversController> _logger;
//        private readonly AppDbContext _context;

//        public DriversController(ICacheService cacheService,
//                                 ILogger<DriversController> logger,
//                                 AppDbContext context)
//        {
//            _cacheService = cacheService;
//            _logger = logger;
//            _context = context;
//        }

//        [HttpGet("drivers")]
//        public async Task<IActionResult> ObterUsers( )
//        {
//            var cacheData = _cacheService.GetData<IEnumerable<Driver>>("drivers");

//            if(cacheData != null && cacheData.Count() > 0)
//            {
//                Console.WriteLine("Cache!");
//                return Ok(cacheData);
//            }

//            cacheData = await _context.Drivers.ToListAsync();

//            var expiryTime = DateTimeOffset.Now.AddSeconds(10);
//            _cacheService.SetData("drivers", cacheData, expiryTime);
//            Console.WriteLine("Banco de Dados");
//            return Ok(cacheData);

//        } 
        
//        [HttpGet("drivers{id:int}")]
//        public async Task<IActionResult> ObterUsers(int id)
//        {
//            var cacheData = _cacheService.GetData<Driver>("drivers");

//            if(cacheData != null)
//            {
//                Console.WriteLine("Cache!");
//                return Ok(cacheData);
//            }

//            cacheData = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == id);

//            var expiryTime = DateTimeOffset.Now.AddSeconds(10);
//            _cacheService.SetData("drivers", cacheData, expiryTime);
//            Console.WriteLine("Banco de Dados");
//            return Ok(cacheData);

//        }

//        [HttpPost("AddDriver")]
//        public async Task<IActionResult> AddDriver(Driver driver)    
//        {
//            var addObej = await _context.Drivers.AddAsync(driver);
//            var expiryTime = DateTimeOffset.Now.AddSeconds(30);

//            _cacheService.SetData<Driver>($"driver{driver.Id}", addObej.Entity, expiryTime);

//            await _context.SaveChangesAsync();

//            return Ok(addObej.Entity);
//        }

//        [HttpDelete("DeleteDriver")]
//        public async Task<IActionResult> DeleteDriver(int id)
//        {
//            var driver = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == id);

//            if (driver != null)
//            {
//                _context.Remove(driver);
//                _cacheService.RemoveData($"driver{id}");
//                await _context.SaveChangesAsync();

//                return NoContent();
//            }

//            return NotFound("Driver not exist!");
//        }
//    }
//}
