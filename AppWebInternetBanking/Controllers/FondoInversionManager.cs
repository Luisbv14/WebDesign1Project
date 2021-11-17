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
    public class FondoInversionManager
    {
        string UrlBase = "http://localhost:49220/api/FondoInversiones/";
        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }
        public async Task<FondoInversion> ObtenerFonfoInversion(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<FondoInversion>(response);
        }

        public async Task<IEnumerable<FondoInversion>> ObtenerFondoInversiones(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<FondoInversion>>(response);
        }
        public async Task<FondoInversion> Ingresar(FondoInversion fondoInversion, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(fondoInversion), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<FondoInversion>(await response.Content.ReadAsStringAsync());
        }

        public async Task<FondoInversion> Actualizar(FondoInversion fondoInversion, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(fondoInversion), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<FondoInversion>(await response.Content.ReadAsStringAsync());
        }
        public async Task<FondoInversion> Eliminar(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<FondoInversion>(await response.Content.ReadAsStringAsync());
        }
    }
}