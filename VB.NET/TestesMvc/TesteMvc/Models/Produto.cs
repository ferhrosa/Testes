using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesteMvc.Models
{
    public class Produto
    {
        public static List<Produto> Produtos = new List<Produto>();

        public static void Criar(Produto produto)
        {
            produto.Salvar();
        }

        public static Produto Carregar(long id)
        {
            return Produtos.Where((p) => p.Id == id).FirstOrDefault();
        }

        public static void Alterar(Produto produto)
        {
            var produtoLista = Produtos.Where((p) => p.Id == produto.Id).FirstOrDefault();

            produtoLista.Descricao = produto.Descricao;
            produtoLista.Quantidade = produto.Quantidade;
        }


        public long? Id { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }

        public void Salvar()
        {
            if ( !Produtos.Contains(this) )
            {
                this.Id = 1;

                if ( Produtos.Any() )
                    this.Id = Produtos.Last().Id + 1;

                Produtos.Add(this);
            }
        }


    }
}