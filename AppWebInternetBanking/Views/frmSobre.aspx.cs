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

    public partial class frmSobre : System.Web.UI.Page
    {
        IEnumerable<Sobre> sobres = new ObservableCollection<Sobre>();
        SobreManager sobreManager = new SobreManager();
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
                sobres = await sobreManager.ObtenerSobres(Session["Token"].ToString());
                gvSobres.DataSource = sobres.ToList();
                gvSobres.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de sobres" + e.Message;
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
    
        //arreglar y terminar este codigo
        protected DateTime isfutureDate(DateTime first, DateTime last)
        {
            if (first>=last)
            {
                lblResultado.Text = "La fecha indicada menor o igual a la inicial";
                lblResultado.Visible = true;
                lblResultado.ForeColor = Color.Maroon;
            } else
            {
                return last;
            }
            return first;
        }
        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                if (!string.IsNullOrEmpty(txtDescripcion.Text) && !string.IsNullOrEmpty(txtMonto.Text))
                {
                    Sobre sobre = new Sobre()
                    {
                        CodigoCuenta = Convert.ToInt32(ddlCodigoCuenta.SelectedValue),
                        Descripcion = txtDescripcion.Text,
                        FechaHoraInicio = DateTime.Now,
                        Frecuencia = ddlFrecuencia.SelectedValue,
                        FechaHoraFinal = Convert.ToDateTime(txtFechaHoraFinal.Text),
                        Monto = Convert.ToDecimal(txtMonto.Text)
                    };

                    Sobre sobreIngresado = await sobreManager.Ingresar(sobre, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(sobreIngresado.Descripcion))
                    {
                        lblResultado.Text = "Sobre ingresado con exito";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Nuevo sobre incluido", sobreIngresado.Descripcion, "testertestingprogrammer@gmail.com",
                            Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                }
                else
                {
                    lblResultado.Text = "Sobre no ingresado. Motivo: Motivo: Espacio(s) obligatorio(s) no ingresado(s).";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                    InicializarControles();
                }
            }
            else // modificar
            {
                if (!string.IsNullOrEmpty(txtDescripcion.Text) && !string.IsNullOrEmpty(txtMonto.Text))
                {
                    Sobre sobre = new Sobre()
                    {
                        CodigoSobre = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoCuenta = Convert.ToInt32(ddlCodigoCuenta.SelectedValue),
                        Descripcion = txtDescripcion.Text,
                        FechaHoraInicio = DateTime.Now,
                        Frecuencia = ddlFrecuencia.SelectedValue,
                        FechaHoraFinal = Convert.ToDateTime(txtFechaHoraFinal.Text),
                        Monto = Convert.ToDecimal(txtMonto.Text)
                    };

                    Sobre sobreActualizado = await sobreManager.Actualizar(sobre, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(sobreActualizado.Descripcion))
                    {
                        lblResultado.Text = "Sobre actualizado con éxito.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Sobre actualizado con éxito", sobreActualizado.Descripcion, "testertestingprogrammer@gmail.com",
                        Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                }
                else
                {
                    lblResultado.Text = "Sobre no actualizado. Motivo: Motivo: Espacio(s) obligatorio(s) no ingresado(s).";
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
                Sobre sobre = await sobreManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(sobre.Descripcion))
                {
                    lblResultado.Visible = false;
                    ltrModalMensaje.Text = "Sobre eliminado.";
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
                    Vista = "frmSobre.aspx",
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
            ltrTituloMantenimiento.Text = "Nuevo sobre.";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            lblResultado.Text = string.Empty;

            txtCodigoMant.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtFechaHoraInicio.Text = string.Empty;
            txtFechaHoraFinal.Text = string.Empty;
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
        protected void gvSobres_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSobres.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar sobre.";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    ddlCodigoCuenta.SelectedValue = row.Cells[1].Text.Trim();
                    txtDescripcion.Text = row.Cells[2].Text.Trim();
                    ddlFrecuencia.SelectedValue = row.Cells[3].Text.Trim();
                    txtFechaHoraInicio.Text = row.Cells[4].Text.Trim();
                    txtFechaHoraFinal.Text = row.Cells[5].Text.Trim();
                    txtMonto.Text = row.Cells[6].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "¿Está seguro que desea eliminar el sobre #" + row.Cells[0].Text + "?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

    }


}