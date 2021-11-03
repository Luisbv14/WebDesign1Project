using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class Sobre
    {
        public int CodigoSobre { get; set; }
        public int CodigoCuenta { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public string Frecuencia { get; set; }
        public DateTime FechaHoraFinal { get; set; }
        public decimal Monto { get; set; }
    }
}