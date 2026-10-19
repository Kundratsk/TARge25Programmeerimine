using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;

namespace TARge25shop.ApplicationServices.Services
{
    public class ChuckNorrisServices : IChuckNorrisServices
    {
        private readonly HttpClient _httpClient;

        public ChuckNorrisServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ChuckNorrisRootDto> ChuckNorrisResultHttpClient()
        {
            //var respone on vaja ühendada https://api.chucknorris.io/jokes/random
            var response = await _httpClient.GetAsync("https://api.chucknorris.io/jokes/random");
            //annab veateate, kui response ei ole edukas
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    //olek true laseb ignoreerida täpset tähe suurustundlikust json muutujate nimes
                    PropertyNameCaseInsensitive = true
                };
                //muutuja var joke tuleb deserialiseerida
                var joke = JsonSerializer.Deserialize<ChuckNorrisRootDto>(json, options);

                //null andestav operaator (!) on turvaline viis kuna API alati tagastab JSON
                return joke!;
            }

            return null;
        }
    }
}