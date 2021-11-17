using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class Marchamo
    {
        public int CodigoPago { get; set; }
        public int CodigoUsuario { get; set; }
        public string PlacaVehiculo { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public DateTime FechaHoraPago { get; set; }
    }
}