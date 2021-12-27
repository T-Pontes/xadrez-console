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
            try
            {
                Tabuleiro tabuleiro = new Tabuleiro(8, 8);
                tabuleiro.ColocarPeca(new Torre(tabuleiro, enums.Cor.Preta), new Posicao(0, 0));
                tabuleiro.ColocarPeca(new Torre(tabuleiro, enums.Cor.Preta), new Posicao(1, 3));
                tabuleiro.ColocarPeca(new Rei(tabuleiro, enums.Cor.Preta), new Posicao(0, 2));
                tabuleiro.ColocarPeca(new Torre(tabuleiro, enums.Cor.Branca), new Posicao(3, 5));
                tabuleiro.ColocarPeca(new Torre(tabuleiro, enums.Cor.Branca), new Posicao(2, 6));
                tabuleiro.ColocarPeca(new Rei(tabuleiro, enums.Cor.Branca), new Posicao(0, 3));
                Tela.ImprimirTabuleiro(tabuleiro);
            }
            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}
