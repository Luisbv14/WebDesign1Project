//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SinpeMovil
    {
        public int CodigoSinpe { get; set; }
        public int CodigoCuenta { get; set; }
        public string NumeroTelefonoEmisor { get; set; }
        public string NumeroTelefonoRemitente { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public System.DateTime FechaHora { get; set; }
        public decimal Comision { get; set; }
    
        public Cuenta Cuenta { get; set; }
    }
}
