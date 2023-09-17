using CarMake.Core.Filters;
using CarMake.IService;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CarMake.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarMakeController : ControllerBase
    {
        private readonly ICarMakeService carMakeService;
        public CarMakeController(ICarMakeService carMakeService)
        {
            this.carMakeService= carMakeService;
        }

        [HttpGet(Name = "GetCarMake")]
        public async Task<IActionResult> Get([FromQuery] CarFilter filter)
        {
            return Ok(await carMakeService.GetCars(filter));
        }
    }
}
