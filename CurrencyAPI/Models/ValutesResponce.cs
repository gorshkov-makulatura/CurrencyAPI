using Newtonsoft.Json;

namespace CurrencyAPI.Models
{
    public class ValutesResponce
    {
        [JsonProperty("Valute")]
        public Dictionary<string, ValuteResponce> Valutes { get; set; }
    }
}
