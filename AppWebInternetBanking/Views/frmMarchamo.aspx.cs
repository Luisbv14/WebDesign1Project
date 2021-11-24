using AppWebInternetBanking.Controllers;
using AppWebInternetBanking.Models;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void btn_Exportar_Excel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=ReporteMarchamos.xls");
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gvMarchamos.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
        protected void btn_Exportar_PDF_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ReporteMarchamos.pdf");
            Response.Charset = "";
            Response.ContentType = "application/pdf";

            //To Export all pages.
            gvMarchamos.AllowPaging = false;

            using (StringWriter sw = new StringWriter())
            {

                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    gvMarchamos.RenderControl(hw);
                    StringReader sr = new StringReader(sw.ToString());
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(new Paragraph(string.Format("Reporte marchamos del {0}", DateTime.Now)));
                    pdfDoc.Add(new Paragraph("_"));
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }
            }
        }

        protected void btn_Exportar_CSV_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ReporteMarchamos.csv");
            Response.Charset = "";
            Response.ContentType = "text/csv";

            //To Export all pages.
            gvMarchamos.AllowPaging = false;
            //this.BindGrid();

            StringBuilder sb = new StringBuilder();
            foreach (TableCell cell in gvMarchamos.HeaderRow.Cells)
            {
                //Append data with separator.
                sb.Append(cell.Text + ',');
            }
            //Append new line character.
            sb.Append("\r\n");

            foreach (GridViewRow row in gvMarchamos.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    //Append data with separator.
                    sb.Append(cell.Text + ',');
                }
                //Append new line character.
                sb.Append("\r\n");
            }

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        protected void btn_Exportar_Portapapeles_Click(object sender, EventArgs e)
        {
            
        }

        protected void btn_Exportar_Word_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/ms-word";
            Response.AddHeader("content-disposition", "attachment;filename=ReporteMarchamos.doc");
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gvMarchamos.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }
}