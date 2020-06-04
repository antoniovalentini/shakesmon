using System.Threading.Tasks;
using Avalentini.Shakesmon.Api.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Avalentini.Shakesmon.Api.IntegrationTests.Controllers
{
    public class PokemonControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;
        private readonly string _pokemonGetUrl = $"api/pokemon/{PokemonName}";
        private const string PokemonName = "charizard";

        public PokemonControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact]
        public void Simple()
        {
            _output.WriteLine("simple integration test");
            //// Arrange
            //var client = _factory.CreateClient();

            //// Act
            //var response = await client.GetAsync(_pokemonGetUrl);

            //// Assert
            //response.EnsureSuccessStatusCode();
            //var dto = JsonConvert.DeserializeObject<GetDto>(await response.Content.ReadAsStringAsync());
            //Assert.True(dto.Name.Equals(PokemonName));
            //_output.WriteLine(dto.Description);
        }
    }
}
