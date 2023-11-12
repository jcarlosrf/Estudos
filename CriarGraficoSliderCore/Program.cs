using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json;

namespace CriarGraficoSliderCore
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                var grafico = new GraficoDados
                {
                    EixoXInicio = 0
                    ,
                    EixoXFinal = 40
                    ,
                    EixoXIntervalo = 10
                    ,
                    ExibirEixoX = true
                    ,
                    TituloInferior = "Teste de rodapé"
                    ,
                    TituloSuperior = "Slider"
                    ,
                    Tipo = TipoGrasfico.Slider
                };

                grafico.AddFaixaValores(new List<float> { 19 }, "Valor de X");

                string json = JsonConvert.SerializeObject(grafico, Formatting.Indented);

                GerarGraficos.Gerar(json, @"C:\testes", "teste gráfico v1.docx", "!#Slide#!");


                var graficolinha = new GraficoDados
                {
                    EixoXInicio = 14
                    ,
                    EixoXFinal = 41
                    ,
                    EixoXIntervalo = 1
                    ,
                    ExibirEixoX = true
                    ,
                    EixoYInicio = 0
                    ,
                    EixoYFinal = 20000
                    ,
                    EixoYIntervalo = 2000
                    ,
                    TituloInferior = "Teste de rodapé"
                    ,
                    TituloSuperior = "Gráfico de linhas"
                    ,
                    Tipo = TipoGrasfico.Linha
                };

                List<float> valores = new List<float> { 78.8f, 99.2f, 123.9f, 153.7f, 189.3f, 231.3f, 280.6f, 337.8f, 403.8f, 479f, 564.1f, 659.5f, 765.2f, 881.4f, 1007.80f, 1143.70f, 1288.50f, 1440.90f, 1599.40f, 1762.30f, 1927.40f, 2092.50f, 2255.00f, 2412.10f, 2561.20f, 2699.30f, 2823.80f, 2932.20f, };
                graficolinha.AddFaixaValores(valores, "3");

                valores = new List<float> { 83.5f, 105f, 131.2f, 162.6f, 200.1f, 244.4f, 296.4f, 356.8f, 426.3f, 505.7f, 595.5f, 696.2f, 807.9f, 930.7f, 1064.40f, 1208.30f, 1361.70f, 1523.40f, 1691.90f, 1865.20f, 2041.30f, 2217.80f, 2391.80f, 2560.70f, 2721.40f, 2871.10f, 3006.80f, 3125.90f };
                graficolinha.AddFaixaValores(valores, "10");

                valores = new List<float> { 94.5f, 118.7f, 148f, 183.3f, 225.3f, 275f, 333.2f, 400.9f, 478.9f, 567.9f, 668.7f, 781.7f, 907.3f, 1045.60f, 1196.30f, 1358.90f, 1532.70f, 1716.20f, 1908.00f, 2106.10f, 2308.00f, 2511.30f, 2712.90f, 2909.80f, 3098.60f, 3276.00f, 3438.90f, 3584.00f };
                graficolinha.AddFaixaValores(valores, "50");

                valores = new List<float> { 107f, 134.2f, 167.1f, 206.6f, 253.7f, 309.4f, 374.7f, 450.5f, 537.9f, 637.7f, 750.8f, 877.8f, 1019.00f, 1174.70f, 1344.60f, 1528.40f, 1725.00f, 1933.40f, 2151.70f, 2378.00f, 2609.60f, 2843.70f, 3077.10f, 3306.50f, 3528.00f, 3738.10f, 3933.10f, 4109.30f };
                graficolinha.AddFaixaValores(valores, "90");

                valores = new List<float> { 113.4f, 142.1f, 176.8f, 218.5f, 268.2f, 326.9f, 395.8f, 475.7f, 567.9f, 673.3f, 792.6f, 926.6f, 1075.8f, 1240.4f, 1420.1f, 1614.7f, 1823.1f, 2044.2f, 2276.2f, 2516.9f, 2763.8f, 3013.9f, 3263.8f, 3510.1f, 3748.8f, 3976.0f, 4187.9f, 4380.7f };
                graficolinha.AddFaixaValores(valores, "97");

                string json1 = JsonConvert.SerializeObject(graficolinha, Formatting.Indented);

                
                GerarGraficos.Gerar(json1, @"C:\testes", "teste gráfico v1.docx", "!#grafico#!");

            }
            catch  (Exception ex)
            {
                string x = ex.Message;
            }
        }

        private static void Grafico()
        {
            int largura = 800;
            int altura = 500;

            List<float> valoresY = new List<float>
            {
                97,
                113.4f,
                142.1f,
                176.8f,
                218.5f,
                268.2f,
                326.9f,
                395.8f,
                475.7f,
                567.9f,
                673.3f,
                792.6f,
                926.6f,
                1075.8f,
                1240.4f,
                1420.1f,
                1614.7f,
                1823.1f,
                2044.2f,
                2276.2f,
                2516.9f,
                2763.8f,
                3013.9f,
                3263.8f,
                3510.1f,
                3748.8f,
                3976.0f,
                4187.9f,
                4380.7f
            };

            using Bitmap grafico = new Bitmap(largura, altura);
            using Graphics g = Graphics.FromImage(grafico);

            g.Clear(Color.White);

            using Pen linhaHorizontal = new Pen(Color.LightGray);
            for (int i = 1000; i <= 4000; i += 1000)
            {
                int y = altura - (i * altura / 4000);
                g.DrawLine(linhaHorizontal, 0, y, largura, y);
            }

            using Pen linha = new Pen(Color.Blue, 2);

            float passoX = largura / (valoresY.Count - 1);

            for (int i = 0; i < valoresY.Count; i++)
            {
                int x = (int)(i * passoX);
                int y = altura - (int)(valoresY[i] * altura / 4000);

                g.DrawEllipse(linha, x - 2, y - 2, 4, 4);
            }

            using Font fonte = new Font("Arial", 8);
            using SolidBrush brush = new SolidBrush(Color.Black);

            float passoY = 1000 * altura / 4000;

            for (int i = 0; i <= 4000; i += 1000)
            {
                int y = altura - (i * altura / 4000);
                g.DrawLine(linhaHorizontal, 0, y, largura, y);
                g.DrawString(i.ToString(), fonte, brush, 5, y - 10);
            }

            for (int i = 0; i < valoresY.Count; i++)
            {
                int x = (int)(i * passoX);
                g.DrawString((i + 1).ToString(), fonte, brush, x - 10, altura - 20);
            }


            string caminhoDoArquivo = @"C:\testes\grafico_customizado.png";
            grafico.Save(caminhoDoArquivo, ImageFormat.Png);

            Console.WriteLine("Gráfico de linhas customizado gerado com sucesso e salvo em: " + caminhoDoArquivo);
        }

      
    }
}
