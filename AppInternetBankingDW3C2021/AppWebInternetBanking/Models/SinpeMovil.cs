using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class SinpeMovil
    {
        public int CodigoSinpe { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoMoneda { get; set; }
        public string NumeroTelefonoEmisor { get; set; }
        public string NumeroTelefonoRemitente { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal Comision { get; set; }
    }
}