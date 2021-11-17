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
                reportes = await reporteManager.ObtenerReportes(Session["Token"].ToString());
                gvReportes.DataSource = reportes.ToList();
                gvReportes.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de reportes" + e.Message;
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
                if (!string.IsNullOrEmpty(txtDescripcion.Text))
                {
                    Reporte reporte = new Reporte()
                    {
                        CodigoCuenta = Convert.ToInt32(ddlCodigoCuenta.SelectedValue),
                        TipoReporte = ddlTipoReporte.SelectedValue,
                        Descripcion = txtDescripcion.Text,
                        FechaHora = DateTime.Now
                    };

                    Reporte reporteIngresado = await reporteManager.Ingresar(reporte, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(reporteIngresado.Descripcion))
                    {
                        lblResultado.Text = "Reporte ingresado con éxito.";
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
                }
                else
                {
                    lblResultado.Text = "Reporte no ingresado. Motivo: No ingresó la descripción.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                    InicializarControles();
                }
            }
            else // modificar
            {
                if (!string.IsNullOrEmpty(txtDescripcion.Text))
                {
                    Reporte reporte = new Reporte()
                    {
                        CodigoReporte = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoCuenta = Convert.ToInt32(ddlCodigoCuenta.SelectedValue),
                        TipoReporte = ddlTipoReporte.SelectedValue,
                        Descripcion = txtDescripcion.Text,
                        FechaHora = DateTime.Now
                    };

                    Reporte reporteActualizado = await reporteManager.Actualizar(reporte, Session["Token"].ToString());
                    if (!string.IsNullOrEmpty(reporteActualizado.Descripcion))
                    {
                        lblResultado.Text = "Reporte actualizado con éxito.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Reporte actualizado con éxito", reporteActualizado.Descripcion, "testertestingprogrammer@gmail.com",
                            Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                }
                else
                {
                    lblResultado.Text = "Reporte no actualizado. Motivo: No ingresó la descripción.";
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

                Reporte reporte = await reporteManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(reporte.Descripcion))
                {
                    ltrModalMensaje.Text = "Reporte eliminado.";
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
            ltrTituloMantenimiento.Text = "Nuevo reporte.";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            lblResultado.Text = string.Empty;

            ddlTipoReporte.Enabled = true;
            txtCodigoMant.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
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
        protected void gvReportes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvReportes.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar reporte.";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    ddlCodigoCuenta.SelectedValue = row.Cells[1].Text.Trim();
                    ddlTipoReporte.SelectedValue = row.Cells[2].Text.Trim();
                    txtDescripcion.Text = row.Cells[3].Text.Trim();
                    txtFechaHora.Text = row.Cells[4].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "¿Está seguro que desea eliminar el reporte #" + row.Cells[0].Text + "?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}