<%@ Page Title="Painel de Pedidos" Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PainelPedidos.aspx.cs" Inherits="TechsysLog.Web.PainelPedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Painel de Pedidos</h2>

    <!-- Área para exibir notificações -->
    <div id="notificacoes" style="margin-bottom: 20px;">
        <h3>Notificações</h3>
        <ul id="notificacoesLista"></ul>
    </div>

    <!-- Tabela de pedidos -->
    <div id="pedidosPanel" class="table-responsive">
        <table id="pedidosTable" class="table table-striped">
            <thead>
                <tr>
                    <th>Número do Pedido</th>
                    <th>Descrição</th>
                    <th>Valor</th>
                    <th>Status Pedido</th>
                    <th>Status Entregue</th>
                </tr>
            </thead>
            <tbody>
                <!-- As linhas serão preenchidas dinamicamente -->
            </tbody>
        </table>
    </div>

    <!-- Scripts do SignalR e Toastr -->
    <script src="https://cdn.jsdelivr.net/npm/@microsoft/signalr@5.0.11/dist/browser/signalr.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>

    <!-- Script para atualizar a interface com as notificações -->
    <script type="text/javascript">
        let _hubConnection;
        let notificacaoAtual = null; // Variável para rastrear a notificação atual

        function atualizarTabelaPedidos(pedidos) {
            console.log("Pedidos recebidos:", pedidos); // Depuração: inspecione os pedidos recebidos

            if (!Array.isArray(pedidos)) {
                console.error("Pedidos não é um array:", pedidos);
                return;
            }

            var tbody = document.querySelector("#pedidosTable tbody");

            pedidos.forEach(function (pedido) {
                // Cria um novo objeto normalizado para cada pedido
                const pedidoNormalizado = normalizarPropriedades(pedido);
                console.log("Pedido normalizado:", pedidoNormalizado); // Depuração: inspecione o pedido normalizado

                // Verifica se os campos necessários estão presentes
                if (!pedidoNormalizado.numeroPedido || !pedidoNormalizado.descricao || !pedidoNormalizado.valor || !pedidoNormalizado.statusPedido) {
                    console.error("Pedido incompleto:", pedidoNormalizado);
                    return;
                }

                // Verifica se o status da entrega está presente
                const statusEntrega = pedidoNormalizado.entrega ? pedidoNormalizado.entrega.status : "N/A";

                // Verifica se o pedido já existe na tabela
                const linhaExistente = tbody.querySelector(`tr[data-pedido-id="${pedidoNormalizado.numeroPedido}"]`);

                if (linhaExistente) {
                    console.log("Linha existente encontrada:", linhaExistente); // Depuração: inspecione a linha existente

                    // Atualiza as células da linha existente
                    const cells = linhaExistente.cells;

                    if (cells && cells.length >= 5) { // Verifica se a linha tem pelo menos 5 células
                        cells[0].textContent = pedidoNormalizado.numeroPedido; // Número do Pedido
                        cells[1].textContent = pedidoNormalizado.descricao; // Descrição
                        cells[2].textContent = pedidoNormalizado.valor; // Valor
                        cells[3].textContent = pedidoNormalizado.statusPedido; // Status Pedido
                        cells[4].textContent = statusEntrega; // Status Entregue
                        console.log("Linha atualizada com sucesso."); // Depuração

                        // Exibe a notificação rica associada à linha atualizada
                        if (notificacaoAtual && notificacaoAtual.numeroPedido === pedidoNormalizado.numeroPedido) {
                            exibirNotificacaoRica('Pedido Atualizado', notificacaoAtual.mensagem);
                            notificacaoAtual = null; // Reseta a notificação atual após exibi-la
                        }
                    } else {
                        console.error("Linha existente não tem células suficientes:", cells);
                    }
                } else {
                    console.log("Adicionando nova linha para o pedido:", pedidoNormalizado.numeroPedido); // Depuração

                    // Adiciona uma nova linha
                    var row = tbody.insertRow();
                    row.setAttribute("data-pedido-id", pedidoNormalizado.numeroPedido);

                    // Adiciona as células
                    var cellNumero = row.insertCell(0);
                    var cellDescricao = row.insertCell(1);
                    var cellValor = row.insertCell(2);
                    var cellStatusPedido = row.insertCell(3);
                    var cellStatusEntregue = row.insertCell(4);

                    // Preenche as células com os dados do pedido
                    cellNumero.textContent = pedidoNormalizado.numeroPedido;
                    cellDescricao.textContent = pedidoNormalizado.descricao;
                    cellValor.textContent = pedidoNormalizado.valor;
                    cellStatusPedido.textContent = pedidoNormalizado.statusPedido;
                    cellStatusEntregue.textContent = statusEntrega;

                    console.log("Nova linha adicionada com sucesso."); // Depuração

                    // Exibe a notificação rica associada à nova linha
                    if (notificacaoAtual && notificacaoAtual.numeroPedido === pedidoNormalizado.numeroPedido) {
                        exibirNotificacaoRica('Novo Pedido', notificacaoAtual.mensagem);
                        notificacaoAtual = null; // Reseta a notificação atual após exibi-la
                    }
                }
            });
        }

        function normalizarPropriedades(objeto) {
            const normalizado = {};

            for (const chave in objeto) {
                if (objeto.hasOwnProperty(chave)) {
                    // Converte a chave para camelCase
                    const chaveNormalizada = chave.charAt(0).toLowerCase() + chave.slice(1);
                    normalizado[chaveNormalizada] = objeto[chave];
                }
            }

            // Normaliza o objeto Entrega, se existir
            if (normalizado.entrega) {
                normalizado.entrega = normalizarPropriedades(normalizado.entrega);
            }

            return normalizado;
        }

        function exibirNotificacaoRica(titulo, mensagem) {
            toastr.options = {
                closeButton: true,
                progressBar: true,
                positionClass: "toast-top-right",
                timeOut: 5000,
                extendedTimeOut: 1000,
            };
            toastr.success(mensagem, titulo);
        }

        function iniciarSignalR() {
            _hubConnection = new signalR.HubConnectionBuilder()
                .withUrl("https://localhost:5001/pedidoEntregaHub")
                .configureLogging(signalR.LogLevel.Information)
                .build();

            _hubConnection.onclose(async () => {
                console.log("Conexão SignalR fechada. Tentando reconectar...");
                await reconectarSignalR();
            });

            _hubConnection.start()
                .then(() => {
                    console.log("Conectado ao SignalR Hub.");
                })
                .catch(err => {
                    console.error("Erro ao conectar ao SignalR Hub:", err);
                });

            _hubConnection.on("ReceivePedidoNotification", (data) => {
                console.log("Notificação recebida:", data); // Depuração: inspecione o objeto recebido

                const pedido = data.pedido; // Acessa o objeto pedido
                const mensagem = data.mensagem; // Acessa a mensagem

                if (pedido) {
                    console.log(pedido);
                    // Armazena a notificação atual
                    notificacaoAtual = {
                        numeroPedido: pedido.numeroPedido,
                        mensagem: mensagem
                    };
                    console.log(notificacaoAtual);
                    atualizarTabelaPedidos([pedido]); // Atualiza a tabela com o novo pedido
                } else {
                    console.error("Pedido recebido é undefined.");
                }
            });

            // Método para receber notificações de atualização de entrega
            _hubConnection.on("ReceiveEntregaNotification", (data) => {
                console.log("Notificação de entrega recebida:", data); // Depuração: inspecione o objeto recebido

                const entrega = data.entrega; // Acessa o objeto entrega
                const mensagem = data.mensagem; // Acessa a mensagem

                if (entrega) {
                    console.log(entrega);
                    // Armazena a notificação atual
                    notificacaoAtual = {
                        numeroPedido: entrega.pedidoId, // Assumindo que o objeto entrega tem um PedidoId
                        mensagem: mensagem
                    };
                    console.log(notificacaoAtual);
                    // Atualiza a tabela de pedidos com a nova entrega
                    atualizarTabelaPedidos([{ ...entrega.pedido, entrega: entrega }]); // Atualiza o pedido com a entrega
                } else {
                    console.error("Entrega recebida é undefined.");
                }
            });

        }

        $(document).ready(function () {
            iniciarSignalR();
        });
    </script>
</asp:Content>


