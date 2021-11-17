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
    public partial class frmMoneda : System.Web.UI.Page
    {
        IEnumerable<Moneda> monedas = new ObservableCollection<Moneda>();
        MonedaManager monedaManager = new MonedaManager();
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
                monedas = await monedaManager.ObtenerMonedas(Session["Token"].ToString());
                gvMonedas.DataSource = monedas.ToList();
                gvMonedas.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de monedas" + e.Message;
                lblStatus.Visible = true;
            }
        }
        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                if (!string.IsNullOrEmpty(txtDescripcion.Text))
                {
                    Moneda moneda = new Moneda()
                    {
                        Descripcion = txtDescripcion.Text,
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Moneda monedaIngresada = await monedaManager.Ingresar(moneda, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(monedaIngresada.Descripcion))
                    {
                        lblResultado.Text = "Moneda ingresada con éxito.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Nueva moneda incluida", monedaIngresada.Descripcion, "testertestingprogrammer@gmail.com",
                            Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                }
                else
                {
                    lblResultado.Text = "Moneda no ingresada. Motivo: No ingresó la descripción.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                    InicializarControles();
                }
            }
            else // modificar
            {
                if (!string.IsNullOrEmpty(txtDescripcion.Text))
                {
                    Moneda moneda = new Moneda()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        Descripcion = txtDescripcion.Text,
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Moneda monedaActualizada = await monedaManager.Actualizar(moneda, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(monedaActualizada.Descripcion))
                    {
                        lblResultado.Text = "Moneda actualizada con éxito.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Moneda actualizaao con exito", monedaActualizada.Descripcion, "testertestingprogrammer@gmail.com",
                            Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                }
                else
                {
                    lblResultado.Text = "Moneda no actualizada. Motivo: No ingresó la descripción.";
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

                Moneda moneda = await monedaManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(moneda.Descripcion))
                {
                    ltrModalMensaje.Text = "Moneda eliminada";
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
                    Vista = "frmMoneda.aspx",
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
            ltrTituloMantenimiento.Text = "Nueva moneda.";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            lblResultado.Text = string.Empty;

            ddlEstadoMant.Enabled = false;
            txtCodigoMant.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
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

        protected void gvMonedas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvMonedas.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar moneda.";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtDescripcion.Text = row.Cells[1].Text.Trim();
                    ddlEstadoMant.SelectedValue = row.Cells[2].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ddlEstadoMant.Enabled = false;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "¿Está seguro que desea eliminar la moneda #" + row.Cells[0].Text + "?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

    }
}