using System;
using System.Collections.Generic;
using System.Text;

namespace Joins
{
    public class Produtos
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }

        public List<Produtos> Lista1()
        {
            return new List<Produtos>()
               {
                    new Produtos{ Codigo = 1 , Descricao = "Produto 1", Preco = 1}
                    , new Produtos{ Codigo = 2 , Descricao = "Produto 2", Preco = 2 }
                    , new Produtos{ Codigo = 3 , Descricao = "Produto 3", Preco = 3 }
               };
        }


        public List<Produtos> Lista2()
        {
            return new List<Produtos>()
               {
                    new Produtos{ Codigo = 1 , Descricao = "Produto 1", Preco = 4}
                    , new Produtos{ Codigo = 2 , Descricao = "Produto 2", Preco = 5 }
               };
        }
    }
}
