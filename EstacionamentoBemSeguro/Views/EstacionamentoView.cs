using EstacionamentoBemSeguro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EstacionamentoBemSeguro.Controllers.EstacionamentoController;

namespace EstacionamentoBemSeguro.Views
{
    internal class EstacionamentoView
    {
        public EstacionamentoView()
        {
        }

        public Estacionamento PegarConfiguracoesIniciais(Estacionamento estacionamento)
        {
            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine("-------------------- Configurações Inicias ---------------------");
            Console.WriteLine("----------------------------------------------------------------");

            Console.WriteLine("\r\nQuantas vagas pequenas para motos terão?");
            estacionamento.QtdeVagaPequena = int.Parse(Utils.LerNumeros(false));

            Console.WriteLine("\r\n\r\nQuantas vagas médias para carros terão?");
            estacionamento.QtdeVagaMedia = int.Parse(Utils.LerNumeros(false));

            Console.WriteLine("\r\n\r\nQuantas vagas grandes para vans terão?");
            estacionamento.QtdeVagaGrande = int.Parse(Utils.LerNumeros(false));

            Console.WriteLine("\r\n\r\nQual o preço que será cobrado por hora?");
            Console.Write("R$ ");
            estacionamento.PrecoHora = double.Parse(Utils.LerNumeros(true));

            return estacionamento;
        }

        public void MostrarInfoEstacionamento(Estacionamento estacionamento)
        {
            Console.Clear();

            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine("------------ Bem Vindo ao Estacionamento Bem Seguro ------------");
            Console.WriteLine("----------------------------------------------------------------");

            Console.WriteLine($"\r\n    Preço cobrado por hora    R$ {estacionamento.PrecoHora.ToString("N2")}");
            Console.WriteLine($"    Valor do caixa atualmente R$ {estacionamento.Caixa.ToString("N2")}");

            Console.WriteLine("\r\n----------------------------------------------------------------");

            Console.WriteLine($"\r\n    Total de vagas:                        {estacionamento.TotalVagas()}");
            Console.WriteLine($"    Total de vagas disponíveis:            {estacionamento.TotalVagasDisponiveis()}");
            Console.WriteLine($"    Total de vagas ocupadas:               {estacionamento.TotalVagasOcupadas()}");
            Console.WriteLine($"\r\n    Total de vagas disponíveis para moto:  {estacionamento.TotalVagasDisponiveisMoto()}");
            Console.WriteLine($"    Total de vagas disponíveis para carro: {estacionamento.TotalVagasDisponiveisCarro()}");
            Console.WriteLine($"    Total de vagas disponíveis para van:   {estacionamento.TotalVagasDisponiveisVan()}");
        }

        public int PegarInfoSaidaVeiculo(Estacionamento estacionamento)
        {
            Console.Clear();

            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine("----------------------- Saída de Veículo -----------------------");
            Console.WriteLine("----------------------------------------------------------------");

            if (estacionamento.TemAvisos())
            {
                MostraAvisos(estacionamento);
                Console.WriteLine("\r\n----------------------------------------------------------------");
            }

            MostrarListaVeiculosEstacionados(estacionamento.RetornaDicVeiculosEstacionados());
            Console.WriteLine("        0 - Cancelar saída de veículo");

            Console.WriteLine("\r\nDigite uma opções númericas para realizar a saída do veículo ou digite '0' para voltar ao menu principal:");
            return int.Parse(Utils.LerNumeros(false));
        }

        private void MostrarListaVeiculosEstacionados(IDictionary<int, Veiculo> dicVeiculosEstacionados)
        {
            Console.WriteLine("\r\n    Veículos:\r\n");
            foreach (var dicVeiculo in dicVeiculosEstacionados)
                Console.WriteLine($"        {dicVeiculo.Key} - {dicVeiculo.Value.ToString()}");
        }

        public bool ConfirmarSaidaVeiculo(Veiculo veiculo)
        {
            Console.WriteLine($"\r\n\r\nDeseja mesmo realizar a saída do veículo - {veiculo.ToString()}?");
            Console.WriteLine($"Digite 's' para Sim ou 'n' para Não:");
            string resposta = Utils.LerRespostaPergunta().ToLower();

            if (resposta.Equals("n"))
            {
                return false;
            }

            return true;
        }

        public void MostraAvisos(Estacionamento estacionamento)
        {
            if (estacionamento.TemAvisos())
            {
                string avisos = "";
                foreach (var aviso in estacionamento.Avisos)
                {
                    if (string.IsNullOrEmpty(avisos))
                    {
                        avisos = aviso.Mensagem;
                    }
                    else
                    {
                        avisos += $"\r\n        {aviso.Mensagem}";
                    }
                }

                Console.WriteLine("\r\n----------------------------------------------------------------");

                Console.WriteLine("\r\n    Avisos:\r\n");
                Console.WriteLine("        " + avisos);

                Console.WriteLine("\r\n----------------------------------------------------------------");
            }
        }

