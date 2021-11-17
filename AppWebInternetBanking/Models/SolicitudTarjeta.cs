using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class SolicitudTarjeta
    {
        public int CodigoSolicitud { get; set; }
        public int CodigoUsuario { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; }
    }
}