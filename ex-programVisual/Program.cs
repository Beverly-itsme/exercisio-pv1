/* using System;
using System.Linq;

class Programa
{
    static void Main()
    {
        int quantidade;

        // Solicita quantidade de números entre 3 e 10
        do
        {
            Console.Write("Quantos números deseja informar? (entre 3 e 10): ");
        } while (!int.TryParse(Console.ReadLine(), out quantidade) || quantidade < 3 || quantidade > 10);

        double[] numeros = new double[quantidade];

        // Solicita os números, validando a entrada
        for (int i = 0; i < quantidade; i++)
        {
            double valor;
            Console.Write($"Digite o número {i + 1}: ");
            while (!double.TryParse(Console.ReadLine(), out valor))
            {
                Console.Write("Valor inválido. Digite um número decimal válido: ");
            }
            numeros[i] = valor;
        }

        // Calcula soma e média usando LINQ
        double soma = numeros.Sum();
        double media = numeros.Average();

        Console.WriteLine($"\nSoma dos valores: {soma}");
        Console.WriteLine($"Média dos valores: {media}");

        Console.WriteLine("\nPressione Enter para sair...");
        Console.ReadLine();
    }
}
*/
