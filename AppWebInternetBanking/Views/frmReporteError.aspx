<%@ Page Async="true" Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReporteError.aspx.cs" Inherits="AppWebInternetBanking.Views.frmReporteError" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Reporte de Errores</h1>
    <div class="container">
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvErrores" runat="server" AutoGenerateColumns="false"
        CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" 
        HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White" 
        AlternatingRowStyle-BackColor="LightBlue" >
        <Columns>
            <asp:BoundField HeaderText="Codigo" DataField="Codigo" />
            <asp:BoundField HeaderText="Usuario" DataField="CodigoUsuario" />
            <asp:BoundField HeaderText="Fecha" DataField="FechaHora" />
            <asp:BoundField HeaderText="Fuente" DataField="Fuente" />
            <asp:BoundField HeaderText="Numero" DataField="Numero" />
            <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" />
            <asp:BoundField HeaderText="Vista" DataField="Vista" />
            <asp:BoundField HeaderText="Accion" DataField="Accion" />
        </Columns>
    </asp:GridView>
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
    </div>
</asp:Content>
