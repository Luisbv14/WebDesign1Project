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
    public partial class frmTransferencia : System.Web.UI.Page
    {
        IEnumerable<Transferencia> transferencias = new ObservableCollection<Transferencia>();
        TransferenciaManager transferenciaManager = new TransferenciaManager();

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}