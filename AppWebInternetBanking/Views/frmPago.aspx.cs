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

        IEnumerable<Cuenta> cuentas = new ObservableCollection<Cuenta>();
        CuentaManager cuentaManager = new CuentaManager();

        IEnumerable<Servicio> servicios = new ObservableCollection<Servicio>();
        ServicioManager servicioManager = new ServicioManager();

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
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de pagos" + e.Message;
                lblStatus.Visible = true;
            }
            try
            {
                servicios = await servicioManager.ObtenerServicios(Session["Token"].ToString());
                ddlCodigoServicio.DataSource = servicios.ToList();
                ddlCodigoServicio.DataBind();
                ddlCodigoServicio.DataTextField = "Descripcion";
                ddlCodigoServicio.DataValueField = "Codigo";
                ddlCodigoServicio.DataBind();

            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de servicios" + e.Message;
                lblStatus.Visible = true;
            }
            try
            {
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());
                ddlCodigoCuenta.DataSource = cuentas.ToList();
                ddlCodigoCuenta.DataBind();
                ddlCodigoCuenta.DataTextField = "IBAN";
                ddlCodigoCuenta.DataValueField = "Codigo";
                ddlCodigoCuenta.DataBind();

            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de cuentas" + e.Message;
                lblStatus.Visible = true;
            }

        }
        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                if (!string.IsNullOrEmpty(txtMonto.Text))
                {
                    Pago pago = new Pago()
                    {
                        CodigoServicio = Convert.ToInt32(ddlCodigoServicio.SelectedValue),
                        CodigoCuenta = Convert.ToInt32(ddlCodigoCuenta.SelectedValue),
                        FechaHora = DateTime.Now,
                        Monto = Convert.ToDecimal(txtMonto.Text)
                    };

                    Pago pagoIngresado = await pagoManager.Ingresar(pago, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(Convert.ToString(pagoIngresado.CodigoCuenta)))
                    {
                        lblResultado.Text = "Pago ingresado con éxito.";
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
                }
                else
                {
                    lblResultado.Text = "Pago no procesado. Motivo: No ingresó el monto del servicio.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                    InicializarControles();
                }
            }   
        }
        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ltrTituloMantenimiento.Text = "Nuevo pago de servicio.";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            lblResultado.Text = string.Empty;

            txtCodigoMant.Text = string.Empty;
            txtFechaHora.Text = string.Empty;
            txtMonto.Text = string.Empty;
            LimpiarControles();
            ScriptManager.RegisterStartupScript(this,
            this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }
        private void LimpiarControles()
        {
            foreach (var item in Page.Controls)
            {
                if (item is TextBox)
                    ((TextBox)item).Text = String.Empty;
            }
        }
    }
}