﻿<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmSinpeMovil.aspx.cs" Inherits="AppWebInternetBanking.Views.frmSinpeMovil" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

         function openModal() {
             $('#myModal').modal('show'); //ventana de mensajes
         }

         function openModalMantenimiento() {
             $('#myModalMantenimiento').modal('show'); //ventana de mantenimiento
         }

         function CloseModal() {
             $('#myModal').modal('hide');//cierra ventana de mensajes
         }

         function CloseMantenimiento() {
             $('#myModalMantenimiento').modal('hide'); //cierra ventana de mantenimiento
         }

         $(document).ready(function () { //filtrar el datagridview
             $("#myInput").on("keyup", function () {
                 var value = $(this).val().toLowerCase();
                 $("#MainContent_gvSinpeMoviles tr").filter(function () {
                     $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                 });
             });
         });
    </script>
      <h1>Mantenimiento de Sinpe Movil</h1>
    <div class="container">
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvSinpeMoviles" runat="server" AutoGenerateColumns="false"
      CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" 
        HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White" 
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvSinpeMoviles_RowCommand" >
        <Columns>
            <asp:BoundField HeaderText="Codigo Sinpe" DataField="CodigoSinpe" />
            <asp:BoundField HeaderText="Codigo Cuenta" DataField="CodigoCuenta" />
            <asp:BoundField HeaderText="Tel.Emisor" DataField="NumeroTelefonoEmisor" />
            <asp:BoundField HeaderText="Tel.Remitente" DataField="NumeroTelefonoRemitente" />
            <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" />
            <asp:BoundField HeaderText="Monto" DataField="Monto" />
            <asp:BoundField HeaderText="Fecha Envio" DataField="FechaHora" />
            <asp:BoundField HeaderText="Comision" DataField="Comision" />
            <asp:ButtonField HeaderText="Modificar" CommandName="Modificar" 
                 ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn btn-primary" ButtonType="Button" Text="Modificar" />
            <asp:ButtonField HeaderText="Eliminar" CommandName="Eliminar"
                 ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Eliminar" />
        </Columns>
    </asp:GridView>
    <asp:LinkButton type="Button" CssClass="btn btn-success" ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
      Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo"    />
    <br />
    <asp:Label ID="lblStatus" ForeColor="Maroon" runat="server" Visible="false" />
    <asp:Label ID="lblResultado" ForeColor="Maroon" Visible="False" runat="server" />
    </div>
          <!-- VENTANA MODAL -->
  <div id="myModal" class="modal fade" role="dialog">
  <div class="modal-dialog modal-sm">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Mantenimiento de Sinpe Movil</h4>
      </div>
      <div class="modal-body">
        <p><asp:Literal ID="ltrModalMensaje" runat="server" /></p>
      </div>
      <div class="modal-footer">
         <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnAceptarModal" OnClick="btnAceptarModal_Click"  runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
         <asp:LinkButton type="button"  CssClass="btn btn-danger" ID="btnCancelarModal" OnClick="btnCancelarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
      </div>
    </div>
  </div>
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
                  <td><asp:Literal ID="ltrCodigoMant" Text="Codigo" runat="server" /></td>
                  <td><asp:TextBox ID="txtCodigoMant" runat="server" Enabled="false" CssClass="form-control" /></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrCodigoCuenta" Text="Codigo Cuenta" runat="server" /></td>
                  <td><asp:DropDownList ID="ddlCodigoCuenta" runat="server" CssClass="form-control" /></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrNumeroTelefonoEmisor" Text="Telefono Emisor" runat="server" /></td>
                  <td><asp:TextBox ID="txtNumeroTelefonoEmisor" MaxLength="8" runat="server" CssClass="form-control" /></td>
                  <td><asp:RequiredFieldValidator ID="rfvNumeroTelefonoEmisor" runat="server"
               ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtNumeroTelefonoEmisor" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrNumeroTelefonoRemitente" Text="Telefono Remitente" runat="server" /></td>
                  <td><asp:TextBox ID="txtNumeroTelefonoRemitente" MaxLength="8" runat="server" CssClass="form-control" /></td>
                  <td><asp:RequiredFieldValidator ID="rfvNumeroTelefonoRemitente" runat="server"
               ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtNumeroTelefonoRemitente" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrDescripcion" Text="Descripcion" runat="server" /></td>
                  <td><asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" /></td>
                  <td><asp:RequiredFieldValidator ID="rfvDescripcion" runat="server"
               ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtDescripcion" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrMonto" Text="Monto" runat="server" /></td>
                  <td><asp:TextBox TextMode="Number" min="500" ID="txtMonto" runat="server" CssClass="form-control" /></td>
                  <td><asp:RequiredFieldValidator ID="rfvMonto" runat="server"
               ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtMonto" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
              </tr>
              <!--
              <tr>
                  <td><asp:Literal ID="ltrFechaHora" Text="Fecha" runat="server" /></td>
                  <td><asp:TextBox TextMode="DateTimeLocal" ID="txtFechaHora" runat="server" CssClass="form-control" /></td>
              </tr> -->
              <tr>
                  <td><asp:Literal ID="ltrComision" Text="Comision" runat="server" /></td>
                  <td> <asp:DropDownList ID="ddlComision"  CssClass="form-control" runat="server">
                    <asp:ListItem Value="0">0%</asp:ListItem>
                    <asp:ListItem Value="1">1%</asp:ListItem>
                </asp:DropDownList></td>
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
