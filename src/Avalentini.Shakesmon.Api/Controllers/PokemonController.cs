using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Features.PokemonTranslator;

namespace Avalentini.Shakesmon.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly PokemonTranslatorFeature _feature;

        public PokemonController(PokemonTranslatorFeature feature)
        {
            _feature = feature;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Please, provide a pokemon name.");
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            // TODO: make language customizable?
            if (string.IsNullOrEmpty(name))
                return BadRequest("Pokemon name cannot be empty. Don't know what to choose? Try 'charizard'");

            var result = await _feature.ExecuteAsync(name);

            if (!result.IsSuccess)
                return StatusCode(500, result.Error);

            return Ok(new GetDto
            {
                Name = name,
                Description = result.Description,
            });
        }
    }

    public class GetDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
