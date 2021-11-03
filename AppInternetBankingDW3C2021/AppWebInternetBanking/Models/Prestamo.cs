using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{

    public partial class Prestamo
    {
        public int CodigoPrestamo { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoMoneda { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal Monto { get; set; }
        public string Plazo { get; set; }
        public string Tasa { get; set; }
    }
}