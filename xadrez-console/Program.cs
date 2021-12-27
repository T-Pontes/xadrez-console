using excecoes;
using System;
using tabuleiro;
using xadrez;

namespace xadrez_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PosicaoXadrez posicaoXadrez = new PosicaoXadrez(7, 'C');
            Console.WriteLine(posicaoXadrez.ToPosicao());
            Console.ReadLine();
        }
    }
}
