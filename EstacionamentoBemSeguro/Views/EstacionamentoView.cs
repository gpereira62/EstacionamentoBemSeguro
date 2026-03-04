using System.Text;
using EstacionamentoBemSeguro.Models;
using static EstacionamentoBemSeguro.Controllers.EstacionamentoController;

namespace EstacionamentoBemSeguro.Views
{
    internal class EstacionamentoView
    {
        private static readonly Dictionary<Veiculo.Type, string> _titulos = new()
        {
            [Veiculo.Type.Moto]  = "Estacionar Moto",
            [Veiculo.Type.Carro] = "Estacionar Carro",
            [Veiculo.Type.Van]   = "Estacionar Van",
        };

        public Estacionamento PegarConfiguracoesIniciais(Estacionamento estacionamento)
        {
            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine("-------------------- Configurações Iniciais --------------------");
            Console.WriteLine("----------------------------------------------------------------");

            Console.WriteLine("\r\nQuantas vagas pequenas para motos terão?");
            estacionamento.QtdeVagaPequena = int.Parse(Utils.LerNumeros(false));

            Console.WriteLine("\r\n\r\nQuantas vagas médias para carros terão?");
            estacionamento.QtdeVagaMedia = int.Parse(Utils.LerNumeros(false));

            Console.WriteLine("\r\n\r\nQuantas vagas grandes para vans terão?");
            estacionamento.QtdeVagaGrande = int.Parse(Utils.LerNumeros(false));

            Console.WriteLine("\r\n\r\nQual é o preço que será cobrado por hora?");
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

            Console.WriteLine($"\r\n    Preço cobrado por hora    R$ {estacionamento.PrecoHora:N2}");
            Console.WriteLine($"    Valor do caixa atualmente R$ {estacionamento.Caixa:N2}");

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
                MostraAvisos(estacionamento);

            MostrarListaVeiculosEstacionados(estacionamento.RetornaDicVeiculosEstacionados());
            Console.WriteLine("        0 - Cancelar saída de veículo");

            Console.WriteLine("\r\nDigite uma das opções númericas para realizar a saída do veículo ou digite '0' para voltar ao menu principal:");
            return int.Parse(Utils.LerNumeros(false));
        }

        private void MostrarListaVeiculosEstacionados(IDictionary<int, Veiculo> dicVeiculosEstacionados)
        {
            Console.WriteLine("\r\n    Veículos:\r\n");
            foreach (var dicVeiculo in dicVeiculosEstacionados)
                Console.WriteLine($"        {dicVeiculo.Key} - {dicVeiculo.Value}");
        }

        public bool ConfirmarSaidaVeiculo(Veiculo veiculo)
        {
            Console.WriteLine($"\r\n\r\nDeseja mesmo realizar a saída do veículo - {veiculo}?");
            Console.WriteLine($"Digite 's' para Sim ou 'n' para Não:");
            return !Utils.LerRespostaPergunta().Equals("n", StringComparison.OrdinalIgnoreCase);
        }

        public void MostraAvisos(Estacionamento estacionamento)
        {
            if (!estacionamento.TemAvisos()) return;

            string mensagens = string.Join("\r\n        ", estacionamento.Avisos.Select(a => a.Mensagem));

            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine("\r\n    Avisos:\r\n");
            Console.WriteLine("        " + mensagens);
            Console.WriteLine("\r\n----------------------------------------------------------------");
        }

        public Opcoes MostrarOpcoesMenu()
        {
            Console.WriteLine("\r\n    Digite uma das opções númericas para a ação ser executada:");
            Console.WriteLine("\r\n        1 - Estacionar uma moto");
            Console.WriteLine("        2 - Estacionar um carro");
            Console.WriteLine("        3 - Estacionar uma van");
            Console.WriteLine("        4 - Saída de veículo");
            Console.WriteLine("        5 - Quantas vagas as vans ocupam?");
            Console.WriteLine("        6 - Listar veículos estacionados");
            Console.WriteLine("        7 - Como funciona o Estacionamento Bem Seguro?");
            Console.WriteLine("        0 - Fechar o programa\r\n");

            Opcoes opcao = (Opcoes)int.Parse(Utils.LerNumeros(false));

            Console.Clear();

            return opcao;
        }

