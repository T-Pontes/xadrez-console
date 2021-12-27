using enums;
using System;
using tabuleiro;

namespace xadrez
{
    internal class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        private int turno;
        private Cor jogadorAtual;
        public bool terminada { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada= false;
            ColocarPecas();
        }

        public void ExecutaMovimento(Posicao pOrigem, Posicao pDestino)
        {
            Peca p = tab.RetirarPeca(pOrigem);
            p.IncrementarQtdeMovimentos();
            Peca pecaCapturada = tab.RetirarPeca(pDestino);
            tab.ColocarPeca(p,pDestino);
        }

        private void ColocarPecas()
        {

            tab.ColocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('C',1).ToPosicao());
            tab.ColocarPeca(new Rei(tab, Cor.Preta), new PosicaoXadrez('E',3).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('G',5).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('A',7).ToPosicao());
            tab.ColocarPeca(new Rei(tab, Cor.Branca), new PosicaoXadrez('H',2).ToPosicao());

        }
    }
}
