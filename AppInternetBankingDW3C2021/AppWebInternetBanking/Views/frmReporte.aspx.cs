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
    public partial class frmReporte : System.Web.UI.Page
    {
        IEnumerable<Reporte> reportes = new ObservableCollection<Reporte>();
        ReporteManager reporteManager = new ReporteManager();
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
                reportes = await reporteManager.ObtenerReportes(Session["Token"].ToString());
                gvReportes.DataSource = reportes.ToList();
                gvReportes.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de reportes";
                lblStatus.Visible = true;
            }
        }
        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                Reporte reporte = new Reporte()
                {
                    CodigoCuenta = Convert.ToInt32(txtCodigoCuenta.Text),
                    TipoReporte = ddlTipoReporte.SelectedValue,
                    Descripcion = txtDescripcion.Text,
                    FechaHora = Convert.ToDateTime(txtFechaHora.Text)
                };

                Reporte reporteIngresado = await reporteManager.Ingresar(reporte, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(reporteIngresado.Descripcion))
                {
                    lblResultado.Text = "Reporte ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nuevo reporte incluido", reporteIngresado.Descripcion, "testertestingprogrammer@gmail.com",
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
                Reporte reporte = new Reporte()
                {
                    CodigoReporte = Convert.ToInt32(txtCodigoMant.Text),
                    CodigoCuenta = Convert.ToInt32(txtCodigoCuenta.Text),
                    TipoReporte = ddlTipoReporte.SelectedValue,
                    Descripcion = txtDescripcion.Text,
                    FechaHora = Convert.ToDateTime(txtFechaHora.Text)
                };

                Reporte reporteActualizado = await reporteManager.Actualizar(reporte, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(reporteActualizado.Descripcion))
                {
                    lblResultado.Text = "Reporte actualizado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Reporte actualizado con exito", reporteActualizado.Descripcion, "testertestingprogrammer@gmail.com",
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

                Reporte reporte = await reporteManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(reporte.Descripcion))
                {
                    ltrModalMensaje.Text = "Reporte eliminado";
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
                    Vista = "frmReporte.aspx",
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
            ltrTituloMantenimiento.Text = "Nuevo reporte";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;

            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            ltrCodigoCuenta.Visible = true;
            txtCodigoCuenta.Visible = true;

            txtDescripcion.Visible = true;
            ltrDescripcion.Visible = true;

            ltrFechaHora.Visible = true;
            txtFechaHora.Visible = true;

            ddlTipoReporte.Enabled = true;
            txtCodigoMant.Text = string.Empty;
            txtCodigoCuenta.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtFechaHora.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvReportes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvReportes.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar reporte";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoCuenta.Text = row.Cells[1].Text.Trim();
                    txtDescripcion.Text = row.Cells[3].Text.Trim();
                    txtFechaHora.Text = row.Cells[4].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el reporte?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}