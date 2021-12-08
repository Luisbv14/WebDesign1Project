<%@ Page Async="true" Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmTransferencia.aspx.cs" Inherits="AppWebInternetBanking.Views.frmTransferencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.12/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" />
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.2/pdfmake.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.2/vfs_fonts.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>
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
                $("#MainContent_gvTransferencias tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
        $(document).ready(function () {
            $('[id*=gvTransferencias]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                dom: 'Bfrtip',
                'aoColumnDefs': [{ 'bSortable': false, 'aTargets': [0] }],
                'iDisplayLength': 20,
                buttons: [
                    { extend: 'copy', text: 'Copy to clipboard', className: 'exportExcel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'excel', text: 'Export to Excel', className: 'exportExcel', filename: 'Transferencias_Excel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'csv', text: 'Export to CSV', className: 'exportExcel', filename: 'Transferencias_Csv', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'pdf', text: 'Export to PDF', className: 'exportExcel', filename: 'Transferencias_Pdf', orientation: 'landscape', pageSize: 'LEGAL', exportOptions: { modifier: { page: 'all' }, columns: ':visible' } }
                ]
            });
        });
    </script>

    <h1>Procesamiento de Transferencias</h1>
    <div class="container">
        <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
        <br />
        <asp:GridView ID="gvTransferencias" runat="server" AutoGenerateColumns="false"
            CssClass="table table-sm" HeaderStyle-CssClass="thead-dark"
            HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White"
            AlternatingRowStyle-BackColor="LightBlue">
            <Columns>
                <asp:BoundField HeaderText="Codigo Transferencia" DataField="Codigo" />
                <asp:BoundField HeaderText="Cuenta Origen" DataField="CuentaOrigen" />
                <asp:BoundField HeaderText="Cuenta Destino" DataField="CuentaDestino" />
                <asp:BoundField HeaderText="Fecha Transferencia" DataField="FechaHora" />
                <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" />
                <asp:BoundField HeaderText="Monto" DataField="Monto" />
                <asp:BoundField HeaderText="Estado" DataField="Estado" />
            </Columns>
        </asp:GridView>
        <asp:LinkButton type="Button" CssClass="btn btn-info" ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
            Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo" />

        <asp:LinkButton type="Button" CssClass="btn btn-success" ID="btn_Exportar_Excel" runat="server" OnClick="btn_Exportar_Excel_Click"
            Text="<span aria-hidden='true' class='glyphicon glyphicon-arrow-down'></span> Excel" />

        <asp:LinkButton type="Button" CssClass="btn btn-primary" ID="btn_Exportar_Word" runat="server" OnClick="btn_Exportar_Word_Click"
            Text="<span aria-hidden='true' class='glyphicon glyphicon-arrow-down'></span> Word" />

        <asp:LinkButton type="Button" CssClass="btn btn-danger" ID="btn_Exportar_PDF" runat="server" OnClick="btn_Exportar_PDF_Click"
            Text="<span aria-hidden='true' class='glyphicon glyphicon-arrow-down'></span> PDF" />

        <asp:LinkButton type="Button" CssClass="btn btn-success" ID="btn_Exportar_CSV" runat="server" OnClick="btn_Exportar_CSV_Click"
            Text="<span aria-hidden='true' class='glyphicon glyphicon-arrow-down'></span> CSV" />

        <asp:LinkButton type="Button" CssClass="btn btn-warning" ID="btn_Exportar_Portapapeles" runat="server" OnClick="btn_Exportar_Portapapeles_Click"
            Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-saved'></span> Portapapeles " />
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
                    <h4 class="modal-title">
                        <asp:Literal ID="ltrTituloMantenimiento" runat="server"></asp:Literal></h4>
                </div>
                <div class="modal-body">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodigoMant" Text="Codigo" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodigoMant" runat="server" Enabled="false" CssClass="form-control" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCuentaOrigen" Text="Cuenta Origen" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlCuentaOrigen" CssClass="form-control" runat="server"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCuentaDestino" Text="Cuenta Destino" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlCuentaDestino" CssClass="form-control" runat="server"></asp:DropDownList></td>
                        </tr>
                        <!--
              <tr>
                  <td><asp:Literal ID="ltrFechaHora" Text="Fecha Transferencia" runat="server" /></td>
                  <td><asp:TextBox TextMode="DateTimeLocal" ID="txtFechaHora" runat="server" CssClass="form-control" /></td>
              </tr> -->
                        <tr>
                            <td>
                                <asp:Literal ID="ltrDescripcion" Text="Descripcion" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server"
                                    ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtDescripcion" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrMonto" Text="Monto" runat="server" /></td>
                            <td>
                                <asp:TextBox TextMode="Number" min="1" ID="txtMonto" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvMonto" runat="server"
                                    ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtMonto" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal Text="Estado" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlEstadoMant" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="A">Activo</asp:ListItem>
                                    <asp:ListItem Value="I">Inactivo</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" OnClick="btnAceptarMant_Click" CssClass="btn btn-success" ID="btnAceptarMant" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
                    <asp:LinkButton type="button" OnClick="btnCancelarMant_Click" CssClass="btn btn-danger" ID="btnCancelarMant" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
