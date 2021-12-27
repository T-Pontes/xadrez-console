using System;
using tabuleiro;

namespace xadrez
{
    internal class PosicaoXadrez
    {
        public int Linha { get; set; }
        public char Coluna { get; set; }

        public PosicaoXadrez(int linha, char coluna)
        {
            Linha = linha;
            Coluna = coluna;
        }

        public Posicao ToPosicao()
        {
            return new Posicao(8 - Linha, Coluna - 'A');
        }

        public override string ToString()
        {
            return "" + Coluna+Linha;
        }
    }
}
