using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class Reporte
    {
        public int CodigoReporte { get; set; }
        public int CodigoCuenta { get; set; }
        public string TipoReporte { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaHora { get; set; }
    }
}