        public Opcoes MostrarOpcoesMenu()
        {
            Console.WriteLine("\r\n    Digite uma das opções númericas para a ação ser executada:");
            Console.WriteLine("\r\n        1 - Estacionar uma moto");
            Console.WriteLine("        2 - Estacionar um carro");
            Console.WriteLine("        3 - Estacionar uma van");
            Console.WriteLine("        4 - Saída de veículo");
            Console.WriteLine("        5 - Quantas vagas as vans ocupam");
            Console.WriteLine("        6 - Listar Veículos Estacionados");
            Console.WriteLine("        0 - Fechar o programa\r\n");

            Opcoes opcao = (Opcoes)int.Parse(Utils.LerNumeros(false));

            Console.Clear();

            return opcao;
        }

        private void MostrarTitulo(Veiculo.Type tipo)
        {
            switch (tipo)
            {
                case Veiculo.Type.Carro:
                    Console.WriteLine("\r\n----------------------------------------------------------------");
                    Console.WriteLine("----------------------- Estacionar Carro -----------------------");
                    Console.WriteLine("----------------------------------------------------------------");
                    break;
                case Veiculo.Type.Moto:
                    Console.WriteLine("\r\n----------------------------------------------------------------");
                    Console.WriteLine("------------------------ Estacionar Moto -----------------------");
                    Console.WriteLine("----------------------------------------------------------------");
                    break;
                case Veiculo.Type.Van:
                    Console.WriteLine("\r\n----------------------------------------------------------------");
                    Console.WriteLine("------------------------ Estacionar Van ------------------------");
                    Console.WriteLine("----------------------------------------------------------------");
                    break;
                default:
                    break;
            }
        }

        public Veiculo PegarInfoVeiculo(Veiculo.Type tipo)
        {
            MostrarTitulo(tipo);

            Veiculo veiculo = new(tipo);

            Console.WriteLine("\r\nDigite qual é o veículo:");
            veiculo.Nome = Utils.LerStrings();

            Console.WriteLine("\r\n\r\nDigite a placa do veículo:");
            veiculo.Placa = Utils.LerStrings();

            return veiculo;
        }

        public bool ChecarPagamentoSaida(double valorCobrado, TimeSpan tempoEstacionado, double precoHora)
        {
            Console.WriteLine($"\r\nTempo que o veículo ficou estacionado {tempoEstacionado.ToString("hh':'mm':'ss")}.");
            Console.WriteLine($"Preço por hora            R$ {precoHora.ToString("N2")}");
            Console.WriteLine($"Valor total a ser cobrado R$ {valorCobrado.ToString("N2")}");

            Console.WriteLine("\r\nPagamento foi realizado ?");
            Console.WriteLine($"Digite 's' para Sim ou 'n' para Não:");
            string resposta = Utils.LerRespostaPergunta().ToLower();
            if (resposta.Equals("n"))
            {
                return false;
            }

            return true;
        }

        public void QuantasVagasVansOcupam(Estacionamento estacionamento)
        {
            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine("---------------- Quantas Vagas as Vans Ocupam? -----------------");
            Console.WriteLine("----------------------------------------------------------------");

            string retorno = "";
            List<Vaga> vagasVansOcupam = estacionamento.ListaVagasVans();

            foreach (var vaga in vagasVansOcupam)
            {
                if (vaga.Veiculo != null)
                {
                    if (vaga.Tipo == Vaga.Type.Media && vaga.Veiculo.Tipo == Veiculo.Type.Van)
                    {
                        if (retorno.Equals(""))
                        {
                            retorno += $"\r\n   Van: {vaga.Veiculo.Nome} - está ocupando 3 vagas médias de carro";
                        }
                        else
                        {
                            retorno += $"\n   Van: {vaga.Veiculo.Nome} - está ocupando 3 vagas médias de carro";
                        }
                    }
                    else
                    {
                        if (retorno.Equals(""))
                        {
                            retorno += $"\r\n   Van: {vaga.Veiculo.Nome} - está ocupando 1 vaga grande";
                        }
                        else
                        {
                            retorno += $"\n   Van: {vaga.Veiculo.Nome} - está ocupando 1 vaga grande";
                        }
                    }
                }

            }

            Console.WriteLine(retorno);

            Console.WriteLine("\r\n Aperte Enter para voltar ao menu principal");
            Console.ReadLine();
        }

        public void ListarVeiculosEstacionados(Estacionamento estacionamento)
        {
            MostrarListaVeiculosEstacionados(estacionamento.RetornaDicVeiculosEstacionados());
            Console.WriteLine("\r\n Aperte Enter para voltar ao menu principal");
            Console.ReadLine();
        }
    }
}
