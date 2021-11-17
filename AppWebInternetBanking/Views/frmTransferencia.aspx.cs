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
    public partial class frmTransferencia : System.Web.UI.Page
    {
        IEnumerable<Transferencia> transferencias = new ObservableCollection<Transferencia>();
        TransferenciaManager transferenciaManager = new TransferenciaManager();

        IEnumerable<Cuenta> cuentas = new ObservableCollection<Cuenta>();
        CuentaManager cuentaManager = new CuentaManager();

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
                transferencias = await transferenciaManager.ObtenerTransferencias(Session["Token"].ToString());
                gvTransferencias.DataSource = transferencias.ToList();
                gvTransferencias.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de pagos" + e.Message;
                lblStatus.Visible = true;
            }
            try
            {
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());
                ddlCuentaOrigen.DataSource = cuentas.ToList();
                ddlCuentaOrigen.DataBind();
                ddlCuentaOrigen.DataTextField = "IBAN";
                ddlCuentaOrigen.DataValueField = "Codigo";
                ddlCuentaOrigen.DataBind();

            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de cuentas origen" + e.Message;
                lblStatus.Visible = true;
            }
            try
            {
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());
                ddlCuentaDestino.DataSource = cuentas.ToList();
                ddlCuentaDestino.DataBind();
                ddlCuentaDestino.DataTextField = "IBAN";
                ddlCuentaDestino.DataValueField = "Codigo";
                ddlCuentaDestino.DataBind();

            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de cuentas destino" + e.Message;
                lblStatus.Visible = true;
            }
        }
        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                if (!string.IsNullOrEmpty(txtMonto.Text) && !string.IsNullOrEmpty(txtDescripcion.Text))
                {
                    Transferencia transferencia = new Transferencia()
                    {
                        CuentaOrigen = Convert.ToInt32(ddlCuentaOrigen.SelectedValue), //Falta validar que la cuenta origen no sea igual a la destino o viceversa
                        CuentaDestino = Convert.ToInt32(ddlCuentaDestino.SelectedValue),
                        FechaHora = DateTime.Now,
                        Descripcion = txtDescripcion.Text,
                        Monto = Convert.ToDecimal(txtMonto.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Transferencia transferenciaIngresado = await transferenciaManager.Ingresar(transferencia, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(Convert.ToString(transferenciaIngresado.CuentaOrigen)))
                    {
                        lblResultado.Text = "Transferencia ingresada con éxito.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Nueva transferencia incluida", Convert.ToString(transferenciaIngresado.CuentaOrigen), "testertestingprogrammer@gmail.com",
                            Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                }
                else
                {
                    lblResultado.Text = "Transferencia no procesada. Motivo: No ingresó la descripción y/o el monto de la transferencia.";
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
            ltrTituloMantenimiento.Text = "Nuevo Transferencia.";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            lblResultado.Text = string.Empty;

            ddlEstadoMant.Enabled = false;

            txtCodigoMant.Text = string.Empty;
            txtFechaHora.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
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
