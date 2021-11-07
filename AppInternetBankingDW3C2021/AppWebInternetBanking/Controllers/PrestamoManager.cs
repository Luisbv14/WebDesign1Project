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
    public class PrestamoManager
    {
        string UrlBase = "http://localhost:49220/api/Prestamos/";
        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<Prestamo> ObtenerPrestamo(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Prestamo>(response);
        }

        public async Task<IEnumerable<Prestamo>> ObtenerPrestamos(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Prestamo>>(response);
        }

        public async Task<Prestamo> Ingresar(Prestamo prestamo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(prestamo), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Prestamo>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Prestamo> Actualizar(Prestamo prestamo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(prestamo), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Prestamo>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Prestamo> Eliminar(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Prestamo>(await response.Content.ReadAsStringAsync());
        }
    }
}