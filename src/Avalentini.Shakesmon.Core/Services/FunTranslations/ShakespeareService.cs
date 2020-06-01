using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Services.FunTranslations.Dto;
using Newtonsoft.Json;

namespace Avalentini.Shakesmon.Core.Services.FunTranslations
{
    public interface IShakespeareService
    {
        Task<TranslateResponse> Translate(string text);
    }

    public class ShakespeareService : IShakespeareService
    {
        private readonly HttpClient _client;
        private const string ShakespeareApiBaseUrl = "https://api.funtranslations.com/translate/";
        private const string ShakespeareRequestUri = "shakespeare.json";

        public ShakespeareService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(ShakespeareApiBaseUrl);
        }

        public async Task<TranslateResponse> Translate(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new TranslateResponse{Error = $"{nameof(text)} is empty."};

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("text", text)
            });
            var shakeResponse = await _client.PostAsync(ShakespeareRequestUri, content);
            if (!shakeResponse.IsSuccessStatusCode)
                return new TranslateResponse{Error = "Unable to translate text."};
            var translation = JsonConvert.DeserializeObject<Translation>(await shakeResponse.Content.ReadAsStringAsync());
            return new TranslateResponse {Translation = translation};
        }
    }
}
