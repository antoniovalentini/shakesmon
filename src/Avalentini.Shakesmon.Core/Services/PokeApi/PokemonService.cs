using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Common.Cache;
using Avalentini.Shakesmon.Core.Services.PokeApi.Dto;
using Newtonsoft.Json;

namespace Avalentini.Shakesmon.Core.Services.PokeApi
{
    public interface IPokemonService
    {
        Task<GetPokemonResponse> GetPokemon(string name);
        Task<GetSpeciesResponse> GetSpecies(string id);
    }

    public class PokemonService : IPokemonService
    {
        public const string PokemonInvalidResponseError = "Error while fetching pokemon information.";
        public const string PokemonNotFoundError = "Unable to find the desired pokemon.";
        public const string EmptyPokemonNameError = "Please provide a pokemon name.";

        public const string EmptyPokemonIdError = "Please provide a pokemon id.";
        public const string SpeciesInvalidResponseError = "Error while fetching species.";
        public const string SpeciesNotFoundError = "Unable to find the desired species.";

        private readonly HttpClient _client;
        private readonly ICache _cache;
        private const string PokeApiBaseUrl = "https://pokeapi.co/api/v2/";
        private const string PokeApiPokemonFeature = "pokemon";
        private const string PokeApiSpeciesFeature = "pokemon-species";

        public PokemonService(HttpClient client, ICache cache)
        {
            _client = client;
            _cache = cache;
            _client.BaseAddress = new Uri(PokeApiBaseUrl);
        }

        public async Task<GetPokemonResponse> GetPokemon(string name)
        {
            if (string.IsNullOrEmpty(name))
                return new GetPokemonResponse{Error = EmptyPokemonNameError};

            var key = ComputePokemonCacheKey(name);
            var cached = _cache.Get<Pokemon>(key);
            if (cached != null)
                return new GetPokemonResponse{Pokemon = cached};

            var response = await _client.GetAsync($"{PokeApiPokemonFeature}/{name}");
            if (!response.IsSuccessStatusCode)
                return response.StatusCode switch
                {
                    HttpStatusCode.NotFound => new GetPokemonResponse{Error = PokemonNotFoundError},
                    _ => new GetPokemonResponse{Error = PokemonInvalidResponseError},
                };

            var pokemon = JsonConvert.DeserializeObject<Pokemon>(await response.Content.ReadAsStringAsync());
            _cache.Set(key, pokemon);

            return new GetPokemonResponse{Pokemon = pokemon};
        }

        public async Task<GetSpeciesResponse> GetSpecies(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new GetSpeciesResponse{Error = EmptyPokemonIdError};

            var key = ComputeSpeciesCacheKey(id);
            var cached = _cache.Get<Species>(key);
            if (cached != null)
                return new GetSpeciesResponse{FlavorTextEntry = cached.GetFlavorByLanguage("en")};

            var response = await _client.GetAsync($"{PokeApiSpeciesFeature}/{id}");
            // TODO: handle requests limit reached
            if (!response.IsSuccessStatusCode)
                return response.StatusCode switch
                {
                    HttpStatusCode.NotFound => new GetSpeciesResponse{Error = SpeciesNotFoundError},
                    _ => new GetSpeciesResponse {Error = SpeciesInvalidResponseError},
                };

            var species = JsonConvert.DeserializeObject<Species>(await response.Content.ReadAsStringAsync());
            _cache.Set(key, species);

            return new GetSpeciesResponse {FlavorTextEntry = species.GetFlavorByLanguage("en")};
        }

        private static string ComputePokemonCacheKey(string pokemonName)
        {
            return $"pokemon_{pokemonName}";
        }

        private static string ComputeSpeciesCacheKey(string id)
        {
            return $"species_{id}";
        }
    }
}
