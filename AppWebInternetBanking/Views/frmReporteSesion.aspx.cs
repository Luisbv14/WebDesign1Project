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
    public partial class frmReporteSesion : System.Web.UI.Page
    {
        IEnumerable<Sesion> sesiones = new ObservableCollection<Sesion>();
        SesionManager sesionManager = new SesionManager();

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
                sesiones = await sesionManager.ObtenerSesiones(Session["Token"].ToString());
                gvSesiones.DataSource = sesiones.ToList();
                gvSesiones.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de sesiones " + e.Message;
                lblStatus.Visible = true;
            }
        }
    }
}