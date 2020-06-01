using System.Collections.Generic;
using Newtonsoft.Json;

namespace Avalentini.Shakesmon.Core.Services.PokeApi.Dto
{
    public class Species
    {
        [JsonProperty("flavor_text_entries")]
        public List<FlavorTextEntry> FlavorTextEntries { get; set; }
    }
}
