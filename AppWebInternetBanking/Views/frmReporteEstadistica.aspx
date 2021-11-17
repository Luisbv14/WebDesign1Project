<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReporteEstadistica.aspx.cs" Inherits="AppWebInternetBanking.Views.frmReporteEstadistica" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Reporte de Estadisticas</h1>
    <div class="container">
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvEstadisticas" runat="server" AutoGenerateColumns="false"
        CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" 
        HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White" 
        AlternatingRowStyle-BackColor="LightBlue" >
        <Columns>
            <asp:BoundField HeaderText="Codigo" DataField="Codigo" />
            <asp:BoundField HeaderText="Usuario" DataField="CodigoUsuario" />
            <asp:BoundField HeaderText="Fecha" DataField="FechaHora" />
            <asp:BoundField HeaderText="Dispositivo" DataField="PlataformaDispositivo" />
            <asp:BoundField HeaderText="Navegador" DataField="Navegador" />
            <asp:BoundField HeaderText="Vista" DataField="Vista" />
            <asp:BoundField HeaderText="Accion" DataField="Accion" />
        </Columns>
    </asp:GridView>
    <br />
    <asp:Label ID="lblStatus" ForeColor="Maroon" runat="server" Visible="false" />
    </div>
</asp:Content>
