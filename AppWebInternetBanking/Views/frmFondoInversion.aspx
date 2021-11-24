<%@ Page Async="true" Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmFondoInversion.aspx.cs" Inherits="AppWebInternetBanking.Views.frmFondoInversion" %>
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
                 $("#MainContent_gvFondoInversiones tr").filter(function () {
                     $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                 });
             });
         });
    </script> 

    <h1>Mantenimiento de Fondos de Inversiones</h1>
    <div class="container">
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvFondoInversiones" runat="server" AutoGenerateColumns="false"
      CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" 
        HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White" 
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvFondoInversiones_RowCommand" >
        <Columns>
            <asp:BoundField HeaderText="Codigo Inversión" DataField="CodigoInversion" />
            <asp:BoundField HeaderText="Codigo Cuenta" DataField="CodigoCuenta" />
            <asp:BoundField HeaderText="Plazo" DataField="Plazo" />
             <asp:BoundField HeaderText="Tasa" DataField="Tasa" />
             <asp:BoundField HeaderText="Monto" DataField="Monto" />
             <asp:BoundField HeaderText="Fecha Inicio" DataField="FechaHoraInicio" />
            <asp:ButtonField HeaderText="Modificar" CommandName="Modificar" 
                ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn btn-primary" ButtonType="Button" Text="Modificar" />
            <asp:ButtonField HeaderText="Eliminar" CommandName="Eliminar"
                ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Eliminar" />
        </Columns>
    </asp:GridView>
     <asp:LinkButton type="Button" CssClass="btn btn-info" ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
      Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo"    />
    
    <asp:LinkButton type="Button" CssClass="btn btn-success" ID="btn_Exportar_Excel" runat="server" OnClick="btn_Exportar_Excel_Click"
      Text="<span aria-hidden='true' class='glyphicon glyphicon-arrow-down'></span> Excel"    />
    
    <asp:LinkButton type="Button" CssClass="btn btn-primary" ID="btn_Exportar_Word" runat="server" OnClick="btn_Exportar_Word_Click"
      Text="<span aria-hidden='true' class='glyphicon glyphicon-arrow-down'></span> Word"    />
    
    <asp:LinkButton type="Button" CssClass="btn btn-danger" ID="btn_Exportar_PDF" runat="server" OnClick="btn_Exportar_PDF_Click"
      Text="<span aria-hidden='true' class='glyphicon glyphicon-arrow-down'></span> PDF"    />
    
    <asp:LinkButton type="Button" CssClass="btn btn-success" ID="btn_Exportar_CSV" runat="server" OnClick="btn_Exportar_CSV_Click"
      Text="<span aria-hidden='true' class='glyphicon glyphicon-arrow-down'></span> CSV"    />
    
    <asp:LinkButton type="Button" CssClass="btn btn-warning" ID="btn_Exportar_Portapapeles" runat="server" OnClick="btn_Exportar_Portapapeles_Click"
      Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-saved'></span> Portapapeles "    />
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
        <h4 class="modal-title">Mantenimiento de Fondos de Inversiones</h4>
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
                  <td><asp:Literal ID="ltrCodigoMant" Text="Codigo Inversión" runat="server" /></td>
                  <td><asp:TextBox ID="txtCodigoMant" runat="server" Enabled="false" CssClass="form-control" /></td>
              </tr>
              <tr>
                  <td><asp:Literal ID="ltrCodigoCuenta" Text="Codigo Cuenta" runat="server" /></td>
                 <td><asp:DropDownList ID="ddlCodigoCuenta" CssClass="form-control" runat="server"></asp:DropDownList></td>
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
              <tr>
                  <td><asp:Literal ID="ltrMonto" Text="Monto" runat="server" /></td>
                  <td><asp:TextBox TextMode="Number" min="1" ID="txtMonto" runat="server" CssClass="form-control" /></td>
                  <td><asp:RequiredFieldValidator ID="rfvMonto" runat="server"
               ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtMonto" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
              </tr>
               <!--
              <tr>
                  <td><asp:Literal ID="ltrFechaHoraInicio" Text="Fecha" runat="server" /></td>
                  <td><asp:TextBox TextMode="DateTimeLocal" ID="txtFechaHoraInicio" runat="server" CssClass="form-control" /></td>
                  <td><asp:RequiredFieldValidator ID="rfvFechaHoraInicio" runat="server"
               ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtFechaHoraInicio" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
             </tr>  -->
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
