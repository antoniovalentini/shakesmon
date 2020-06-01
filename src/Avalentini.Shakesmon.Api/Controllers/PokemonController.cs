using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Avalentini.Shakesmon.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            await Task.CompletedTask;
            return Ok(new GetDto
            {
                Name = name,
                Description = "some description",
            });
        }
    }

    public class GetDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
