<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmSolicitudCita.aspx.cs" Inherits="AppWebInternetBanking.Views.frmSolicitudCita" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.12/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" />
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.2/pdfmake.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.2/vfs_fonts.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>
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
        $(document).ready(function () {
            $('[id*=gvSolicitudCitas]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                dom: 'Bfrtip',
                'aoColumnDefs': [{ 'bSortable': false, 'aTargets': [0] }],
                'iDisplayLength': 20,
                buttons: [
                    { extend: 'copy', text: 'Copy to clipboard', className: 'exportExcel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'excel', text: 'Export to Excel', className: 'exportExcel', filename: 'Citas_Excel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'csv', text: 'Export to CSV', className: 'exportExcel', filename: 'Citas_Csv', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'pdf', text: 'Export to PDF', className: 'exportExcel', filename: 'Citas_Pdf', orientation: 'landscape', pageSize: 'LEGAL', exportOptions: { modifier: { page: 'all' }, columns: ':visible' } }
                ]
            });
        });
    </script>

    <h1>Mantenimiento de Solicitudes de Citas</h1>
    <div class="container">
        <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
        <br />
        <asp:GridView ID="gvSolicitudCitas" runat="server" AutoGenerateColumns="false"
            CssClass="table table-sm" HeaderStyle-CssClass="thead-dark"
            HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White"
            AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvSolicitudCitas_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Codigo Cita" DataField="CodigoCita" />
                <asp:BoundField HeaderText="Codigo Usuario" DataField="CodigoUsuario" />
                <asp:BoundField HeaderText="Tipo Cita" DataField="TipoCita" />
                <asp:BoundField HeaderText="Fecha Cita" DataField="FechaHoraCita" />
                <asp:BoundField HeaderText="Sede" DataField="Sede" />
                <asp:ButtonField HeaderText="Modificar" CommandName="Modificar"
                    ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn btn-primary" ButtonType="Button" Text="Modificar" />
                <asp:ButtonField HeaderText="Eliminar" CommandName="Eliminar"
                    ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Eliminar" />
            </Columns>
        </asp:GridView>
        <asp:LinkButton type="Button" CssClass="btn btn-success" ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
            Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo" />
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
                    <h4 class="modal-title">Mantenimiento de Solicitudes de Citas</h4>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Literal ID="ltrModalMensaje" runat="server" /></p>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnAceptarModal" OnClick="btnAceptarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
                    <asp:LinkButton type="button" CssClass="btn btn-danger" ID="btnCancelarModal" OnClick="btnCancelarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
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
                    <h4 class="modal-title">
                        <asp:Literal ID="ltrTituloMantenimiento" runat="server"></asp:Literal></h4>
                </div>
                <div class="modal-body">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodigoMant" Text="Codigo Cita" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodigoMant" runat="server" Enabled="false" CssClass="form-control" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodigoUsuario" Text="Codigo Usuario" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlUsuarios" CssClass="form-control" runat="server"></asp:DropDownList></td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Literal ID="ltrTipoCita" Text="Tipo Cita" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlTipoCita" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="Licencia - Primera vez">Licencia - Primera vez</asp:ListItem>
                                    <asp:ListItem Value="Licencia - Renovacion">Licencia - Renovacion</asp:ListItem>
                                    <asp:ListItem Value="LicenciaLicencia - Duplicidad">Licencia - Duplicidad</asp:ListItem>
                                    <asp:ListItem Value="Licencia - Permiso de conducir">Licencia - Permiso de conducir</asp:ListItem>
                                    <asp:ListItem Value="Pasaporte - Primera vez">Pasaporte - Primera vez</asp:ListItem>
                                    <asp:ListItem Value="Pasaporte - Renovacion">Pasaporte - Renovacion</asp:ListItem>
                                    <asp:ListItem Value="Pasaporte - Menores">Pasaporte - Menores</asp:ListItem>
                                    <asp:ListItem Value="Pasaporte - Pedida/Robo">Pasaporte - Pedida/Robo</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Literal ID="ltrFechaHoraCita" Text="Fecha cita" runat="server" /></td>
                            <td>
                                <asp:TextBox TextMode="DateTimeLocal" ID="txtFechaHoraCita" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvFechaHoraCita" runat="server"
                                    ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtFechaHoraCita" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal Text="Sede" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlSede" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="San José">San José</asp:ListItem>
                                    <asp:ListItem Value="Alajuela">Alajuela</asp:ListItem>
                                    <asp:ListItem Value="Limón">Limón</asp:ListItem>
                                    <asp:ListItem Value="Cartago">Cartago</asp:ListItem>
                                    <asp:ListItem Value="Heredia">Heredia</asp:ListItem>
                                    <asp:ListItem Value="Puntarenas">Puntarenas</asp:ListItem>
                                    <asp:ListItem Value="Guanacaste">Guanacaste</asp:ListItem>
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
    <br />
    <br />
    <div class="row">
        <div class="col-sm">
            <div id="canvas-holder" style="width: 40%">
                <canvas id="solicitudCita-chart"></canvas>
            </div>
            <script>
                var colors = ["#000099", "#33cc33", "#ff0066", "#000099", "#000099", "#000099"]; //cambiar los colores de las barras tiene que ser 1 por cada uno
                new Chart(document.getElementById("solicitudCita-chart"), {
                    type: 'bar',
                    data: {
                        labels: [<%= this.labelsGrafico %>],
                        datasets: [{
                            label: "Cantidad por Cita",
                            backgroundColor: colors,
                            data: [<%= this.dataGrafico %>]
                        }]
                    },
                    options: {
                        title: {
                            display: true,
                            text: 'Cantidad de citas por cada tipo'
                        }
                    }
                });
            </script>
        </div>
    </div>

</asp:Content>
