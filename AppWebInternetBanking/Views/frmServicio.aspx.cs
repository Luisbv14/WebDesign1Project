﻿using AppWebInternetBanking.Controllers;
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
                servicios = await servicioManager.ObtenerServicios(Session["Token"].ToString());
                gvServicios.DataSource = servicios.ToList();
                gvServicios.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de servicios" + e.Message;
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                if (!string.IsNullOrEmpty(txtDescripcion.Text))
                {
                    Servicio servicio = new Servicio()
                    {
                        Descripcion = txtDescripcion.Text,
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Servicio servicioIngresado = await servicioManager.Ingresar(servicio, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(servicioIngresado.Descripcion))
                    {
                        lblResultado.Text = "Servicio ingresado con éxito.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Nuevo servicio incluido", servicioIngresado.Descripcion, "testertestingprogrammer@gmail.com",
                            Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                }
                else
                {
                    lblResultado.Text = "Servicio no ingresado. Motivo: No ingresó la descripción.";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                    InicializarControles();
                }
            }
            else // modificar
            {
                if (!string.IsNullOrEmpty(txtDescripcion.Text))
                {
                    Servicio servicio = new Servicio()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        Descripcion = txtDescripcion.Text,
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Servicio servicioActualizado = await servicioManager.Actualizar(servicio, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(servicioActualizado.Descripcion))
                    {
                        lblResultado.Text = "Servicio actualizado con éxito.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Servicio actualizado con exito", servicioActualizado.Descripcion, "testertestingprogrammer@gmail.com",
                            Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                }
                else
                {
                    lblResultado.Text = "Servicio no actualizado. Motivo: No ingresó la descripción.";
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

                Servicio servicio = await servicioManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(servicio.Descripcion))
                {
                    ltrModalMensaje.Text = "Servicio eliminado.";
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
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            lblResultado.Text = string.Empty;

            ddlEstadoMant.Enabled = false;
            txtCodigoMant.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            ddlEstadoMant.SelectedIndex = default;
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

        protected void gvServicios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvServicios.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar servicio.";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtDescripcion.Text = row.Cells[1].Text.Trim();
                    ddlEstadoMant.SelectedValue = row.Cells[2].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ddlEstadoMant.Enabled = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "¿Está seguro que desea eliminar el servicio #" + row.Cells[0].Text + "?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }


    }
}