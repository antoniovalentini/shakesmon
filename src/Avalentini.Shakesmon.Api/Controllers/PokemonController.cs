using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Services.FunTranslations;
using Avalentini.Shakesmon.Core.Services.PokeApi;

namespace Avalentini.Shakesmon.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        private readonly IShakespeareService _shakespeareService;

        public PokemonController(IPokemonService pokemonService, IShakespeareService shakespeareService)
        {
            _pokemonService = pokemonService;
            _shakespeareService = shakespeareService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            // TODO: make language customizable?
            if (string.IsNullOrEmpty(name))
                return BadRequest("Pokemon name cannot be empty. Don't know what to choose? Try 'charizard'");

            var pokeResult = await _pokemonService.GetPokemon(name);
            if (!pokeResult.IsSuccess)
                return StatusCode(500, pokeResult.Error);

            var speciesResult = await _pokemonService.GetSpecies(pokeResult.Pokemon.Id);
            if (!speciesResult.IsSuccess)
                return StatusCode(500, speciesResult.Error);

            var translationResult = await _shakespeareService.Translate(speciesResult.FlavorTextEntry.FlavorText);
            if (!translationResult.IsSuccess)
                return StatusCode(500, translationResult.Error);

            return Ok(new GetDto
            {
                Name = name,
                Description = translationResult.Translation.Contents.Translated,
            });
        }
    }

    public class GetDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
