<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmCuenta.aspx.cs" Inherits="AppWebInternetBanking.Views.frmCuenta" %>
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
                     $("#MainContent_gvCuentas tr").filter(function () {
                         $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                     });
                 });
             });
         </script> 
    <h1>Mantenimiento de Cuentas</h1>
     <div class="container">
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvCuentas" runat="server" AutoGenerateColumns="false"
      CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" 
        HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White" 
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvCuentas_RowCommand" >
        <Columns>
            <asp:BoundField HeaderText="Codigo Cuenta" DataField="Codigo" />
            <asp:BoundField HeaderText="Codigo Usuario" DataField="CodigoUsuario" />
            <asp:BoundField HeaderText="Codigo Moneda" DataField="CodigoMoneda" />
            <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" />
            <asp:BoundField HeaderText="IBAN" DataField="IBAN" />
             <asp:BoundField HeaderText="Saldo" DataField="Saldo" />
            <asp:BoundField HeaderText="Estado" DataField="Estado" />
            <asp:ButtonField HeaderText="Modificar" CommandName="Modificar" 
                 ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn btn-primary" ButtonType="Button" Text="Modificar" />
            <asp:ButtonField HeaderText="Eliminar" CommandName="Eliminar"
                 ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Eliminar" />
        </Columns>
    </asp:GridView>
         
    <asp:LinkButton type="Button" CssClass="btn btn-success" ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
      Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo"/>
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
        <h4 class="modal-title">Mantenimiento de Cuentas</h4>
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
                  <td><asp:Literal ID="ltrCodigoMant" Text="Codigo Cuenta" runat="server" /></td>
                  <td><asp:TextBox ID="txtCodigoMant" runat="server" Enabled="false" CssClass="form-control" /></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrCodigoUsuario" Text="Codigo Usuario" runat="server" /></td>
                  <td><asp:DropDownList ID="ddlUsuarios" CssClass="form-control" runat="server" ></asp:DropDownList></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrCodigoMoneda" Text="Codigo Moneda" runat="server" /></td>
                 <td><asp:DropDownList ID="ddlCodigoMoneda" CssClass="form-control" runat="server" ></asp:DropDownList></td>
              </tr>

              <tr>
                  <td><asp:Literal ID="ltrDescripcion" Text="Descripcion" runat="server" /></td>
                  <td><asp:DropDownList ID="ddlDescripcion" CssClass="form-control" runat="server" >
                    <asp:ListItem Value="Cuenta corriente">Cuenta corriente</asp:ListItem>
                    <asp:ListItem Value="Cuenta de ahorro">Cuenta de ahorro</asp:ListItem>
                    <asp:ListItem Value="Cuenta de nómina">Cuenta de nómina</asp:ListItem>
                    <asp:ListItem Value="Cuenta remunerada">Cuenta remunerada</asp:ListItem>
                    <asp:ListItem Value="Cuenta con chequera">Cuenta con chequera</asp:ListItem>
                  </asp:DropDownList></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrIBAN" Text="IBAN" runat="server" /></td>
                  <td><asp:TextBox ID="txtIBAN" MaxLength="21" runat="server" CssClass="form-control" /></td>
                  <td><asp:RequiredFieldValidator ID="rfvIBAN" runat="server"
               ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtIBAN" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
              </tr>

              <tr>
                  <td><asp:Literal ID="ltrSaldo" Text="Saldo" runat="server" /></td>
                  <td><asp:TextBox ID="txtSaldo" TextMode="Number" min="1" runat="server" CssClass="form-control" /></td>
                  <td><asp:RequiredFieldValidator ID="rfvSaldo" runat="server"
               ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtSaldo" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
              </tr>

              <tr>
                  <td><asp:Literal Text="Estado" runat="server" /></td>
                  <td> <asp:DropDownList ID="ddlEstadoMant"  CssClass="form-control" runat="server">
                    <asp:ListItem Value="A">Activo</asp:ListItem>
                    <asp:ListItem Value="I">Inactivo</asp:ListItem>
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
