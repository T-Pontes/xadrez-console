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
                PartidaDeXadrez partida = new PartidaDeXadrez();
                while (!partida.terminada)
                {
                    Console.Clear();
                    Tela.ImprimirTabuleiro(partida.tab);
                    Console.Write("Digite a posição de origem: ");
                    Posicao pOrigem=Tela.LerPosicaoXadrez().ToPosicao();
                    bool[,] posicoesPossiveis = partida.tab.Peca(pOrigem).MovimentosPossiveis();//partida.tab.Peca(pOrigem).MovimentosPossiveis();
                    Console.Clear();
                    Tela.ImprimirTabuleiro(partida.tab,posicoesPossiveis);
                    Console.Write("Digite a posição de destino: ");
                    Posicao pDestino = Tela.LerPosicaoXadrez().ToPosicao();
                    partida.ExecutaMovimento(pOrigem, pDestino);
                }
            }
            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}
