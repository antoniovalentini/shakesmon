using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Services.PokeApi;

namespace Avalentini.Shakesmon.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var pokeResult = await _pokemonService.GetPokemon(name);
            // TODO: replace with 500?
            if (!pokeResult.IsSuccess)
                return BadRequest(pokeResult.Error);

            var speciesResult = await _pokemonService.GetSpecies(pokeResult.Pokemon.Id);
            // TODO: replace with 500?
            if (!speciesResult.IsSuccess)
                return BadRequest(speciesResult.Error);

            return Ok(new GetDto
            {
                Name = name,
                Description = speciesResult.FlavorTextEntry.FlavorText,
            });
        }
    }

    public class GetDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
