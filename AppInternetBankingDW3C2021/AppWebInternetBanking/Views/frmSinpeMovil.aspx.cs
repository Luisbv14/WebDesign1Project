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
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de sinpe movil";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                SinpeMovil sinpeMovil = new SinpeMovil()
                {
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    CodigoMoneda = Convert.ToInt32(txtCodigoMoneda.Text),
                    NumeroTelefonoEmisor = txtNumeroTelefonoEmisor.Text,
                    NumeroTelefonoRemitente = txtNumeroTelefonoRemitente.Text,
                    Descripcion = txtDescripcion.Text,
                    Monto = Convert.ToDecimal(txtMonto.Text),
                    FechaHora = Convert.ToDateTime(txtFechaHora.Text),
                    Comision = Convert.ToDecimal(txtComision.Text)
                };

                SinpeMovil sinpeMovilIngresado = await sinpeMovilManager.Ingresar(sinpeMovil, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(sinpeMovilIngresado.Descripcion))
                {
                    lblResultado.Text = "Sinpe Movil ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nuevo sinpe movil incluido", sinpeMovilIngresado.Descripcion, "testertestingprogrammer@gmail.com",
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
                SinpeMovil sinpeMovil = new SinpeMovil()
                {
                    CodigoSinpe = Convert.ToInt32(txtCodigoMant.Text),
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    CodigoMoneda = Convert.ToInt32(txtCodigoMoneda.Text),
                    NumeroTelefonoEmisor = txtNumeroTelefonoEmisor.Text,
                    NumeroTelefonoRemitente = txtNumeroTelefonoRemitente.Text,
                    Descripcion = txtDescripcion.Text,
                    Monto = Convert.ToDecimal(txtMonto.Text),
                    FechaHora = Convert.ToDateTime(txtFechaHora.Text),
                    Comision = Convert.ToDecimal(txtComision.Text)
                };

                SinpeMovil sinpeMovilActualizado = await sinpeMovilManager.Actualizar(sinpeMovil, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(sinpeMovilActualizado.Descripcion))
                {
                    lblResultado.Text = "Sinpe Movil actualizado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Sinpe Movil actualizado con exito", sinpeMovilActualizado.Descripcion, "testertestingprogrammer@gmail.com",
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

                SinpeMovil sinpeMovil = await sinpeMovilManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(sinpeMovil.Descripcion))
                {
                    ltrModalMensaje.Text = "Sinpe Movil eliminado";
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
            ltrTituloMantenimiento.Text = "Nuevo sinpe movil";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;

            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            ltrCodigoUsuario.Visible = true;
            txtCodigoUsuario.Visible = true;

            ltrCodigoMoneda.Visible = true;
            txtCodigoMoneda.Visible = true;

            ltrNumeroTelefonoEmisor.Visible = true;
            txtNumeroTelefonoEmisor.Visible = true;

            ltrNumeroTelefonoRemitente.Visible = true;
            txtNumeroTelefonoRemitente.Visible = true;

            txtDescripcion.Visible = true;
            ltrDescripcion.Visible = true;

            ltrMonto.Visible = true;
            txtMonto.Visible = true;

            ltrFechaHora.Visible = true;
            txtFechaHora.Visible = true;

            ltrComision.Visible = true;
            txtComision.Visible = true;

            txtCodigoMant.Text = string.Empty;
            txtCodigoUsuario.Text = string.Empty;
            txtCodigoMoneda.Text = string.Empty;
            txtNumeroTelefonoEmisor.Text = string.Empty;
            txtNumeroTelefonoRemitente.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtMonto.Text = string.Empty;
            txtFechaHora.Text = string.Empty;
            txtComision.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }
        protected void gvSinpeMoviles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSinpeMoviles.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar sinpe movil";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoUsuario.Text = row.Cells[1].Text.Trim();
                    txtCodigoMoneda.Text = row.Cells[2].Text.Trim();
                    txtNumeroTelefonoEmisor.Text = row.Cells[3].Text.Trim();
                    txtNumeroTelefonoRemitente.Text = row.Cells[4].Text.Trim();
                    txtDescripcion.Text = row.Cells[5].Text.Trim();
                    txtMonto.Text = row.Cells[6].Text.Trim();
                    txtFechaHora.Text = row.Cells[7].Text.Trim();
                    txtComision.Text = row.Cells[8].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el sinpe movil?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

    }
}