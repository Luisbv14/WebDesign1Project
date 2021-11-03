using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class SolicitudCita
    {
        public int CodigoCita { get; set; }
        public int CodigoUsuario { get; set; }
        public string TipoCita { get; set; }
        public DateTime FechaHoraCita { get; set; }
        public string Sede { get; set; }
    }
}