        private void MostrarTitulo(Veiculo.Type tipo)
        {
            string titulo = _titulos.TryGetValue(tipo, out string? t) ? t : tipo.ToString();
            string centrado = titulo.PadLeft((64 + titulo.Length) / 2).PadRight(64);
            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine($"-{centrado}-");
            Console.WriteLine("----------------------------------------------------------------");
        }

        public Veiculo? PegarInfoVeiculo(Veiculo.Type tipo)
        {
            MostrarTitulo(tipo);

            Veiculo veiculo = new(tipo);

            Console.WriteLine("\r\nDigite 'sair' em qualquer momento para voltar ao menu principal!");

            Console.WriteLine("\r\nDigite qual é o veículo:");
            veiculo.Nome = Utils.LerStrings();

            if (veiculo.Nome.Equals("sair", StringComparison.OrdinalIgnoreCase))
                return null;

            Console.WriteLine("\r\n\r\nDigite a placa do veículo:");
            veiculo.Placa = Utils.LerStrings();

            if (veiculo.Placa.Equals("sair", StringComparison.OrdinalIgnoreCase))
                return null;

            return veiculo;
        }

        public bool ChecarPagamentoSaida(double valorCobrado, TimeSpan tempoEstacionado, double precoHora)
        {
            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine("--------------------------- Pagamento --------------------------");
            Console.WriteLine("----------------------------------------------------------------");

            Console.WriteLine($"\r\nTempo que o veículo ficou estacionado {tempoEstacionado:hh\\:mm\\:ss}.");
            Console.WriteLine($"Preço por hora            R$ {precoHora:N2}");
            Console.WriteLine($"Valor total a ser cobrado R$ {valorCobrado:N2}");

            Console.WriteLine("\r\nPagamento foi realizado ?");
            Console.WriteLine($"Digite 's' para Sim ou 'n' para Não:");
            return !Utils.LerRespostaPergunta().Equals("n", StringComparison.OrdinalIgnoreCase);
        }

        public void QuantasVagasVansOcupam(Estacionamento estacionamento)
        {
            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine("---------------- Quantas Vagas as Vans Ocupam? -----------------");
            Console.WriteLine("----------------------------------------------------------------");

            var sb = new StringBuilder();
            foreach (var vaga in estacionamento.ListaVagasVans())
            {
                if (vaga.Veiculo is null) continue;
                string desc = vaga.Tipo == Vaga.Type.Media
                    ? $"   Van: {vaga.Veiculo.Nome} - está ocupando 3 vagas médias de carro"
                    : $"   Van: {vaga.Veiculo.Nome} - está ocupando 1 vaga grande";
                sb.AppendLine(desc);
            }

            Console.WriteLine(sb.ToString());
            Console.WriteLine("\r\nAperte enter para voltar ao menu principal");
            Console.ReadLine();
        }

        public void ListarVeiculosEstacionados(Estacionamento estacionamento)
        {
            MostrarListaVeiculosEstacionados(estacionamento.RetornaDicVeiculosEstacionados());
            Console.WriteLine("\r\nAperte enter para voltar ao menu principal");
            Console.ReadLine();
        }

