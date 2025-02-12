<%@ Page Title="Pedidos" Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pedidos.aspx.cs" Inherits="TechsysLog.Web.Pedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Cadastro de Pedidos</h2>
    <div id="alertContainer" class="mt-3"></div>
    <asp:Label ID="ErrorMessage" runat="server" CssClass="text-danger" />
    <div class="form-group">
        <label for="NumeroPedido">Número do Pedido</label>
        <asp:TextBox ID="txtNumeroPedido" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <label for="Descricao">Descrição</label>
        <asp:TextBox ID="txtDescricao" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <label for="Valor">Valor</label>
        <asp:TextBox ID="txtValor" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <label for="Cep">CEP</label>
        <asp:TextBox ID="txtCep" runat="server" CssClass="form-control" OnBlur="obterEnderecoPorCep()" />
    </div>
    <div class="form-group">
        <label for="Rua">Rua</label>
        <asp:TextBox ID="txtRua" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <label for="Numero">Número</label>
        <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <label for="Bairro">Bairro</label>
        <asp:TextBox ID="txtBairro" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <label for="Cidade">Cidade</label>
        <asp:TextBox ID="txtCidade" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <label for="Estado">Estado</label>
        <asp:TextBox ID="txtEstado" runat="server" CssClass="form-control" />
    </div>
    <asp:Button ID="CadastrarPedidoButton" runat="server" Text="Cadastrar Pedido" CssClass="btn btn-primary" OnClick="CadastrarPedidoButton_Click" />
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

        function showPedidoModal(numeroPedido, descricao, valor) {
            var modalHtml = '<div class="modal fade" id="pedidoModal" tabindex="-1" role="dialog" aria-labelledby="pedidoModalLabel" aria-hidden="true">' +
                '<div class="modal-dialog" role="document">' +
                '<div class="modal-content">' +
                '<div class="modal-header">' +
                '<h5 class="modal-title" id="pedidoModalLabel">Pedido Registrado</h5>' +
                '<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
                '<span aria-hidden="true">&times;</span>' +
                '</button>' +
                '</div>' +
                '<div class="modal-body">' +
                '<p><strong>Número do Pedido:</strong> ' + numeroPedido + '</p>' +
                '<p><strong>Descrição:</strong> ' + descricao + '</p>' +
                '<p><strong>Valor:</strong> ' + valor + '</p>' +
                '</div>' +
                '<div class="modal-footer">' +
                '<button type="button" class="btn btn-primary" onclick="redirectToPedidos()">OK</button>' +
                '</div>' +
                '</div>' +
                '</div>' +
                '</div>';
            document.body.insertAdjacentHTML('beforeend', modalHtml);
            $('#pedidoModal').modal('show');
        }

        function redirectToPedidos() {
            window.location.href = 'Pedidos.aspx';
        }


        function obterEnderecoPorCep() {
            var cep = document.getElementById('<%= txtCep.ClientID %>').value;
            if (cep && cep.length === 8) {
                $.ajax({
                    type: "GET",
                    url: "/Endereco/ObterEnderecoPorCep?cep=" + cep,
                    data: { cep: cep.toString() },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var endereco = response;
                        console.log(endereco); // Adicione este log para verificar a estrutura da resposta
                        if (endereco) {
                            document.getElementById('<%= txtRua.ClientID %>').value = endereco.Logradouro;
                    document.getElementById('<%= txtBairro.ClientID %>').value = endereco.Bairro;
                    document.getElementById('<%= txtCidade.ClientID %>').value = endereco.Localidade;
                    document.getElementById('<%= txtEstado.ClientID %>').value = endereco.Uf;
                }
            },
            error: function (error) {
                console.log(error);
                showErrorAlert("Erro ao buscar o endereço. Verifique o CEP e tente novamente.");
            }
        });
            } else {
                showErrorAlert("CEP inválido. O CEP deve ter 8 dígitos.");
            }
        }
    </script>
</asp:Content>


