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
    public partial class frmFondoInversion : System.Web.UI.Page
    {
        IEnumerable<FondoInversion> fondoInversiones = new ObservableCollection<FondoInversion>();
        FondoInversionManager fondoInversionManager = new FondoInversionManager();
        static string _codigo = string.Empty;

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
                fondoInversiones = await fondoInversionManager.ObtenerFondoInversiones(Session["Token"].ToString());
                gvFondoInversiones.DataSource = fondoInversiones.ToList();
                gvFondoInversiones.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de fondos de inversiones";
                lblStatus.Visible = true;
            }
        }
        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                FondoInversion fondoInversion = new FondoInversion()
                {
                    CodigoCuenta = Convert.ToInt32(txtCodigoCuenta.Text),
                    Plazo = ddlPlazo.SelectedValue,
                    Tasa = ddlTasa.SelectedValue,
                    Monto = Convert.ToInt32(txtMonto.Text),
                    FechaHoraInicio = Convert.ToDateTime(txtFechaHoraInicio.Text)
                };

                FondoInversion fondoInversionIngresado = await fondoInversionManager.Ingresar(fondoInversion, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(fondoInversionIngresado.Tasa))
                {
                    lblResultado.Text = "Fondo de inversion ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nuevo fondo de inversion incluido", fondoInversionIngresado.Tasa, "testertestingprogrammer@gmail.com",
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
            else // modificar
            {
                FondoInversion fondoInversion = new FondoInversion()
                {
                    CodigoInversion = Convert.ToInt32(txtCodigoMant.Text),
                    CodigoCuenta = Convert.ToInt32(txtCodigoCuenta.Text),
                    Plazo = ddlPlazo.SelectedValue,
                    Tasa = ddlTasa.SelectedValue,
                    Monto = Convert.ToInt32(txtMonto.Text),
                    FechaHoraInicio = Convert.ToDateTime(txtFechaHoraInicio.Text)
                };

                FondoInversion fondoInversionActualizado = await fondoInversionManager.Actualizar(fondoInversion, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(fondoInversionActualizado.Tasa))
                {
                    lblResultado.Text = "Fondo de inversion actualizado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Servicio actualizado con exito", fondoInversionActualizado.Tasa, "testertestingprogrammer@gmail.com",
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

        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {
            try
            {

                FondoInversion fondoInversion = await fondoInversionManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(fondoInversion.Tasa))
                {
                    ltrModalMensaje.Text = "Fondo de inversion eliminado";
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
                    CodigoUsuario =
                    Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    FechaHora = DateTime.Now,
                    Vista = "frmFondoInversion.aspx",
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
            ltrTituloMantenimiento.Text = "Nuevo fondo de inversion";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;

            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            ltrCodigoCuenta.Visible = true;
            txtCodigoCuenta.Visible = true;

            ltrTasa.Visible = true;

            ltrMonto.Visible = true;
            txtMonto.Visible = true;

            ltrFechaHoraInicio.Visible = true;
            txtFechaHoraInicio.Visible = true;

            ddlPlazo.Enabled = true;
            ddlTasa.Enabled = true;

            txtCodigoMant.Text = string.Empty;
            txtCodigoCuenta.Text = string.Empty;
            txtMonto.Text = string.Empty;
            txtFechaHoraInicio.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }
        protected void gvFondoInversiones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvFondoInversiones.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar fondo de inversion";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoCuenta.Text = row.Cells[1].Text.Trim();
                    txtMonto.Text = row.Cells[5].Text.Trim();
                    txtFechaHoraInicio.Text = row.Cells[6].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el fondo de inversion?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}