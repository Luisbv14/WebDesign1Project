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
    public partial class frmServicio : System.Web.UI.Page
    {
        IEnumerable<Servicio> servicios = new ObservableCollection<Servicio>();
        ServicioManager servicioManager = new ServicioManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
            } else
            {
                InicializarControles();
            }
        }

        private async void InicializarControles()
        {
            try
            {
                servicios = await servicioManager.ObtenerServicios(Session["Token"].ToString());
                gvServicios.DataSource = servicios.ToList();
                gvServicios.DataBind();
            } catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de servicios";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtCodigoMant.Text)) //Si esto es así, está intersertando
            {
                Servicio servicio = new Servicio()
                {
                    Descripcion = txtDescripcion.Text,
                    Estado = ddlEstadoMant.SelectedValue
                };

                Servicio servicioIngresado = await servicioManager.Ingresar(servicio, Session["Token"].ToString());

                if(!string.IsNullOrEmpty(servicioIngresado.Descripcion))
                {
                    lblResultado.Text = "Servicio ingresado con éxito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nuevo servicio incluido", servicioIngresado.Descripcion, "svillagra07@gmail.com", Convert.ToInt32(Session["CodigoUsuario"].ToString()));
                }
            } else // modificar
            {
                Servicio servicio = new Servicio()
                {
                    Descripcion = txtDescripcion.Text,
                    Estado = ddlEstadoMant.SelectedValue
                };

                Servicio servicioActualizado = await servicioManager.Ingresar(servicio, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(servicioActualizado.Descripcion))
                {
                    lblResultado.Text = "Servicio ingresado con éxito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nuevo servicio incluido", servicioActualizado.Descripcion, "svillagra07@gmail.com", Convert.ToInt32(Session["CodigoUsuario"].ToString()));
                }
                /*
                lblResultado.Text = "Hubo un error al efectuar la operación";
                lblResultado.Visible = true;
                lblResultado.ForeColor = Color.Maroon; */
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
                
                Servicio servicio = await servicioManager.Eliminar(lblCodigoEliminar.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(servicio.Descripcion))
                {
                    lblCodigoEliminar.Text = string.Empty;
                    ltrModalMensaje.Text = "Servicio eliminado";
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
            ltrTituloMantenimiento.Text = "Nuevo servicio";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-sucess";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;
            txtDescripcion.Visible = true;
            ltrDescripcion.Visible = true;
            ddlEstadoMant.Enabled = false;
            txtCodigoMant.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
            this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }



        protected void gvServicios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvServicios.Rows[index];



            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar servicio";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtDescripcion.Text = row.Cells[1].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    lblCodigoEliminar.Text = row.Cells[0].Text;
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el servicio # " + lblCodigoEliminar.Text + "?";
                    ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}