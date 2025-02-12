<%@ Page Title="Register" Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="TechsysLog.Web.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Cadastro de Usuário</h2>
    <div id="alertContainer"></div> <!-- Adicione este elemento para exibir alertas -->
    <div class="form-group">
        <label for="Nome">Nome</label>
        <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <label for="Email">Email</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <label for="Senha">Senha</label>
        <asp:TextBox ID="txtSenha" runat="server" CssClass="form-control" TextMode="Password" />
    </div>
    <asp:Button ID="RegisterButton" runat="server" Text="Registrar" CssClass="btn btn-primary" OnClick="RegisterButton_Click" />
    <asp:Label ID="ErrorMessage" runat="server" CssClass="text-danger" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        function showErrorAlert(message) {
            var alertHtml = '<div class="alert alert-danger alert-dismissible fade show" role="alert">' +
                '<strong>Erro!</strong> ' + message +
                '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
                '<span aria-hidden="true">&times;</span>' +
                '</button>' +
                '</div>';
            document.getElementById('alertContainer').innerHTML = alertHtml;
        }

        function showUserModal(nome, email, id) {
            var modalHtml = '<div class="modal fade" id="userModal" tabindex="-1" role="dialog" aria-labelledby="userModalLabel" aria-hidden="true">' +
                '<div class="modal-dialog" role="document">' +
                '<div class="modal-content">' +
                '<div class="modal-header">' +
                '<h5 class="modal-title" id="userModalLabel">Usuário Registrado</h5>' +
                '<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
                '<span aria-hidden="true">&times;</span>' +
                '</button>' +
                '</div>' +
                '<div class="modal-body">' +
                '<p><strong>ID:</strong> ' + id + '</p>' +
                '<p><strong>Nome:</strong> ' + nome + '</p>' +
                '<p><strong>Email:</strong> ' + email + '</p>' +
                '</div>' +
                '<div class="modal-footer">' +
                '<button type="button" class="btn btn-primary" onclick="redirectToLogin()">OK</button>' +
                '</div>' +
                '</div>' +
                '</div>' +
                '</div>';
            document.body.insertAdjacentHTML('beforeend', modalHtml);
            $('#userModal').modal('show');
        }

        function redirectToLogin() {
            window.location.href = 'Login.aspx';
        }
    </script>
</asp:Content>


