using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CriarGraficoSliderCore
{
    public enum TipoGrasfico
    {
        Slider, Linha
    }
    public class GraficoDados
    {
        public GraficoDados()
        {
            Dados = new List<Pontos>();
        }

        public TipoGrasfico Tipo { get; set; }
        public string TituloSuperior { get; set; }
        public string TituloInferior { get; set; }
        public bool ExibirEixoX { get; set; }
        public float EixoXInicio { get; set; }
        public float EixoXFinal { get; set; }
        public float EixoXIntervalo { get; set; }
        public float EixoYInicio { get; set; }
        public float EixoYFinal { get; set; }
        public float EixoYIntervalo { get; set; }
        public List<Pontos> Dados { get; set; }

        public void AddFaixaValores (List<float> dados, string legenda)
        {
            var pontos = new Pontos() { Valores = dados, Legenda = legenda };
            Dados.Add(pontos);
        }
    }

    public class Pontos
    {
        public string Legenda { get; set; }
        public List<float> Valores { get; set; }
    }

}