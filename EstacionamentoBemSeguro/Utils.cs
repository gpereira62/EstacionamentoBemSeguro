using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstacionamentoBemSeguro
{
    internal class Utils
    {
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
                            entrada = entrada.Remove(entrada.Length - 1);
                            Console.Write("\b \b"); //Remove o último caractere digitado
                            break;
                        case ConsoleKey.Enter:
                            if (!string.IsNullOrEmpty(entrada))
                                continuarLoop = false;
                            break;
                        case ConsoleKey key when ((ConsoleKey.S == key) || (ConsoleKey.N == key) &&
                                                    entrada.Length == 0):
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

        public static string LerNumeros(bool numeroQuebrados)
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
                    }
                    else
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
    }
}
