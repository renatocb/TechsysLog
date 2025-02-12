<%@ Page Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TechsysLog.Web.Login"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Login</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <h2 class="text-center">Login</h2>
                <asp:Label ID="ErrorMessage" runat="server" ForeColor="Red" CssClass="d-block text-center mb-3"></asp:Label>
                <div class="form-group">
                    <asp:Label ID="EmailLabel" runat="server" Text="Email:" CssClass="form-label"></asp:Label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="SenhaLabel" runat="server" Text="Senha:" CssClass="form-label"></asp:Label>
                    <asp:TextBox ID="txtSenha" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group text-center">
                    <asp:Button ID="LoginButton" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="LoginButton_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
