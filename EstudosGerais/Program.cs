using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace EstudosGerais
{
    class Program
    {
        static void Main(string[] args)
        {
            int largura = 400;
            int altura = 50;

            int[] medidas = new int[] { 0, 10, 20, 30, 40, 28 };

            using Bitmap grafico = new Bitmap(largura, altura);
            using Graphics g = Graphics.FromImage(grafico);

            g.Clear(Color.White);

            using Pen linha = new Pen(Color.Black, 2);
            g.DrawLine(linha, 0, altura / 2, largura, altura / 2);

            using Pen marcacao = new Pen(Color.Black, 2);
            foreach (var medida in medidas)
            {
                int x = (medida * largura) / medidas[medidas.Length - 1];
                g.DrawLine(marcacao, x, altura / 2 - 5, x, altura / 2 + 5);

                if (medida == 28)
                {
                    g.FillEllipse(Brushes.Red, x - 5, altura / 2 - 5, 10, 10);
                }
            }

            string caminhoDoArquivo = "grafico_slider.jpg";
            grafico.Save(caminhoDoArquivo, ImageFormat.Jpeg);

            Console.WriteLine("Gráfico gerado com sucesso e salvo em: " + caminhoDoArquivo);
        }
    }
}
