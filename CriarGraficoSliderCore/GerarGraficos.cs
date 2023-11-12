using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace CriarGraficoSliderCore
{
    public class GerarGraficos
    {
        public static void Gerar(string jsonGrafico, string pasta, string nomeDocx, string chaveDocx)
        {
            var grafico = JsonConvert.DeserializeObject<GraficoDados>(jsonGrafico);
            string arquivo = string.Empty;

            if (grafico.Tipo == TipoGrasfico.Slider)
            {
                arquivo = GraficoSlider(grafico, pasta);
            }
            else
            {
                arquivo = GraficoLinhas(grafico, pasta);
            }

            WordOpenXML.ReplaceTextWithImage(Path.Combine(pasta, nomeDocx), chaveDocx, arquivo, arquivo.Replace("png", "docx"));
        }

        public static string GraficoSlider(GraficoDados dados, string pasta)
        {
            int larguraGrafico = 600; // Largura total do gráfico
            int margem = 30; // Margem desejada à esquerda e à direita
            int largura = larguraGrafico + 2 * margem; // Largura total com margens

            Color azulPersonalizado = Color.FromArgb(56, 140, 231);
            Font fonteIntervalos = new Font("Calibri", 10);
            Font FonteTitulo = new Font("Calibri", 14);
            Font FonteRodape = new Font("Calibri", 12);

            int altura = 120; // Altura desejada

            List<float> medidas = new List<float>();
            for (float i = dados.EixoXInicio; i <= dados.EixoXFinal; i += dados.EixoXIntervalo)
            {
                medidas.Add(i);
            }

            using Bitmap grafico = new Bitmap(largura, altura);

            using (Graphics g = Graphics.FromImage(grafico))
            {
                // Define um fundo transparente
                grafico.MakeTransparent();
                g.Clear(Color.Transparent);

                // Escrever o título superior e inferior
                StringFormat tituloFormat = new StringFormat();
                tituloFormat.Alignment = StringAlignment.Center;

                g.DrawString(dados.TituloSuperior, FonteTitulo, Brushes.Black, largura / 2, 0, tituloFormat);
                g.DrawString(dados.TituloInferior, FonteRodape, Brushes.Black, largura / 2, altura - 20, tituloFormat);               

                using Pen linha = new Pen(azulPersonalizado, 3);  
                g.DrawLine(linha, margem, altura / 2, largura - margem, altura / 2);  

                using Pen marcacao = new Pen(azulPersonalizado, 3);
                foreach (var medida in medidas)
                {
                    int x = (int)(((medida - dados.EixoXInicio) * larguraGrafico) / (dados.EixoXFinal - dados.EixoXInicio)) + margem;
                    g.DrawLine(marcacao, x, altura / 2 - 6, x, altura / 2 + 6);

                    if (dados.ExibirEixoX)
                    {
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;                        
                        g.DrawString(medida.ToString(), fonteIntervalos, Brushes.Black, x, altura / 2 + 10, stringFormat);
                    }
                }                

                foreach (var pontos in dados.Dados)
                {
                    foreach (var valor in pontos.Valores)
                    {
                        int xx = (int)(((valor - dados.EixoXInicio) * larguraGrafico) / (dados.EixoXFinal - dados.EixoXInicio)) + margem;
                        g.FillEllipse(Brushes.Red, xx, altura / 2 - 5, 10, 10);
                    }
                }                
            }

            string caminhoDoArquivo = Path.Combine(pasta, "slider" + DateTime.Now.ToString("hhmmss") + ".png");
            grafico.Save(caminhoDoArquivo, ImageFormat.Png);

            return caminhoDoArquivo;
        }

        private static string GraficoLinhas(GraficoDados dados, string pasta)
        {
            int larguraGrafico = 700; // Largura total do gráfico
            int margemesquerda = 60; // Margem desejada à esquerda e à direita
            int margemdireita = 30; // Margem desejada à esquerda e à direita
            int margemSuperior = 50; // Margem superior
            int margemInferior = 100; // Margem inferior
            int largura = larguraGrafico + margemdireita + margemesquerda; // Largura total com margens
            int altura = 600;

            Font fonteIntervalos = new Font("Calibri", 10, FontStyle.Regular);
            Font FonteTitulo = new Font("Calibri", 14);
            Font FonteRodape = new Font("Calibri", 12);

            List<float> medidas = new List<float>();
            for (float i = dados.EixoXInicio; i <= dados.EixoXFinal; i += dados.EixoXIntervalo)
            {
                medidas.Add(i);
            }

            using Bitmap grafico = new Bitmap(largura, altura);
            using (Graphics g = Graphics.FromImage(grafico))
            {
                // Define um fundo transparente
                grafico.MakeTransparent();
                g.Clear(Color.Transparent);

                // Escrever o título superior e inferior
                StringFormat tituloFormat = new StringFormat();
                tituloFormat.Alignment = StringAlignment.Center;

                g.DrawString(dados.TituloSuperior, FonteTitulo, Brushes.Black, largura / 2, 5, tituloFormat);
                g.DrawString(dados.TituloInferior, FonteRodape, Brushes.Black, largura / 2, altura - 30, tituloFormat);

                int eixoXInicio = margemesquerda;
                int eixoXFinal = largura - margemdireita;
                int eixoYInicio = altura - margemInferior;
                int eixoYFinal = margemSuperior;

                using Pen linhaHorizontal = new Pen(Color.LightGray);
                for (int i = (int)0; i <= dados.EixoYFinal; i += (int)dados.EixoYIntervalo)
                {
                    tituloFormat.Alignment = StringAlignment.Far;
                    int y = altura - margemInferior - ((i - (int)dados.EixoYInicio) * (altura - margemSuperior - margemInferior) / (int)dados.EixoYFinal);
                    g.DrawLine(linhaHorizontal, eixoXInicio, y, eixoXFinal, y);
                    g.DrawString(i.ToString(), fonteIntervalos, Brushes.Black, margemesquerda - 10, y - 6, tituloFormat);
                }

                float passoX = (larguraGrafico) / (dados.Dados[0].Valores.Count - 1);

                float[] somaValores = new float[dados.Dados[0].Valores.Count];

                Color azulPersonalizado = Color.FromArgb(56, 140, 231);
                Color[] cores = { azulPersonalizado, Color.Red, Color.Green, Color.Yellow, Color.Orange }; // Exemplo de cores diferentes

                for (int j = 0; j < dados.Dados.Count; j++)
                {
                    var pontos = dados.Dados[j];

                    using Pen linha = new Pen(cores[j % cores.Length], 2);

                    for (int i = 0; i < pontos.Valores.Count; i++)
                    {
                        somaValores[i] += pontos.Valores[i];
                        int x = margemesquerda + (int)(i * passoX);
                        int y = altura - margemInferior - (int)(somaValores[i] * (altura - margemSuperior - margemInferior) / (int)dados.EixoYFinal);

                        g.DrawEllipse(linha, x - 2, y - 2, 4, 4);

                        if (i > 0)
                        {
                            int xAnterior = margemesquerda + (int)((i - 1) * passoX);
                            int yAnterior = altura - margemInferior - (int)(somaValores[i - 1] * (altura - margemSuperior - margemInferior) / (int)dados.EixoYFinal);
                            g.DrawLine(linha, xAnterior, yAnterior, x, y);
                        }
                    }
                }

                if (dados.ExibirEixoX)
                {
                    int ii = 0;
                    foreach (var medida in medidas)
                    {
                        int x = margemesquerda + (int)(ii * passoX);
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        g.DrawString(medida.ToString(), fonteIntervalos, Brushes.Black, x, altura - margemInferior + 10, stringFormat);

                        ii++;
                    }
                }

                // Desenha a legenda
                int legendaY = altura - margemInferior + 50;
                int legendaX = margemesquerda;

                for (int i = 0; i < dados.Dados.Count; i++)
                {
                    g.DrawLine(new Pen(cores[i % cores.Length], 2), legendaX, legendaY, legendaX + 20, legendaY);
                    g.DrawString(dados.Dados[i].Legenda, fonteIntervalos, Brushes.Black, legendaX + 25, legendaY - 8);
                    legendaX += 120;
                }
            }

            string caminhoDoArquivo = Path.Combine(pasta, "graficolinhas" + DateTime.Now.ToString("hhmmss") + ".png");
            grafico.Save(caminhoDoArquivo, ImageFormat.Png);

            return caminhoDoArquivo;
        }

    }
}
