<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmPrestamo.aspx.cs" Inherits="AppWebInternetBanking.Views.frmPrestamo" %>
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
                 $("#MainContent_gvPrestamos tr").filter(function () {
                     $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                 });
             });
         });
     </script> 
     <h1>Mantenimiento de Prestamos</h1>
    <div class="container">
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvPrestamos" runat="server" AutoGenerateColumns="false"
      CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" 
        HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White" 
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvPrestamos_RowCommand" >
        <Columns>
            <asp:BoundField HeaderText="Codigo Prestamo" DataField="CodigoPrestamo" />
            <asp:BoundField HeaderText="Codigo Usuario" DataField="CodigoUsuario" />
            <asp:BoundField HeaderText="Fecha Prestamo" DataField="FechaHora" />
            <asp:BoundField HeaderText="Monto" DataField="Monto" />
            <asp:BoundField HeaderText="Plazo" DataField="Plazo" />
            <asp:BoundField HeaderText="Tasa" DataField="Tasa" />

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
        <h4 class="modal-title">Mantenimiento de Prestamos</h4>
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
                  <td><asp:Literal ID="ltrCodigoUsuario" Text="Codigo Usuario" runat="server" /></td>
                  <td><asp:DropDownList ID="ddlUsuarios" CssClass="form-control" runat="server" ></asp:DropDownList></td>
              </tr>
              <!--
              <tr>
                  <td><asp:Literal ID="ltrFechaHora" Text="Fecha" runat="server" /></td>
                  <td><asp:TextBox TextMode="DateTimeLocal" ID="txtFechaHora" runat="server" CssClass="form-control" /></td>
                  <td><asp:RequiredFieldValidator ID="rfvFechaHora" runat="server"
               ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtFechaHora" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
              </tr>  -->
              <tr>
                  <td><asp:Literal ID="ltrMonto" Text="Monto" runat="server" /></td>
                  <td><asp:TextBox TextMode="Number" min="1" ID="txtMonto" runat="server" CssClass="form-control" /></td>
                  <td><asp:RequiredFieldValidator ID="rfvMonto" runat="server"
               ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtMonto" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
              </tr>
              <tr>
                  <td><asp:Literal Text="Plazo" runat="server" /></td>
                  <td> <asp:DropDownList ID="ddlPlazo"  CssClass="form-control" runat="server">
                    <asp:ListItem Value="3 meses">3 meses</asp:ListItem>
                    <asp:ListItem Value="6 meses">6 meses</asp:ListItem>
                    <asp:ListItem Value="12 meses">12 meses</asp:ListItem>
                </asp:DropDownList></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrTasa" Text="Tasa" runat="server" /></td>
                   <td> <asp:DropDownList ID="ddlTasa"  CssClass="form-control" runat="server">
                    <asp:ListItem Value="1%">1%</asp:ListItem>
                    <asp:ListItem Value="2%">2%</asp:ListItem>
                    <asp:ListItem Value="3%">3%</asp:ListItem>
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
