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
    public partial class frmMarchamo : System.Web.UI.Page
    {
        IEnumerable<Marchamo> marchamos = new ObservableCollection<Marchamo>();
        MarchamoManager marchamoManager = new MarchamoManager();
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
                marchamos = await marchamoManager.ObtenerMarchamos(Session["Token"].ToString());
                gvMarchamos.DataSource = marchamos.ToList();
                gvMarchamos.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de marchamos";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                Marchamo marchamo = new Marchamo()
                {
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    PlacaVehiculo = txtPlacaVehiculo.Text,
                    Monto = Convert.ToDecimal(txtMonto.Text),
                    CodigoMoneda = Convert.ToInt32(txtCodigoMoneda.Text),
                    Estado = ddlEstadoMant.SelectedValue,
                    FechaHoraPago = Convert.ToDateTime(txtFechaHoraPago.Text)
                };

                Marchamo marchamoIngresado = await marchamoManager.Ingresar(marchamo, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(marchamoIngresado.PlacaVehiculo))
                {
                    lblResultado.Text = "Marchamo ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nuevo marchamo incluido", marchamoIngresado.PlacaVehiculo, "testertestingprogrammer@gmail.com",
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
                Marchamo marchamo = new Marchamo()
                {
                    CodigoPago = Convert.ToInt32(txtCodigoMant.Text),
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    PlacaVehiculo = txtPlacaVehiculo.Text,
                    Monto = Convert.ToDecimal(txtMonto.Text),
                    CodigoMoneda = Convert.ToInt32(txtCodigoMoneda.Text),
                    Estado = ddlEstadoMant.SelectedValue,
                    FechaHoraPago = Convert.ToDateTime(txtFechaHoraPago.Text)
                };

                Marchamo marchamoActualizado = await marchamoManager.Actualizar(marchamo, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(marchamoActualizado.PlacaVehiculo))
                {
                    lblResultado.Text = "Marchamo actualizado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Marchamo actualizado con exito", marchamoActualizado.PlacaVehiculo, "testertestingprogrammer@gmail.com",
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

                Marchamo marchamo = await marchamoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(marchamo.PlacaVehiculo))
                {
                    ltrModalMensaje.Text = "Marchamo eliminado";
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
                    Vista = "frmMarchamo.aspx",
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
            ltrTituloMantenimiento.Text = "Nuevo marchamo";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;

            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            ltrCodigoUsuario.Visible = true;
            txtCodigoUsuario.Visible = true;

            ltrPlacaVehiculo.Visible = true;
            txtPlacaVehiculo.Visible = true;

            ltrMonto.Visible = true;
            txtMonto.Visible = true;

            ltrCodigoMoneda.Visible = true;
            txtCodigoMoneda.Visible = true;

            ddlEstadoMant.Enabled = false;

            txtFechaHoraPago.Visible = true;
            ltrFechaHoraPago.Visible = true;

            txtCodigoMant.Text = string.Empty;
            txtCodigoUsuario.Text = string.Empty;
            txtPlacaVehiculo.Text = string.Empty;
            txtMonto.Text = string.Empty;
            txtCodigoMoneda.Text = string.Empty;
            txtFechaHoraPago.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvMarchamos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvMarchamos.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar marchamo";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoUsuario.Text = row.Cells[1].Text.Trim();
                    txtPlacaVehiculo.Text = row.Cells[2].Text.Trim();
                    txtMonto.Text = row.Cells[3].Text.Trim();
                    txtCodigoMoneda.Text = row.Cells[4].Text.Trim();
                    txtFechaHoraPago.Text = row.Cells[5].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el Pago del marchamo?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}