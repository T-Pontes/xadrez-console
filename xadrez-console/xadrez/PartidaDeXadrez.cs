using enums;
using excecoes;
using System;
using tabuleiro;

namespace xadrez
{
    internal class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
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

        public void RealizaJogada(Posicao pOrigem, Posicao pDestino)
        {
            ExecutaMovimento(pOrigem, pDestino);
            turno++;
            MudaJogador();
        }

        public void ValidarPosicaoOrigem(Posicao pOrigem)
        {
            if (tab.Peca(pOrigem)==null)
            {
                throw new TabuleiroException("Não existe peça válida na posição de origem escolhida.");
            }
            if (jogadorAtual!=tab.Peca(pOrigem).Cor)
            {
                throw new TabuleiroException("A peça escolhida não pertence ao jogador da vez.");
            }
            if (!tab.Peca(pOrigem).ExistemMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça escolhida.");
            }
        }

        public void ValidarPosicaoDestino(Posicao pOrigem, Posicao pDestino)
        {
            if (!tab.Peca(pOrigem).PodeMoverPara(pDestino))
            {
                throw new TabuleiroException("Posição de destino inválida");
            }
        }
        private void MudaJogador()
        {
            if (jogadorAtual==Cor.Branca)
            {
                jogadorAtual = Cor.Preta;
            }
            else
            {
                jogadorAtual=Cor.Branca;
            }
        }

        private void ColocarPecas()
        {

            tab.ColocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('c',1).ToPosicao());
            tab.ColocarPeca(new Rei(tab, Cor.Branca), new PosicaoXadrez('d',1).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('e',1).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('c', 2).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('d', 2).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('e', 2).ToPosicao());

            tab.ColocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('c', 8).ToPosicao());
            tab.ColocarPeca(new Rei(tab, Cor.Preta), new PosicaoXadrez('d', 8).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('e', 8).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('c', 7).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('d', 7).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('e', 7).ToPosicao());
        }
    }
}
