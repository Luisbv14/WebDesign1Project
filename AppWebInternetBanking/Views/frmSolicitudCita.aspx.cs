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
                solicitudCitas = await solicitudCitaManager.ObtenerSolicitudCitas(Session["Token"].ToString());
                gvSolicitudCitas.DataSource = solicitudCitas.ToList();
                gvSolicitudCitas.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de solicitudes de citas" + e.Message;
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
                if (Convert.ToDateTime(txtFechaHoraCita.Text) >= DateTime.Now) //debe ser mayor a la fecha actual, no se este vacio no esta validado todavia
                {
                    SolicitudCita solicitudCita = new SolicitudCita()
                    {
                        CodigoUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue),
                        TipoCita = ddlTipoCita.SelectedValue,
                        FechaHoraCita = Convert.ToDateTime(txtFechaHoraCita.Text),
                        Sede = ddlSede.SelectedValue
                    };

                    SolicitudCita citaIngresada = await solicitudCitaManager.Ingresar(solicitudCita, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(citaIngresada.TipoCita))
                    {
                        lblResultado.Text = "Solicitud de cita ingresada con éxito.";
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
                }
                else
                {
                    lblResultado.Text = "Solicitud de cita no ingresada. Motivo: Fecha inválida.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                    InicializarControles();
                }
            }
            else // modificar
            {
                if (Convert.ToDateTime(txtFechaHoraCita.Text) >= DateTime.Now) //debe ser mayor a la fecha actual
                {
                    SolicitudCita solicitudCita = new SolicitudCita()
                    {
                        CodigoCita = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue),
                        TipoCita = ddlTipoCita.SelectedValue,
                        FechaHoraCita = Convert.ToDateTime(txtFechaHoraCita.Text),
                        Sede = ddlSede.SelectedValue
                    };

                    SolicitudCita citaActualizada = await solicitudCitaManager.Actualizar(solicitudCita, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(citaActualizada.TipoCita))
                    {
                        lblResultado.Text = "Solicitud de cita actualizada con éxito.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Solicitud de cita actualizada con éxito", citaActualizada.TipoCita, "testertestingprogrammer@gmail.com",
                            Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                }
                else
                {
                    lblResultado.Text = "Solicitud de cita no actualizada. Motivo: Fecha inválida."; 
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

                SolicitudCita solicitudCita = await solicitudCitaManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(solicitudCita.TipoCita))
                {
                    ltrModalMensaje.Text = "Solicitud cita eliminada.";
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
            lblResultado.Text = string.Empty;

            ddlSede.Enabled = true;
            ddlTipoCita.Enabled = true;
            txtCodigoMant.Text = string.Empty;
            txtFechaHoraCita.Text = string.Empty;
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

        protected void gvSolicitudCitas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSolicitudCitas.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar solicitud cita.";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    ddlUsuarios.SelectedValue = row.Cells[1].Text.Trim();
                    ddlTipoCita.SelectedValue = row.Cells[2].Text.Trim();
                    txtFechaHoraCita.Text = row.Cells[3].Text.Trim();
                    ddlSede.SelectedValue = row.Cells[4].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "¿Está seguro que desea eliminar la solicitud cita #" + row.Cells[0].Text +"?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}