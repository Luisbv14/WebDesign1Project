<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmSolicitudCita.aspx.cs" Inherits="AppWebInternetBanking.Views.frmSolicitudCita" %>
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
                 $("#MainContent_gvSolicitudCitas tr").filter(function () {
                     $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                 });
             });
         });
    </script> 

     <h1>Mantenimiento de solicitud citas</h1>
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvSolicitudCitas" runat="server" AutoGenerateColumns="false"
      CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" 
        HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White" 
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvSolicitudCitas_RowCommand" >
        <Columns>
            <asp:BoundField HeaderText="Codigo Cita" DataField="CodigoCita" />
            <asp:BoundField HeaderText="Codigo Usuario" DataField="CodigoUsuario" />
            <asp:BoundField HeaderText="Tipo Cita" DataField="TipoCita" />
            <asp:BoundField HeaderText="Fecha Cita" DataField="FechaHoraCita" />
            <asp:BoundField HeaderText="Sede" DataField="Sede" />
            <asp:ButtonField HeaderText="Modificar" CommandName="Modificar" 
                ControlStyle-CssClass="btn btn-primary" ButtonType="Button" Text="Modificar" />
            <asp:ButtonField HeaderText="Eliminar" CommandName="Eliminar"
                ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Eliminar" />
        </Columns>
    </asp:GridView>
    <asp:LinkButton type="Button" CssClass="btn btn-success" ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
      Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo"    />
    <br />
    <asp:Label ID="lblStatus" ForeColor="Maroon" runat="server" Visible="false" />

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
                  <td><asp:Literal ID="ltrCodigoMant" Text="Codigo Cita" runat="server" /></td>
                  <td><asp:TextBox ID="txtCodigoMant" runat="server" Enabled="false" CssClass="form-control" /></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrCodigoUsuario" Text="Codigo Usuario" runat="server" /></td>
                  <td><asp:TextBox TextMode="Number" ID="txtCodigoUsuario" runat="server" CssClass="form-control" /></td>
              </tr>

              <tr>
                  <td><asp:Literal ID="ltrTipoCita" Text="Tipo Cita" runat="server" /></td>
                   <td> <asp:DropDownList ID="ddlTipoCita"  CssClass="form-control" runat="server">
                    <asp:ListItem Value="Licencia-Primera vez">Licencia-Primera vez</asp:ListItem>
                    <asp:ListItem Value="Licencia-Renovacion">Licencia-Renovacion</asp:ListItem>
                    <asp:ListItem Value="Licencia-Duplicidad">Licencia-Duplicidad</asp:ListItem>
                    <asp:ListItem Value="Permiso de conducir">Permiso de conducir</asp:ListItem>
                    <asp:ListItem Value="Pasaporte-Primera vez">Pasaporte-Primera vez</asp:ListItem>
                    <asp:ListItem Value="Pasaporte-Renovacion">Pasaporte-Renovacion</asp:ListItem>
                    <asp:ListItem Value="Pasaporte-Menores">Pasaporte-Menores</asp:ListItem>
                    <asp:ListItem Value="Pasaporte-Pedida/Robo">Pasaporte-Pedida/Robo</asp:ListItem>
                </asp:DropDownList></td>
              </tr>

              <tr>
                  <td><asp:Literal ID="ltrFechaHoraCita" Text="Fecha cita" runat="server" /></td>
                  <td><asp:TextBox TextMode="DateTimeLocal" ID="txtFechaHoraCita" runat="server" CssClass="form-control" /></td>
              </tr>
              <tr>
                  <td><asp:Literal Text="Sede" runat="server" /></td>
                  <td> <asp:DropDownList ID="ddlSede"  CssClass="form-control" runat="server">
                    <asp:ListItem Value="San Jose">San Jose</asp:ListItem>
                    <asp:ListItem Value="Alajuela">Alajuela</asp:ListItem>
                    <asp:ListItem Value="Puntarenas">Puntarenas</asp:ListItem>
                    <asp:ListItem Value="Cartago">Cartago</asp:ListItem>
                    <asp:ListItem Value="Limon">Limon</asp:ListItem>
                    <asp:ListItem Value="Guanacaste">Guanacaste</asp:ListItem>
                    <asp:ListItem Value="Heredia">Heredia</asp:ListItem>
                </asp:DropDownList></td>
              </tr>
          </table>
          <asp:Label ID="lblResultado" ForeColor="Maroon" Visible="False" runat="server" />
      </div>
      <div class="modal-footer">
        <asp:LinkButton type="button" OnClick="btnAceptarMant_Click" CssClass="btn btn-success" ID="btnAceptarMant" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
         <asp:LinkButton type="button" OnClick="btnCancelarMant_Click"  CssClass="btn btn-danger" ID="btnCancelarMant"  runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
      </div>
    </div>
  </div>
</div>

     <!-- VENTANA MODAL -->
  <div id="myModal" class="modal fade" role="dialog">
  <div class="modal-dialog modal-sm">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Mantenimiento de solicitudes de citas</h4>
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
</asp:Content>
