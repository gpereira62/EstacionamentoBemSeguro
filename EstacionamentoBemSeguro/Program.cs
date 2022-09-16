﻿using EstacionamentoBemSeguro;
using EstacionamentoBemSeguro.Models;

const int Sair_Programa = 0;
const int Estacionar_Moto = 1;
const int Estacionar_Carro = 2;
const int Estacionar_Van = 3;
const int Saida_Veiculo = 4;

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
    Console.WriteLine($"    Valor do Caixa atualmente R$ {estacionamento.Caixa.ToString("N2")}");

    Console.WriteLine("\r\n----------------------------------------------------------------");

    Console.WriteLine($"\r\n    Total de vagas:             {estacionamento.TotalVagas()}");
    Console.WriteLine($"    Total de vagas disponíveis: {estacionamento.TotalVagasDisponiveis()}");
    Console.WriteLine($"    Total de vagas ocupadas:    {estacionamento.TotalVagasOcupadas()}");

    MostraUltimasMensagens(estacionamento);

    Console.WriteLine("\r\n----------------------------------------------------------------");

    Console.WriteLine("\r\n    Digite uma das opções númericas para a ação ser executada:");
    Console.WriteLine("\r\n        1 - Estacionar uma moto");
    Console.WriteLine("        2 - Estacionar um carro");
    Console.WriteLine("        3 - Estacionar uma van");
    Console.WriteLine("        4 - Saída de veículo");
    Console.WriteLine("        0 - Fechar o programa\r\n");

    int opcao = int.Parse(Utils.LerNumeros(false));
    switch (opcao)
    {
        case Sair_Programa:
            fechar = false;
            break;
        case Estacionar_Moto:
            Console.Clear();
            EstacionarVeiculo(estacionamento, Veiculo.Type.Moto);
            estacionamento.Avisos.Add(new Aviso("Moto adicionada com sucesso!"));
            break;
        case Estacionar_Carro:
            Console.Clear();
            EstacionarVeiculo(estacionamento, Veiculo.Type.Carro);
            estacionamento.Avisos.Add(new Aviso("Carro adicionado com sucesso!"));
            break;
        case Estacionar_Van:
            Console.Clear();
            EstacionarVeiculo(estacionamento, Veiculo.Type.Van);
            estacionamento.Avisos.Add(new Aviso("Van adicionada com sucesso!"));
            break;
        case Saida_Veiculo:
            Console.Clear();
            SaidaVeiculo(estacionamento);
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
        if (resposta.Equals("y"))
        {
            CobrancaSaida(estacionamento, veiculoEncontrado);
        }
        else
        {
            estacionamento.Avisos.Add(new Aviso("Saída de veículo cancelada!"));
        }

    }
}

static void MostrarListaVeiculosEstacionados(IDictionary<int, Veiculo> dicVeiculosEstacionados)
{
    Console.WriteLine("\r\n    Veículos:\r\n");
    foreach (var dicVeiculo in dicVeiculosEstacionados)
        Console.WriteLine($"        {dicVeiculo.Key} - {dicVeiculo.Value.ToString()}");
}

static void CobrancaSaida(Estacionamento estacionamento, Veiculo? veiculo)
{
    double valorCobrado = new();
    DateTime dataHoraSaida = new();
    TimeSpan tempoEstacionado = veiculo.DataHoraEntrada - dataHoraSaida;

    if (tempoEstacionado.Hours < 1)
    {
        valorCobrado = estacionamento.PrecoHora;
    } else
    {
        valorCobrado = int.Parse(tempoEstacionado.Hours.ToString()) * estacionamento.PrecoHora;
    }

    Console.WriteLine($"Preço por hora R$ {estacionamento.PrecoHora}.");
    Console.WriteLine($"Tempo que o carro ficou estacionado {tempoEstacionado.ToString("HH:mm")}.");
    Console.WriteLine($"Valor total a ser cobrado R$ {valorCobrado.ToString("N2")}");
    Console.ReadLine();
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