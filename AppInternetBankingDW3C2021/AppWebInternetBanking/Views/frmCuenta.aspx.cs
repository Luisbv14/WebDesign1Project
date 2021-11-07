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
    public partial class frmCuenta : System.Web.UI.Page
    {
        IEnumerable<Cuenta> cuentas = new ObservableCollection<Cuenta>();
        CuentaManager cuentaManager = new CuentaManager();
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
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());
                gvCuentas.DataSource = cuentas.ToList();
                gvCuentas.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de cuentas";
                lblStatus.Visible = true;
            }

        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                Cuenta cuenta = new Cuenta()
                {
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    CodigoMoneda = Convert.ToInt32(txtCodigoMoneda.Text),
                    Descripcion = txtDescripcion.Text,
                    IBAN = txtIBAN.Text,
                    Saldo = Convert.ToDecimal(txtSaldo.Text),
                    Estado = ddlEstadoMant.SelectedValue
                };
            
                Cuenta cuentaIngresada = await cuentaManager.Ingresar(cuenta, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(cuentaIngresada.Descripcion))
                {
                    lblResultado.Text = "Cuenta ingresada con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nueva cuenta incluida", cuentaIngresada.Descripcion, "testertestingprogrammer@gmail.com",
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
                Cuenta cuenta = new Cuenta()
                {
                    Codigo = Convert.ToInt32(txtCodigoMant.Text),
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    CodigoMoneda = Convert.ToInt32(txtCodigoMoneda.Text),
                    Descripcion = txtDescripcion.Text,
                    IBAN = txtIBAN.Text,
                    Saldo = Convert.ToDecimal(txtSaldo.Text),
                    Estado = ddlEstadoMant.SelectedValue
                };

                Cuenta cuentaActualizada = await cuentaManager.Actualizar(cuenta, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(cuentaActualizada.Descripcion))
                {
                    lblResultado.Text = "Cuenta actualizada con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Cuenta actualizada con exito", cuentaActualizada.Descripcion, "testertestingprogrammer@gmail.com",
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

                Cuenta cuenta = await cuentaManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(cuenta.Descripcion))
                {
                    ltrModalMensaje.Text = "Cuenta eliminada";
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
                    Vista = "frmCuenta.aspx",
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
            ltrTituloMantenimiento.Text = "Nueva cuenta";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;

            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            ltrCodigoUsuario.Visible = true;
            txtCodigoUsuario.Visible = true;

            ltrCodigoMoneda.Visible = true;
            txtCodigoMoneda.Visible = true;

            txtDescripcion.Visible = true;
            ltrDescripcion.Visible = true;

            ltrIBAN.Visible = true;
            txtIBAN.Visible = true;

            ltrSaldo.Visible = true;
            txtSaldo.Visible = true;

            ddlEstadoMant.Enabled = false;

            txtCodigoMant.Text = string.Empty;
            txtCodigoMoneda.Text = string.Empty;
            txtCodigoUsuario.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtIBAN.Text = string.Empty;
            txtSaldo.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }
        protected void gvCuentas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvCuentas.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar cuenta";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoUsuario.Text = row.Cells[1].Text.Trim();
                    txtCodigoMoneda.Text = row.Cells[2].Text.Trim();
                    txtDescripcion.Text = row.Cells[3].Text.Trim();
                    txtIBAN.Text = row.Cells[4].Text.Trim();
                    txtSaldo.Text = row.Cells[5].Text.Trim();
                    btnAceptarMant.Visible = true;
                    txtIBAN.Enabled = false;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar  la cuenta?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }


    }
}


