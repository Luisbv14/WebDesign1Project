using AppWebInternetBanking.Controllers;
using AppWebInternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppWebInternetBanking.Views
{
    public partial class frmPago : System.Web.UI.Page
    {
        IEnumerable<Pago> pagos = new ObservableCollection<Pago>();
        PagoManager pagoManager = new PagoManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                    InicializarControles();
            }
        }
        private async void InicializarControles()
        {
            try
            {
                pagos = await pagoManager.ObtenerPagos(Session["Token"].ToString());
                gvPagos.DataSource = pagos.ToList();
                gvPagos.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de pagos";
                lblStatus.Visible = true;
            }
  
        }
        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                Pago pago = new Pago()
                {
                    CodigoServicio = Convert.ToInt32(txtCodigoServicio.Text),
                    CodigoCuenta = Convert.ToInt32(txtCodigoCuenta.Text),
                    FechaHora = Convert.ToDateTime(txtFechaHora.Text),
                    Monto = Convert.ToDecimal(txtMonto.Text)
                };

                Pago pagoIngresado = await pagoManager.Ingresar(pago, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(Convert.ToString(pagoIngresado.CodigoCuenta)))
                {
                    lblResultado.Text = "Pago ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nuevo pago incluido", Convert.ToString(pagoIngresado.CodigoCuenta), "testertestingprogrammer@gmail.com",
                        Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                }
                else
                {
                    lblResultado.Text = "Hubo un error al efectuar la operacion";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
            }   
        }
        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
        }
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ltrTituloMantenimiento.Text = "Nuevo pago";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;

            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            ltrCodigoServicio.Visible = true;
            txtCodigoServicio.Visible = true;

            txtCodigoCuenta.Visible = true;
            ltrCodigoCuenta.Visible = true;

            ltrFechaHora.Visible = true;
            txtFechaHora.Visible = true;

            ltrMonto.Visible = true;
            txtMonto.Visible = true;


            txtCodigoMant.Text = string.Empty;
            txtCodigoServicio.Text = string.Empty;
            txtCodigoCuenta.Text = string.Empty;
            txtFechaHora.Text = string.Empty;
            txtMonto.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }
    }
}