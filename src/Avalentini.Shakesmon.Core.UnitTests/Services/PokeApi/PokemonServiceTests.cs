using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Services.PokeApi;
using Avalentini.Shakesmon.Core.Services.PokeApi.Dto;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace Avalentini.Shakesmon.Core.UnitTests.Services.PokeApi
{
    public class PokemonServiceTests
    {
        [Fact]
        public async Task GetPokemon_ShouldReturnPokemon_WhenHttpClientReturnsOk()
        {
            // ARRANGE
            var pokemon = new Pokemon {Id = "6"};
            var client = MockHttpClient(HttpStatusCode.OK, pokemon);
            var sut = new PokemonService(client);

            // ACT
            var result = await sut.GetPokemon("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.NotNull(result.Pokemon);
            Assert.True(result.Pokemon.Id == "6");
        }

        [Fact]
        public async Task GetPokemon_ShouldReturnError_WhenHttpClientReturnsError()
        {
            // ARRANGE
            var pokemon = new Pokemon {Id = "6"};
            var client = MockHttpClient(HttpStatusCode.InternalServerError, pokemon);
            var sut = new PokemonService(client);

            // ACT
            var result = await sut.GetPokemon("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.Null(result.Pokemon);
            Assert.True(result.Error.Equals(PokemonService.InvalidResponseError, StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task GetPokemon_ShouldReturnError_WhenNoNameIsProvided()
        {
            // ARRANGE
            var pokemon = new Pokemon {Id = "6"};
            var client = MockHttpClient(HttpStatusCode.InternalServerError, pokemon);
            var sut = new PokemonService(client);

            // ACT
            var result = await sut.GetPokemon("");

            // ASSERT
            Assert.NotNull(result);
            Assert.Null(result.Pokemon);
            Assert.True(result.Error.Equals(PokemonService.EmptyPokemonNameError, StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task GetPokemon_ShouldReturnNotFoundError_WhenPokemonNotFound()
        {
            // ARRANGE
            var pokemon = new Pokemon {Id = "6"};
            var client = MockHttpClient(HttpStatusCode.NotFound, pokemon);
            var sut = new PokemonService(client);

            // ACT
            var result = await sut.GetPokemon("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.Null(result.Pokemon);
            Assert.True(result.Error.Equals(PokemonService.PokemonNotFoundError, StringComparison.InvariantCultureIgnoreCase));
        }

        private static HttpClient MockHttpClient(HttpStatusCode status, Pokemon pokemon)
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = status,
                    Content = new StringContent(JsonConvert.SerializeObject(pokemon)),
                });
            return new HttpClient(handler.Object);
        }
    }
}
