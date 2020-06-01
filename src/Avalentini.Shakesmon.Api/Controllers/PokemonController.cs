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
            var pokeResult = await _pokemonService.GetPokemon(name);
            // TODO: replace with 500?
            if (!pokeResult.IsSuccess)
                return BadRequest(pokeResult.Error);

            var speciesResult = await _pokemonService.GetSpecies(pokeResult.Pokemon.Id);
            // TODO: replace with 500?
            if (!speciesResult.IsSuccess)
                return BadRequest(speciesResult.Error);

            var translationResult = await _shakespeareService.Translate(speciesResult.FlavorTextEntry.FlavorText);
            // TODO: replace with 500?
            if (!translationResult.IsSuccess)
                return BadRequest(translationResult.Error);

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
