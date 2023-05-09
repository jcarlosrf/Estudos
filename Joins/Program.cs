using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Joins
{
    class Program
    {
        static void Main(string[] args)
        {
            var produtos = new Produtos();


            var final = produtos.Lista1()
               .Join(produtos.Lista2()
                    , prod => true
                    , outro => true
                    , (prod, outro) => new Produtos
                    {
                        Codigo = prod.Codigo,
                        Descricao = outro.Descricao,
                        Preco = outro.Preco
                    }
               );
               
            foreach (Produtos prod in final)
            {
                Console.WriteLine($" {prod.Codigo} | {prod.Descricao} | {prod.Preco} ");
            }




            // -------------
            Console.WriteLine();
            Console.WriteLine("*********************************************");
            Console.WriteLine();

            var agrupado = produtos.Lista2().Union(produtos.Lista1())
                .GroupBy(prod => prod.Codigo)
                .Select(prd => new Produtos
                {
                    Codigo = prd.Key,
                    Descricao = prd.Select(x => x.Descricao).FirstOrDefault(),
                    Preco = prd.Sum(x => x.Preco)
                }
                    );

            foreach (Produtos prod in agrupado)
            {
                Console.WriteLine($" {prod.Codigo} | {prod.Descricao} | {prod.Preco} ");
            }

        }



    }
}
