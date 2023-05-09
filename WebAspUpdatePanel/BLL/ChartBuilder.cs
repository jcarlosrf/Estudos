using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;

namespace WebAspUpdatePanel.BLL
{
    public class ChartBuilder
    {
        private readonly Chart chart;
        private readonly DataTable table;

        public ChartBuilder(Chart chart)
        {
            this.chart = chart;
            this.table = new DataTable();
        }

        public ChartBuilder SetTitle(string title)
        {
            this.chart.Titles.Add(title);
            return this;
        }

        public ChartBuilder SetLegend(string legend)
        {
            this.chart.Legends.Add(legend);
            return this;
        }

        public ChartBuilder AddArea(string areaName, string axisXTitle, string axisYTitle)
        {
            var chartArea = new ChartArea(areaName);
            chartArea.AxisX.Title = axisXTitle;
            chartArea.AxisY.Title = axisYTitle;
            this.chart.ChartAreas.Add(chartArea);
            return this;
        }

        public ChartBuilder AddSeries(string seriesName, string areaName, string xField, string yField, SeriesChartType chartType)
        {
            this.table.Columns.Add(xField);
            this.table.Columns.Add(yField);
            var series = new Series(seriesName);
            series.ChartType = chartType;
            series.ChartArea = areaName;
            series.XValueMember = xField;
            series.YValueMembers = yField;
            this.chart.Series.Add(series);
            return this;
        }

        public void AddData(object dataSource, string xField, string yField)
        {
            foreach (var dataItem in (IEnumerable)dataSource)
            {
                var row = this.table.NewRow();
                row[xField] = DataBinder.GetPropertyValue(dataItem, xField);
                row[yField] = DataBinder.GetPropertyValue(dataItem, yField);
                this.table.Rows.Add(row);
            }
            this.chart.DataSource = this.table;
        }
    }

    public class ChartDataItem
    {
        public string XValue { get; set; }
        public double YValue { get; set; }
    }


}