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
    public partial class frmPrestamo : System.Web.UI.Page
    {
        IEnumerable<Prestamo> prestamos = new ObservableCollection<Prestamo>();
        PrestamoManager prestamoManager = new PrestamoManager();
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
                prestamos = await prestamoManager.ObtenerPrestamos(Session["Token"].ToString());
                gvPrestamos.DataSource = prestamos.ToList();
                gvPrestamos.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de prestamos";
                lblStatus.Visible = true;
            }
        }
        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                Prestamo prestamo = new Prestamo()
                {
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    CodigoMoneda = Convert.ToInt32(txtCodigoMoneda.Text),
                    FechaHora = Convert.ToDateTime(txtFechaHora.Text),
                    Monto = Convert.ToDecimal(txtMonto.Text),
                    Plazo = ddlPlazo.SelectedValue,
                    Tasa = txtTasa.Text
                };

                Prestamo prestamoIngresado = await prestamoManager.Ingresar(prestamo, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(prestamoIngresado.Tasa))
                {
                    lblResultado.Text = "Prestamo ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nuevo prestamo incluido",prestamoIngresado.Tasa, "testertestingprogrammer@gmail.com",
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
                Prestamo prestamo = new Prestamo()
                {
                    CodigoPrestamo = Convert.ToInt32(txtCodigoMant.Text),
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    CodigoMoneda = Convert.ToInt32(txtCodigoMoneda.Text),
                    FechaHora = Convert.ToDateTime(txtFechaHora.Text),
                    Monto = Convert.ToDecimal(txtMonto.Text),
                    Plazo = ddlPlazo.SelectedValue,
                    Tasa = txtTasa.Text
                };

                Prestamo prestamoActualizado = await prestamoManager.Actualizar(prestamo, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(prestamoActualizado.Tasa))
                {
                    lblResultado.Text = "Prestamo actualizado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Prestamo actualizado con exito", prestamoActualizado.Tasa, "testertestingprogrammer@gmail.com",
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

                Prestamo prestamo = await prestamoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(prestamo.Tasa))
                {
                    ltrModalMensaje.Text = "Prestamo eliminado";
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
                    Vista = "frmPrestamo.aspx",
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
            ltrTituloMantenimiento.Text = "Nuevo prestamo";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;

            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            txtCodigoUsuario.Visible = true;
            ltrCodigoUsuario.Visible = true;

            ltrCodigoMoneda.Visible = true;
            txtCodigoMoneda.Visible = true;

            ltrFechaHora.Visible = true;
            txtFechaHora.Visible = true;

            ltrMonto.Visible = true;
            txtMonto.Visible = true;

            ltrTasa.Visible = true;
            txtTasa.Visible = true;


            ddlPlazo.Enabled = true;

            txtCodigoMant.Text = string.Empty;
            txtCodigoUsuario.Text = string.Empty;
            txtCodigoMoneda.Text = string.Empty;
            txtFechaHora.Text = string.Empty;
            txtMonto.Text = string.Empty;
            txtTasa.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }
        protected void gvPrestamos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvPrestamos.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar prestamo";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoUsuario.Text = row.Cells[1].Text.Trim();
                    txtCodigoMoneda.Text = row.Cells[2].Text.Trim();
                    txtFechaHora.Text = row.Cells[3].Text.Trim();
                    txtMonto.Text = row.Cells[4].Text.Trim();
                    txtTasa.Text = row.Cells[5].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el prestamo?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}