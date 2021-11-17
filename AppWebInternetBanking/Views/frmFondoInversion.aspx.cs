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
                fondoInversiones = await fondoInversionManager.ObtenerFondoInversiones(Session["Token"].ToString());
                gvFondoInversiones.DataSource = fondoInversiones.ToList();
                gvFondoInversiones.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de fondos de inversiones." + e.Message;
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
                lblStatus.Text = "Hubo un error al cargar la lista de cuentas." + e.Message;
                lblStatus.Visible = true;
            }
        }
        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            lblResultado.Text = "";
            lblResultado.Visible = false;

                    if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
                    {
                        if (!string.IsNullOrEmpty(txtMonto.Text))
                        {
                            FondoInversion fondoInversion = new FondoInversion()
                            {
                                CodigoCuenta = Convert.ToInt32(ddlCodigoCuenta.SelectedValue),
                                Plazo = ddlPlazo.SelectedValue,
                                Tasa = ddlTasa.SelectedValue,
                                Monto = Convert.ToDecimal(txtMonto.Text),
                                FechaHoraInicio = DateTime.Now
                            };

                            FondoInversion fondoInversionIngresado = await fondoInversionManager.Ingresar(fondoInversion, Session["Token"].ToString());

                            if (!string.IsNullOrEmpty (fondoInversionIngresado.Tasa))
                            {
                                lblResultado.Text = "Fondo de inversión ingresado con éxito.";
                                lblResultado.Visible = true;
                                lblResultado.ForeColor = Color.Green;
                                btnAceptarMant.Visible = false;
                                InicializarControles();

                                Correo correo = new Correo();
                                correo.Enviar("Nuevo fondo de inversión incluido", Convert.ToString(fondoInversionIngresado.Monto), "testertestingprogrammer@gmail.com",
                                Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                                ScriptManager.RegisterStartupScript(this,
                                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                            }
                        }
                        else
                        {
                            lblResultado.Text = "Fondo de inversión no ingresado. Motivo: No ingresó el monto.";
                            lblResultado.Visible = true;
                            lblResultado.ForeColor = Color.Maroon;
                            InicializarControles();
                        }
                    }
                    else // modificar
                    {
                        if (!string.IsNullOrEmpty(txtMonto.Text))
                        {
                            FondoInversion fondoInversion = new FondoInversion()
                            {
                                CodigoInversion = Convert.ToInt32(txtCodigoMant.Text),
                                CodigoCuenta = Convert.ToInt32(ddlCodigoCuenta.SelectedValue),
                                Plazo = ddlPlazo.SelectedValue,
                                Tasa = ddlTasa.SelectedValue,
                                Monto = Convert.ToDecimal(txtMonto.Text),
                                FechaHoraInicio = DateTime.Now //Hay que ver como hacer que cuando se actualice no se actualice la fecha de inicio
                            };

                            FondoInversion fondoInversionActualizado = await fondoInversionManager.Actualizar(fondoInversion, Session["Token"].ToString());

                            if (!string.IsNullOrEmpty(fondoInversionActualizado.Tasa))
                            {
                                lblResultado.Text = "Fondo de inversión actualizado con éxito.";
                                lblResultado.Visible = true;
                                lblResultado.ForeColor = Color.Green;
                                btnAceptarMant.Visible = false;
                                InicializarControles();

                                Correo correo = new Correo();
                                correo.Enviar("Fondo de inversión actualizado con éxito", fondoInversionActualizado.Tasa, "testertestingprogrammer@gmail.com",
                                Convert.ToInt32(Session["CodigoUsuario"].ToString()));
                                ScriptManager.RegisterStartupScript(this,
                                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                            }
                        }
                        else
                        {
                            lblResultado.Text = "Fondo de inversión no actualizado. Motivo: No ingresó el monto.";
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
                FondoInversion fondoInversion = await fondoInversionManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(fondoInversion.Tasa))
                {
                    ltrModalMensaje.Text = "Fondo de inversión eliminado.";
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
            ltrTituloMantenimiento.Text = "Nuevo fondo de inversión.";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            lblResultado.Text = string.Empty;

            txtCodigoMant.Text = string.Empty;
            txtMonto.Text = string.Empty;
            txtFechaHoraInicio.Text = string.Empty;
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
        protected void gvFondoInversiones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvFondoInversiones.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar fondo de inversión.";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    ddlCodigoCuenta.SelectedValue = row.Cells[1].Text.Trim();
                    ddlPlazo.SelectedValue = row.Cells[2].Text.Trim();
                    ddlTasa.SelectedValue = row.Cells[3].Text.Trim();
                    txtMonto.Text = row.Cells[4].Text.Trim();
                    txtFechaHoraInicio.Text = row.Cells[5].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "¿Está seguro que desea eliminar el fondo de inversión #" + row.Cells[0].Text + "?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}