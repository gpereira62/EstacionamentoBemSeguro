namespace EstacionamentoBemSeguro
{
    internal class Utils
    {
        private static void TratarBackspace(ref string entrada)
        {
            entrada = entrada.Remove(entrada.Length - 1);
            Console.Write("\b \b");
        }

        public static string LerRespostaPergunta()
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
                            TratarBackspace(ref entrada);
                            break;
                        case ConsoleKey.Enter:
                            if (!string.IsNullOrEmpty(entrada))
                                continuarLoop = false;
                            break;
                        case ConsoleKey key when (ConsoleKey.S == key || ConsoleKey.N == key) && entrada.Length == 0:
                            entrada += cki.KeyChar;
                            Console.Write(cki.KeyChar);
                            break;
                    }
                }
            return entrada;
        }

        public static string LerStrings()
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
                            TratarBackspace(ref entrada);
                            break;
                        case ConsoleKey.Enter:
                            if (!string.IsNullOrEmpty(entrada))
                                continuarLoop = false;
                            break;
                        default:
                            entrada += cki.KeyChar;
                            Console.Write(cki.KeyChar);
                            break;
                    }
                }
            return entrada;
        }

        public static string LerNumeros(bool numerosQuebrados)
        {
            ConsoleKeyInfo cki;
            string entrada = "";
            bool continuarLoop = true;
            while (continuarLoop)
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);

                    bool ehDigito = (ConsoleKey.D0 <= cki.Key && cki.Key <= ConsoleKey.D9)
                                 || (ConsoleKey.NumPad0 <= cki.Key && cki.Key <= ConsoleKey.NumPad9);
                    bool ehVirgula = numerosQuebrados && cki.Key == ConsoleKey.OemComma;

                    switch (cki.Key)
                    {
                        case ConsoleKey.Backspace:
                            if (entrada.Length == 0) continue;
                            TratarBackspace(ref entrada);
                            break;
                        case ConsoleKey.Enter:
                            if (!string.IsNullOrEmpty(entrada))
                                continuarLoop = false;
                            break;
                        default:
                            if (ehDigito || ehVirgula)
                            {
                                entrada += cki.KeyChar;
                                Console.Write(cki.KeyChar);
                            }
                            break;
                    }
                }
            return entrada;
        }
    }
}
