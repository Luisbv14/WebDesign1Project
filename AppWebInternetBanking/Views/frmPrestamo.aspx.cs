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

        IEnumerable<Usuario> usuarios = new ObservableCollection<Usuario>();
        UsuarioManager usuarioManager = new UsuarioManager();

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
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de prestamos" + e.Message;
                lblStatus.Visible = true;
            }
            try
            {
                usuarios = await usuarioManager.ObtenerUsuarios(Session["Token"].ToString());
                ddlUsuarios.DataSource = usuarios.ToList();
                ddlUsuarios.DataBind();
                ddlUsuarios.DataTextField = "Nombre";
                ddlUsuarios.DataValueField = "Codigo";
                ddlUsuarios.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de usuarios " + e.Message;
                lblStatus.Visible = true;
            }
        }
        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                if (!string.IsNullOrEmpty(txtMonto.Text))
                {
                    Prestamo prestamo = new Prestamo()
                    {
                        CodigoUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue),
                        FechaHora = DateTime.Now,
                        Monto = Convert.ToDecimal(txtMonto.Text),
                        Plazo = ddlPlazo.SelectedValue,
                        Tasa = ddlTasa.SelectedValue
                    };

                    Prestamo prestamoIngresado = await prestamoManager.Ingresar(prestamo, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(prestamoIngresado.Tasa))
                    {
                        lblResultado.Text = "Préstamo ingresado con éxito.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Nuevo préstamo incluido", prestamoIngresado.Tasa, "testertestingprogrammer@gmail.com",
                            Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                }
                else
                {
                    lblResultado.Text = "Préstamo no ingresado. Motivo: No ingresó el monto.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                    InicializarControles();
                }
            }
            else // modificar
            {
                if(!string.IsNullOrEmpty(txtMonto.Text))
                {
                    Prestamo prestamo = new Prestamo()
                    {
                        CodigoPrestamo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue),
                        FechaHora = DateTime.Today,
                        Monto = Convert.ToDecimal(txtMonto.Text),
                        Plazo = ddlPlazo.SelectedValue,
                        Tasa = ddlTasa.SelectedValue
                    };

                    Prestamo prestamoActualizado = await prestamoManager.Actualizar(prestamo, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(prestamoActualizado.Tasa))
                    {
                        lblResultado.Text = "Préstamo actualizado con éxito.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Préstamo actualizado con éxito", prestamoActualizado.Tasa, "testertestingprogrammer@gmail.com",
                        Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                }
                else
                {
                    lblResultado.Text = "Préstamo no actualizado. Motivo: No ingresó el monto.";
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
        protected void gvPrestamos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvPrestamos.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar préstamo.";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    ddlUsuarios.SelectedValue = row.Cells[1].Text.Trim();
                    txtFechaHora.Text = row.Cells[2].Text.Trim();
                    txtMonto.Text = row.Cells[3].Text.Trim();
                    ddlPlazo.SelectedValue = row.Cells[4].Text.Trim();
                    ddlTasa.SelectedValue = row.Cells[5].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "¿Está seguro que desea eliminar el prestamo #" + row.Cells[0].Text + "?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}