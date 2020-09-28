using System.Text.Json.Serialization;

namespace EfCore_Produtos.Utils
{
    public class MoedaInfo
    {

        [JsonPropertyName("high")]
        public string Valor { get; set; }

    }
}