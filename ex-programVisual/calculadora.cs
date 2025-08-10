/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex_programVisual
{
    using System;

    class Calculadora
    {
        static void Main()
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("=== Calculadora ===");
                Console.WriteLine("1 - Somar");
                Console.WriteLine("2 - Subtrair");
                Console.WriteLine("3 - Multiplicar");
                Console.WriteLine("4 - Dividir");
                //Console.WriteLine("5 - Resto da divisão");
               // Console.WriteLine("6 - Potenciação");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao)) // ele tenta converter o int para string mas tem o um sinal de negacao entao se ele nao consege ele manda a mensagem
                {
                    Console.WriteLine("Opção inválida! Pressione Enter para tentar novamente.");
                    Console.ReadLine();
                    continue;
                }

                if (opcao == 0)
                    break;

                if (opcao < 0 || opcao > 6)
                {
                    Console.WriteLine("Opção inválida! Pressione Enter para tentar novamente.");
                    Console.ReadLine();
                    continue;
                }

                // Solicita os dois valores para as operações
                double valor1 = LerDouble("Digite o primeiro valor: ");
                double valor2 = LerDouble("Digite o segundo valor: ");

                switch (opcao)
                {
                    case 1:
                        Console.WriteLine($"Resultado da soma: {valor1 + valor2}");
                        break;
                    case 2:
                        Console.WriteLine($"Resultado da subtração: {valor1 - valor2}");
                        break;
                    case 3:
                        Console.WriteLine($"Resultado da multiplicação: {valor1 * valor2}");
                        break;
                    case 4:
                        if (valor2 == 0)
                            Console.WriteLine("Não é possível dividir por zero.");
                        else
                            Console.WriteLine($"Resultado da divisão: {valor1 / valor2}");
                        break;
                   // case 5:
                        //if (valor2 == 0)
                           // Console.WriteLine("Não é possível dividir por zero.");
                       // else
                           // Console.WriteLine($"Resto da divisão: {valor1 % valor2}");
                       // break;
                    //case 6:
                        // Para potenciação, vamos tratar valores inteiros para expoente
                       // Console.WriteLine($"Resultado da potenciação: {Math.Pow(valor1, valor2)}");
                      //  break;
                }

                Console.WriteLine("Pressione Enter para voltar ao menu.");
                Console.ReadLine();

            } while (true);

            Console.WriteLine("Programa encerrado. Obrigado!");
        }

        static double LerDouble(string mensagem)
        {
            double valor;
            Console.Write(mensagem);
            while (!double.TryParse(Console.ReadLine(), out valor))// ele tenta converter o double para string mas tem o um sinal de negacao entao se ele nao consege ele manda a mensagem 
            {
                Console.Write("Valor inválido. Tente novamente: ");
            }
            return valor;
        }
    }

}*/
