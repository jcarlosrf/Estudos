using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using WebAspUpdatePanel.BLL;

namespace WebAspUpdatePanel
{
    public partial class Default : Page
    {
        public List<ChartDataItem> dataChart = new List<ChartDataItem>();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblTempo.Text = "Dentro: " + DateTime.Now.ToLongTimeString();
            lblTempoGeral.Text = "Fora: " + DateTime.Now.ToLongTimeString();

            // Cria uma nova instância do componente Chart e adiciona a página

            //this.Controls.Add(Chart1);

            // Cria alguns dados de exemplo
            dataChart = new List<ChartDataItem>
                {
                    new ChartDataItem { XValue = "Hora", YValue = DateTime.Now.Hour },
                    new ChartDataItem { XValue = "Minuto", YValue = DateTime.Now.Minute },
                    new ChartDataItem { XValue = "Segundos", YValue = DateTime.Now.Second }
                };

            // Usa o ChartBuilder para configurar o gráfico
            var builder = new ChartBuilder(Chart1);
            builder.SetTitle("Meu Gráfico")
                   .SetLegend("Minha Legenda")
                   .AddArea("Área 1", "Eixo X", "Eixo Y")
                   .AddSeries("Série 1", "Área 1", "XValue", "YValue", SeriesChartType.Doughnut)
                   .AddData(dataChart, "XValue", "YValue");


            builder.AddArea("Área 2", "Eixo X", "Eixo Y")
                    .AddSeries("Série 2", "Área 2", "XValue1", "YValue1", SeriesChartType.Bubble)
                    .AddData(dataChart, "XValue", "YValue");

            // Renderiza o gráfico
            Chart1.DataBind();
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lblTempo.Text = "Dentro: " + DateTime.Now.ToLongTimeString();
            lblTempoGeral.Text = "Fora: " + DateTime.Now.ToLongTimeString();

            dataChart = new List<ChartDataItem>
                {
                    new ChartDataItem { XValue = "Hora", YValue = DateTime.Now.Hour },
                    new ChartDataItem { XValue = "Minuto", YValue = DateTime.Now.Minute },
                    new ChartDataItem { XValue = "Segundos", YValue = DateTime.Now.Second }
                };

            Chart1.Series["Série 1"].Points.DataBind(dataChart, "XValue", "YValue", "");

        }
    }
}