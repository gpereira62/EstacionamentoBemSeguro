using EstacionamentoBemSeguro.Models;
using EstacionamentoBemSeguro.Views;

namespace EstacionamentoBemSeguro.Controllers
{
    internal class EstacionamentoController
    {
        private Estacionamento Estacionamento { get; set; }
        private EstacionamentoView EstacionamentoView { get; set; }

        public EstacionamentoController()
        {
            this.Estacionamento = new();
            this.EstacionamentoView = new();
        }

        public enum Opcoes
        {
            SairPrograma = 0,
            EstacionarMoto = 1,
            EstacionarCarro = 2,
            EstacionarVan = 3,
            SaidaVeiculo = 4,
            QuantasVagasVansOcupam = 5,
            ListarVeiculosEstacionados = 6,
            ComoFunciona = 7,
        }

        public void IniciarEstacionamento()
        {
            Estacionamento = EstacionamentoView.PegarConfiguracoesIniciais(Estacionamento);
            Estacionamento.CriarVagas();

            bool fechar = true;
            while (fechar)
            {
                EstacionamentoView.MostrarInfoEstacionamento(Estacionamento);
                Estacionamento = Aviso.ChecarAvisos(Estacionamento);
                EstacionamentoView.MostraAvisos(Estacionamento);
                Estacionamento.Avisos.Clear();

                try
                {
                    switch (EstacionamentoView.MostrarOpcoesMenu())
                    {
                        case Opcoes.SairPrograma:
                            fechar = false;
                            break;

                        case Opcoes.EstacionarMoto:
                            EstacionarVeiculoDoTipo(Veiculo.Type.Moto,
                                Estacionamento.PodeEstacionarMoto,
                                "Operação de estacionar moto cancelada!");
                            break;

                        case Opcoes.EstacionarCarro:
                            EstacionarVeiculoDoTipo(Veiculo.Type.Carro,
                                Estacionamento.PodeEstacionarCarro,
                                "Operação de estacionar carro cancelada!");
                            break;

                        case Opcoes.EstacionarVan:
                            EstacionarVeiculoDoTipo(Veiculo.Type.Van,
                                Estacionamento.PodeEstacionarVan,
                                "Operação de estacionar van cancelada!");
                            break;

                        case Opcoes.SaidaVeiculo:
                            if (Estacionamento.TotalVagasOcupadas() > 0)
                                SaidaVeiculo();
                            else
                                Estacionamento.Avisos.Add(new Aviso("Não há nenhum veículo estacionado!"));
                            break;

                        case Opcoes.QuantasVagasVansOcupam:
                            if (Estacionamento.ExisteVanEstacionada())
                                EstacionamentoView.QuantasVagasVansOcupam(Estacionamento);
                            else
                                Estacionamento.Avisos.Add(new Aviso("Não há nenhuma van estacionada!"));
                            break;

                        case Opcoes.ListarVeiculosEstacionados:
                            if (Estacionamento.TotalVagasOcupadas() > 0)
                                EstacionamentoView.ListarVeiculosEstacionados(Estacionamento);
                            else
                                Estacionamento.Avisos.Add(new Aviso("Não há nenhum veículo estacionado!"));
                            break;

                        case Opcoes.ComoFunciona:
                            EstacionamentoView.MostrarComoFunciona(Estacionamento);
                            break;
                    }
                }
                catch (FormatException)
                {
                    Estacionamento.Avisos.Add(new Aviso("Opção inválida! Tente novamente."));
                }
            }
        }

        private void EstacionarVeiculoDoTipo(Veiculo.Type tipo, Func<bool> podeEstacionar, string msgCancelado)
        {
            if (!podeEstacionar()) return;

            Veiculo? veiculo = EstacionamentoView.PegarInfoVeiculo(tipo);
            if (veiculo is not null)
                Estacionamento.EstacionarVeiculo(veiculo);
            else
                Estacionamento.Avisos.Add(new Aviso(msgCancelado));
        }

        private void SaidaVeiculo()
        {
            while (true)
            {
                int opcao = EstacionamentoView.PegarInfoSaidaVeiculo(Estacionamento);

                if (opcao == 0)
                {
                    Estacionamento.Avisos.Add(new Aviso("Saída de veículo cancelada!"));
                    break;
                }

                Estacionamento.RetornaDicVeiculosEstacionados().TryGetValue(opcao, out Veiculo? veiculoEncontrado);

                if (veiculoEncontrado is null)
                {
                    Estacionamento.Avisos.Add(new Aviso("Veículo não encontrado! Tente novamente."));
                    continue;
                }

                if (!EstacionamentoView.ConfirmarSaidaVeiculo(veiculoEncontrado))
                {
                    Estacionamento.Avisos.Add(new Aviso("Saída de veículo cancelada!"));
                    break;
                }

                if (!PagamentoSaida(veiculoEncontrado))
                {
                    Estacionamento.Avisos.Add(new Aviso("Saída de veículo cancelada! Pagamento não realizado."));
                    break;
                }

                Estacionamento.ExcluirVeiculo(veiculoEncontrado);
                Estacionamento.Avisos.Add(new Aviso("Saída de veículo concluída com sucesso!"));
                break;
            }
        }

        private bool PagamentoSaida(Veiculo veiculo)
        {
            Console.Clear();
            var info = Estacionamento.InfoPagamentoSaida(veiculo);
            bool pago = EstacionamentoView.ChecarPagamentoSaida(info.Item1, info.Item2, Estacionamento.PrecoHora);

            if (pago)
                Estacionamento.Caixa += info.Item1;

            return pago;
        }
    }
}
