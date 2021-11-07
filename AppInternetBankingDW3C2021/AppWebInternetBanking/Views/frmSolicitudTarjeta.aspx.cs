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
    public partial class frmSolicitudTarjeta : System.Web.UI.Page
    {
        IEnumerable<SolicitudTarjeta> solicitudTarjeta = new ObservableCollection<SolicitudTarjeta>();
        SolicitudTarjetaManager solicitudTarjetaManager = new SolicitudTarjetaManager();
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
                solicitudTarjeta = await solicitudTarjetaManager.ObtenerSolicitudTarjetas(Session["Token"].ToString());
                gvSolicitudTarjeta.DataSource = solicitudTarjeta.ToList();
                gvSolicitudTarjeta.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de solicitudes de tarjetas";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoSolicitud.Text)) //insertar
            {
                SolicitudTarjeta solicitudTarjeta = new SolicitudTarjeta()
                {
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    Descripcion = ddlDescripcion.SelectedValue,
                    FechaHora = Convert.ToDateTime(txtFechaHora.Text),
                    Estado = ddlEstadoMant.SelectedValue
                };

                SolicitudTarjeta solitudIngresada = await solicitudTarjetaManager.Ingresar(solicitudTarjeta, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(solitudIngresada.Descripcion))
                {
                    lblResultado.Text = "Servicio ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nueva solicitud incluida", solitudIngresada.Descripcion, "testertestingprogrammer@gmail.com",
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
                SolicitudTarjeta solicitudTarjeta = new SolicitudTarjeta()
                {
                    CodigoSolicitud = Convert.ToInt32(txtCodigoSolicitud.Text),
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    Descripcion = ddlDescripcion.SelectedValue,
                    FechaHora = Convert.ToDateTime(txtFechaHora.Text),
                    Estado = ddlEstadoMant.SelectedValue
                };

                SolicitudTarjeta solicitudActualizada = await solicitudTarjetaManager.Actualizar(solicitudTarjeta, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(solicitudActualizada.Descripcion))
                {
                    lblResultado.Text = "Solicitud de cita actualizada con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Solicitud de cita actualizada con exito", solicitudActualizada.Descripcion, "testertestingprogrammer@gmail.com",
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

                SolicitudTarjeta solicitud = await solicitudTarjetaManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(solicitud.Descripcion))
                {
                    ltrModalMensaje.Text = "Solicitud eliminada";
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
                    Vista = "frmSolicitudTarjeta.aspx",
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
            ltrTituloMantenimiento.Text = "Nueva Solicitud";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;

            ltrCodigoSolicitud.Visible = true;
            txtCodigoSolicitud.Visible = true;
            txtCodigoSolicitud.Text = string.Empty;

            ltrCodigoUsuario.Visible = true;
            txtCodigoUsuario.Visible = true;
            txtCodigoUsuario.Text = string.Empty;

            ltrDescripcion.Visible = true;

            ltrFechaHora.Visible = true;
            txtFechaHora.Visible = true;
            txtFechaHora.Text = string.Empty;

            ddlEstadoMant.Enabled = true;
            ddlDescripcion.Enabled = true;

            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvSolicitudTarjeta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSolicitudTarjeta.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar solicitud tarjeta";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoSolicitud.Text = row.Cells[0].Text.Trim();
                    txtCodigoUsuario.Text = row.Cells[1].Text.Trim();
                    txtFechaHora.Text = row.Cells[3].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar la solicitud de tarjeta?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}