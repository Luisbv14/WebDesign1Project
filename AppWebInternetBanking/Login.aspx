<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AppWebInternetBanking.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Iniciar sesion</title>
    <style>
        * {
			box-sizing: border-box;
		}

		body {
			background: #f2f2f2;
			display: flex;
			justify-content: center;
			align-items: center;
			flex-direction: column;
			font-family: 'Montserrat', sans-serif;
			height: 100vh;
			margin: -20px 0 50px;
		}

		h1 {
			font-weight: bold;
			margin: 0;
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

		.form-container {
			position: absolute;
			top: 0;
			height: 100%;
		}

		.log-in-container {
			left: 0;
			width: 50%;
			z-index: 2;
		}


		.overlay-container {
			position: absolute;
			top: 0;
			left: 50%;
			width: 50%;
			height: 100%;
		}


		.overlay {
			background: #FF416C;
			background: -webkit-linear-gradient(to right, #ff8533, #ff3333);
			background: linear-gradient(to right, #ff8533, #ff3333);
			background-repeat: no-repeat;
			background-size: cover;
			background-position: 0 0;
			color: #FFFFFF;
			position: relative;
			left: -100%;
			height: 100%;
			width: 200%;
		}

		.overlay-panel {
			position: absolute;
			display: flex;
			align-items: center;
			justify-content: center;
			flex-direction: column;
			padding: 0 40px;
			text-align: center;
			top: 0;
			height: 100%;
			width: 50%;
		}


		.overlay-right {
			right: 0;
		}

		.overlay-panel img {
			width: 300px;
			height: 300px;
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
    <div class="container" id="container">
		<div class="form-container log-in-container">
			<form class="modal-content animate" runat="server">

				<h1>Log-In</h1>

				<asp:TextBox ID="txtUsername" runat="server" placeholder="Ingrese su nombre de usuario"></asp:TextBox>
				<asp:RequiredFieldValidator ID="rfqvUsername" runat="server" ErrorMessage="El nombre de usuario es requerido"
				ControlToValidate="txtUsername" ForeColor="Maroon"></asp:RequiredFieldValidator>


				<asp:TextBox ID="txtPassword" runat="server" placeholder="Ingrese su clave" TextMode="Password"></asp:TextBox> 
				<asp:RequiredFieldValidator ID="rfqvPassword" runat="server" ErrorMessage="El password es requerido"
				ControlToValidate="txtPassword" ForeColor="Maroon"></asp:RequiredFieldValidator>

				<asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Maroon"></asp:Label>
>

				<asp:Button ID="Button1" runat="server" Text="Aceptar" CssClass="hover-button" OnClick="btnAceptar_Click"/>
				<input type="reset" value="Limpiar" class="hover-button" />
				<asp:HyperLink CssClass="hover-register" ID="HyperLink2" runat="server" NavigateUrl="~/Registro.aspx">Registrarme</asp:HyperLink>

			</form>
		</div>
		<div class="overlay-container">
			<div class="overlay">
				<div class="overlay-panel overlay-right">
					<h1>Internet Banking</h1>
					<img src="https://www.pngarts.com/files/6/Vector-Bank-Transparent.png" />
				</div>
			</div>
		</div>
	</div>
</body>
</html>