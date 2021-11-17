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
                marchamos = await marchamoManager.ObtenerMarchamos(Session["Token"].ToString());
                gvMarchamos.DataSource = marchamos.ToList();
                gvMarchamos.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de marchamos" + e.Message;
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
            lblResultado.Text = "";
            lblResultado.Visible = false;

            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                if (!string.IsNullOrEmpty(txtMonto.Text) && !string.IsNullOrEmpty(txtPlacaVehiculo.Text))
                {
                            Marchamo marchamo = new Marchamo()
                            {
                                CodigoUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue),
                                PlacaVehiculo = txtPlacaVehiculo.Text,
                                Monto = Convert.ToDecimal(txtMonto.Text),
                                Estado = ddlEstadoMant.SelectedValue,
                                FechaHoraPago = DateTime.Now
                            };

                            Marchamo marchamoIngresado = await marchamoManager.Ingresar(marchamo, Session["Token"].ToString());

                            if (!string.IsNullOrEmpty(marchamoIngresado.Estado))
                            {
                                lblResultado.Text = "Marchamo ingresado con éxito.";
                                lblResultado.Visible = true;
                                lblResultado.ForeColor = Color.Green;
                                btnAceptarMant.Visible = false;
                                InicializarControles();

                                Correo correo = new Correo();
                                correo.Enviar("Nuevo pago de marchamo incluido", marchamoIngresado.Estado, "testertestingprogrammer@gmail.com",
                                Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                                ScriptManager.RegisterStartupScript(this,
                                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                            }
                }
                else
                {
                        lblResultado.Text = "Pago de marchamo no ingresado. Motivo: No ingresó el monto y/o la placa del vehículo.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Maroon;
                        InicializarControles();
                }
            }
            
            else // modificar
            {
                if (!string.IsNullOrEmpty(txtMonto.Text) && !string.IsNullOrEmpty(txtPlacaVehiculo.Text))
                {
                    Marchamo marchamo = new Marchamo()
                        {
                            CodigoPago = Convert.ToInt32(txtCodigoMant.Text),
                            CodigoUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue),
                            PlacaVehiculo = txtPlacaVehiculo.Text,
                            Monto = Convert.ToDecimal(txtMonto.Text),
                            Estado = ddlEstadoMant.SelectedValue,
                            FechaHoraPago = DateTime.Now //Hay que ver como hacer que cuando se actualice no se actualice la fecha de pago
                    };

                        Marchamo marchamoActualizado = await marchamoManager.Actualizar(marchamo, Session["Token"].ToString());

                        if (!string.IsNullOrEmpty(marchamoActualizado.PlacaVehiculo))
                        {
                            lblResultado.Text = "Pago del marchamo actualizado con éxito.";
                            lblResultado.Visible = true;
                            lblResultado.ForeColor = Color.Green;
                            btnAceptarMant.Visible = false;
                            InicializarControles();

                            Correo correo = new Correo();
                            correo.Enviar("Pago del marchamo actualizado con éxito.", marchamoActualizado.PlacaVehiculo, "testertestingprogrammer@gmail.com",
                                Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                            ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                        }
                } else
                {
                    lblResultado.Text = "Pago de marchamo no actualizado. Motivo: No ingresó el monto y/o la placa del vehículo.";
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

                Marchamo marchamo = await marchamoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(marchamo.PlacaVehiculo))
                {
                    ltrModalMensaje.Text = "Pago de marchamo eliminado.";
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
            ltrTituloMantenimiento.Text = "Nuevo marchamo.";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            lblResultado.Text = string.Empty;
            ddlEstadoMant.Enabled = false;

            txtCodigoMant.Text = string.Empty;
            txtPlacaVehiculo.Text = string.Empty;
            txtMonto.Text = string.Empty;
            txtFechaHoraPago.Text = string.Empty;
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

        protected void gvMarchamos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvMarchamos.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar pago de marchamo.";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    ddlUsuarios.SelectedValue = row.Cells[1].Text.Trim();
                    txtPlacaVehiculo.Text = row.Cells[2].Text.Trim();
                    txtMonto.Text = row.Cells[3].Text.Trim();
                    ddlEstadoMant.SelectedValue = row.Cells[4].Text.Trim();
                    txtFechaHoraPago.Text = row.Cells[5].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "¿Está seguro que desea eliminar el pago del marchamo #" + row.Cells[0].Text + "?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}