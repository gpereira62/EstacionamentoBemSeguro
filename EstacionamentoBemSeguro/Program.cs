using EstacionamentoBemSeguro.Models;

const int Sair_Programa = 0;
const int Estacionar_Moto = 1;
const int Estacionar_Carro = 2;
const int Estacionar_Van = 3;

bool fechar = true;

while (fechar)
{

    Estacionamento estacionamento = new();

    Console.WriteLine("\r\nQuantas vagas pequenas para motos terão ?");
    estacionamento.QtdeVagaPequena = int.Parse(LerNumeros(false));

    Console.WriteLine("\r\n\r\nQuantas vagas médias para carros terão ?");
    estacionamento.QtdeVagaMedia = int.Parse(LerNumeros(false));

    Console.WriteLine("\r\n\r\nQuantas vagas grandes para vans terão ?");
    estacionamento.QtdeVagaGrande = int.Parse(LerNumeros(false));

    Console.WriteLine("\r\n\r\nQual o preço que será cobrado por hora ?");
    Console.Write("R$ ");
    estacionamento.PrecoHora = double.Parse(LerNumeros(true));

    estacionamento.CriarVagas();

    Console.Clear();

    Console.WriteLine("\r\n------------ Bem Vindo ao Estacionamento Bem Seguro ------------");

    Console.WriteLine($"\r\nPreço cobrado por hora R$ {estacionamento.PrecoHora}");
    Console.WriteLine($"\r\nTotal de vagas: {estacionamento.TotalVagas()}");
    Console.WriteLine($"Total de vagas dispóniveis: {estacionamento.TotalVagasDisponiveis()}");
    Console.WriteLine($"Total de vagas ocupadas: {estacionamento.TotalVagasOcupadas()}");

    Console.WriteLine("\r\nDigite uma das opções para serem executadas:");
    Console.WriteLine("1 - Estacionar uma moto");
    Console.WriteLine("2 - Estacionar um carro");
    Console.WriteLine("3 - Estacionar uma van");
    //Console.WriteLine("4 - Quantas vagas restam ?");
    //Console.WriteLine("5 - Quantas vagas totais há no estacionamento ?");
    //Console.WriteLine("6 - O estacionamento está cheio ?");
    //Console.WriteLine("7 - O estacionamento está vazio ?");
    //Console.WriteLine("8 - Quantas vagas as vans estão ocupadando ?");
    Console.WriteLine("0 - Fechar o programa\r\n");

    bool conversao = int.TryParse(Console.ReadLine(), out int opcao);

    if (!conversao)
    {
        Console.WriteLine("Opção Invalida! Tente Novamente.");
        continue;
    }

    switch (opcao)
    {
        case Sair_Programa:
            fechar = false;
            break;
        case Estacionar_Moto:
            Console.Clear();
            EstacionarVeiculo(estacionamento, Veiculo.Type.Moto);
            break;
        case Estacionar_Carro:
            Console.Clear();
            EstacionarVeiculo(estacionamento, Veiculo.Type.Carro);
            break;
        case Estacionar_Van:
            Console.Clear();
            EstacionarVeiculo(estacionamento, Veiculo.Type.Van);
            break;
        default:
            Console.WriteLine("Opção Invalida! Tente Novamente.");
            break;
    }
}

static void EstacionarVeiculo(Estacionamento estacionamento, Veiculo.Type tipo)
{
    Veiculo veiculo = new(tipo);

    Console.WriteLine("\r\nDigite o nome do veículo:");
    veiculo.Nome = LerStrings();

    Console.WriteLine("\r\n\r\nDigite a placa do veículo:");
    veiculo.Placa = LerStrings();

    estacionamento.EstacionarVeiculo(veiculo);
}

static string LerStrings()
{
    ConsoleKeyInfo cki;
    string entrada = "";
    bool continuarLoop = true;
    while (continuarLoop)
        if (Console.KeyAvailable)
        {
            cki = Console.ReadKey(true);

                switch (cki.Key)
                {
                    case ConsoleKey.Backspace:
                        if (entrada.Length == 0) continue;
                        entrada = entrada.Remove(entrada.Length - 1);
                        Console.Write("\b \b"); //Remove o último caractere digitado
                        break;
                    case ConsoleKey.Enter:
                            if (!string.IsNullOrEmpty(entrada))
                                continuarLoop = false;
                            break;
                    case ConsoleKey key:
                        entrada += cki.KeyChar;
                        Console.Write(cki.KeyChar);
                        break;
            }
        }
    return entrada;
}

static string LerNumeros(bool numeroQuebrados)
{
    ConsoleKeyInfo cki;
    string entrada = "";
    bool continuarLoop = true;
    while (continuarLoop)
        if (Console.KeyAvailable)
        {
            cki = Console.ReadKey(true);

            if (!numeroQuebrados)
            {
                switch (cki.Key)
                {
                    case ConsoleKey.Backspace:
                        if (entrada.Length == 0) continue;
                        entrada = entrada.Remove(entrada.Length - 1);
                        Console.Write("\b \b"); //Remove o último caractere digitado
                        break;
                    case ConsoleKey.Enter:
                        if (!string.IsNullOrEmpty(entrada))
                            continuarLoop = false;
                        break;
                    case ConsoleKey key when ((ConsoleKey.D0 <= key) && (key <= ConsoleKey.D9) ||
                                                (ConsoleKey.NumPad0 <= key) && (key <= ConsoleKey.NumPad9)):
                        entrada += cki.KeyChar;
                        Console.Write(cki.KeyChar);
                        break;
                }
            } else
            {
                switch (cki.Key)
                {
                    case ConsoleKey.Backspace:
                        if (entrada.Length == 0) continue;
                        entrada = entrada.Remove(entrada.Length - 1);
                        Console.Write("\b \b"); //Remove o último caractere digitado
                        break;
                    case ConsoleKey.Enter:
                        continuarLoop = false;
                        break;
                    case ConsoleKey key when ((ConsoleKey.D0 <= key) && (key <= ConsoleKey.D9) ||
                                                (ConsoleKey.NumPad0 <= key) && (key <= ConsoleKey.NumPad9) ||
                                                (ConsoleKey.OemComma == key)):
                        entrada += cki.KeyChar;
                        Console.Write(cki.KeyChar);
                        break;
                }
            }

        }
    return entrada;
}