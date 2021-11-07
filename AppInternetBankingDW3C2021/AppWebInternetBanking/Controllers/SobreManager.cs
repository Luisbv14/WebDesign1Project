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
    public class SobreManager
    {
        string UrlBase = "http://localhost:49220/api/Sobres/";
        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<Sobre> ObtenerSobre(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Sobre>(response);
        }

        public async Task<IEnumerable<Sobre>> ObtenerSobres(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Sobre>>(response);
        }

        public async Task<Sobre> Ingresar(Sobre sobre, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(sobre), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Sobre>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Sobre> Actualizar(Sobre sobre, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(sobre), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Sobre>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Sobre> Eliminar(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Sobre>(await response.Content.ReadAsStringAsync());
        }
    }
}