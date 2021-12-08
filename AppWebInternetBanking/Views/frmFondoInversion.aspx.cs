using AppWebInternetBanking.Controllers;
using AppWebInternetBanking.Models;
using iTextSharp.text;
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
    public partial class frmFondoInversion : System.Web.UI.Page
    {
        IEnumerable<FondoInversion> fondoInversiones = new ObservableCollection<FondoInversion>();
        FondoInversionManager fondoInversionManager = new FondoInversionManager();
        static string _codigo = string.Empty;

        IEnumerable<Cuenta> cuentas = new ObservableCollection<Cuenta>();
        CuentaManager cuentaManager = new CuentaManager();

        public string labelsGrafico = string.Empty;
        public string backgroundcolorsGrafico = string.Empty;
        public string dataGrafico = string.Empty;

       async protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                {
                    fondoInversiones = await fondoInversionManager.ObtenerFondoInversiones(Session["Token"].ToString());
                    InicializarControles();
                    ObtenerDatosgrafico();
                }
                    
            }
        }
        private void ObtenerDatosgrafico()
        {
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColors = new StringBuilder();

            var random = new Random();

            foreach (var fondoInversion in fondoInversiones.GroupBy(e => e.Plazo).
                Select(group => new
                {
                    Plazo = group.Key,
                    Cantidad = group.Count()
                }).OrderBy(x => x.Plazo))
            {
                string color = String.Format("#{0:X6}", random.Next(0x1000000));
                labels.Append(string.Format("'{0}',", fondoInversion.Plazo));
                data.Append(string.Format("'{0}',", fondoInversion.Cantidad));
                backgroundColors.Append(string.Format("'{0}',", color));

                labelsGrafico = labels.ToString().Substring(0, labels.Length - 1);
                dataGrafico = data.ToString().Substring(0, data.Length - 1);
                backgroundcolorsGrafico = backgroundColors.ToString().Substring(backgroundColors.Length - 1);
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
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btn_Exportar_Excel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=ReporteFondoInversiones.xls");
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gvFondoInversiones.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
        protected void btn_Exportar_PDF_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ReporteFondoInversiones.pdf");
            Response.Charset = "";
            Response.ContentType = "application/pdf";

            //To Export all pages.
            gvFondoInversiones.AllowPaging = false;

            using (StringWriter sw = new StringWriter())
            {

                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    gvFondoInversiones.RenderControl(hw);
                    StringReader sr = new StringReader(sw.ToString());
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(new Paragraph(string.Format("Reporte Fondo de inversiones del {0}", DateTime.Now)));
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
            Response.AddHeader("content-disposition", "attachment;filename=ReporteFondoInversiones.csv");
            Response.Charset = "";
            Response.ContentType = "text/csv";

            //To Export all pages.
            gvFondoInversiones.AllowPaging = false;
            //this.BindGrid();

            StringBuilder sb = new StringBuilder();
            foreach (TableCell cell in gvFondoInversiones.HeaderRow.Cells)
            {
                //Append data with separator.
                sb.Append(cell.Text + ',');
            }
            //Append new line character.
            sb.Append("\r\n");

            foreach (GridViewRow row in gvFondoInversiones.Rows)
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
            Response.AddHeader("content-disposition", "attachment;filename=ReporteFondoInversiones.doc");
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gvFondoInversiones.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }
}