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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class INTERNET_BANKING_DW1_3C2021 : DbContext
    {
        public INTERNET_BANKING_DW1_3C2021()
            : base("name=INTERNET_BANKING_DW1_3C2021")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cuenta> Cuenta { get; set; }
        public virtual DbSet<Error> Error { get; set; }
        public virtual DbSet<Estadistica> Estadistica { get; set; }
        public virtual DbSet<FondoInversion> FondoInversion { get; set; }
        public virtual DbSet<Moneda> Moneda { get; set; }
        public virtual DbSet<Pago> Pago { get; set; }
        public virtual DbSet<Reporte> Reporte { get; set; }
        public virtual DbSet<Servicio> Servicio { get; set; }
        public virtual DbSet<Sesion> Sesion { get; set; }
        public virtual DbSet<Sobre> Sobre { get; set; }
        public virtual DbSet<SolicitudCita> SolicitudCita { get; set; }
        public virtual DbSet<SolicitudTarjeta> SolicitudTarjeta { get; set; }
        public virtual DbSet<Transferencia> Transferencia { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Marchamo> Marchamo { get; set; }
        public virtual DbSet<SinpeMovil> SinpeMovil { get; set; }
        public virtual DbSet<Prestamo> Prestamo { get; set; }
    }
}