        public void MostrarComoFunciona(Estacionamento estacionamento)
        {
            Console.Clear();

            Console.WriteLine("\r\n================================================================");
            Console.WriteLine("         BEM-VINDO AO ESTACIONAMENTO BEM SEGURO!");
            Console.WriteLine("         O estacionamento mais organizado da cidade.");
            Console.WriteLine("================================================================");

            Console.WriteLine("\r\n  Aqui, cada veículo tem seu lugar — e a gente garante isso!");
            Console.WriteLine("  Confira abaixo tudo que você precisa saber:");

            // --- Tipos de vagas ---
            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine("  [P]  TIPOS DE VAGAS");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine($"\r\n  Pequena  (P)  → {estacionamento.QtdeVagaPequena,3} vagas  │ Ideal para motos");
            Console.WriteLine($"  Média    (M)  → {estacionamento.QtdeVagaMedia,3} vagas  │ Ideal para carros");
            Console.WriteLine($"  Grande   (G)  → {estacionamento.QtdeVagaGrande,3} vagas  │ Ideal para vans");

            // --- Regras por veículo ---
            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine("  [R]  REGRAS DE ESTACIONAMENTO POR VEÍCULO");
            Console.WriteLine("----------------------------------------------------------------");

            Console.WriteLine("\r\n  MOTO  ");
            Console.WriteLine("    ✔ Pode estacionar em QUALQUER tipo de vaga disponível.");
            Console.WriteLine("    ✔ Preferência: Pequena → Média → Grande.");
            Console.WriteLine("    ✔ Não ocupa mais de 1 vaga.");

            Console.WriteLine("\r\n  CARRO");
            Console.WriteLine("    ✔ Pode estacionar em vagas Médias ou Grandes.");
            Console.WriteLine("    ✔ Preferência: Média → Grande.");
            Console.WriteLine("    ✔ Não pode usar vagas Pequenas.");

            Console.WriteLine("\r\n  VAN");
            Console.WriteLine("    ✔ Prioridade: ocupa 1 vaga Grande (quando disponível).");
            Console.WriteLine("    ✔ Alternativa: ocupa exatamente 3 vagas Médias.");
            Console.WriteLine("    ✗ Se não houver vaga Grande nem 3 Médias livres, a van");
            Console.WriteLine("      não pode entrar no estacionamento.");

            // --- Cobrança ---
            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine("  [$]  COBRANÇA");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine($"\r\n  Preço por hora: R$ {estacionamento.PrecoHora:N2}");
            Console.WriteLine("\r\n  Regras de cobrança:");
            Console.WriteLine("    • A cobrança é feita por hora cheia.");
            Console.WriteLine("    • Menos de 1 hora? Ainda assim é cobrada 1 hora.");
            Console.WriteLine("    • Exemplo: 2h30min = cobra 2 horas.");
            Console.WriteLine("    • O pagamento é confirmado na saída do veículo.");
            Console.WriteLine("    • O valor vai direto para o caixa do estacionamento.");

            // --- Fluxo de entrada/saída ---
            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine("  [»]  COMO FUNCIONA A ENTRADA E SAÍDA");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("\r\n  ENTRADA:");
            Console.WriteLine("    1. Escolha o tipo de veículo no menu.");
            Console.WriteLine("    2. Informe o nome e a placa do veículo.");
            Console.WriteLine("    3. O sistema encontra a melhor vaga automaticamente.");
            Console.WriteLine("    4. Pronto! O horário de entrada é registrado.");

            Console.WriteLine("\r\n  SAÍDA:");
            Console.WriteLine("    1. Selecione 'Saída de veículo' no menu.");
            Console.WriteLine("    2. Escolha o veículo pela lista.");
            Console.WriteLine("    3. Confirme a saída.");
            Console.WriteLine("    4. Realize o pagamento.");
            Console.WriteLine("    5. A vaga é liberada automaticamente.");

            // --- Avisos automáticos ---
            Console.WriteLine("\r\n----------------------------------------------------------------");
            Console.WriteLine("  [!]  AVISOS AUTOMÁTICOS");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("\r\n  O sistema avisa automaticamente quando:");
            Console.WriteLine("    • O estacionamento estiver cheio ou vazio.");
            Console.WriteLine("    • Todas as vagas de um tipo específico forem ocupadas.");
            Console.WriteLine("    • Não houver espaço para o veículo que tenta entrar.");

            Console.WriteLine("\r\n================================================================");
            Console.WriteLine("  Dúvidas? Fale com a nossa equipe. Bom estacionamento!");
            Console.WriteLine("================================================================");

            Console.WriteLine("\r\nAperte enter para voltar ao menu principal");
            Console.ReadLine();
        }
    }
}
