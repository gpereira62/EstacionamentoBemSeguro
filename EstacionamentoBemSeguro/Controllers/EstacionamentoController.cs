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
        }


        public void IniciarEstacionamento()
        {
            bool fechar = true;

            Estacionamento = EstacionamentoView.PegarConfiguracoesIniciais(Estacionamento);
            Estacionamento.CriarVagas();

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
                            if (Estacionamento.PodeEstacionarMoto())
                            {
                                Veiculo? veiculo = EstacionamentoView.PegarInfoVeiculo(Veiculo.Type.Moto);

                                if (veiculo is not null)
                                {
                                    Estacionamento.EstacionarVeiculo(veiculo);
                                }
                                else
                                {
                                    Estacionamento.Avisos.Add(new Aviso("Operação de estacionar moto cancelada!"));
                                }
                            }

                            break;
                        case Opcoes.EstacionarCarro:

                            if (Estacionamento.PodeEstacionarCarro())
                            {
                                Veiculo? veiculo = EstacionamentoView.PegarInfoVeiculo(Veiculo.Type.Carro);

                                if (veiculo is not null)
                                {
                                    Estacionamento.EstacionarVeiculo(veiculo);
                                }
                                else
                                {
                                    Estacionamento.Avisos.Add(new Aviso("Operação de estacionar carro cancelado!"));
                                }
                            }

                            break;
                        case Opcoes.EstacionarVan:
                            if (Estacionamento.PodeEstacionarVan())
                            {
                                Veiculo? veiculo = EstacionamentoView.PegarInfoVeiculo(Veiculo.Type.Van);

                                if (veiculo is not null)
                                {
                                    Estacionamento.EstacionarVeiculo(veiculo);
                                }
                                else
                                {
                                    Estacionamento.Avisos.Add(new Aviso("Operação de estacionar van cancelado!"));
                                }
                            }

                            break;
                        case Opcoes.SaidaVeiculo:
                            if (Estacionamento.TotalVagasOcupadas() > 0)
                            {
                                SaidaVeiculo();
                            }
                            else
                            {
                                Estacionamento.Avisos.Add(new Aviso("Não há nenhum veículo estacionado!"));
                            }

                            break;
                        case Opcoes.QuantasVagasVansOcupam:
                            if (Estacionamento.ExisteVanEstacionada())
                            {
                                EstacionamentoView.QuantasVagasVansOcupam(Estacionamento);
                            }
                            else
                            {
                                Estacionamento.Avisos.Add(new Aviso("Não há nenhuma van estacionada!"));
                            }
                            break;
                        case Opcoes.ListarVeiculosEstacionados:
                            if (Estacionamento.TotalVagasOcupadas() > 0)
                            {
                                EstacionamentoView.ListarVeiculosEstacionados(Estacionamento);
                            }
                            else
                            {
                                Estacionamento.Avisos.Add(new Aviso("Não há nenhum veículo estacionado!"));
                            }
                            break;
                    }
                }
                catch (Exception)
                {
                    Estacionamento.Avisos.Add(new Aviso("Opção Invalida! Tente Novamente."));
                    continue;
                }
            }
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

                bool pagamentoRealizado = PagamentoSaida(veiculoEncontrado);

                if (!pagamentoRealizado)
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

            bool pagamentoRealizado = true;

            var info = Estacionamento.InfoPagamentoSaida(veiculo);
            pagamentoRealizado = EstacionamentoView.ChecarPagamentoSaida(info.Item1, info.Item2, Estacionamento.PrecoHora);

            if (pagamentoRealizado)
            {
                Estacionamento.Caixa += info.Item1;
            }

            return pagamentoRealizado;
        }

    }
}
