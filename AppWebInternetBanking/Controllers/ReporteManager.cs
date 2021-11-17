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
    public class ReporteManager
    {
        string UrlBase = "http://localhost:49220/api/Reportes/";
        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<Reporte> ObtenerReporte(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Reporte>(response);
        }

        public async Task<IEnumerable<Reporte>> ObtenerReportes(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Reporte>>(response);
        }
        public async Task<Reporte> Ingresar(Reporte reporte, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(reporte), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Reporte>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Reporte> Actualizar(Reporte reporte, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(reporte), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Reporte>(await response.Content.ReadAsStringAsync());
        }
        public async Task<Reporte> Eliminar(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Reporte>(await response.Content.ReadAsStringAsync());
        }
    }
}