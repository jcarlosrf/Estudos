using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ConsumePokemonAPI;

namespace ConsolePokemonAPI01
{
    class Program
    {
        /*********************************************************
         * 
         * Comparação Entre Exec 01 e Exec 02 - Pesquisa Pokemon
         * 
         *********************************************************/
        static void Main(string[] args)
        {
           
            var stopwatch = new Stopwatch();

            Console.WriteLine("************************************");
            Console.WriteLine("Exec 01 - Execução sincrona");

            // Inicia Contagem de Tempo
            stopwatch.Start();

            // Exec 01 - Sincrono 
            //ExecuteSincrono();

            // Finaliza Contagem de Tempo
            stopwatch.Stop();
            
            Console.WriteLine("");

            Console.WriteLine($"Exec 01 - Tempo passado: {stopwatch.Elapsed}");
            Console.WriteLine("************************************");
           
            Console.ReadKey();


            Console.WriteLine("");

            Console.WriteLine("************************************");
            Console.WriteLine("Exec 02 - Execução assincrona");
            
            // Inicia Contagem de Tempo
            stopwatch = Stopwatch.StartNew();
          
            // Exec 02 - Assincrono
            ExecuteAssincrono().GetAwaiter().GetResult();

            // Finaliza Contagem de Tempo
            stopwatch.Stop();
            
            Console.WriteLine($"Exec 02 - Tempo passado: {stopwatch.Elapsed}");
            Console.WriteLine("************************************");

            Console.ReadKey();

        }

        /*********************************************************
        * 
        * Exec 01 - Pesquisa Pokemon
        * 
        *********************************************************/
        static void ExecuteSincrono()
        {

           

         


    ////declarando a variavel do tipo StreamWriter para
    //abrir ou criar um arquivo para escrita
    StreamWriter x;

            // define local e Nome do arquivo
            string CaminhoNome = Directory.GetCurrentDirectory() + "\\A_01_Sinc\\" ;

            //sing File_01_Path = new Directory.GetCurrentDirectory() + "\\A_01_Sinc\\";
            //public string File_02_Path = "";

            if (!Directory.Exists(CaminhoNome))
            {

                Directory.CreateDirectory(CaminhoNome);
            }

           // string CaminhoNome = Directory.GetCurrentDirectory() + "\\A_Exec01_Sincrono.txt";

            //Inica Arquivo
            x = File.CreateText(CaminhoNome + "A_Exec01_Sincrono.txt");

            // Inica Arquivo
           // x.WriteLine(Directory.GetCurrentDirectory());
            //pulando linha sem escrita
          //  x.WriteLine();

            //Define Cabecario do Aqruivo
            x.WriteLine("Id|Name|Height|Weight|Abilities|Type|Front_Default|");

            // IniciaLista Pokemon
            PokemonApi pokemonApi = new PokemonApi();

            // Corre a Lista Pokemon
            foreach (string name in pokemonApi.Pokemons)
            {
                // Busca Dados Pokemon
                string response = pokemonApi.GetPokemon(name, CaminhoNome);
                
                // Log na tela 
                Console.WriteLine(response);

               // Grava Aqruivo 
                x.WriteLine(response);
            }


            //fechando o arquivo texto
            x.Close();

        }
        /*********************************************************
         * 
         * Exec 02 - Pesquisa Pokemon
         * 
         *********************************************************/

        static async Task ExecuteAssincrono()
        {


            ////declarando a variavel do tipo StreamWriter para
            //abrir ou criar um arquivo para escrita
            StreamWriter x;

            // define local e Nome do arquivo
            string CaminhoNome = Directory.GetCurrentDirectory() + "\\A_02_Assinc\\";

            //sing File_01_Path = new Directory.GetCurrentDirectory() + "\\A_01_Sinc\\";
            //public string File_02_Path = "";

            if (!Directory.Exists(CaminhoNome))
            {

                Directory.CreateDirectory(CaminhoNome);
            }

            // string CaminhoNome = Directory.GetCurrentDirectory() + "\\A_Exec01_Sincrono.txt";

            //Inica Arquivo
            x = File.CreateText(CaminhoNome + "A_Exec02_Assincrono.txt");

            //  string CaminhoNome = Directory.GetCurrentDirectory() + "\\A_Exec02_Assincrono.txt";
            //e associando o caminho e nome ao metodo

            // Inica Arquivo
            //x = File.CreateText(CaminhoNome);

            //escrevendo o titulo

            // x.WriteLine(Directory.GetCurrentDirectory());
            //pulando linha sem escrita
            // x.WriteLine();

            //Define Cabecario do Aqruivo
            x.WriteLine("Id|Name|Height|Weight|Abilities|Type|Front_Default|");

            // Inicia Lista Pokemon 
            PokemonApi pokemonApi = await Task.Run(()=> new PokemonApi());

           Parallel.ForEach(pokemonApi.Pokemons, name =>
            {
                // Pesquisa Pokemon 
                string response = pokemonApi.GetPokemonAsync(name, CaminhoNome).GetAwaiter().GetResult();

              // Log Tela
                Console.WriteLine(response);

                // x.WriteLine();
                //Grava Arquivo
                x.WriteLine(response);

            });



            //fechando o arquivo texto
            x.Close();

        }


    }
}
