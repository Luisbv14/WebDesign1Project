using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class Pago
    {
        public Cuenta Cuenta { get; set; }
        public Servicio Servicio { get; set; }

    }
}