<%@ Page Async="true" Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmMarchamo.aspx.cs" Inherits="AppWebInternetBanking.Views.frmMarchamo" %>

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
                $("#MainContent_gvMarchamos tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
        $(document).ready(function () {
            $('[id*=gvMarchamos]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                dom: 'Bfrtip',
                'aoColumnDefs': [{ 'bSortable': false, 'aTargets': [0] }],
                'iDisplayLength': 20,
                buttons: [
                    { extend: 'copy', text: 'Copy to clipboard', className: 'exportExcel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'excel', text: 'Export to Excel', className: 'exportExcel', filename: 'Marchamos_Excel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'csv', text: 'Export to CSV', className: 'exportExcel', filename: 'Marchamos_Csv', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'pdf', text: 'Export to PDF', className: 'exportExcel', filename: 'Marchamos_Pdf', orientation: 'landscape', pageSize: 'LEGAL', exportOptions: { modifier: { page: 'all' }, columns: ':visible' } }
                ]
            });
        });
    </script>

    <h1>Mantenimiento de Marchamos</h1>
    <div class="container">
        <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
        <br />
        <asp:GridView ID="gvMarchamos" runat="server" AutoGenerateColumns="false"
            CssClass="table table-sm" HeaderStyle-CssClass="thead-dark"
            HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White"
            AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvMarchamos_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Codigo Pago" DataField="CodigoPago" />
                <asp:BoundField HeaderText="Codigo Usuario" DataField="CodigoUsuario" />
                <asp:BoundField HeaderText="Placa Vehiculo" DataField="PlacaVehiculo" />
                <asp:BoundField HeaderText="Monto" DataField="Monto" />
                <asp:BoundField HeaderText="Estado" DataField="Estado" />
                <asp:BoundField HeaderText="Fecha Pago" DataField="FechaHoraPago" />
                <asp:ButtonField HeaderText="Modificar" CommandName="Modificar"
                    ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn btn-primary" ButtonType="Button" Text="Modificar" />
                <asp:ButtonField HeaderText="Eliminar" CommandName="Eliminar"
                    ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Eliminar" />
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

    <!-- VENTANA MODAL -->
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Mantenimiento de Marchamos</h4>
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
                                <asp:Literal ID="ltrCodigoMant" Text="Codigo Pago" runat="server" /></td>
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
                                <asp:Literal ID="ltrPlacaVehiculo" Text="Placa Vehiculo" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtPlacaVehiculo" runat="server" CssClass="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvPlacaVehiculo" runat="server"
                                    ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtPlacaVehiculo" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
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
                        <!--
               <tr>
                  <td><asp:Literal ID="ltrFechaHoraPago" Text="Fecha Pago" runat="server" /></td>
                  <td><asp:TextBox TextMode="DateTimeLocal" ID="txtFechaHoraPago" runat="server" CssClass="form-control" /></td>
              <td><asp:RequiredFieldValidator ID="rfvFechaHoraPago" runat="server"
               ErrorMessage="*Espacio Obligatorio*" ControlToValidate="txtFechaHoraPago" ForeColor="Maroon" EnableClientScript="False"></asp:RequiredFieldValidator></td>
               </tr> -->
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
     <div id="canvas-holder" style="width:40%">
		            <canvas id="vistas-chart"></canvas>
	            </div>
              <script >
                  var barColors = ["#76D7C4", "#F4D03F", "#76D7C4", "#F4D03F", "#76D7C4"];

                  new Chart(document.getElementById("vistas-chart"), {
                      type: 'bar',
                      data: {
                          labels: [<%= this.labelsGrafico %>],
                          datasets: [{
                              label: "Pago x Cliente",
                              backgroundColor: barColors,
                            data: [<%= this.dataGrafico %>]
                          }]
                      },
                      options: {

                          title: {
                              display: true,
                              text: 'Cantidad de pago de marchamo por cliente'
                          }
                      }
                  });
              </script>
                </div>
            </div>
</asp:Content>
