using enums;
using excecoes;
using System.Collections.Generic;
using tabuleiro;

namespace xadrez
{
    internal class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }

        public Peca VulneravelEmPassant { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            xeque = false;
            VulneravelEmPassant = null;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao pOrigem, Posicao pDestino)
        {
            Peca p = tab.RetirarPeca(pOrigem);
            p.IncrementarQtdeMovimentos();
            Peca pecaCapturada = tab.RetirarPeca(pDestino);
            tab.ColocarPeca(p, pDestino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            //#Jogada epecial roque pequeno
            if (p is Rei && pDestino.Coluna == pOrigem.Coluna + 2)
            {
                Posicao origemT = new Posicao(pOrigem.Linha, pOrigem.Coluna + 3);
                Posicao destinoT = new Posicao(pOrigem.Linha, pOrigem.Coluna + 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQtdeMovimentos(); 
                tab.ColocarPeca(T,destinoT);
            }
            //#Jogada epecial roque grande
            if (p is Rei && pDestino.Coluna == pOrigem.Coluna - 2)
            {
                Posicao origemT = new Posicao(pOrigem.Linha, pOrigem.Coluna - 4);
                Posicao destinoT = new Posicao(pOrigem.Linha, pOrigem.Coluna - 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQtdeMovimentos();
                tab.ColocarPeca(T, destinoT);
            }
            //#Jogada espacial en passant
            if (p is Peao)
            {
                if (pOrigem.Coluna != pDestino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP=new Posicao(pDestino.Linha+1, pDestino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(pDestino.Linha - 1, pDestino.Coluna);
                    }
                    pecaCapturada=tab.RetirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
            }
            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao pOrigem, Posicao pDestino, Peca pecaCapturada)
        {
            Peca p = tab.RetirarPeca(pDestino);
            p.DecrementarQtdeMovimentos();
            if (pecaCapturada != null)
            {
                tab.ColocarPeca(pecaCapturada, pDestino);
                capturadas.Remove(pecaCapturada);
            }
            tab.ColocarPeca(p, pOrigem);

            //#Jogada epecial roque pequeno
            if (p is Rei && pDestino.Coluna == pOrigem.Coluna + 2)
            {
                Posicao origemT = new Posicao(pOrigem.Linha, pOrigem.Coluna + 3);
                Posicao destinoT = new Posicao(pOrigem.Linha, pOrigem.Coluna + 1);
                Peca T = tab.RetirarPeca(destinoT);
                T.DecrementarQtdeMovimentos();
                tab.ColocarPeca(T, origemT);
            }
            //#Jogada epecial roque grande
            if (p is Rei && pDestino.Coluna == pOrigem.Coluna - 2)
            {
                Posicao origemT = new Posicao(pOrigem.Linha, pOrigem.Coluna - 4);
                Posicao destinoT = new Posicao(pOrigem.Linha, pOrigem.Coluna - 1);
                Peca T = tab.RetirarPeca(destinoT);
                T.IncrementarQtdeMovimentos();
                tab.ColocarPeca(T, origemT);
            }
            //#Jogada especial en passant
            if (p is Peao)
            {
                if (pOrigem.Coluna!= pDestino.Coluna && pecaCapturada == VulneravelEmPassant)
                {
                    Peca peao = tab.RetirarPeca(pDestino);
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(3, pDestino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, pDestino.Coluna);
                    }
                    tab.ColocarPeca(peao, posP);
                }
            }
        }
        public void RealizaJogada(Posicao pOrigem, Posicao pDestino)
        {
            Peca pecaCapturada = ExecutaMovimento(pOrigem, pDestino);
            if (EstaEmXeque(jogadorAtual))
            {
                DesfazMovimento(pOrigem, pDestino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque.");
            }

            if (EstaEmXeque(CorAdversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if (TesteXequeMate(CorAdversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                MudaJogador();
            }

            Peca p = tab.Peca(pDestino);
            //Jogada especial en passant
            if(p is Peao && (pDestino.Linha==pOrigem.Linha-2 || pDestino.Linha == pOrigem.Linha + 2))
            {
                VulneravelEmPassant = p;
            }
            else
            {
                VulneravelEmPassant = null;
            }
        }

        public void ValidarPosicaoOrigem(Posicao pOrigem)
        {
            if (tab.Peca(pOrigem) == null)
            {
                throw new TabuleiroException("Não existe peça válida na posição de origem escolhida.");
            }
            if (jogadorAtual != tab.Peca(pOrigem).Cor)
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
            if (!tab.Peca(pOrigem).MovimentoPossivel(pDestino))
            {
                throw new TabuleiroException("Posição de destino inválida");
            }
        }
        private void MudaJogador()
        {
            if (jogadorAtual == Cor.Branca)
            {
                jogadorAtual = Cor.Preta;
            }
            else
            {
                jogadorAtual = Cor.Branca;
            }
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        private Cor CorAdversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }

        private Peca Rei(Cor cor)
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmXeque(Cor cor)
        {
            Peca r = Rei(cor);
            if (r == null)
            {
                throw new TabuleiroException($"Não existe rei da cor {cor} no tabuleiro");
            }
            foreach (Peca x in PecasEmJogo(CorAdversaria(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[r.Posicao.Linha, r.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequeMate(Cor cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i < tab.Linhas; i++)
                {
                    for (int j = 0; j < tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao pOrigem = x.Posicao;
                            Posicao pDestino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(pOrigem, pDestino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(pOrigem, pDestino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branca));
            ColocarNovaPeca('c', 1, new Bispo(tab, Cor.Branca));
            ColocarNovaPeca('d', 1, new Dama(tab, Cor.Branca));
            ColocarNovaPeca('e', 1, new Rei(tab, Cor.Branca, this));
            ColocarNovaPeca('f', 1, new Bispo(tab, Cor.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('a', 2, new Peao(tab, Cor.Branca,this));
            ColocarNovaPeca('b', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('c', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('d', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('e', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('f', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('g', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('h', 2, new Peao(tab, Cor.Branca, this));

            ColocarNovaPeca('a', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preta));
            ColocarNovaPeca('c', 8, new Bispo(tab, Cor.Preta));
            ColocarNovaPeca('d', 8, new Dama(tab, Cor.Preta));
            ColocarNovaPeca('e', 8, new Rei(tab, Cor.Preta, this));
            ColocarNovaPeca('f', 8, new Bispo(tab, Cor.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preta));
            ColocarNovaPeca('h', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('a', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('b', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('c', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('d', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('e', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('f', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('g', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('h', 7, new Peao(tab, Cor.Preta, this));

        }
    }
}
