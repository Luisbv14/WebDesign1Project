using AppWebInternetBanking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppWebInternetBanking.Controllers
{

    public class SinpeMovilManager
    {
        string UrlBase = "http://localhost:49220/api/SinpeMoviles/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<SinpeMovil> ObtenerSinpeMovil(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<SinpeMovil>(response);
        }
        public async Task<IEnumerable<SinpeMovil>> ObtenerSinpeMoviles(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<SinpeMovil>>(response);
        }

        public async Task<SinpeMovil> Ingresar(SinpeMovil sinpeMovil, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(sinpeMovil), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<SinpeMovil>(await response.Content.ReadAsStringAsync());
        }
        public async Task<SinpeMovil> Actualizar(SinpeMovil sinpeMovil, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(sinpeMovil), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<SinpeMovil>(await response.Content.ReadAsStringAsync());
        }
        public async Task<SinpeMovil> Eliminar(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<SinpeMovil>(await response.Content.ReadAsStringAsync());
        }
    }
}