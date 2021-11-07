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
    public partial class frmSolicitudCita : System.Web.UI.Page
    {
        IEnumerable<SolicitudCita> solicitudCitas = new ObservableCollection<SolicitudCita>();
        SolicitudCitaManager solicitudCitaManager = new SolicitudCitaManager();
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
                solicitudCitas = await solicitudCitaManager.ObtenerSolicitudCitas(Session["Token"].ToString());
                gvSolicitudCitas.DataSource = solicitudCitas.ToList();
                gvSolicitudCitas.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de solicitudes de citas";
                lblStatus.Visible = true;
            }
        }
        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                SolicitudCita solicitudCita = new SolicitudCita()
                {
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    TipoCita = ddlTipoCita.SelectedValue,
                    FechaHoraCita = Convert.ToDateTime(txtFechaHoraCita.Text),
                    Sede = ddlSede.SelectedValue
                };

                SolicitudCita citaIngresada = await solicitudCitaManager.Ingresar(solicitudCita, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(citaIngresada.TipoCita))
                {
                    lblResultado.Text = "Solicitud de cita ingresada con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nueva solicitud de cita incluida", citaIngresada.TipoCita, "testertestingprogrammer@gmail.com",
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
                SolicitudCita solicitudCita = new SolicitudCita()
                {
                    CodigoCita = Convert.ToInt32(txtCodigoMant.Text),
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    TipoCita = ddlTipoCita.SelectedValue,
                    FechaHoraCita = Convert.ToDateTime(txtFechaHoraCita.Text),
                    Sede = ddlSede.SelectedValue
                };

                SolicitudCita citaActualizada = await solicitudCitaManager.Actualizar(solicitudCita, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(citaActualizada.TipoCita))
                {
                    lblResultado.Text = "Solicitud de cita actualizada con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Solicitud de cita actualizada con exito", citaActualizada.TipoCita, "testertestingprogrammer@gmail.com",
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

                SolicitudCita solicitudCita = await solicitudCitaManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(solicitudCita.TipoCita))
                {
                    ltrModalMensaje.Text = "Solicitud cita eliminada";
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
                    Vista = "frmServicio.aspx",
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
            ltrTituloMantenimiento.Text = "Nueva solicitud de cita";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;

            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            ltrCodigoUsuario.Visible = true;
            txtCodigoUsuario.Visible = true;

            ltrTipoCita.Visible = true;

            ltrFechaHoraCita.Visible = true;
            txtFechaHoraCita.Visible = true;

            ddlSede.Enabled = true;
            ddlTipoCita.Enabled = true;
            txtCodigoMant.Text = string.Empty;
            txtCodigoUsuario.Text = string.Empty;
            txtFechaHoraCita.Text = string.Empty;

            ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvSolicitudCitas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSolicitudCitas.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar solicitud cita";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoUsuario.Text = row.Cells[1].Text.Trim();
                    txtFechaHoraCita.Text = row.Cells[2].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar la solicitud cita?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}