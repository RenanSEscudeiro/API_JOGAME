﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EfCore_Produtos.Utils
{
    public class Moeda
    {

        // Elementos cruciais para uma comunicação API
        // EdnPoint - URL
        //Métodos - GET / POST / PUT / DELETE
        // Status Code
        // Header
        // Body

        [JsonPropertyName("USD")]
        public MoedaInfo MoedaInfo { get; set; }

        public float GetDolarValue()
        {
            float dolarHoje = 1;

            using (var client = new HttpClient())
            {
                // URL
                client.BaseAddress = new System.Uri("https://economia.awesomeapi.com.br/");
                // Headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Response                          // GET (action)
                HttpResponseMessage response = client.GetAsync("all/USD-BRL").Result;

                // Status Code
                if (response.IsSuccessStatusCode)
                {
                    // Body
                    string cotacao = response.Content.ReadAsStringAsync().Result;
                    Moeda convertido = JsonSerializer.Deserialize<Moeda>(cotacao);

                    // Conversão para float 
                    dolarHoje = float.Parse(convertido.MoedaInfo.Valor.Replace('.', ','));
                }
            }

            return dolarHoje;
        }

    }
}
