using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class FondoInversion
    {
        public int CodigoInversion { get; set; }
        public int CodigoCuenta { get; set; }
        public string Plazo { get; set; }
        public string Tasa { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaHoraInicio { get; set; }
    }
}