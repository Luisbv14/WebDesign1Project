using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class Cuenta
    {
        public Moneda Moneda { get; set; }
        public Usuario Usuario { get; set; }

    }
}