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
    public class ServicioManager
    {
        /// <summary>
        /// Esta clase conecta con el controlador de servicios en el API REST
        /// </summary>
        string UrlBase = "http://localhost:49220/api/Servicios/";
        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            ///

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;

        }

        public async Task<Servicio> ObtenerServicio(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Servicio>(response);
        }

        /// <summary>
        /// Este metodo obtiene la lista de servicios del API
        /// </summary>
        /// <param name="token"></param>
        /// <returns> Lista IEnumerable de objetos Servicio </returns>

        public async Task<IEnumerable<Servicio>> ObtenerServicios(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Servicio>>(response);
        }

        public async Task<Servicio> Ingresar(Servicio servicio, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(servicio), Encoding.UTF8, "application/json"));
            return JsonConvert.DeserializeObject<Servicio>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Servicio> Actualizar(Servicio servicio, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(servicio), Encoding.UTF8, "application/json"));
            return JsonConvert.DeserializeObject<Servicio>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Servicio> Eliminar(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));
            return JsonConvert.DeserializeObject<Servicio>(await response.Content.ReadAsStringAsync());
        }
    }
}