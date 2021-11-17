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
    public partial class frmSinpeMovil : System.Web.UI.Page
    {
        IEnumerable<SinpeMovil> sinpeMoviles = new ObservableCollection<SinpeMovil>();
        SinpeMovilManager sinpeMovilManager = new SinpeMovilManager();
        static string _codigo = string.Empty;

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
                sinpeMoviles = await sinpeMovilManager.ObtenerSinpeMoviles(Session["Token"].ToString());
                gvSinpeMoviles.DataSource = sinpeMoviles.ToList();
                gvSinpeMoviles.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de sinpe movil" + e.Message;
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
                if (!string.IsNullOrEmpty(txtNumeroTelefonoEmisor.Text) && !string.IsNullOrEmpty(txtNumeroTelefonoRemitente.Text) && !string.IsNullOrEmpty(txtDescripcion.Text) && !string.IsNullOrEmpty(txtMonto.Text))
                {
                    SinpeMovil sinpeMovil = new SinpeMovil()
                    {
                        CodigoCuenta = Convert.ToInt32(ddlCodigoCuenta.SelectedValue),
                        NumeroTelefonoEmisor = txtNumeroTelefonoEmisor.Text,
                        NumeroTelefonoRemitente = txtNumeroTelefonoRemitente.Text,
                        Descripcion = txtDescripcion.Text,
                        Monto = Convert.ToDecimal(txtMonto.Text),
                        FechaHora = DateTime.Now,
                        Comision = Convert.ToDecimal(ddlComision.SelectedValue)
                    };

                    SinpeMovil sinpeMovilIngresado = await sinpeMovilManager.Ingresar(sinpeMovil, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(sinpeMovilIngresado.Descripcion))
                    {
                        lblResultado.Text = "Sinpe Móvil ingresado con éxito.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Nuevo sinpe móvil incluido", sinpeMovilIngresado.Descripcion, "testertestingprogrammer@gmail.com",
                            Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                }
                else
                {
                    lblResultado.Text = "Sinpe Móvil no ingresado. Motivo: Espacio(s) obligatorio(s) no ingresado(s).";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                    InicializarControles();
                }
            }
            else // modificar
            {
                if (!string.IsNullOrEmpty(txtNumeroTelefonoEmisor.Text) && !string.IsNullOrEmpty(txtNumeroTelefonoRemitente.Text) && !string.IsNullOrEmpty(txtDescripcion.Text) && !string.IsNullOrEmpty(txtMonto.Text))
                {
                    SinpeMovil sinpeMovil = new SinpeMovil()
                    {
                        CodigoSinpe = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoCuenta = Convert.ToInt32(ddlCodigoCuenta.SelectedValue),
                        NumeroTelefonoEmisor = txtNumeroTelefonoEmisor.Text,
                        NumeroTelefonoRemitente = txtNumeroTelefonoRemitente.Text,
                        Descripcion = txtDescripcion.Text,
                        Monto = Convert.ToDecimal(txtMonto.Text),
                        FechaHora = DateTime.Now,
                        Comision = Convert.ToDecimal(ddlComision.SelectedValue)
                    };

                    SinpeMovil sinpeMovilActualizado = await sinpeMovilManager.Actualizar(sinpeMovil, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(sinpeMovilActualizado.Descripcion))
                    {
                        lblResultado.Text = "Sinpe Móvil actualizado con éxito.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Sinpe Móvil actualizado con éxito", sinpeMovilActualizado.Descripcion, "testertestingprogrammer@gmail.com",
                            Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                }
                else
                {
                    lblResultado.Text = "Sinpe Móvil no actualizado. Motivo: Espacio(s) obligatorio(s) no ingresado(s).";
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

        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {
            try
            {

                SinpeMovil sinpeMovil = await sinpeMovilManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(sinpeMovil.Descripcion))
                {
                    ltrModalMensaje.Text = "Sinpe Móvil eliminado.";
                    btnAceptarModal.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModal(); });", true);
                    InicializarControles();
                }
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    FechaHora = DateTime.Now,
                    Vista = "frmSinpeMovil.aspx",
                    Accion = "btnAceptarModal_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult,
                    Descripcion = ex.Message
                };
                Error errorIngresado = await errorManager.Ingresar(error);
            }
        }
        protected void btnCancelarModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ltrTituloMantenimiento.Text = "Nuevo sinpe móvil.";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            lblResultado.Text = string.Empty;

            txtCodigoMant.Text = string.Empty;
            txtNumeroTelefonoEmisor.Text = string.Empty;
            txtNumeroTelefonoRemitente.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtMonto.Text = string.Empty;
            txtFechaHora.Text = string.Empty;
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
        protected void gvSinpeMoviles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSinpeMoviles.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar sinpe móvil.";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    ddlCodigoCuenta.SelectedValue = row.Cells[1].Text.Trim();
                    txtNumeroTelefonoEmisor.Text = row.Cells[2].Text.Trim();
                    txtNumeroTelefonoRemitente.Text = row.Cells[3].Text.Trim();
                    txtDescripcion.Text = row.Cells[4].Text.Trim();
                    txtMonto.Text = row.Cells[5].Text.Trim();
                    txtFechaHora.Text = row.Cells[6].Text.Trim();
                    ddlComision.SelectedValue = row.Cells[7].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "¿Está seguro que desea eliminar el sinpe móvil #" + row.Cells[0].Text + "?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

    }
}