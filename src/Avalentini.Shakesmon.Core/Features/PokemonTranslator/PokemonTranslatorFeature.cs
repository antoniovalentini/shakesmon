using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Common;
using Avalentini.Shakesmon.Core.Services.FunTranslations;
using Avalentini.Shakesmon.Core.Services.PokeApi;

namespace Avalentini.Shakesmon.Core.Features.PokemonTranslator
{
    public class PokemonTranslatorFeature
    {
        private readonly IPokemonService _pokemonService;
        private readonly IShakespeareService _shakespeareService;

        public PokemonTranslatorFeature(IPokemonService pokemonService, IShakespeareService shakespeareService)
        {
            _pokemonService = pokemonService;
            _shakespeareService = shakespeareService;
        }

        public async Task<ExecuteResult> ExecuteAsync(string name)
        {
            var pokeResult = await _pokemonService.GetPokemon(name);
            if (!pokeResult.IsSuccess)
                return Error(pokeResult.Error);

            var speciesResult = await _pokemonService.GetSpecies(pokeResult.Pokemon.Id);
            if (!speciesResult.IsSuccess)
                return Error(speciesResult.Error);

            var translationResult = await _shakespeareService.Translate(speciesResult.FlavorTextEntry.FlavorText);
            if (!translationResult.IsSuccess)
                return Error(translationResult.Error);

            return new ExecuteResult {Description = translationResult.Translation.Contents.Translated};
        }

        private static ExecuteResult Error(string message)
        {
            return new ExecuteResult{Error = message};
        }
    }

    public class ExecuteResult : BaseResponse
    {
        public string Description { get; set; }
    }
}
