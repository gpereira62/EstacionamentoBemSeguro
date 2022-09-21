using EstacionamentoBemSeguro;
using EstacionamentoBemSeguro.Models;

const int Sair_Programa = 0;
const int Estacionar_Moto = 1;
const int Estacionar_Carro = 2;
const int Estacionar_Van = 3;
const int Saida_Veiculo = 4;
const int Quantas_Vagas_Vans_Ocupadas = 5;

bool fechar = true;
Estacionamento estacionamento = new();

Console.WriteLine("Quantas vagas pequenas para motos terão?");
estacionamento.QtdeVagaPequena = int.Parse(Utils.LerNumeros(false));

Console.WriteLine("\r\n\r\nQuantas vagas médias para carros terão?");
estacionamento.QtdeVagaMedia = int.Parse(Utils.LerNumeros(false));

Console.WriteLine("\r\n\r\nQuantas vagas grandes para vans terão?");
estacionamento.QtdeVagaGrande = int.Parse(Utils.LerNumeros(false));

Console.WriteLine("\r\n\r\nQual o preço que será cobrado por hora?");
Console.Write("R$ ");
estacionamento.PrecoHora = double.Parse(Utils.LerNumeros(true));

estacionamento.CriarVagas();

while (fechar)
{
    Console.Clear();

    Console.WriteLine("\r\n------------ Bem Vindo ao Estacionamento Bem Seguro ------------");

    Console.WriteLine($"\r\n    Preço cobrado por hora    R$ {estacionamento.PrecoHora.ToString("N2")}");
    Console.WriteLine($"    Valor do caixa atualmente R$ {estacionamento.Caixa.ToString("N2")}");

    Console.WriteLine("\r\n----------------------------------------------------------------");

    Console.WriteLine($"\r\n    Total de vagas:                        {estacionamento.TotalVagas()}");
    Console.WriteLine($"    Total de vagas disponíveis:            {estacionamento.TotalVagasDisponiveis()}");
    Console.WriteLine($"    Total de vagas ocupadas:               {estacionamento.TotalVagasOcupadas()}");
    Console.WriteLine($"\r\n    Total de vagas disponíveis para moto:  {estacionamento.TotalVagasDisponiveisMoto()}");
    Console.WriteLine($"    Total de vagas disponíveis para carro: {estacionamento.TotalVagasDisponiveisCarro()}");
    Console.WriteLine($"    Total de vagas disponíveis para van:   {estacionamento.TotalVagasDisponiveisVan()}");

    ValidaAvisos(estacionamento);
    MostraUltimasMensagens(estacionamento);

    Console.WriteLine("\r\n----------------------------------------------------------------");

    Console.WriteLine("\r\n    Digite uma das opções númericas para a ação ser executada:");
    Console.WriteLine("\r\n        1 - Estacionar uma moto");
    Console.WriteLine("        2 - Estacionar um carro");
    Console.WriteLine("        3 - Estacionar uma van");
    Console.WriteLine("        4 - Saída de veículo");
    Console.WriteLine("        0 - Fechar o programa\r\n");

    int opcao = int.Parse(Utils.LerNumeros(false));

    Console.Clear();
    switch (opcao)
    {
        case Sair_Programa:
            fechar = false;
            break;
        case Estacionar_Moto:
            if (estacionamento.TotalVagasDisponiveis() > 0)
            {
                EstacionarVeiculo(estacionamento, Veiculo.Type.Moto);
                estacionamento.Avisos.Add(new Aviso("Moto estacionada com sucesso!"));
            } else
            {
                estacionamento.Avisos.Add(new Aviso("Não há espaço no estacionamento para estacionar a moto!"));
            }

            break;
        case Estacionar_Carro:
            if (estacionamento.EncontrarVagaCarro() != null)
            {
                EstacionarVeiculo(estacionamento, Veiculo.Type.Carro);
                estacionamento.Avisos.Add(new Aviso("Carro estacionado com sucesso!"));
            }
            else
            {
                estacionamento.Avisos.Add(new Aviso("Não há espaço no estacionamento para estacionar o carro!"));
            }

            break;
        case Estacionar_Van:
            if (estacionamento.EncontrarVagaVan() != null)
            {
                EstacionarVeiculo(estacionamento, Veiculo.Type.Van);
                estacionamento.Avisos.Add(new Aviso("Van estacionada com sucesso!"));
            }
            else if (estacionamento.EncontrarTresVagasMediasVan().Select(x => x != null).Count() == 3)
            {
                EstacionarVeiculo(estacionamento, Veiculo.Type.Van);
                estacionamento.Avisos.Add(new Aviso("Vaga grande para van não encontrada! Van estacionada com sucesso em 3 vagas de carro."));
            }
            else
            {
                estacionamento.Avisos.Add(new Aviso("Não há espaço no estacionamento para estacionar a van!"));
            }

            break;
        case Saida_Veiculo:
            if (estacionamento.TotalVagasOcupadas() > 0)
            {
                SaidaVeiculo(estacionamento);
            } else
            {
                estacionamento.Avisos.Add(new Aviso("Não há nenhum veículo estacionado!"));
            }

            break;
        default:
            estacionamento.Avisos.Add(new Aviso("Opção Invalida! Tente Novamente."));
            continue;
    }
}

static void EstacionarVeiculo(Estacionamento estacionamento, Veiculo.Type tipo)
{
    Veiculo veiculo = new(tipo);

    Console.WriteLine("\r\nDigite qual é o veículo:");
    veiculo.Nome = Utils.LerStrings();

    Console.WriteLine("\r\n\r\nDigite a placa do veículo:");
    veiculo.Placa = Utils.LerStrings();

    estacionamento.EstacionarVeiculo(veiculo);
}


