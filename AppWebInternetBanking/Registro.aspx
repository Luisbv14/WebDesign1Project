<%@ Page async="true" Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="AppWebInternetBanking.Registro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Registro</title>
    <style>
        * {
			box-sizing: border-box;
		}

		body {
			background: #bfbfbf;
			display: flex;
			justify-content: center;
			align-items: center;
			flex-direction: column;
			font-family: 'Montserrat', sans-serif;
			height: 100vh;
			margin: -20px 0 50px;
			background-image: url("https://img.rawpixel.com/s3fs-private/rawpixel_images/website_content/v960-ning-30.jpg?w=800&dpr=1&fit=default&crop=default&q=65&vib=3&con=3&usm=15&bg=F4F4F3&ixlib=js-2.2.1&s=63dd5f402645ef52fb7dfb592aec765a");
			background-repeat: no-repeat;
			background-size: cover;
		}

		h1 {
			font-weight: bold;
			margin: 0;
			font-family:'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;
			color: #1a8cff;
		}

		p {
			font-size: 14px;
			font-weight: 100;
			line-height: 20px;
			letter-spacing: 0.5px;
			margin: 20px 0 30px;
		}

		span {
			font-size: 12px;
		}

		a {
			color: #333;
			font-size: 14px;
			text-decoration: none;
			margin: 15px 0;
		}

		button {
			border-radius: 20px;
			border: 1px solid #FF4B2B;
			background-color: #FF4B2B;
			color: #FFFFFF;
			font-size: 12px;
			font-weight: bold;
			padding: 12px 45px;
			letter-spacing: 1px;
			text-transform: uppercase;
			transition: transform 80ms ease-in;
		}

		form {
			background-color: #FFFFFF;
			display: flex;
			align-items: center;
			justify-content: center;
			flex-direction: column;
			padding: 0 50px;
			height: 100%;
			text-align: center;
		}

		input {
			background-color: #eee;
			border: none;
			padding: 12px 15px;
			margin: 8px 0;
			width: 100%;
		}

		.container {
			background-color: #fff;
			border-radius: 10px;
  			box-shadow: 0 14px 28px rgba(0,0,0,0.25), 0 10px 10px rgba(0,0,0,0.22);
			position: relative;
			overflow: hidden;
			width: 768px;
			max-width: 100%;
			min-height: 480px;
		}

		.deco-button {
			background-color: #99ccff
		}

		.hover-button:hover {
			background-color: #ff6666;
			cursor: pointer;
		}

		.hover-register:hover {
			color: #ff8533;
			text-decoration: underline;
		}

		/* Full-width input fields */
        input[type=text], input[type=password] {
            width: 100%;
            padding: 15px;
            margin: 5px 0 22px 0;
            display: inline-block;
            border: none;
            background: #f1f1f1;
        }
    </style>
</head>
<body>
	<br />
	<br />

	<div class="container" id="container">
		<form class="modal-content animate" runat="server">

			<br />
			<br />
			<h1>Registro</h1>
			<br />

			<asp:TextBox ID="txtIdentificacion" Placeholder="Ingrese su identificación" runat="server"></asp:TextBox>
			<asp:RequiredFieldValidator ID="rfvIdentificacion" runat="server"
				ErrorMessage="La identificacion es requerida" ControlToValidate="txtIdentificacion" ForeColor="Maroon"></asp:RequiredFieldValidator>
			<asp:TextBox ID="txtNombre" Placeholder="Ingrese su nombre y apellidos" runat="server"></asp:TextBox>
			<asp:RequiredFieldValidator ID="rfvNombre" runat="server"
				ErrorMessage="El nombre es requerido" ControlToValidate="txtNombre" ForeColor="Maroon"></asp:RequiredFieldValidator>
			<asp:TextBox ID="txtEmail" Placeholder="Ingrese su correo electrónico" runat="server"></asp:TextBox>
			<asp:RequiredFieldValidator ID="rfvEmail" runat="server"
				ErrorMessage="El correo electronico es requerido" ControlToValidate="txtEmail" ForeColor="Maroon"></asp:RequiredFieldValidator>

			<asp:TextBox TextMode="DateTimeLocal" CssClass="izquierda" ID="txtFechaNacimiento" Placeholder="Ingrese su fecha de nacimiento" runat="server"></asp:TextBox>
			<asp:RequiredFieldValidator ID="rfvFechaNac" runat="server" ForeColor="Maroon"
				ErrorMessage="La fecha de nacimiento es requerida" ControlToValidate="txtFechaNacimiento"></asp:RequiredFieldValidator>

			<asp:TextBox ID="txtUsername" Placeholder="Ingrese su nombre de usuario" runat="server"></asp:TextBox>
			<asp:RequiredFieldValidator ID="rfvUsername" runat="server" ForeColor="Maroon"
				ErrorMessage="El nombre de usuario es requerido" ControlToValidate="txtUsername"></asp:RequiredFieldValidator>
			<asp:TextBox ID="txtPassword" Placeholder="Ingrese su password" TextMode="Password" runat="server"></asp:TextBox>
			<asp:RequiredFieldValidator ID="rfvPassword" runat="server" ForeColor="Maroon"
				ErrorMessage="El password es requerido" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
			<asp:TextBox ID="txtConfirmarPassword" Placeholder="Confirme su password" TextMode="Password" runat="server"></asp:TextBox>
			<asp:RequiredFieldValidator ID="rfvConfirmarPassword" runat="server" ForeColor="Maroon"
				ErrorMessage="El password es requerido" ControlToValidate="txtConfirmarPassword"></asp:RequiredFieldValidator>
			
			<asp:CompareValidator ID="cvPassword" runat="server" ErrorMessage="Los password deben coincidir"
				ControlToValidate="txtPassword" ControlToCompare="txtConfirmarPassword" ForeColor="Maroon"></asp:CompareValidator>
			<asp:Label ID="lblStatus" runat="server" Text="" Visible="false" ForeColor="Maroon"></asp:Label>

			<asp:Button ID="btnAceptar" Text="Aceptar" CssClass="deco-button hover-button" Onclick="btnAceptar_Click" runat="server"/>
			<input type="reset" value="Reset" class="deco-button hover-button" />
			<br />
			<br />
		</form>
	</div>

    <div id="myModal">
        
    </div>
</body>
</html>
