using enums;
using tabuleiro;

namespace xadrez
{
    internal class Rei: Peca    
    {
        public Rei(Tabuleiro tab, Cor cor) : base(cor,tab)
        {
        }

        private bool PodeMover(Posicao posicao)
        {
            Peca p = Tab.Peca(posicao);
            return p == null || p.Cor != Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat=new bool[Tab.Linhas,Tab.Colunas];
            Posicao posicao = new Posicao(0, 0);
            //N
            posicao.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
            if (Tab.PosicaoValida(posicao) && PodeMover(posicao))
            {
                mat[posicao.Linha, posicao.Coluna] = true;
            }
            //NE
            posicao.DefinirValores(Posicao.Linha - 1, Posicao.Coluna+1);
            if (Tab.PosicaoValida(posicao) && PodeMover(posicao))
            {
                mat[posicao.Linha, posicao.Coluna] = true;
            }
            //L
            posicao.DefinirValores(Posicao.Linha, Posicao.Coluna+1);
            if (Tab.PosicaoValida(posicao) && PodeMover(posicao))
            {
                mat[posicao.Linha, posicao.Coluna] = true;
            }
            //SE
            posicao.DefinirValores(Posicao.Linha + 1, Posicao.Coluna+1);
            if (Tab.PosicaoValida(posicao) && PodeMover(posicao))
            {
                mat[posicao.Linha, posicao.Coluna] = true;
            }
            //S
            posicao.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
            if (Tab.PosicaoValida(posicao) && PodeMover(posicao))
            {
                mat[posicao.Linha, posicao.Coluna] = true;
            }
            //SO
            posicao.DefinirValores(Posicao.Linha + 1, Posicao.Coluna-1);
            if (Tab.PosicaoValida(posicao) && PodeMover(posicao))
            {
                mat[posicao.Linha, posicao.Coluna] = true;
            }
            //O
            posicao.DefinirValores(Posicao.Linha, Posicao.Coluna-1);
            if (Tab.PosicaoValida(posicao) && PodeMover(posicao))
            {
                mat[posicao.Linha, posicao.Coluna] = true;
            }
            //NO
            posicao.DefinirValores(Posicao.Linha - 1, Posicao.Coluna-1);
            if (Tab.PosicaoValida(posicao) && PodeMover(posicao))
            {
                mat[posicao.Linha, posicao.Coluna] = true;
            }
            return mat;
        }

        public override string ToString()
        {
            return "R";
        }
    }
}
