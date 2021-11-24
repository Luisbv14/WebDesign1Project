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
    public partial class frmReporteEstadistica : System.Web.UI.Page
    {
        IEnumerable<Estadistica> estadisticas = new ObservableCollection<Estadistica>();
        EstadisticaManager estadisticaManager = new EstadisticaManager();

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
                estadisticas = await estadisticaManager.ObtenerEstadisticas(Session["Token"].ToString());
                gvEstadisticas.DataSource = estadisticas.ToList();
                gvEstadisticas.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de estadisticas " + e.Message;
                lblStatus.Visible = true;
            }
        }
    }
}