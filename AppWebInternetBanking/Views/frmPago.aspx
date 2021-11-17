<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmPago.aspx.cs" Inherits="AppWebInternetBanking.Views.frmPago" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

      <script type="text/javascript">

         function openModalMantenimiento() {
             $('#myModalMantenimiento').modal('show'); //ventana de mantenimiento
         }


         function CloseMantenimiento() {
             $('#myModalMantenimiento').modal('hide'); //cierra ventana de mantenimiento
         }

         $(document).ready(function () { //filtrar el datagridview
             $("#myInput").on("keyup", function () {
                 var value = $(this).val().toLowerCase();
                 $("#MainContent_gvPagos tr").filter(function () {
                     $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                 });
             });
         });
      </script> 
   
    <h1>Procesamiento de Pagos</h1>
    <div class="container">
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvPagos" runat="server" AutoGenerateColumns="false"
      CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" 
        HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White" 
        AlternatingRowStyle-BackColor="LightBlue" >
        <Columns>
            <asp:BoundField HeaderText="Codigo Pago" DataField="Codigo" />
            <asp:BoundField HeaderText="Codigo Servicio" DataField="CodigoServicio" />
            <asp:BoundField HeaderText="Codigo Cuenta" DataField="CodigoCuenta" />
            <asp:BoundField HeaderText="Fecha Pago" DataField="FechaHora" />
            <asp:BoundField HeaderText="Monto" DataField="Monto" />
        </Columns>
    </asp:GridView>
    <asp:LinkButton type="Button" CssClass="btn btn-success" ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
      Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo"    />
    <br />
    <asp:Label ID="lblStatus" ForeColor="Maroon" runat="server" Visible="false" />
    <asp:Label ID="lblResultado" ForeColor="Maroon" Visible="False" runat="server" />
    </div>
      <!--VENTANA DE MANTENIMIENTO -->
  <div id="myModalMantenimiento" class="modal fade" role="dialog">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title"><asp:Literal ID="ltrTituloMantenimiento" runat="server"></asp:Literal></h4>
      </div>
      <div class="modal-body">
          <table style="width: 100%;">
              <tr>
                  <td><asp:Literal ID="ltrCodigoMant" Text="Codigo Pago" runat="server" /></td>
                  <td><asp:TextBox ID="txtCodigoMant" runat="server" Enabled="false" CssClass="form-control" /></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrCodigoServicio" Text="Codigo Servicio" runat="server" /></td>
                 <td><asp:DropDownList ID="ddlCodigoServicio" CssClass="form-control" runat="server" ></asp:DropDownList></td>
              </tr>
              <tr>
                 <td><asp:Literal ID="ltrCodigoCuenta" Text="Codigo Cuenta" runat="server" /></td>
                 <td><asp:DropDownList ID="ddlCodigoCuenta" CssClass="form-control" runat="server" ></asp:DropDownList></td>
              </tr>
              <!--
              <tr>
                  <td><asp:Literal ID="ltrFechaHora" Text="Fecha" runat="server" /></td>
                  <td><asp:TextBox TextMode="DateTimeLocal" ID="txtFechaHora" runat="server" CssClass="form-control" /></td>
              </tr> -->
              <tr>
                  <td><asp:Literal ID="ltrMonto" Text="Monto Servicio" runat="server" /></td>
                  <td><asp:TextBox TextMode="Number" min="1" ID="txtMonto" runat="server" CssClass="form-control" /></td>
                  <td><asp:RequiredFieldValidator ID="rfvMonto" runat="server"
               ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtMonto" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
              </tr>
          </table>
      </div>
      <div class="modal-footer">
        <asp:LinkButton type="button" OnClick="btnAceptarMant_Click" CssClass="btn btn-success" ID="btnAceptarMant" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
         <asp:LinkButton type="button" OnClick="btnCancelarMant_Click"  CssClass="btn btn-danger" ID="btnCancelarMant"  runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
      </div>
    </div>
  </div>
</div>
</asp:Content>
