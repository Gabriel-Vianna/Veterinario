using System;
using Newtonsoft.Json;

namespace Veterinario.Core
{
    public class Pet
    {
        [JsonProperty(PropertyName = "id")]
        public String Id { get; set; }

        [JsonProperty(PropertyName = "nome")]
        public String Nome { get; set; }

        [JsonProperty(PropertyName = "raca")]
        public string Raca { get; set; }

        [JsonProperty(PropertyName = "dono")]
        public string Dono { get; set; }

        [JsonProperty(PropertyName = "dataAgendamento")]
        public string DataAgendamento { get; set; }

        [JsonProperty(PropertyName = "telefone")]
        public string Telefone { get; set; }

        [JsonProperty(PropertyName = "pk")]
        public string PartitionKey { get; set; } = "pet";
    }
}
