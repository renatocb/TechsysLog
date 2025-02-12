<%@ Page Title="Entregas" Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
CodeBehind="Entregas.aspx.cs" Inherits="TechsysLog.Web.Entregas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Registro de Entregas</h2>
    <div class="form-group">
        <label for="NumeroPedido">Número do Pedido</label>
        <asp:TextBox ID="NumeroPedidoTextBox" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <label for="DataHoraEntrega">Data/Hora de Entrega</label>
        <asp:TextBox ID="DataHoraEntregaTextBox" runat="server" CssClass="form-control" />
    </div>
    <asp:Button ID="RegistrarEntregaButton" runat="server" Text="Registrar Entrega" CssClass="btn btn-primary" OnClick="RegistrarEntregaButton_Click" />
    <asp:Label ID="ErrorMessage" runat="server" CssClass="text-danger" />
</asp:Content>
