using CurrencyAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CurrencyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private static readonly HttpClient _httpClient = new();

        private static async Task<Dictionary<string, ValuteResponce>> TryGetCbrValues()
        {
            HttpResponseMessage responce =
                await _httpClient.GetAsync("https://www.cbr-xml-daily.ru/daily_json.js");

            if (!responce.IsSuccessStatusCode)
                return new Dictionary<string, ValuteResponce>();

            string content = await responce.Content.ReadAsStringAsync();
            ValutesResponce valutesResponce =
                JsonConvert.DeserializeObject<ValutesResponce>(content);

            return valutesResponce.Valutes;
        }

        [HttpGet]
        [Route("/currencies")]
        public async Task<ActionResult> GetValutes(int page, int pageSize)
        {
            Dictionary<string, ValuteResponce> valutes =
                await TryGetCbrValues();

            if (page < 0 || pageSize < 0)
                return BadRequest("Page or PageSize cannot be less than zero");
            
            if (page == 0 && pageSize == 0)
                return Ok(valutes);

            return Ok(valutes.Skip(pageSize * page).Take(pageSize));
        }

        [HttpGet]
        [Route("/currency")]
        public async Task<ActionResult> GetValute(string name)
        {
            Dictionary<string, ValuteResponce> valutes =
                await TryGetCbrValues();

            if (!valutes.ContainsKey(name))
                return BadRequest("Requested currency doesn't exists.");

            return Ok(valutes[name]);
        }
    }
}