static void SaidaVeiculo(Estacionamento estacionamento)
{
    IDictionary<int, Veiculo> dicVeiculosEstacionados = estacionamento.RetornaDicVeiculosEstacionados();

    while (true)
    {
        Console.Clear();
        MostraUltimasMensagens(estacionamento);

        Console.WriteLine("\r\n----------------------------------------------------------------");

        MostrarListaVeiculosEstacionados(dicVeiculosEstacionados);
        Console.WriteLine("        0 - Cancelar saída de veículo");

        Console.WriteLine("\r\nDigite uma opções númericas para realizar a saída do veículo ou digite '0' para cancelar a operação:");
        int opcao = int.Parse(Utils.LerNumeros(false));

        if (opcao == 0)
        {
            estacionamento.Avisos.Add(new Aviso("Saída de veículo cancelada!"));
            break;
        }

        dicVeiculosEstacionados.TryGetValue(opcao, out Veiculo? veiculoEncontrado);

        if (veiculoEncontrado is null)
        {
            estacionamento.Avisos.Add(new Aviso("Veículo não encontrado! Tente novamente."));
            continue;
        }

        Console.WriteLine($"\r\n\r\nDeseja mesmo realizar a saída do veículo - {veiculoEncontrado.ToString()}?");
        Console.WriteLine($"Digite 's' para Sim ou 'n' para Não:");
        string resposta = Utils.LerRespostaPergunta().ToLower();
        if (resposta.Equals("n"))
        {
            estacionamento.Avisos.Add(new Aviso("Saída de veículo cancelada!"));
            break;
        }

        Console.Clear();
        bool pagamentoRealizado = PagamentoSaida(estacionamento, veiculoEncontrado);

        if (!pagamentoRealizado)
        {
            estacionamento.Avisos.Add(new Aviso("Saída de veículo cancelada! Pagamento não realizado."));
            break;
        }

        estacionamento.ExcluirVeiculo(veiculoEncontrado);

        estacionamento.Avisos.Add(new Aviso("Saída de veículo concluída com sucesso!"));
        break;

    }
}

static void MostrarListaVeiculosEstacionados(IDictionary<int, Veiculo> dicVeiculosEstacionados)
{
    Console.WriteLine("\r\n    Veículos:\r\n");
    foreach (var dicVeiculo in dicVeiculosEstacionados)
        Console.WriteLine($"        {dicVeiculo.Key} - {dicVeiculo.Value.ToString()}");
}

static bool PagamentoSaida(Estacionamento estacionamento, Veiculo? veiculo)
{
    bool pagamentoRealizado = true;

    double valorCobrado = new();
    DateTime dataHoraSaida = DateTime.Now;
    TimeSpan tempoEstacionado = dataHoraSaida - veiculo.DataHoraEntrada;

    if (tempoEstacionado.Hours < 1)
    {
        valorCobrado = estacionamento.PrecoHora;
    } else
    {
        valorCobrado = int.Parse(tempoEstacionado.Hours.ToString()) * estacionamento.PrecoHora;
    }

    Console.WriteLine($"\r\nTempo que o veículo ficou estacionado {tempoEstacionado.ToString("hh':'mm':'ss")}.");
    Console.WriteLine($"Preço por hora            R$ {estacionamento.PrecoHora.ToString("N2")}");
    Console.WriteLine($"Valor total a ser cobrado R$ {valorCobrado.ToString("N2")}");

    Console.WriteLine("\r\nPagamento foi realizado ?");
    Console.WriteLine($"Digite 's' para Sim ou 'n' para Não:");
    string resposta = Utils.LerRespostaPergunta().ToLower();
    if (resposta.Equals("n"))
    {
        pagamentoRealizado = false;
    }

    estacionamento.Caixa += valorCobrado;

    return pagamentoRealizado;
}

static void MostraUltimasMensagens(Estacionamento estacionamento)
{
    if (estacionamento.Avisos.Any())
    {
        Console.WriteLine("\r\n----------------------------------------------------------------");

        Console.WriteLine("\r\n    Avisos:\r\n");
        Console.WriteLine("        " + estacionamento.MostrarMensagens());
        estacionamento.Avisos.Clear();
    }
}

static void ValidaAvisos(Estacionamento estacionamento)
{
    if (estacionamento.TotalVagasDisponiveis() == 0)
    {
        estacionamento.Avisos.Add(new Aviso("Estacionamento Cheio!"));
    }
    else
    {
        if (estacionamento.TotalVagasDisponiveisMoto() == 0 && estacionamento.QtdeVagaPequena > 0)
        {
            estacionamento.Avisos.Add(new Aviso("Todas as vagas pequenas para motos foram ocupadas!"));
        }

        if (estacionamento.TotalVagasDisponiveisCarro() == 0 && estacionamento.QtdeVagaMedia > 0)
        {
            estacionamento.Avisos.Add(new Aviso("Todas as vagas médias para carros foram ocupadas!"));
        }

        if (estacionamento.TotalVagasDisponiveisVan() == 0 && estacionamento.QtdeVagaGrande > 0)
        {
            estacionamento.Avisos.Add(new Aviso("Todas as vagas grandes para vans foram ocupadas!"));
        }
    }

    if (estacionamento.TotalVagasOcupadas() == 0)
    {
        estacionamento.Avisos.Add(new Aviso("Estacionamento Vazio!"));
    }


}
