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
    public class SolicitudTarjetaManager
    {
        string UrlBase = "http://localhost:49220/api/SolicitudTarjetas/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }
        public async Task<SolicitudTarjeta> ObtenerSolicitudTarjeta(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<SolicitudTarjeta>(response);
        }
        public async Task<IEnumerable<SolicitudTarjeta>> ObtenerSolicitudTarjetas(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<SolicitudTarjeta>>(response);
        }
        public async Task<SolicitudTarjeta> Ingresar(SolicitudTarjeta solicitudTarjeta, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(solicitudTarjeta), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<SolicitudTarjeta>(await response.Content.ReadAsStringAsync());
        }
        public async Task<SolicitudTarjeta> Actualizar(SolicitudTarjeta solicitudTarjeta, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(solicitudTarjeta), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<SolicitudTarjeta>(await response.Content.ReadAsStringAsync());
        }
        public async Task<SolicitudTarjeta> Eliminar(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<SolicitudTarjeta>(await response.Content.ReadAsStringAsync());
        }
    }
}